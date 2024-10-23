using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace NFS.Unpacker
{
    class BinUnpack
    {
        private static Boolean bZZData = true;
        private static List<BinEntry> m_EntryTable = new List<BinEntry>();
        private static List<String> m_BinList = new List<String>();

        public static void iDoIt(String m_IndexFile, String m_DstFolder, Boolean bOldZDirFormat = false)
        {
            BinHashList.iLoadProject();
            using (FileStream TIndexStream = File.OpenRead(m_IndexFile))
            {
                // 12 bytes - NFS:HP2 - [SLUS-20362]
                // 24 bytes - Everything else

                Int32 dwTotalFiles = (Int32)TIndexStream.Length / 24;
                if (bOldZDirFormat)
                {
                    dwTotalFiles = (Int32)TIndexStream.Length / 12;
                }

                var lpTable = TIndexStream.ReadBytes((Int32)TIndexStream.Length);

                m_EntryTable.Clear();
                using (MemoryStream TEntryReader = new MemoryStream(lpTable))
                {
                    for (Int32 i = 0; i < dwTotalFiles; i++)
                    {
                        if (bOldZDirFormat)
                        {
                            UInt32 dwNameHash = TEntryReader.ReadUInt32();
                            UInt32 dwLocalOffset = TEntryReader.ReadUInt32();
                            Int32 dwSize = TEntryReader.ReadInt32();

                            var TEntry = new BinEntry
                            {
                                dwNameHash = dwNameHash,
                                dwArchiveID = 0,
                                dwLocalOffset = dwLocalOffset << 11,
                                dwTotalOffset = 0,
                                dwSize = dwSize,
                                dwChecksum = 0,
                            };

                            m_EntryTable.Add(TEntry);
                        }
                        else
                        {
                            UInt32 dwNameHash = TEntryReader.ReadUInt32();
                            Int32 dwArchiveID = TEntryReader.ReadInt32();
                            UInt32 dwLocalOffset = TEntryReader.ReadUInt32();
                            UInt32 dwTotalOffset = TEntryReader.ReadUInt32();
                            Int32 dwSize = TEntryReader.ReadInt32();
                            UInt32 dwChecksum = TEntryReader.ReadUInt32();

                            var TEntry = new BinEntry
                            {
                                dwNameHash = dwNameHash,
                                dwArchiveID = dwArchiveID,
                                dwLocalOffset = dwLocalOffset << 11,
                                dwTotalOffset = dwTotalOffset,
                                dwSize = dwSize,
                                dwChecksum = dwChecksum,
                            };

                            m_EntryTable.Add(TEntry);
                        }
                    }

                    TEntryReader.Dispose();
                }
                TIndexStream.Dispose();
            }

            //Need for Speed - Undercover (Oct 9, 2008 prototype) (doesn't contains ZZDATAX.BIN files)
            String m_HooFile = Path.GetDirectoryName(m_IndexFile) + @"\" + "NAMES.HOO";
            if (File.Exists(m_HooFile))
            {
                m_BinList.Clear();
                m_BinList = File.ReadAllLines(m_HooFile).ToList();

                for (Int32 i = 0; i < m_BinList.Count; i++)
                {
                    m_BinList[i] = m_BinList[i].Replace("Outfile: ", "");
                }

                bZZData = false;
            }

            foreach (var m_Entry in m_EntryTable)
            {
                String m_FileName = BinHashList.iGetNameFromHashList(m_Entry.dwNameHash);

                String m_FullPath = null;
                String m_ArchiveFile = null;
                String m_ZZDataFile = "ZZDATA";

                if (bZZData)
                {
                    if (!bOldZDirFormat)
                    {
                        m_ZZDataFile += "{0}";
                    }

                    m_FullPath = m_DstFolder + String.Format(m_ZZDataFile + @"\{1}", m_Entry.dwArchiveID.ToString(), m_FileName);
                    m_ArchiveFile = Path.GetDirectoryName(m_IndexFile) + @"\" + String.Format(m_ZZDataFile + ".BIN", m_Entry.dwArchiveID.ToString());
                }
                else
                {
                    m_FullPath = m_DstFolder + Path.GetFileNameWithoutExtension(m_BinList[m_Entry.dwArchiveID]) + @"\" + m_FileName;
                    m_ArchiveFile = Path.GetDirectoryName(m_IndexFile) + @"\" + m_BinList[m_Entry.dwArchiveID];
                }

                Utils.iSetInfo("[UNPACKING]: " + m_FileName);
                Utils.iCreateDirectory(m_FullPath);

                BinHelpers.ReadWriteFile(m_ArchiveFile, m_FullPath, m_Entry.dwLocalOffset, m_Entry.dwSize);
            }
        }
    }
}
