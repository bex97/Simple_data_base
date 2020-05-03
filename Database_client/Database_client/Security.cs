using System;
using System.Security;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Net;
using System.Security.Cryptography;

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
                    password+=key.KeyChar;
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

        static public string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

    }
}
