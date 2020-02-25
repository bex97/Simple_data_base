using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rejestracja_użytkownikow
{

    class Program
    {
        

        static void Main(string[] args)
        {
            string conn_string = @" Data Source = (LocalDB)\MSSQLLocalDB; " +
                                 @" AttachDbFilename = D:\Projekty\C#\Rejestracja_użytkownikow\Rejestracja_użytkownikow\Database1.mdf;" +
                                 @" Integrated Security = True";
            ConnectionSQL.connect_to_database(conn_string);

            Console.ReadKey();

        }

    }
}
