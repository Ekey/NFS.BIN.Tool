using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace NFS.Packer
{
    class BinPack
    {
        private static List<BinEntry> m_EntryTable = new List<BinEntry>();

        public static String[] iGetRootDirectories(String m_Directory)
        {
            String[] m_Result = Directory.GetDirectories(m_Directory);
            for (Int32 i = 0; i < m_Result.Length; i++)
            {
                m_Result[i] = m_Result[i].Replace(m_Directory, "");
            }

            return m_Result;
        }

        public static UInt32 iAlignUInt32(UInt32 dwValue, UInt32 dwAlignSize)
        {
            if (dwValue == 0)
            {
                return dwValue;
            }

            return dwValue + ((dwAlignSize - (dwValue % dwAlignSize)) % dwAlignSize);
        }

        public static void iDoIt(String m_SrcDirectory, String m_DstDirectory)
        {
            m_EntryTable.Clear();

            Utils.iCreateDirectory(m_DstDirectory);
            String[] m_RootDirectories = iGetRootDirectories(m_SrcDirectory);

            foreach (var m_Root in m_RootDirectories)
            {
                String m_ArchiveNum = Regex.Match(m_Root, @"\d+").Value;

                using (BinaryWriter TBinWriter = new BinaryWriter(File.Open(m_DstDirectory + m_Root + ".BIN", FileMode.Create)))
                {
                    foreach (String m_File in Directory.GetFiles(m_SrcDirectory + m_Root, "*.*", SearchOption.AllDirectories))
                    {
                        var lpBuffer = File.ReadAllBytes(m_File);

                        String m_FileName = m_File.Replace(m_SrcDirectory + m_Root + @"\", "");

                        Console.WriteLine("[PACKING]: {0}", m_FileName);

                        var m_Entry = new BinEntry();

                        if (!m_FileName.Contains("__Unknown"))
                        {
                            m_Entry.dwNameHash = BinHash.iGetHash(m_FileName.ToUpper());
                        }
                        else
                        {
                            m_Entry.dwNameHash = Convert.ToUInt32(Path.GetFileNameWithoutExtension(m_FileName), 16);
                        }

                        m_Entry.dwArchiveID = Convert.ToInt32(m_ArchiveNum);
                        m_Entry.dwLocalOffset = (UInt32)TBinWriter.BaseStream.Position >> 11;
                        m_Entry.dwTotalOffset = (UInt32)TBinWriter.BaseStream.Position >> 11;
                        m_Entry.dwSize = lpBuffer.Length;
                        m_Entry.dwChecksum = BinHash.iGetDataHash(lpBuffer);
                        m_EntryTable.Add(m_Entry);

                        UInt32 dwAlignedSize = iAlignUInt32((UInt32)lpBuffer.Length, (UInt32)2048);
                        Array.Resize(ref lpBuffer, (Int32)dwAlignedSize);

                        TBinWriter.Write(lpBuffer);
                    }

                    TBinWriter.Dispose();
                }
            }

            using (BinaryWriter TBinWriter = new BinaryWriter(File.Open(m_DstDirectory + "ZDIR.BIN", FileMode.Create)))
            {
                m_EntryTable = m_EntryTable.OrderBy(x => x.dwNameHash).ToList();

                foreach (var m_Entry in m_EntryTable)
                {
                    TBinWriter.Write(m_Entry.dwNameHash);
                    TBinWriter.Write(m_Entry.dwArchiveID);
                    TBinWriter.Write(m_Entry.dwLocalOffset);
                    TBinWriter.Write(m_Entry.dwTotalOffset);
                    TBinWriter.Write(m_Entry.dwSize);
                    TBinWriter.Write(m_Entry.dwChecksum);
                }

                TBinWriter.Dispose();
            }
        }
    }
}
