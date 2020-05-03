using System;
using System.Net;
using System.Security;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Rejestracja_użytkownikow
{
    class Program
    {
        static public void rejestracja(Serwer sw, System.Data.SqlClient.SqlConnection conn)
        {
            try
            {
                Users new_user = reciveUser(sw);
                DataExchangeSQL.newUserRegister(new_user, conn);
                sw.send("true");
            }
            catch (Exception)
            {
                
            }
        }

        static public void logowanie(Serwer sw, System.Data.SqlClient.SqlConnection conn, string user_name)
        {
            try
            {
                string password = sw.recive();
                DataExchangeSQL.GetAccesToAccount(user_name, password, conn);
                sw.send("true");
            }
            catch (Exception)
            {
                
            }
        }

        static public void sendMessage(Serwer sw)
        {
            string message = Console.ReadLine();
            sw.send(message);
        }

        static public string reciveMessage(Serwer sw)
        {
            return sw.recive();
        }

        static public Users reciveUser(Serwer sw)
        {
            return sw.reciveUser();
        }

        static public void reciveFile(Serwer sw)
        {
            sw.recive_file();
        }

        static public Serwer new_connection()
        {
            return new Serwer(IPAddress.Any);
        }

        static public void action(Serwer sw)
        {
            string conn_string = @" Data Source = (LocalDB)\MSSQLLocalDB; " +
                                @" AttachDbFilename = D:\Projekty\C#\Rejestracja_użytkownikow\Rejestracja_użytkownikow\Database1.mdf; " +
                                @" Integrated Security = True";
            System.Data.SqlClient.SqlConnection conn = ConnectionSQL.connectToDatabase(conn_string);


            /*bool login_state = false;
            do
            {
                string user_name = reciveMessage(sw);
                if (user_name == "new user")
                {
                    bool register_state = false;
                    do
                    {
                        try
                        {
                            //Rejestracja nowego użytkownika
                            
                        }
                        catch (UserNameAlreadyExistException e)
                        {

                        }
                        
                    } while (!register_state);
                }
                else
                {
                    try
                    {
                        //logowanie 
                    }
                    catch (BadPasswordException e)
                    {
                        
                    }

                }
            } while (!login_state);*/

            bool escape = false;
            do
            {
                string i = sw.recive();
                switch (Convert.ToInt32(i))
                {
                    case 1:
                        {
                            Console.WriteLine("Email");
                            break;
                        }
                    case 2:
                        {
                            reciveFile(sw);
                            break;
                        }
                    case 3:
                        {
                            escape = true;
                            break;
                        }
                    default:
                        break;
                }

            } while (!escape);
            Console.WriteLine("See you later!");
        }
    

        static void Main(string[] args)
        {
            while (true)
            {
                Serwer sw = new_connection();
                sw.connect();
                Thread t1 = new Thread(() => { action(sw); });
                t1.Start();
            }
        }

    }
}
