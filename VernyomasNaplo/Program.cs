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
        /// Megnézi, hogy létezik-e a Users.json fájl, és ha igen, akkor üres-e, ez alapján dönt a regisztráció vagy bejelentkezés között. Mivel amikor ezt írtam még nem volt menüszerkezet, csak egy felhasználót lehet regisztrálni.
        /// </summary>
        static void CheckUsersFile()
        {
            if (!File.Exists("Users.json")) // Nem --> Létrehozzuk a .jsont és a mappát is, ez egy első indítás
            {
                File.Create("Users.json").Close();
                Directory.CreateDirectory("Users");
                // Regisztráció
            }

            else // Igen --> Ha üres: nincs felhasználó, regisztráció | van felhasználó, bejelentkezés
            {
                FileInfo users = new FileInfo("Users.json");
                if (users.Length > 0)
                {
                    // Bejelentkezés
                }
            }
        }

        /// <summary>
        /// Regisztrációs függvény, kezeli a felhasználónevekben a speciális karaktereket és hogy üres-e. Bekéri a jelszót, születési dátumot és nemet.
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

                if (string.IsNullOrEmpty(username)) // Üres név kezelése
                {
                    Console.Clear();
                    Console.WriteLine("A felhasználónév nem lehet üres");
                    allowed = false;
                }

                foreach (char specialChar in specialChars) // Speciális karakterek
                {
                    if (username.Contains(specialChar))
                    {
                        Console.Clear();
                        Console.WriteLine("A felhasználónév nem tartalmazhat speciális karaktereket (\\ / : * ? \" < > |)");
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
                gender = Console.ReadLine();
            } while (gender != "Férfi" && gender != "Nő");

            // Felhasználó létrehozása és mentése a Users.json fájlba
            File.AppendAllText("users.csv", $"{username};{password};{gender};{birthDate}\n", Encoding.UTF8);
        }

        // Bejelentkezés
        static void Login()
        {
            
        }
    }
}