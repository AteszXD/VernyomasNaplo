using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VernyomasNaplo
{
    internal class Program
    {
        static List<string> records;
        static void Main(string[] _)
        {
            Console.Write("Adja meg a vérnyomás értékét: ");
            string record = Console.ReadLine();
            WriteCSVFile(record,user);
            ReadCSVFile(user);
            DisplayRecords();
        }

        static void ReadCSVFile(string username)
        {
            // a username.csv fájl beolvasása, ez a létező eredményeknek van
            records = File.ReadAllLines($"Users/{username}.csv").ToList();            
        }

        static void DisplayRecords()
        {
            Console.WriteLine($"{user} Vérnyomásmérései");
            foreach (string record in records)
            {
                Console.Write($"| {record.Split(';')[0]} | {record.Split(';')[1]}\t| {record.Split(';')[2]}\t|\n");
            }
        }

        static void WriteCSVFile(string record, string username)
        {
            File.AppendAllText($"Users/{username}.csv",$"{username};{DateTime.Now};{record}\n", Encoding.UTF8);
        }
    }
}
