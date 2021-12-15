using System;

namespace NFS.Unpacker
{
    class BinEntry
    {
        public UInt32 dwHash { get; set; }
        public Int32 dwArchiveID { get; set; }
        public UInt32 dwOffset { get; set; } // * 2048
        public UInt32 dwExtraOffset { get; set; }
        public Int32 dwDecompressedSize { get; set; }
        public UInt32 dwCRC { get; set; }
    }
}
