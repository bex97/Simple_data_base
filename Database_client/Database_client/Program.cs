using System;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace Rejestracja_użytkownikow
{

    class Program
    {
        static public void rejestracja(Client cl)
        {
            Console.Clear();
            Users new_user = new Users();
            try
            {
                //Wysyłanie new user przez TCP/IP
                cl.send(new_user);

                //Sprawdzenie odpowiedzi z serwera
                if (cl.recive() == "true") Console.WriteLine("Udalo sie zarejestrowac!");
            }
            catch (UserNameAlreadyExistException e)
            {
                //Obsługa zdarzenia
            }
            Console.Clear();
        }

        static public void logowanie(Client cl)
        {
            Console.Write("Password: ");
            string hash_password = Security.ComputeSha256Hash(Security.hidePassword());
            try
            {
                //Wysyłanie hash_password przez TCP/IP
                cl.send(hash_password);

                //Sprawdzenie odpowiedzi z serwera
                if (cl.recive() == "true") Console.WriteLine("Udało się zalogować!");
            }
            catch (BadPasswordException e)
            {

            }
        }
        
        static public void sendText(Client cl)
        {
            string message = Console.ReadLine();
            try
            {
                cl.send(message);
            }
            catch (Exception)
            {
                //Błąd wysyłania wiadomości
            }
        }
        
        static public void sendFile(Client cl)
        {
            Console.Clear();
            Console.WriteLine("\b\bSerwerFTP");
            Console.WriteLine("Podaj sciezke pliku: ");
            string path = Console.ReadLine();
            Console.WriteLine(path);
            string[] name = path.Split('\\');
            string file_name = name[name.Length - 1];
            //1 opcja
            FileStream fs = File.Open(@path, FileMode.Open);
            BinaryReader binary_reader = new BinaryReader(fs);
            cl.send(file_name, binary_reader, (int)fs.Length);
            fs.Close();
        }
        

        static void Main(string[] args)
        {
            string user_name;
            bool login_state = false;
            Client cl = new Client("127.0.0.1");

            if (cl.connect())
            {
                Console.WriteLine("Połączono!");
                /*do
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
                        //Rejsetracja
                    }
                    else
                    {
                        //Logowanie
                    }
                } while (!login_state);*/

                bool action = false;
                do
                {
                    Console.Clear();
                    Console.WriteLine("Witaj Benio!\nCo chcialbys zrobić?\n1. Skrzynka pocztowa\n2. Serwer FTP\n3. Esc aby wyjść i zakończyć");
                    System.ConsoleKeyInfo keyInfo = Console.ReadKey();
                    switch (keyInfo.Key)
                    {
                        case ConsoleKey.D1:
                            {
                                cl.send("1");
                                Console.Clear();
                                Console.WriteLine("\b\bE-mail");
                                break;
                            }
                        case ConsoleKey.D2:
                            {
                                do
                                {
                                    cl.send("2");
                                    sendFile(cl);
                                } while (Console.ReadKey().Key != ConsoleKey.Escape);
                                break;
                            }
                        case ConsoleKey.Escape:
                            {
                                cl.send("3");
                                Console.WriteLine("Dzieki za odwiedziny! Zapraszamy ponownie!");
                                action = true;
                                break;
                            }
                    }
                } while (!action);
            }
            else
            {
                Console.WriteLine("Blad polaczenia! Uruchom ponownie aplikacje...\nJesli problem nadal wystepuje skantaktuj sie z dostawca...");
            }
        }

    }
}
