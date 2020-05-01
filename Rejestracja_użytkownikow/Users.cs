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

        public Users(string un, string pass, string rn, int a, string e)
        {
            user_name = un;
            password = pass;
            real_name = rn;
            age = a;
            email = e;
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
            return this.email;
        }
        public void toString()
        {
            Console.WriteLine(user_name);
            Console.WriteLine(password);
            Console.WriteLine(real_name);
            Console.WriteLine(age);
            Console.WriteLine(email);
        }
    }
}
