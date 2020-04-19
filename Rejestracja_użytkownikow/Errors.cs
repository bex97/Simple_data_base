using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rejestracja_użytkownikow
{
    class Errors : Exception
    {

    }

    public class BadPasswordException : Exception
    {
        string message;
        override public string Message
        {
            get
            {
                return message;
            }
        }
        public BadPasswordException(string s)
        {
            this.message = s;
        }
        
    }

    class UserNameAlreadyExistException : Exception
    {
        public new string Message = "Uzytkownik o podanej nazwie juz istnieje";
    }
}
