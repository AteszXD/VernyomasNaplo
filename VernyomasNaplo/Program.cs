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

                        WriteCentered("Adja meg a vérnyomás értékét (például 120/80): ");

                        string record;
                        int systole;
                        int diastole;

                        while (true)
                        {
                            record = ReadCentered("");

                            // Hány '/' van az inputban
                            string[] parts = record.Split('/');
                            if (parts.Length != 2)
                            {
                                WriteCentered("Hibás formátum! Példa: 120/80");
                                continue;
                            }

                            // Ellenőrizzük, hogy tényleg számok-e
                            bool ok1 = int.TryParse(parts[0], out systole);
                            bool ok2 = int.TryParse(parts[1], out diastole);

                            if (!ok1 || !ok2)
                            {
                                WriteCentered("Mindkét értéknek számnak kell lennie!");
                                continue;
                            }

                            // Ellenőrizzük, hogy érvényes értékek-e
                            if (systole <= 0 || diastole <= 0)
                            {
                                WriteCentered("Mindkét értéknek 0 felett kell lennie!");
                                continue;
                            }

                            // Ha minden jó, mehetünk tovább
                            break;
                        }

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
                            if (cPoint < 5)
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

                        WriteCentered("Adja meg a vérnyomás értékét (például 120/80): ");

                        string record;
                        int systole;
                        int diastole;

                        while (true)
                        {
                            record = ReadCentered("");

                            // Hány '/' van az inputban
                            string[] parts = record.Split('/');
                            if (parts.Length != 2)
                            {
                                WriteCentered("Hibás formátum! Példa: 120/80");
                                continue;
                            }

                            // Ellenőrizzük, hogy tényleg számok-e
                            bool ok1 = int.TryParse(parts[0], out systole);
                            bool ok2 = int.TryParse(parts[1], out diastole);

                            if (!ok1 || !ok2)
                            {
                                WriteCentered("Mindkét értéknek számnak kell lennie!");
                                continue;
                            }

                            // Ellenőrizzük, hogy érvényes értékek-e
                            if (systole <= 0 || diastole <= 0)
                            {
                                WriteCentered("Mindkét értéknek 0 felett kell lennie!");
                                continue;
                            }

                            // Ha minden jó, mehetünk tovább
                            break;
                        }

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
                        WriteCentered("*** FELHASZNÁLÓ KIVÁLASZTÁSA ***");
                        Console.ForegroundColor = ConsoleColor.White;

                        string targetUser = DisplayUserSelectMenu();

                        if (targetUser != null)
                        {
                            ReadCSVFile(targetUser);
                            DisplayRecordsMenu(targetUser);
                        }
                        else
                        {
                            break;
                        }

                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        WriteCentered("*** MÉRÉSI ADAT MÓDOSÍTÁSA ***");
                        Console.ForegroundColor = ConsoleColor.White;

                        break;

                    case 3: // Felhasználó törlése

                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        WriteCentered("*** FELHASZNÁLÓ TÖRLÉSE ***");
                        Console.ForegroundColor = ConsoleColor.White;

                        DeleteUser();

                        break;

                    case 4: // Felhasználó jelszavának módosítása

                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        WriteCentered("*** JELSZÓ ÁTÍRÁS ***");
                        Console.ForegroundColor = ConsoleColor.White;

                        ResetUserPassword();

                        break;

                    case 5: // Kilépés
                        Console.Clear();
                        Console.Beep();
                        WriteCentered("Biztosan kilép? (i/n): ");
                        if (Console.ReadKey().Key != ConsoleKey.I)
                        {
                            cPoint = 0;
                        }
                        break;
                }

            } while (cPoint != 5);
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
                WriteCentered("Felhasználó törlése (ADMIN)");
                if (cPoint == 4)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }
                WriteCentered("Felhasználó jelszavának módosítása (ADMIN)");
                if (cPoint == 5)
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
            DateTime birthDate;

            while (true)
            {
                WriteCentered("Születési dátum (YYYY-MM-DD): ");
                string input = ReadCentered("");

                if (DateTime.TryParseExact(input, "yyyy-MM-dd",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None,
                    out birthDate))
                {
                    // Valid date
                    break;
                }
                else
                {
                    WriteCentered("Hibás dátumformátum! Példa: 2001-05-23");
                }
            }

            // Nem bekérése, csak Férfi vagy Nő lehet
            string gender;
            do
            {
                gender = ReadCentered("Neme (Férfi/Nő): ").ToLower();
            } while (gender != "férfi" && gender != "nő");

            // Felhasználó létrehozása és mentése a users.csv fájlba
            File.AppendAllText("users.csv", $"{username};{hashedPassword};{gender};{birthDate:yyyy-MM-dd}\n", Encoding.UTF8);
            File.Create($"{username}.csv").Close();
            File.Move($"{username}.csv", $"Users/{username}.csv");
            user = username;
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
            string header = $"│ {CenterText("Felhasználó", nameWidth)} │ {CenterText("Dátum", dateWidth)} │ {CenterText("Vérnyomás", bpWidth)} │";

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
                string newRecord = ReadCentered("Adja meg az új vérnyomás értéket: ");

                string[] recordParts = records[menuPoint].Split(';');
                recordParts[2] = newRecord; // Vérnyomás érték frissítése
                records[menuPoint] = string.Join(";", recordParts);

                File.WriteAllLines($"Users/{targetUser}.csv", records, Encoding.UTF8);

                WriteCentered("A rekord sikeresen módosítva. Enterre tovább...");
                Console.ReadLine();

            } while (true);
        }

        /// <summary>
        /// Itt az admin kiválasztja melyik felahsználó mérési adatait szeretné módosítani, vagy törölni.
        /// </summary>
        /// <returns>A kiválasztott felhasználót.</returns>
        static string DisplayUserSelectMenu()
        {
            List<string> users = File.ReadAllLines("users.csv").ToList();

            int menuPoint = 0;

            // Oszlopok szélességének meghatározása
            int nameWidth = "Felhasználó".Length;
            foreach (var u in users)
            {
                string username = u.Split(';')[0];
                nameWidth = Math.Max(nameWidth, StripAnsi(username).Length);
            }

            void ShowTable(int highlight)
            {
                Console.Clear();

                string top = $"┌{new string('─', nameWidth + 2)}┐";
                string header = $"│ {CenterText("Felhasználó", nameWidth)} │";
                string sep = $"├{new string('─', nameWidth + 2)}┤";
                string bottom = $"└{new string('─', nameWidth + 2)}┘";

                WriteCentered(top);
                WriteCentered(header);
                WriteCentered(sep);

                for (int i = 0; i < users.Count; i++)
                {
                    string username = users[i].Split(';')[0];
                    string row = $"│ {CenterText(username, nameWidth)} │";

                    if (i == highlight)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                    }

                    WriteCentered(row);
                }

                Console.ForegroundColor = ConsoleColor.White;
                WriteCentered(bottom);
            }

            while (true)
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
                            if (menuPoint < users.Count - 1) menuPoint++;
                            break;

                        case ConsoleKey.Enter:
                            selected = true;
                            break;
                    }

                } while (!selected);

                string chosenUser = users[menuPoint].Split(';')[0];
                return chosenUser;
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

        /// <summary>
        /// Vérnyomás elemzése a mérés alapján.
        /// </summary>
        /// <param name="record">A mérés.</param>
        static string RateBloodPressure(string record)
        {
            int systole = int.Parse(record.Split('/')[0]);
            int diastole = int.Parse(record.Split('/')[1]);

            if (systole > 135 || diastole > 85)
            // Magasnak mondjuk a vérnyomást, ha a szisztolés érték nagyobb mint 135, a diasztolés érték pedig nagyobb mint 85.
            {
                return $"\u001b[31m{record} (Magas)\u001b[0m"; 
            }
            else if (systole < 100 || diastole < 60)
            // Alacsonynak mondjuk a vérnyomást, ha a szisztolés érték kisebb mint 100, a diasztolés érték pedig kisebb mint 60.
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
                string tension = record.Split(';')[2];

                int systole = int.Parse(tension.Split('/')[0]);
                int diastole = int.Parse(tension.Split('/')[1]);

                if (systole > 135 || diastole > 85)
                // Magasnak mondjuk a vérnyomást, ha a szisztolés érték nagyobb mint 135, a diasztolés érték pedig nagyobb mint 85.
                {
                    high++;
                }
                else if (systole < 100 || diastole < 60)
                // Alacsonynak mondjuk a vérnyomást, ha a szisztolés érték kisebb mint 100, a diasztolés érték pedig kisebb mint 60.
                {
                    low++;
                }
                else
                {
                    normal++;
                }
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
        /// Az ANSI színkódok eltávolítása a margók számításához.
        /// </summary>
        /// <param name="text">A szöveg amivel számolni akarunk</param>
        /// <returns></returns>
        static string StripAnsi(string text)
        {
            return System.Text.RegularExpressions.Regex.Replace(text, @"\u001b\[[0-9;]*m", "");
        }

        /// <summary>
        /// Cellákban lévő szöveg középre igazítása
        /// </summary>
        /// <param name="text">A szöveg amit középre kell igazítani</param>
        /// <param name="width">A cella szélessége (Automatikus kiszámolva a szöveghossz alapján)</param>
        /// <returns></returns>
        static string CenterText(string text, int width)
        {
            int visibleLength = StripAnsi(text).Length;
            if (visibleLength >= width) return text;
            int leftPadding = (width - visibleLength) / 2;
            int rightPadding = width - visibleLength - leftPadding;
            return new string(' ', leftPadding) + text + new string(' ', rightPadding);
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

        /// <summary>
        /// Engedi az adminnak, hogy visszaállítsa egy felhasználó jelszavát.
        /// </summary>
        static void ResetUserPassword()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            WriteCentered("*** JELSZÓ VISSZAÁLLÍTÁSA (ADMIN) ***");
            Console.ForegroundColor = ConsoleColor.White;

            // Felhasználó kiválasztása
            string targetUser = DisplayUserSelectMenu();
            if (targetUser == null) return;

            WriteCentered($"Új jelszó megadása a következő felhasználónak: {targetUser}");
            string newPassword = ReadCentered("Új jelszó: ");

            string hashed = HashPassword(newPassword);

            // Összes felhasználó beolvasása
            List<string> entries = File.ReadAllLines("users.csv").ToList();

            // Jelszócsere
            for (int i = 0; i < entries.Count; i++)
            {
                string[] parts = entries[i].Split(';');

                if (parts[0] == targetUser)
                {
                    parts[1] = hashed;  // Új hash
                    entries[i] = string.Join(";", parts);
                    break;
                }
            }

            // Mentés
            File.WriteAllLines("users.csv", entries);

            Console.Clear();
            WriteCentered($"A(z) {targetUser} jelszava sikeresen visszaállítva!");
            WriteCentered("Enter a folytatáshoz...");
            Console.ReadLine();
        }

        /// <summary>
        /// A felhasználó törlését elvégző függvény.
        /// </summary>
        static void DeleteUser()
        {
            string targetUser = DisplayUserSelectMenu();
            while (targetUser == "admin")
            {
                Console.Clear();
                WriteCentered("Az admin nem törölheti magát! Enterre tovább...");
                Console.ReadLine();
                targetUser = DisplayUserSelectMenu();
            }

            Console.Clear();
            WriteCentered($"Biztosan törli a felhasználót: {targetUser}? (i/n): ");
            if (Console.ReadKey(true).Key == ConsoleKey.I)
            {
                List<string> users = File.ReadAllLines("users.csv").ToList();
                users.RemoveAll(u => u.Split(';')[0] == targetUser);
                File.WriteAllLines("users.csv", users, Encoding.UTF8);
                File.Delete($"Users/{targetUser}.csv");

                WriteCentered($"\nA felhasználó {targetUser} sikeresen törölve. Enterre tovább...");
                Console.ReadLine();
            }
        }
    }
}