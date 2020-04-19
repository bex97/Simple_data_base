using System;
using System.Security;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rejestracja_użytkownikow
{
    public class Users
    {
        string user_name;
        string password;
        string real_name;
        int age;
        string email;

        public Users()
        {
            Console.Write("User name: ");
            user_name = Console.ReadLine();
            Console.Write("Password: ");
            password = Security.ComputeSha256Hash(Security.hidePassword());
            Console.Write("Real name: ");
            real_name = Console.ReadLine();
            Console.Write("Age: ");
            age = Convert.ToInt32(Console.ReadLine());
            Console.Write("Email: ");
            email = Console.ReadLine();
        }

        public string getUserName()
        {
            return user_name;
        }
        public string getRealName()
        {
            return real_name;
        }
        public string getPassword()
        {
            return password;
        }
        public int getAge()
        {
                return age;
        }
        public string getEmail()
        {
            return email;
        }
    }
}
