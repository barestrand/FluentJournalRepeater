using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;

namespace FluentJournalRepeater
{
    class Program
    {
        public static string getPath(string file)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, file);
        }

        public static string getDir()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        public static string makeIdString(int idInt)
        {
            string idRaw = idInt.ToString();
           // string idString = idRaw.Insert(idRaw.Length - 2, ".");

            return idRaw;
        }

        public static int numberFromString(string str)
        {
            MatchCollection mc = Regex.Matches(str, @"\d+");

            string str2 = "";

            for (int i = 0; i < mc.Count; i++)
            {
                str2 += (mc[i].Value);
            }

            int number = 0;
            bool arg1 = int.TryParse(Console.ReadLine(), out number);

            return number;
        }

        public static void changeFileEnding(string nameContains, string newFileEnding)
        {
            string[] filePaths = Directory.GetFiles(getDir());
            Console.WriteLine("Directory consists of " + filePaths.Length + " files.");
            int cc = 0;
            foreach (string myfile in filePaths)
            {
                if (myfile.Contains(nameContains))
                {
                    string filename = Path.ChangeExtension(myfile, newFileEnding);
                    File.Move(myfile, filename);
                    cc++;
                }
            }
            Console.WriteLine(cc + " files were changed.");
        }

        public static void makeJournal(int start, int end)
        {
            // variables to replace
            string journal_DIR = @"#X_BASEDIR\";
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;

            string journal_ITEM_ID = @"#X_ITEM_ID";
            int delta = 10;
            
            // read journal
            var reader = new StreamReader(File.OpenRead(getPath(@"journal_item")));
            string journal_item = reader.ReadToEnd();
            reader.Close();

            // make new journal file
            StreamWriter file;
            file = new StreamWriter(getPath(@"JOURNAL_LONG"), true);
            file.AutoFlush = true;
            
            // run compilation
            journal_item = journal_item.Replace(journal_DIR, baseDir);
            for (int i = start; i <= end; i += delta)
            {
                Console.WriteLine("Running: " + makeIdString(i));

                string journalADD = journal_item;
                
                journalADD = journalADD.Replace(journal_ITEM_ID, makeIdString(i));

                file.WriteLine(journalADD);
            }
            file.Close();

            Console.WriteLine("New journal generated!");
        }
        
            static void Main(string[] args)
        {
            Console.WriteLine("Choose an option: ");
            Console.WriteLine("1. Change file endings to .dat.gz");
            Console.WriteLine("2. Generate repeating of \"journal_item\"");

            switch (Console.ReadLine())
            {
                case "1":
                    Console.WriteLine("Data filenames Contain: ");
                    changeFileEnding(Console.ReadLine(), ".dat.gz");
                    break;
                case "2":
                    Console.WriteLine("Type starting file Int: ");
                    int start = 0;
                    bool arg1 = int.TryParse(Console.ReadLine(), out start);
                    Console.WriteLine("Type ending file Int: ");
                    int end = 0;
                    bool arg2 = int.TryParse(Console.ReadLine(), out end);

                    Console.WriteLine("Journal directory is: \n " + AppDomain.CurrentDomain.BaseDirectory);

                    Console.WriteLine("Working...");
                    if (start != 0 && end != 0)
                    {
                        makeJournal(start, end);
                    }
                    break;
                default:
                    break;
            }
            
            Console.WriteLine("Press ENTER key to exit");
            Console.ReadLine();
        }
    }
}
