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

        // Regisztráció
        static void Register()
        {

        }

        // Bejelentkezés
        static void Login()
        {
            
        }
    }
}