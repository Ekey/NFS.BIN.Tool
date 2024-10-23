using System;
using System.IO;

namespace NFS.Unpacker
{
    class Program
    {
        public static String m_Title = "Need for Speed ZDIR Unpacker";

        static void Main(String[] args)
        {
            Console.Title = m_Title;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(m_Title);
            Console.WriteLine("(c) 2024 Ekey (h4x0r) / v{0}\n", Utils.iGetApplicationVersion());
            Console.ResetColor();

            if (args.Length != 3)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("[Usage]");
                Console.WriteLine("    NFS.Unpacker <m_Format> <m_File> <m_Directory>\n");
                Console.WriteLine("    m_Format - ZDIR format (for NFSHP2 PS2 set -true) other games -false");
                Console.WriteLine("    m_File - Source of ZDIR.BIN index file");
                Console.WriteLine("    m_Directory - Destination directory\n");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[Examples]");
                Console.WriteLine("    NFS.Unpacker E:\\Games\\NFS\\ZDIR.BIN D:\\Unpacked");
                Console.ResetColor();
                return;
            }

            String m_Format = args[0];
            String m_BinFile = args[1];
            String m_Output = Utils.iCheckArgumentsPath(args[2]);

            Boolean bOldZDirFormat = false;
            switch (m_Format)
            {
                case "-false": bOldZDirFormat = false; break;
                case "-true": bOldZDirFormat = true; break;
                default: Utils.iSetError("[ERROR]: Unknown format -> use \"-false\" or \"-true\""); return;
            }

            if (Path.GetFileName(m_BinFile) != "ZDIR.BIN")
            {
                Utils.iSetError("[ERROR]: You must specify ZDIR.BIN fle for extract files");
                return;
            }

            if (!File.Exists(m_BinFile))
            {
                Utils.iSetError("[ERROR]: Input file -> " + m_BinFile + " <- does not exist");
                return;
            }

            BinUnpack.iDoIt(m_BinFile, m_Output, bOldZDirFormat);
        }
    }
}
