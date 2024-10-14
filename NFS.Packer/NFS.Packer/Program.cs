using System;

namespace NFS.Packer
{
    class Program
    {
        public static String m_Title = "Need for Speed ZDIR Packer";

        static void Main(String[] args)
        {
            Console.Title = m_Title;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(m_Title);
            Console.WriteLine("(c) 2024 Ekey (h4x0r) / v{0}\n", Utils.iGetApplicationVersion());
            Console.ResetColor();

            if (args.Length != 2)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("[Usage]");
                Console.WriteLine("    NFS.Packer <m_SrcDirectory> <m_DstDirectory>\n");
                Console.WriteLine("    m_SrcDirectory - Source directory");
                Console.WriteLine("    m_DstDirectory - Destination directory\n");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[Examples]");
                Console.WriteLine("    NFS.Packer D:\\NFSC\\UNP D:\\NFSC\\NEW\n");
                Console.ResetColor();
                return;
            }

            String m_SrcDirectory = Utils.iCheckArgumentsPath(args[0]);
            String m_DstDirectory = Utils.iCheckArgumentsPath(args[1]);

            BinPack.iDoIt(m_SrcDirectory, m_DstDirectory);
        }
    }
}
