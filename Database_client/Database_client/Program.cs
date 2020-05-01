using System;
using System.Windows.Forms;
using System.IO;

namespace Rejestracja_użytkownikow
{

    class Program
    {

        static void Main(string[] args)
        {
            string user_name;
            bool login_state = false;
            Client cl = new Client("127.0.0.1");
            if (cl.connect())
            {
                Console.WriteLine("Połączono!");
                do
                {
                    Console.WriteLine("If u don't have an account please type \"new user\"");
                    Console.Write("Login: ");
                    user_name = Console.ReadLine();
                    
                    try
                    {
                        //Wysyłanie loginu
                        cl.send(user_name);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                                    
                    if (user_name == "new user")
                    {
                        bool register_state = false;
                        do
                        {
                            Console.Clear();
                            Users new_user = new Users();
                            try
                            {
                                //Wysyłanie new user przez TCP/IP
                                cl.send(new_user);
                                //Sprawdzenie odpowiedzi z serwera
                                if (cl.recive() == "true") { Console.WriteLine("Udało się zalogować!"); register_state = true; }
                            }
                            catch (UserNameAlreadyExistException e)
                            {
                                //Obsługa zdarzenia
                                MessageBox.Show(e.Message, "Stan rejestracji", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                Console.Clear();
                            }
                            Console.Clear();

                        } while (!register_state);
                    }
                    else
                    {
                        //Ukrycie hasła
                        Console.Write("Password: ");
                        string hash_password = Security.ComputeSha256Hash(Security.hidePassword());
                        //string hash_password = Console.ReadLine();
                        try
                        {
                            //Wysyłanie hash_password przez TCP/IP
                            cl.send(hash_password);

                            //Sprawdzenie odpowiedzi z serwera
                            if (cl.recive() == "true") { Console.WriteLine("Udało się zalogować!"); login_state = true; }
                        }
                        catch (BadPasswordException e)
                        {
                            MessageBox.Show(e.Message, "Stan logowania u klienta", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                            Console.WriteLine("Podaj sciezke pliku: ");
                            string path = Console.ReadLine();
                            string[] name = path.Split('\\');
                            string file_name = name[name.Length - 1];
                            //1 opcja
                            FileStream fs = File.Open(@path, FileMode.Open);
                            BinaryReader binary_reader = new BinaryReader(fs);
                            cl.send(file_name, binary_reader, (int)fs.Length);
                            fs.Close();

                            break;
                        }
                }
            }
            else
            {
                Console.WriteLine("Blad polaczenia! Uruchom ponownie aplikacje...\nJesli problem nadal wystepuje skantaktuj sie z dostawca...");
            }
            




            Console.ReadKey();

        }

    }
}
