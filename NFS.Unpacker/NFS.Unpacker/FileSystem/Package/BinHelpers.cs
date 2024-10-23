using System;
using System.IO;

namespace NFS.Unpacker
{
    class BinHelpers
    {
        public static void ReadWriteFile(String m_ArchiveFile, String m_FullPath, UInt32 dwOffset, Int32 dwSize)
        {
            Int32 MAX_BUFFER = 524288;
            Int32 dwBytesLeft = dwSize;

            if (!File.Exists(m_ArchiveFile))
            {
                Utils.iSetError("[ERROR]: Input archive -> " + m_ArchiveFile + " <- does not exist");
                return;
            }

            using (BinaryWriter TDstStream = new BinaryWriter(File.Open(m_FullPath, FileMode.Create)))
            {
                using (FileStream TArchiveStream = File.OpenRead(m_ArchiveFile))
                {
                    if (dwSize > TArchiveStream.Length)
                    {
                        return;
                    }

                    TArchiveStream.Seek(dwOffset, SeekOrigin.Begin);

                    if (dwSize < MAX_BUFFER)
                    {
                        var lpBuffer = TArchiveStream.ReadBytes(dwSize);
                        TDstStream.Write(lpBuffer);
                    }
                    else
                    {
                        do
                        {
                            if (dwBytesLeft < MAX_BUFFER)
                            {
                                MAX_BUFFER = dwBytesLeft;
                            }

                            var lpBuffer = TArchiveStream.ReadBytes(MAX_BUFFER);
                            TDstStream.Write(lpBuffer);
                            dwBytesLeft -= MAX_BUFFER;
                        }
                        while (dwBytesLeft > 0);
                    }

                    TArchiveStream.Dispose();
                }
                TDstStream.Dispose();
            }
        }
    }
}
