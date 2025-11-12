using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menu
{
    internal class Program
    {
        static void Main()
        {
            string name = "";
            int szivveres = 0;
            bool man = true;
            int currentPoint = 0;
            do
            {
                bool selected = false;
                do
                {
                    ShowMenu(currentPoint);
                    switch (Console.ReadKey().Key)
                    {
                        case ConsoleKey.Enter:
                            selected = true;
                            break;
                        case ConsoleKey.UpArrow:
                            if (currentPoint > 0)
                            {
                                currentPoint -= 1;
                            }
                            break;
                        case ConsoleKey.DownArrow:
                            {
                                if (currentPoint < 2)
                                    currentPoint += 1;
                            }
                            break;
                        default:
                            Console.Beep();
                            break;
                    }
                } while (!selected);
                switch (currentPoint)
                {
                    case 0: // Adatbekérés

                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("*** ÚJ MÉRÉS RÖGZÍTÉSE ***");
                        Console.ForegroundColor = ConsoleColor.White;

                        Console.Write("Kérem a nevét: ");
                        name = Console.ReadLine();

                        Console.Write("Kérem a szívverésnek a mérési adatát: ");
                        szivveres = int.Parse(Console.ReadLine());

                        Console.Write("Kérem a nemét (f/n): ");
                        man = Console.ReadLine() == "f";

                        Console.WriteLine("Az adatokat sikeresen rögzítettük. Enterre tovább...");

                        break;

                    case 1: // Adatkiírás

                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("*** ADATKIÍRÁS ***");
                        Console.ForegroundColor = ConsoleColor.White;

                        Console.WriteLine($"A neve: {name}");
                        Console.WriteLine($"A születési éve: {szivveres}");
                        Console.WriteLine($"A neme: {(man ? "Férfi" : "Nő")}");

                        Console.WriteLine("Enterre tovább...");
                        Console.ReadKey();

                        break;

                    case 2: // Kilépés
                        Console.Clear();
                        Console.Write("Biztosan kilép? (i/n): ");
                        if (Console.ReadKey().Key != ConsoleKey.I)
                        {
                            currentPoint = 0;
                        }
                        break;
                }
            } while (currentPoint != 2);
        }

        static void ShowMenu(int cPoint)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("*** VÉRNYOMÁSNAPLÓ ***");
            Console.ForegroundColor = ConsoleColor.White;

            if (cPoint == 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
            }
            Console.WriteLine("Új mérési adatok rögzítése");
            if (cPoint == 1)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
            }
            Console.WriteLine("Mérési adatok kiírása");
            if (cPoint == 2)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
            }
            Console.WriteLine("Kilépés");
            Console.ForegroundColor = ConsoleColor.White;

        }
    }
}

