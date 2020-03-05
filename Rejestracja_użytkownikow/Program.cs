using System;
using System.Security;
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
                                 @" AttachDbFilename = C:\Users\Benio\Projekty\VisualStudio\C#\BazaDanych\Rejestracja_użytkownikow\Database1.mdf;" +
                                 @" Integrated Security = True";


            string user_name = string.Empty;
            System.Data.SqlClient.SqlConnection conn =  ConnectionSQL.connect_to_database(conn_string);
            
            bool login_state = false;
            do
            {
                Console.WriteLine("If u don't have an account please type \"new user\"");
                Console.Write("Login: ");
                user_name = Console.ReadLine();
                if (user_name == "new user")
                {
                    bool register_state = false;
                    do
                    {
                        Console.Clear();
                        Users new_user = new Users();
                        try
                        {
                            register_state = DataExchangeSQL.New_user_register(new_user, conn);
                        }
                        catch (UserNameAlreadyExistException e)
                        {
                            MessageBox.Show(e.Message, "Stan rejestracji", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Console.Clear();
                        } Console.Clear();
                        
                    } while (!register_state);
                }
                else
                {
                    sd.Send(user_name);
                    Console.Write("Password: ");
                    SecureString password = Security.hidePassword();
                    try
                    {
                        DataExchangeSQL.GetAccesToAccount(user_name, password, conn);
                        login_state = true;
                    }
                    catch (BadPasswordException e)
                    {
                        MessageBox.Show(e.Message, "Stan logowania", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Console.Clear();
                    }

                }
            } while (!login_state);


            Console.Clear();
            Console.WriteLine("Witaj " + user_name + "\nCo chcialbys/chcialabys zrobić?\n1. Skrzynka pocztowa\n2. Serwer FTP");
            System.ConsoleKeyInfo keyInfo = Console.ReadKey();
            switch (keyInfo.Key)
            {
                case ConsoleKey.D1:
                    {
                        Console.Clear();
                        Console.WriteLine("\b\bE-mail");
                        break;
                    }
                case ConsoleKey.D2:
                    {
                        Console.Clear();
                        Console.WriteLine("\b\bSerwerFTP");
                        break;
                    }
            }
            




            Console.ReadKey();

        }

    }
}
