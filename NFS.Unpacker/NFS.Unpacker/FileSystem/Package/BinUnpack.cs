using System;
using System.IO;
using System.Collections.Generic;

namespace NFS.Unpacker
{
    class BinUnpack
    {
        static List<BinEntry> m_EntryTable = new List<BinEntry>();

        public static void iDoIt(String m_IndexFile, String m_DstFolder)
        {
            BinHashList.iLoadProject();
            using (FileStream TIndexStream = File.OpenRead(m_IndexFile))
            {
                Int32 dwTotalFiles = (Int32)TIndexStream.Length / 24;
                var lpTable = TIndexStream.ReadBytes((Int32)TIndexStream.Length);

                m_EntryTable.Clear();
                using (MemoryStream TEntryReader = new MemoryStream(lpTable))
                {
                    for (Int32 i = 0; i < dwTotalFiles; i++)
                    {
                        UInt32 dwHash = TEntryReader.ReadUInt32();
                        Int32 dwArchiveID = TEntryReader.ReadInt32();
                        UInt32 dwOffset = TEntryReader.ReadUInt32();
                        UInt32 dwExtraOffset = TEntryReader.ReadUInt32();
                        Int32 dwDecompressedSize = TEntryReader.ReadInt32();
                        UInt32 dwCRC = TEntryReader.ReadUInt32();

                        //Skip removed file
                        if (dwOffset == dwExtraOffset)
                        {
                            continue;
                        }

                        var TEntry = new BinEntry
                        {
                            dwHash = dwHash,
                            dwArchiveID = dwArchiveID,
                            dwOffset = dwOffset * 2048,
                            dwExtraOffset = dwExtraOffset,
                            dwDecompressedSize = dwDecompressedSize,
                            dwCRC = dwCRC,
                        };

                        m_EntryTable.Add(TEntry);
                    }

                    TEntryReader.Dispose();
                }
                TIndexStream.Dispose();
            }

            foreach (var m_Entry in m_EntryTable)
            {
                String m_FileName = BinHashList.iGetNameFromHashList(m_Entry.dwHash);
                String m_FullPath = m_DstFolder + m_FileName;
                String m_ArchiveFile = Path.GetDirectoryName(m_IndexFile) + @"\" + String.Format("ZZDATA{0}.BIN", m_Entry.dwArchiveID.ToString());

                Utils.iSetInfo("[UNPACKING]: " + m_FileName);
                Utils.iCreateDirectory(m_FullPath);

                using (FileStream TArchiveStream = File.OpenRead(m_ArchiveFile))
                {
                    if (!File.Exists(m_ArchiveFile))
                    {
                        Utils.iSetError("[ERROR]: Input archive -> " + m_ArchiveFile + " <- does not exist");
                        return;
                    }

                    TArchiveStream.Seek(m_Entry.dwOffset, SeekOrigin.Begin);
                    var lpBuffer = TArchiveStream.ReadBytes(m_Entry.dwDecompressedSize);

                    File.WriteAllBytes(m_FullPath, lpBuffer);

                    TArchiveStream.Dispose();
                }
            }
        }
    }
}
