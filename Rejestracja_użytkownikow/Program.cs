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
            System.Data.SqlClient.SqlConnection conn =  ConnectionSQL.connect_to_database(conn_string);

            Console.WriteLine("If u don't have an account please type \"new user\"");
            Console.Write("Login: ");
            string user_name = Console.ReadLine();
            if (user_name == "new user")
            {
                Users new_user = new Users();
            }
            else
            {
                Console.Write("Password: ");
                string password = Console.ReadLine();
                try
                {
                    DataExchangeSQL.GetAccesToAccount(user_name, password, conn);
                }
                catch (BadPasswordException e)
                {
                    DataExchangeSQL.Login += DataExchangeSQL.NotSuccesfull_login;
                    DataExchangeSQL.OnLogin(e);
                }
                
            }
            



            Console.ReadKey();

        }

    }
}
