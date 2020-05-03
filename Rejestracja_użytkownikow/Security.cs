using System;
using System.Security;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Net;

namespace Rejestracja_użytkownikow
{
    class Security
    {
        static public string hidePassword()
        {
            ConsoleKeyInfo key;
            string password = "";
            do
            {
                key = Console.ReadKey(true);
                if (!char.IsControl(key.KeyChar))
                {
                    password+=(key.KeyChar);
                    Console.Write("*");
                }
                if(key.Key == ConsoleKey.Backspace && password.Length>0)
                {
                    password = password.Remove(password.Length - 1);
                    Console.Write("\b \b");
                }

            } while (key.Key != ConsoleKey.Enter);
            Console.WriteLine();
            return password;
        }
        
    }
}
