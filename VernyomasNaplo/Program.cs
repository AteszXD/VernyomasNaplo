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

        }

        static void ReadCSVFile(string username)
        {
            // a username.csv fájl beolvasása, ez a létező eredményeknek van
            records = File.ReadAllLines($"Users/{username}.csv").ToList();            
        }

        static void DisplayRecords()
        {
            Console.Write("|Név\t\t|Dátum\t\t\t\t|Vérnyomás\t\t|\n");
            foreach (string record in records)
            {
                Console.WriteLine($"|{record.Split(';')[0]}\t|{record.Split(';')[1]}\t|{record.Split(';')[2]}\t|\n");
            }
        }
    }
}
