using System;
using System.IO;

namespace NFS.Unpacker
{
    class Program
    {
        static void Main(String[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Need for Speed ZDIR Unpacker");
            Console.WriteLine("(c) 2021 Ekey (h4x0r) / v{0}\n", Utils.iGetApplicationVersion());
            Console.ResetColor();

            if (args.Length != 2)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("[Usage]");
                Console.WriteLine("    NFS.Unpacker <m_File> <m_Directory>\n");
                Console.WriteLine("    m_File - Source of ZDIR.BIN index file");
                Console.WriteLine("    m_Directory - Destination directory\n");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[Examples]");
                Console.WriteLine("    RS.Unpacker E:\\Games\\NFS\\ZDIR.BIN D:\\Unpacked");
                Console.ResetColor();
                return;
            }

            UInt32 test = BinHash.iGetHash(@"CHARACTERS\MCOP02.BIN");

            String m_BinFile = args[0];
            String m_Output = Utils.iCheckArgumentsPath(args[1]);

            if (!File.Exists(m_BinFile))
            {
                Utils.iSetError("[ERROR]: Input file -> " + m_BinFile + " <- does not exist");
                return;
            }

            BinUnpack.iDoIt(m_BinFile, m_Output);
        }
    }
}
