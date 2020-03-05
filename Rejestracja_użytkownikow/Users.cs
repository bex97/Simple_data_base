using System;
using System.Security;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rejestracja_użytkownikow
{
    class Users
    {
        string user_name;
        SecureString password;
        string real_name;
        int age;
        string email;

        public Users()
        {
            Console.Write("User name: ");
            user_name = Console.ReadLine();
            Console.Write("Password: ");
            password = Security.hidePassword();
            Console.Write("Real name: ");
            real_name = Console.ReadLine();
            Console.Write("Age: ");
            age = Convert.ToInt32(Console.ReadLine());
            Console.Write("Email: ");
            email = Console.ReadLine();
        }

        public string get_user_name()
        {
            return user_name;
        }
        public string get_real_name()
        {
            return real_name;
        }
        public SecureString get_password()
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
        public string get_email()
        {
            return this.email;
        }
    }
}
