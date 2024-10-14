using System;

namespace NFS.Packer
{
    //DisculatorDriver::bFileDirectoryEntry
    class BinEntry
    {
        public UInt32 dwNameHash { get; set; }
        public Int32 dwArchiveID { get; set; }
        public UInt32 dwLocalOffset { get; set; }
        public UInt32 dwTotalOffset { get; set; }
        public Int32 dwSize { get; set; }
        public UInt32 dwChecksum { get; set; }
    }
}