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
        static double normal = 0;
        static double high = 0;
        static double low = 0;
        static string user;
        static string targetUser;
        static List<string> records;

        static void Main(string[] _)
        {
            CheckUsersFile();
        }

        /// <summary>
        /// A bejelentkezés után megnyíló menü kezelése. A felhasználó itt tudja megnézni méréseit és újat rögzíteni (illetve kilépni).
        /// </summary>
        static void LoggedinMenu()
        {
            cPoint = 0;
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

                        DisplayRecords();

                        Console.WriteLine("Enterre tovább...");
                        Console.ReadLine();

                        break;

                    case 2: // Kilépés
                        Console.Clear();
                        Console.Beep();
                        Console.Write("Biztosan kilép? (i/n): ");
                        if (Console.ReadKey().Key == ConsoleKey.I)
                        {
                            // Vissza a főmenübe
                            Console.Clear();
                            LoginMenu();
                            return;
                        }
                        else
                        {
                            cPoint = 0;
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
                    Console.ForegroundColor = ConsoleColor.Red;
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
        /// A LoggedinMenu() bővített változata, adminisztrátori jogosultságokkal. Itt a felhasználó módosíthat mérési adatokat és felhasználókat is.
        /// </summary>
        static void LoggedinAdminMenu()
        {
            cPoint = 0;
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
                            if (cPoint < 4)
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
                    
                    case 2: // Mérési adat módosítása

                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("*** MÉRÉSI ADAT MÓDOSÍTÁSA ***");
                        Console.ForegroundColor = ConsoleColor.White;

                        Console.Write("Adja meg a módosítandó felhasználó nevét: ");
                        targetUser = Console.ReadLine();

                        ReadCSVFile(targetUser);
                        DisplayRecordsMenu(targetUser);

                        break;

                    case 3: // Felhasználó módosítása/törlése

                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("*** FELHASZNÁLÓ MÓDOSÍTÁSA/TÖRLÉSE ***");
                        Console.ForegroundColor = ConsoleColor.White;

                        DisplayUsersMenu();

                        break;

                    case 4: // Kilépés
                        Console.Clear();
                        Console.Beep();
                        Console.Write("Biztosan kilép? (i/n): ");
                        if (Console.ReadKey().Key != ConsoleKey.I)
                        {
                            cPoint = 0;
                        }
                        break;
                }

            } while (cPoint != 4);
            void ShowMenu1(int cPoint)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("*** VÉRNYOMÁSNAPLÓ (ADMIN) ***");
                Console.ForegroundColor = ConsoleColor.White;

                if (cPoint == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine("Új mérési adatok rögzítése");
                if (cPoint == 1)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine("Mérési adatok kiírása");
                if (cPoint == 2)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine("Mérési adat módosítása (ADMIN)");
                if (cPoint == 3)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine("Felhasználó módosítása/törlése (ADMIN)");
                if (cPoint == 4)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
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
        /// Program indításakor megjelenő menü kezelése. A felhasználó itt tud választani a bejelentkezés és új felhasználó regisztrálása között (vagy kilépni)..
        /// </summary>
        static void LoginMenu()
        {
            cPoint = 0;
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
                    case 0: // Regisztráció

                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("*** REGISZTRÁCIÓ ***");
                        Console.ForegroundColor = ConsoleColor.White;

                        Register();

                        break;//

                    case 1: // Bejelentkezés

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
                    Console.ForegroundColor = ConsoleColor.Blue;
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
                    Console.ForegroundColor = ConsoleColor.Red;
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
            if (user == "admin")
            {
                LoggedinAdminMenu();
                
            }
            else
            {
                LoggedinMenu();
            }
               
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
                            ReadCSVFile(user);

                            if (username.ToLower() == "admin") // Ha admin
                            {
                                LoggedinAdminMenu();
                            }
                            else
                            {
                                LoggedinMenu();
                            }
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
                Console.Write($"| {record.Split(';')[0]} | {record.Split(';')[1]}\t| {RateBloodPressure(record.Split(';')[2])}\t|\n");
            }
            AnalyseRatios();
        }

        /// <summary>
        /// Adminisztrátor verziója az eredeti kiírónak. Ez egy menü, ahol kiválasztható egy mérés, majd módosítható a vérnyomás.
        /// </summary>
        /// <param name="targetUser">A felhasználó akinek mérései közül módosítani szertnénk.</param>
        static void DisplayRecordsMenu(string targetUser)
        {
            void ShowMenu3(int cPoint)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("*** VÉRNYOMÁSNAPLÓ ***");
                Console.ForegroundColor = ConsoleColor.White;

                for (int i = 0; i < records.Count; i++)
                {
                    if (i == cPoint)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    Console.WriteLine(records[i]);
                }

                if (cPoint == records.Count)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine("Vissza a főmenübe");
                Console.ForegroundColor = ConsoleColor.White;
            }

            do
            {
                bool selected = false;
                do
                {
                    ShowMenu3(cPoint);
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
                            if (cPoint < records.Count)
                            {
                                cPoint += 1;
                            }
                            break;
                    }
                } while (!selected);

                if (cPoint == records.Count)
                {
                    break;
                }

                Console.Clear();
                Console.WriteLine($"Kiválasztott rekord: {records[cPoint]}");
                Console.Write("Adja meg az új vérnyomás értéket: ");
                string newRecord = Console.ReadLine();

                // A kiválasztott mérés módosítása
                string[] recordParts = records[cPoint].Split(';');
                recordParts[2] = newRecord; // Csak a vérnyomás értékét módosítjuk
                records[cPoint] = string.Join(";", recordParts);

                // A módosított mérés visszaírása a fájlba
                File.WriteAllLines($"Users/{targetUser}.csv", records, Encoding.UTF8);

                Console.WriteLine("A rekord sikeresen módosítva. Enterre tovább...");
                Console.ReadLine();

            } while (cPoint != records.Count);
        }

        /// <summary>
        /// Ebben a menüben lehet az adminisztrátornak felhasználókat törölni.
        /// </summary>
        static void DisplayUsersMenu()
        {
            int userAmount;
            void ShowMenu4(int cPoint)
            {
                string[] users = File.ReadAllLines("users.csv");
                userAmount = users.Length;
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("*** VÉRNYOMÁSNAPLÓ ***");
                Console.ForegroundColor = ConsoleColor.White;

                for (int i = 0; i < userAmount; i++)
                {
                    if (i == cPoint)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    Console.WriteLine(users[i].Split(';')[0]);
                }

                if (cPoint == userAmount)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine("Vissza a főmenübe");
                Console.ForegroundColor = ConsoleColor.White;
            }

            do
            {
                bool selected = false;
                do
                {
                    ShowMenu4(cPoint);
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
                            if (cPoint < userAmount)
                            {
                                cPoint += 1;
                            }
                            break;
                    }
                } while (!selected);

                if (cPoint == userAmount)
                {
                    break;
                }

                List<string> users;
                users = File.ReadAllLines($"users.csv").ToList();

                targetUser = users[cPoint].Split(';')[0];
                Console.WriteLine(targetUser);

                for (int i = 0; i < users.Count; i++)
                {
                    if (users[i].Split(';')[0] == targetUser)
                    {
                        Console.WriteLine(targetUser);
                        users.RemoveAt(i);
                        File.WriteAllLines("users.csv", users, Encoding.UTF8);
                        File.Delete($"Users/{targetUser}.csv");
                        Console.WriteLine($"A felhasználó sikeresen törölve. Enterre tovább...");
                        Console.ReadLine();
                        break;
                    }
                }

            } while (cPoint != userAmount);
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

        /// <summary>
        /// Vérnyomás elemzése a mérés alapján.
        /// </summary>
        /// <param name="record">A mérés.</param>
        static string RateBloodPressure(string record)
        {
            double ratio = double.Parse(record.Split('/')[0]) / double.Parse(record.Split('/')[1]);
            if (ratio > 1.6)
            {
                high++;
                return $"\u001b[31m{record} (Magas)\u001b[0m";
            }
            else if (ratio < 1.4)
            {
                low++;
                return $"\u001b[94m{record} (Alacsony)\u001b[0m";
            }
            normal++;
            return $"\u001b[32m{record} (Jó)\u001b[0m\t";
        }

        /// <summary>
        /// Kiértékeli hogy a mérések hány százaléka volt jó, magas vagy alacsony, mellé adva azt is, hogy hány mérésből hány volt az adott kategóriába.
        /// </summary>
        static void AnalyseRatios()
        {
            double sum = normal + high + low;
            Console.WriteLine($"\u001b[32m{Math.Round((normal / sum) * 100, 2)}% Jó ({sum}-ból {normal})\u001b[0m\t");
            Console.WriteLine($"\u001b[31m{Math.Round((high / sum) * 100, 2)} % Alacsony (({sum}-ból {high})\u001b[0m\t");
            Console.WriteLine($"\u001b[94m{Math.Round((low / sum) * 100, 2)}% Alacsony ({sum}-ból {low})\u001b[0m\t");
            normal = 0;
            high = 0;
            low = 0;
        }
    }
}