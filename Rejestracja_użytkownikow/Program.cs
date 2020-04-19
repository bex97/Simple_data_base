using System;
using System.Net;
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
            Serwer sw = new Serwer(IPAddress.Any);
            sw.connect();
            

            string conn_string = @" Data Source = (LocalDB)\MSSQLLocalDB; " +
                                 @" AttachDbFilename = C:\Users\Benio\Projekty\VisualStudio\C#\BazaDanych\Rejestracja_użytkownikow\Database1.mdf;" +
                                 @" Integrated Security = True";
            System.Data.SqlClient.SqlConnection conn =  ConnectionSQL.connectToDatabase(conn_string);

            
            bool login_state = false;
            do
            {
                string user_name = sw.recive();
                if (user_name == "new user")
                {
                    bool register_state = false;
                    do
                    {
                        Users new_user;
                        try
                        {
                            new_user = sw.reciveUser();
                            new_user.toString();
                            DataExchangeSQL.newUserRegister(new_user, conn);
                        }
                        catch (UserNameAlreadyExistException e)
                        {
                            MessageBox.Show(e.Message, "Stan rejestracji", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            sw.send(e.Message, true);
                        }
                        
                    } while (!register_state);
                }
                else
                {
                    SecureString password;
                    try
                    {
                        password = Security.stringToSecureString(sw.recive());
                        DataExchangeSQL.GetAccesToAccount(user_name, password, conn);
                        login_state = true;
                        sw.send("true", false);
                    }
                    catch (BadPasswordException e)
                    {
                        MessageBox.Show(e.Message, "Stan logowania", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        sw.send(e.Message, true);
                        Console.Clear();
                    }

                }
            } while (!login_state);

            sw.recive_file();
           
            Console.ReadKey();

        }

    }
}
