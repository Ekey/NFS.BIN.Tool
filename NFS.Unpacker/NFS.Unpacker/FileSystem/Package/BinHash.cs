using System;

namespace NFS.Unpacker
{
    class BinHash
    {
        public static UInt32 iGetHash(String m_String, UInt32 dwHash = 0xffffffff)
        {
            for (Int32 i = 0; i < m_String.Length; i++)
            {
                dwHash = 33 * dwHash + m_String[i];
            }
            return dwHash;
        }
    }
}
