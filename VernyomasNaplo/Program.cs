using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace VernyomasNaplo
{
    internal class Program
    {
        static int cPoint = 0;
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
                        WriteCentered("*** ÚJ MÉRÉS RÖGZÍTÉSE ***");
                        Console.ForegroundColor = ConsoleColor.White;

                        WriteCentered("Adja meg a vérnyomás értékét: ");
                        string record = ReadCentered("");
                        WriteCSVFile(record, user);

                        WriteCentered("Az adatokat sikeresen rögzítettük. Enterre tovább...");
                        Console.ReadLine();
                        Console.Clear();

                        ReadCSVFile(user);
                        DisplayRecords();

                        WriteCentered("Enterre tovább...");
                        Console.ReadLine();

                        break;

                    case 1: // Adatkiírás

                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        WriteCentered("*** ADATKIÍRÁS ***");
                        Console.ForegroundColor = ConsoleColor.White;

                        DisplayRecords();

                        WriteCentered("Enterre tovább...");
                        Console.ReadLine();

                        break;

                    case 2: // Kilépés
                        Console.Clear();
                        Console.Beep();
                        WriteCentered("Biztosan kilép? (i/n): ");
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
                WriteCentered("*** VÉRNYOMÁSNAPLÓ ***");
                Console.ForegroundColor = ConsoleColor.White;

                if (cPoint == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }
                WriteCentered("Új mérési adatok rögzítése");
                if (cPoint == 1)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }
                WriteCentered("Mérési adatok kiírása");
                if (cPoint == 2)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }
                WriteCentered("Kilépés");
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
                        WriteCentered("*** ÚJ MÉRÉS RÖGZÍTÉSE ***");
                        Console.ForegroundColor = ConsoleColor.White;

                        WriteCentered("Adja meg a vérnyomás értékét: ");
                        string record = ReadCentered("");
                        WriteCSVFile(record, user);

                        WriteCentered("Az adatokat sikeresen rögzítettük. Enterre tovább...");
                        Console.ReadLine();
                        Console.Clear();

                        ReadCSVFile(user);
                        DisplayRecords();

                        WriteCentered("Enterre tovább...");
                        Console.ReadLine();

                        break;

                    case 1: // Adatkiírás

                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        WriteCentered("*** ADATKIÍRÁS ***");
                        Console.ForegroundColor = ConsoleColor.White;

                        ReadCSVFile(user);
                        DisplayRecords();

                        WriteCentered("Enterre tovább...");
                        Console.ReadLine();

                        break;
                    
                    case 2: // Mérési adat módosítása

                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        WriteCentered("*** MÉRÉSI ADAT MÓDOSÍTÁSA ***");
                        Console.ForegroundColor = ConsoleColor.White;

                        targetUser = ReadCentered("Adja meg a módosítandó felhasználó nevét: ");

                        ReadCSVFile(targetUser);
                        DisplayRecordsMenu(targetUser);

                        break;

                    case 3: // Felhasználó módosítása/törlése

                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        WriteCentered("*** FELHASZNÁLÓ MÓDOSÍTÁSA/TÖRLÉSE ***");
                        Console.ForegroundColor = ConsoleColor.White;

                        DisplayUsersMenu();

                        break;

                    case 4: // Kilépés
                        Console.Clear();
                        Console.Beep();
                        WriteCentered("Biztosan kilép? (i/n): ");
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
                WriteCentered("*** VÉRNYOMÁSNAPLÓ (ADMIN) ***");
                Console.ForegroundColor = ConsoleColor.White;

                if (cPoint == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }
                WriteCentered("Új mérési adatok rögzítése");
                if (cPoint == 1)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }
                WriteCentered("Mérési adatok kiírása");
                if (cPoint == 2)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }
                WriteCentered("Mérési adat módosítása (ADMIN)");
                if (cPoint == 3)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }
                WriteCentered("Felhasználó módosítása/törlése (ADMIN)");
                if (cPoint == 4)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }
                WriteCentered("Kilépés");
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
                        WriteCentered("*** REGISZTRÁCIÓ ***");
                        Console.ForegroundColor = ConsoleColor.White;

                        Register();

                        break;//

                    case 1: // Bejelentkezés

                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        WriteCentered("*** BEJELENTKEZÉS ***");
                        Console.ForegroundColor = ConsoleColor.White;

                        Login();

                        break;

                    case 2: // Kilépés
                        Console.Clear();
                        Console.Beep();
                        WriteCentered("Biztosan kilép? (i/n): ");
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
                WriteCentered("*** VÉRNYOMÁSNAPLÓ ***");
                Console.ForegroundColor = ConsoleColor.White;

                if (currentPoint == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }
                WriteCentered("Regisztráció");
                if (currentPoint == 1)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }
                WriteCentered("Bejelentkezés");
                if (currentPoint == 2)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }
                WriteCentered("Kilépés");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        /// <summary>
        /// Megnézi, hogy létezik-e a users.csv fájl. Ha nem, létrehozza és regisztrációra irányít. Ha igen, megnézi, hogy üres-e. Ha üres, regisztrációra irányít, ha nem, felhozza a felhasználókezelő menüt.
        /// </summary>
        static void CheckUsersFile()
        {
            if (!File.Exists("users.csv")) // Nem --> Létrehozzuk a .csvt és a mappát is, az admin fiókkal. ez egy első indítás
            {
                string adminPassword = HashPassword("admin");
                File.AppendAllText("users.csv", $"admin;{adminPassword};férfi;1970-01-01\n", Encoding.UTF8);
                Directory.CreateDirectory("Users");
                File.Create("Users/admin.csv").Close();
            }
            
            LoginMenu();
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
                username = ReadCentered("Felhasználónév: ");

                // 1. Üres név kezelése
                if (string.IsNullOrEmpty(username))
                {
                    Console.Clear();
                    WriteCentered("A felhasználónév nem lehet üres!");
                    allowed = false;
                }

                // 2. Speciális karakterek kezelése
                foreach (char specialChar in specialChars)
                {
                    if (username.Contains(specialChar))
                    {
                        Console.Clear();
                        WriteCentered("A felhasználónév nem tartalmazhat speciális karaktereket! (\\ / : * ? \" < > |)");
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
                        WriteCentered("A felhasználó már létezik!");
                        allowed = false;
                        break;
                    }
                    else if (username.ToLower() == "admin")
                    {
                        Console.Clear();
                        WriteCentered("Az 'admin' felhasználónév fenntartott!");
                        allowed = false;
                        break;
                    }
                }
            } while (!allowed);

            // Jelszó bekérése és titkosítása (SHA256)
            string password = ReadCentered("Jelszó: ");
            string hashedPassword = HashPassword(password);

            // Születési dátum bekérése, ezt majd DateTime-mal kéne megoldani.
            string birthDate = ReadCentered("Születési dátum (ÉÉÉÉ-HH-NN): ");

            // Nem bekérése, csak Férfi vagy Nő lehet
            string gender;
            do
            {
                gender = ReadCentered("Neme (Férfi/Nő): ").ToLower();
            } while (gender != "férfi" && gender != "nő");

            // Felhasználó létrehozása és mentése a users.csv fájlba
            File.AppendAllText("users.csv", $"{username};{hashedPassword};{gender};{birthDate}\n", Encoding.UTF8);
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

                string username = ReadCentered("Felhasználónév: ");
                string password = ReadCentered("Jelszó: ");
                string hashedInput = HashPassword(password);
                Console.Clear();

                // Felhasználónév és jelszó ellenőrzése
                string[] existingUsers = File.ReadAllLines("users.csv");
                foreach (string u in existingUsers)
                {
                    if (username == u.Split(';')[0]) // Ha megtalálta a felhasználónevet
                    {
                        userExists = true;
                        if (hashedInput == u.Split(';')[1]) // Ha a jelszó is stimmel
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
                            WriteCentered("Hibás jelszó!");
                            break;
                        }
                    }
                }

                if (!userExists) // Ha nem találta meg a felhasználónevet
                {
                    Console.Clear();
                    WriteCentered("A felhasználó nem létezik!");
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
            if (records.Count == 0)
            {
                WriteCentered($"{user} felhasználónak nincsenek mérései.");
                return;
            }

            // Az ANSI kódok eltávolítása a hosszúság számításához, mert valamiért beleszámít.
            string StripAnsi(string text)
            {
                return System.Text.RegularExpressions.Regex.Replace(text, @"\u001b\[[0-9;]*m", "");
            }

            // Cellákban lévő szöveg középre igazítása
            string CenterText(string text, int width)
            {
                string stripped = StripAnsi(text);
                int padding = width - stripped.Length;
                int padLeft = padding / 2;
                int padRight = padding - padLeft;
                return new string(' ', padLeft) + text + new string(' ', padRight);
            }

            // Oszlopok szélességének meghatározása
            int nameWidth = "Felhasználó".Length;
            int dateWidth = "Dátum".Length;
            int bpWidth = "Mérés".Length;

            foreach (string record in records)
            {
                string[] parts = record.Split(';');
                if (parts.Length < 3) continue;

                nameWidth = Math.Max(nameWidth, parts[0].Length);
                dateWidth = Math.Max(dateWidth, parts[1].Length);

                string rating = RateBloodPressure(parts[2]);
                bpWidth = Math.Max(bpWidth, StripAnsi(rating).Length);
            }

            // A táblázat kereteinek kialakítása
            string top = $"┌{new string('─', nameWidth + 2)}┬{new string('─', dateWidth + 2)}┬{new string('─', bpWidth + 2)}┐";
            string separator = $"├{new string('─', nameWidth + 2)}┼{new string('─', dateWidth + 2)}┼{new string('─', bpWidth + 2)}┤";
            string bottom = $"└{new string('─', nameWidth + 2)}┴{new string('─', dateWidth + 2)}┴{new string('─', bpWidth + 2)}┘";

            // Fejléc
            string header = $"│ {CenterText("Dátum", nameWidth)} │ {CenterText("Vérnyomás", dateWidth)} │ {CenterText("Értékelés", bpWidth)} │";

            WriteCentered(top);
            WriteCentered(header);
            WriteCentered(separator);

            // Sorok
            foreach (string record in records)
            {
                string[] parts = record.Split(';');
                if (parts.Length < 3) 
                { 
                    continue; 
                }

                string name = CenterText(parts[0], nameWidth);
                string date = CenterText(parts[1], dateWidth);
                string bp = CenterText(RateBloodPressure(parts[2]), bpWidth);

                WriteCentered($"│ {name} │ {date} │ {bp} │");
            }

            // Lábléc
            WriteCentered(bottom);

            AnalyseRatios();
        }

        /// <summary>
        /// Adminisztrátor verziója az eredeti kiírónak. Ez egy menü, ahol kiválasztható egy mérés, majd módosítható a vérnyomás.
        /// </summary>
        /// <param name="targetUser">A felhasználó akinek mérései közül módosítani szertnénk.</param>
        static void DisplayRecordsMenu(string targetUser)
        {
            if (records.Count == 0)
            {
                WriteCentered($"{user} felhasználónak nincsenek mérései.");
                return;
            }

            // Az ANSI kódok eltávolítása a hosszúság számításához, mert valamiért beleszámít.
            string StripAnsi(string text)
            {
                return System.Text.RegularExpressions.Regex.Replace(text, @"\u001b\[[0-9;]*m", "");
            }

            // Cellákban lévő szöveg középre igazítása
            string CenterText(string text, int width)
            {
                int visibleLength = StripAnsi(text).Length;
                if (visibleLength >= width) return text;
                int leftPadding = (width - visibleLength) / 2;
                int rightPadding = width - visibleLength - leftPadding;
                return new string(' ', leftPadding) + text + new string(' ', rightPadding);
            }

            // Oszlopok szélességének meghatározása
            int nameWidth = "Dátum".Length;
            int dateWidth = "Vérnyomás".Length;
            int bpWidth = "Értékelés".Length;

            foreach (string record in records)
            {
                string[] parts = record.Split(';');
                if (parts.Length < 3) continue;

                nameWidth = Math.Max(nameWidth, parts[0].Length);
                dateWidth = Math.Max(dateWidth, parts[1].Length);
                bpWidth = Math.Max(bpWidth, StripAnsi(RateBloodPressure(parts[2])).Length);
            }

            int menuPoint = 0;

            /// <summary>
            /// A napló táblázati megjelenítése a konzolon, kiemelve a kiválasztott sort. 
            /// </summary>
            void ShowTable(int highlightIndex)
            {
                Console.Clear();

                // A táblázat kereteinek kialakítása
                string top = $"┌{new string('─', nameWidth + 2)}┬{new string('─', dateWidth + 2)}┬{new string('─', bpWidth + 2)}┐";
                string separator = $"├{new string('─', nameWidth + 2)}┼{new string('─', dateWidth + 2)}┼{new string('─', bpWidth + 2)}┤";
                string bottom = $"└{new string('─', nameWidth + 2)}┴{new string('─', dateWidth + 2)}┴{new string('─', bpWidth + 2)}┘";

                // Fejléc
                string header = $"│ {CenterText("Dátum", nameWidth)} │ {CenterText("Vérnyomás", dateWidth)} │ {CenterText("Értékelés", bpWidth)} │";

                WriteCentered(top);
                WriteCentered(header);
                WriteCentered(separator);

                // Sorok
                for (int i = 0; i < records.Count; i++)
                {
                    string[] parts = records[i].Split(';');
                    string name = CenterText(parts[0], nameWidth);
                    string date = CenterText(parts[1], dateWidth);
                    string bp = CenterText(RateBloodPressure(parts[2]), bpWidth);

                    if (i == highlightIndex)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    }

                    WriteCentered($"│ {name} │ {date} │ {bp} │");
                    Console.ResetColor();
                }

                WriteCentered(bottom);

                // Extra sor a kilépéshez
                string exitText = CenterText("Vissza a főmenübe", nameWidth + dateWidth + bpWidth + 6);
                if (highlightIndex == records.Count)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                WriteCentered($"{exitText}");
                Console.ResetColor();

            }

            do
            {
                bool selected = false;

                do
                {
                    ShowTable(menuPoint);

                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.UpArrow:
                            if (menuPoint > 0) menuPoint--;
                            break;
                        case ConsoleKey.DownArrow:
                            if (menuPoint < records.Count) menuPoint++;
                            break;
                        case ConsoleKey.Enter:
                            selected = true;
                            break;
                    }
                } while (!selected);

                if (menuPoint == records.Count)
                {
                    break;
                }

                // A kiválasztott mérés módosítása
                Console.Clear();
                WriteCentered($"Kiválasztott rekord: {records[menuPoint]}");
                WriteCentered("Adja meg az új vérnyomás értéket: ");
                string newRecord = Console.ReadLine();

                string[] recordParts = records[menuPoint].Split(';');
                recordParts[2] = newRecord; // Vérnyomás érték frissítése
                records[menuPoint] = string.Join(";", recordParts);

                File.WriteAllLines($"Users/{targetUser}.csv", records, Encoding.UTF8);

                WriteCentered("A rekord sikeresen módosítva. Enterre tovább...");
                Console.ReadLine();

            } while (true);
        }

        /// <summary>
        /// Ebben a menüben lehet az adminisztrátornak felhasználókat törölni.
        /// </summary>
        static void DisplayUsersMenu()
        {
            List<string> users = File.ReadAllLines("users.csv").ToList();

            if (users.Count == 0)
            {
                WriteCentered("Nincsenek felhasználók!");
                Console.ReadLine();
                return;
            }

            int menuPoint = 0;

            // Cellákban lévő szöveg középre igazítása
            string CenterText(string text, int width)
            {
                int leftPadding = (width - text.Length) / 2;
                int rightPadding = width - text.Length - leftPadding;
                if (leftPadding < 0) leftPadding = 0;
                if (rightPadding < 0) rightPadding = 0;
                return new string(' ', leftPadding) + text + new string(' ', rightPadding);
            }

            // Oszlopok szélességének meghatározása
            int nameWidth = "Felhasználó".Length;
            foreach (var user in users)
            {
                string username = user.Split(';')[0];
                if (username.Length > nameWidth) nameWidth = username.Length;
            }
            string exitText = "Vissza a főmenübe";
            if (exitText.Length > nameWidth) nameWidth = exitText.Length;

            void ShowTable(int highlightIndex)
            {
                Console.Clear();

                string top = $"┌{new string('─', nameWidth + 2)}┐";
                string bottom = $"└{new string('─', nameWidth + 2)}┘";
                string separator = $"├{new string('─', nameWidth + 2)}┤";
                string header = $"│ {CenterText("Felhasználó", nameWidth)} │";

                WriteCentered(top);
                WriteCentered(header);
                WriteCentered(separator);

                for (int i = 0; i < users.Count; i++)
                {
                    string username = users[i].Split(';')[0];
                    string row = $"│ {CenterText(username, nameWidth)} │";

                    if (i == highlightIndex)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    }

                    WriteCentered(row);
                    Console.ResetColor();
                }

                exitText = CenterText("Vissza a főmenübe", nameWidth);
                if (highlightIndex == users.Count)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                WriteCentered($"│ {exitText} │");
                Console.ResetColor();

                WriteCentered(bottom);
            }

            do
            {
                bool selected = false;
                do
                {
                    ShowTable(menuPoint);

                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.UpArrow:
                            if (menuPoint > 0) menuPoint--;
                            break;
                        case ConsoleKey.DownArrow:
                            if (menuPoint < users.Count) menuPoint++;
                            break;
                        case ConsoleKey.Enter:
                            selected = true;
                            break;
                    }
                } while (!selected);

                if (menuPoint == users.Count)
                {
                    break;
                }

                // A kiválasztott felhasználó törlése
                string targetUser = users[menuPoint].Split(';')[0];

                Console.Clear();
                WriteCentered($"Biztosan törli a felhasználót: {targetUser}? (i/n): ");
                if (Console.ReadKey(true).Key == ConsoleKey.I)
                {
                    users.RemoveAt(menuPoint);
                    File.WriteAllLines("users.csv", users, Encoding.UTF8);
                    File.Delete($"Users/{targetUser}.csv");

                    WriteCentered($"\nA felhasználó {targetUser} sikeresen törölve. Enterre tovább...");
                    Console.ReadLine();
                }
            } while (true);
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
                return $"\u001b[31m{record} (Magas)\u001b[0m"; 
            }
            else if (ratio < 1.4) 
            {
                return $"\u001b[94m{record} (Alacsony)\u001b[0m";
            }
            return $"\u001b[32m{record} (Jó)\u001b[0m";
        }

        /// <summary>
        /// Kiértékeli hogy a mérések hány százaléka volt jó, magas vagy alacsony, mellé adva azt is, hogy hány mérésből hány volt az adott kategóriába.
        /// </summary>
        static void AnalyseRatios()
        {
            double normal = 0;
            double high = 0; 
            double low = 0;

            foreach (string record in records)
            {
                double ratio = double.Parse(record.Split(';')[2].Split('/')[0]) / double.Parse(record.Split(';')[2].Split('/')[1]);

                if (ratio > 1.6) high++;
                else if (ratio < 1.4) low++;
                else normal++;
            }

            double sum = normal + high + low;
            WriteCentered($"\u001b[32m{Math.Round((normal / sum) * 100, 2)}% Jó ({sum}-ból {normal})\u001b[0m");
            WriteCentered($"\u001b[31m{Math.Round((high / sum) * 100, 2)}% Magas ({sum}-ból {high})\u001b[0m");
            WriteCentered($"\u001b[94m{Math.Round((low / sum) * 100, 2)}% Alacsony ({sum}-ból {low})\u001b[0m");
        }

        /// <summary>
        /// Középre író függvény.
        /// </summary>
        /// <param name="text">A szöveg amit középre kell írni.</param>
        static void WriteCentered(string text)
        {
            // Az ANSI kódok eltávolítása a hosszúság számításához, mert valamiért beleszámít.
            string StripAnsi(string Rtext)
            {
                return System.Text.RegularExpressions.Regex.Replace(Rtext, @"\u001b\[[0-9;]*m", "");
            }
            string temptext = StripAnsi(text);

            int width = Console.WindowWidth;
            int leftPadding = (width - temptext.Length) / 2;
            if (leftPadding < 0) leftPadding = 0;
            Console.WriteLine(new string(' ', leftPadding) + text);
        }

        /// <summary>
        /// Középen olvasó függvény.
        /// </summary>
        /// <param name="prompt">A szöveg ami után közepen kell bekérni</param>
        /// <returns>A beolvasott szöveg.</returns>
        static string ReadCentered(string prompt)
        {
            int width = Console.WindowWidth;
            int leftPadding = (width - prompt.Length) / 2;
            if (leftPadding < 0) leftPadding = 0;

            Console.Write(new string(' ', leftPadding) + prompt);

            // Kurzor pozíciójának beállítása.
            return Console.ReadLine();
        }

        /// <summary>
        /// Titkosítja a jelszót SHA256-tal.
        /// </summary>
        /// <param name="password">A titkosítatlan jelszó</param>
        /// <returns>A titkosított jelszót</returns>
        static string HashPassword(string password)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }
    }
}