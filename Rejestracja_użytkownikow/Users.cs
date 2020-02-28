using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rejestracja_użytkownikow
{
    class Users
    {
        string user_name;
        string password;
        string real_name;
        int age;

        public Users()
        {
            Console.Write("User name: ");
            user_name = Console.ReadLine();
            Console.Write("Password: ");
            password = Console.ReadLine();
            Console.Write("Real name: ");
            real_name = Console.ReadLine();
            Console.Write("Age: ");
            age = Convert.ToInt32(Console.ReadLine());
        }

        public string get_user_name()
        {
            return user_name;
        }
        public string get_real_name()
        {
            return real_name;
        }
        public string get_password()
        {
            return password;
        }
        public int get_age()
        {
            get:
            {
                return age;
            }
        }
    }
}
