using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace VernyomasNaplo
{
    internal class Program
    {
        static void Main(string[] _)
        {

        }

        /// <summary>
        /// Megnézi, hogy létezik-e a users.csv fájl, és ha igen, akkor üres-e, ez alapján dönt a regisztráció vagy bejelentkezés között. Mivel amikor ezt írtam még nem volt menüszerkezet, csak egy felhasználót lehet regisztrálni.
        /// </summary>
        static void CheckUsersFile()
        {
            if (!File.Exists("users.csv")) // Nem --> Létrehozzuk a .csvt és a mappát is, ez egy első indítás
            {
                File.Create("users.csv").Close();
                Directory.CreateDirectory("Users");
                // Regisztráció
            }

            else // Igen --> Ha üres: nincs felhasználó, regisztráció | van felhasználó, bejelentkezés
            {
                FileInfo users = new FileInfo("users.csv");
                if (users.Length > 0)
                {
                    // Bejelentkezés, majd ha lesz menü akkor az
                }
            }
        }

        /// <summary>
        /// Regisztrációs függvény, kezeli a felhasználónevekben a speciális karaktereket és hogy üres-e. Bekéri a jelszót, születési dátumot és nemetm, illetve lementi a felhasználó adatait és létrehozza az üres naplót.
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
        }

        /// <summary>
        /// Bejelentkezési függvény, ellenőrzi a felhasználónevet és jelszót a users.csv fájl alapján.
        /// </summary>
        static void Login()
        {
            bool loggedIn = false;
            bool userExists = false;

            do
            {
                Console.Write("Felhasználónév: ");
                string username = Console.ReadLine();
                Console.Write("Jelszó: ");
                string password = Console.ReadLine();
                Console.Clear();

                string[] existingUsers = File.ReadAllLines("users.csv");
                foreach (string user in existingUsers)
                {
                    if (username == user.Split(';')[0])
                    {
                        userExists = true;
                        if (password == user.Split(';')[1])
                        {
                            loggedIn = true;
                            break;
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("Hibás jelszó!");
                            break;
                        }
                    }
                }

                if (!userExists)
                {
                    Console.Clear();
                    Console.WriteLine("A felhasználó nem létezik!");
                }

            } while (!loggedIn);
        }
    }
}