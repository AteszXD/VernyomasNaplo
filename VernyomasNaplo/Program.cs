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
        static int cPoint = 0;
        static string user;
        static List<string> records;

        static void Main(string[] _)
        {
            CheckUsersFile();
        }

        /// <summary>
        /// A bejelentkezés után megnyíló menü kezelése. A felhasználó itt tudja megnézni méréseit és újat rögzíteni (illetve kilépni). Ez a menü a nyilakkal irányítható.
        /// </summary>
        static void LoggedinMenu()
        {
            do
            {
                bool selected = false;
                do
                {
                    ShowMenu1(cPoint);
                    switch (Console.ReadKey().Key)
                    {
                        case ConsoleKey.Enter:
                            selected = true;
                            break;
                        case ConsoleKey.UpArrow:
                            if (cPoint > 0)
                            {
                                cPoint -= 1;
                            }
                            break;
                        case ConsoleKey.DownArrow:
                            if (cPoint < 2)
                            {
                                cPoint += 1;
                            }
                            break;
                    }
                } while (!selected);

                switch (cPoint)
                {
                    case 0: // Adatbekérés

                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("*** ÚJ MÉRÉS RÖGZÍTÉSE ***");
                        Console.ForegroundColor = ConsoleColor.White;

                        Console.Write("Adja meg a vérnyomás értékét: ");
                        string record = Console.ReadLine();
                        WriteCSVFile(record, user);

                        Console.WriteLine("Az adatokat sikeresen rögzítettük. Enterre tovább...");
                        Console.ReadLine();
                        Console.Clear();

                        ReadCSVFile(user);
                        DisplayRecords();

                        Console.WriteLine("Enterre tovább...");
                        Console.ReadLine();

                        break;

                    case 1: // Adatkiírás

                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("*** ADATKIÍRÁS ***");
                        Console.ForegroundColor = ConsoleColor.White;

                        ReadCSVFile(user);
                        DisplayRecords();

                        Console.WriteLine("Enterre tovább...");
                        Console.ReadLine();

                        break;

                    case 2: // Kilépés
                        Console.Clear();
                        Console.Beep();
                        Console.Write("Biztosan kilép? (i/n): ");
                        if (Console.ReadKey().Key != ConsoleKey.I)
                        {
                            Program.cPoint = 0;
                        }
                        break;
                }

            } while (cPoint != 2);
            void ShowMenu1(int cPoint)
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

        /// <summary>
        /// Program indításakor megjelenő menü kezelése. A felhasználó itt tud választani a bejelentkezés és új felhasználó regisztrálása között (vagy kilépni). Ez a menü a nyilakkal irányítható.
        /// </summary>
        static void LoginMenu()
        {
            do
            {
                bool selected = false;
                do
                {
                    ShowMenu2(cPoint);
                    switch (Console.ReadKey().Key)
                    {
                        case ConsoleKey.Enter:
                            selected = true;
                            break;
                        case ConsoleKey.UpArrow:
                            if (cPoint > 0)
                            {
                                cPoint -= 1;
                            }
                            break;
                        case ConsoleKey.DownArrow:
                            if (cPoint < 2)
                            {
                                cPoint += 1;
                            }
                            break;
                    }
                } while (!selected);
                switch (cPoint)
                {
                    case 0: // Adatbekérés

                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("*** REGISZTRÁCIÓ ***");
                        Console.ForegroundColor = ConsoleColor.White;

                        Register();

                        break;

                    case 1: // Adatkiírás

                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("*** BEJELENTKEZÉS ***");
                        Console.ForegroundColor = ConsoleColor.White;

                        Login();

                        break;

                    case 2: // Kilépés
                        Console.Clear();
                        Console.Beep();
                        Console.Write("Biztosan kilép? (i/n): ");
                        if (Console.ReadKey().Key != ConsoleKey.I)
                        {
                            cPoint = 0;
                        }
                        break;
                }
            } while (cPoint != 2);

            void ShowMenu2(int currentPoint)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("*** VÉRNYOMÁSNAPLÓ ***");
                Console.ForegroundColor = ConsoleColor.White;

                if (currentPoint == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine("Regisztráció");
                if (currentPoint == 1)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine("Bejelentkezés");
                if (currentPoint == 2)
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

        /// <summary>
        /// Megnézi, hogy létezik-e a users.csv fájl. Ha nem, létrehozza és regisztrációra irányít. Ha igen, megnézi, hogy üres-e. Ha üres, regisztrációra irányít, ha nem, felhozza a felhasználókezelő menüt.
        /// </summary>
        static void CheckUsersFile()
        {
            if (!File.Exists("users.csv")) // Nem --> Létrehozzuk a .csvt és a mappát is, ez egy első indítás
            {
                File.Create("users.csv").Close();
                Directory.CreateDirectory("Users");
                Register();
            }

            else // Igen --> Ha üres: nincs felhasználó, regisztráció | van felhasználó, menü
            {
                FileInfo users = new FileInfo("users.csv");
                if (users.Length > 0)
                {
                    LoginMenu();
                }
                else
                {
                    Register();
                }
            }
        }

        /// <summary>
        /// Regisztrációs függvény, kezeli a felhasználónevekben a speciális karaktereket és hogy üres-e. Bekéri a jelszót, születési dátumot és nemet, illetve lementi a felhasználó adatait és létrehozza az üres naplót.
        /// </summary>
        static void Register()
        {
            string specialChars = "\\/:*?\"<>|"; // Speciális karakterek amiket a Windows nem engedélyez fájl- és mappanévként
            string username;
            bool allowed;

            // Felhasználónév bekérése
            do
            {
                allowed = true;
                Console.Write("Felhasználónév: ");
                username = Console.ReadLine();

                // 1. Üres név kezelése
                if (string.IsNullOrEmpty(username))
                {
                    Console.Clear();
                    Console.WriteLine("A felhasználónév nem lehet üres!");
                    allowed = false;
                }

                // 2. Speciális karakterek kezelése
                foreach (char specialChar in specialChars)
                {
                    if (username.Contains(specialChar))
                    {
                        Console.Clear();
                        Console.WriteLine("A felhasználónév nem tartalmazhat speciális karaktereket! (\\ / : * ? \" < > |)");
                        allowed = false;
                        break;
                    }
                }

                // 3. Duplikált felhasználónév kezelése
                string[] existingUsers = File.ReadAllLines("users.csv");
                foreach (string user in existingUsers)
                {
                    if (username == user.Split(';')[0])
                    {
                        Console.Clear();
                        Console.WriteLine("A felhasználó már létezik!");
                        allowed = false;
                        break;
                    }
                }
            } while (!allowed);

            // Jelszó bekérése
            Console.Write("Jelszó: ");
            string password = Console.ReadLine();

            // Születési dátum bekérése, ezt majd DateTime-mal kéne megoldani.
            Console.Write("Születési dátum (ÉÉÉÉ-HH-NN): ");
            string birthDate = Console.ReadLine();

            // Nem bekérése, csak Férfi vagy Nő lehet
            string gender;
            do
            {
                Console.Write("Neme (Férfi/Nő): ");
                gender = Console.ReadLine().ToLower();
            } while (gender != "férfi" && gender != "nő");

            // Felhasználó létrehozása és mentése a users.csv fájlba
            File.AppendAllText("users.csv", $"{username};{password};{gender};{birthDate}\n", Encoding.UTF8);
            File.Create($"{username}.csv").Close();
            File.Move($"{username}.csv", $"Users/{username}.csv");
            user = username;
            LoggedinMenu();
        }

        /// <summary>
        /// Bejelentkezési függvény, ellenőrzi a felhasználónevet és jelszót a users.csv fájl alapján.
        /// </summary>
        static void Login()
        {
            bool loggedIn;
            bool userExists;

            do
            {
                loggedIn = false;
                userExists = false;

                Console.Write("Felhasználónév: ");
                string username = Console.ReadLine();
                Console.Write("Jelszó: ");
                string password = Console.ReadLine();
                Console.Clear();

                // Felhasználónév és jelszó ellenőrzése
                string[] existingUsers = File.ReadAllLines("users.csv");
                foreach (string u in existingUsers)
                {
                    if (username == u.Split(';')[0]) // Ha megtalálta a felhasználónevet
                    {
                        userExists = true;
                        if (password == u.Split(';')[1]) // Ha a jelszó is stimmel

                        {
                            loggedIn = true;
                            user = username;
                            LoggedinMenu();
                            break;
                        }
                        else // Ha a jelszó nem stimmel
                        {
                            Console.Clear();
                            Console.WriteLine("Hibás jelszó!");
                            break;
                        }
                    }
                }

                if (!userExists) // Ha nem találta meg a felhasználónevet
                {
                    Console.Clear();
                    Console.WriteLine("A felhasználó nem létezik!");
                }
            } while (!loggedIn);
        }

        /// <summary>
        /// A felhasználó naplójának beolvasása.
        /// </summary>
        /// <param name="username">A felhasználó neve, aki be van jelentkezve.</param>
        static void ReadCSVFile(string username)
        {
            records = File.ReadAllLines($"Users/{username}.csv").ToList();            
        }

        /// <summary>
        /// A felhasználó naplójának kiírása a konzolra, egy táblázatszerű formában.
        /// </summary>
        static void DisplayRecords()
        {
            Console.WriteLine($"{user} Vérnyomásmérései");
            foreach (string record in records)
            {
                Console.Write($"| {record.Split(';')[0]} | {record.Split(';')[1]}\t| {record.Split(';')[2]}\t|\n");
            }
        }

        /// <summary>
        /// A felhasználó új mérését rögzíti a naplójába.
        /// </summary>
        /// <param name="record">A mérés, ezt a felhasználó adja meg.</param>
        /// <param name="username">A felhasználó neve, aki be van jelentkezve.</param>
        static void WriteCSVFile(string record, string username)
        {
            File.AppendAllText($"Users/{username}.csv",$"{username};{DateTime.Now};{record}\n", Encoding.UTF8);
        }
    }
}