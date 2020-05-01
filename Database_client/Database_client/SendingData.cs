using System;
using System.Threading;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace Rejestracja_użytkownikow
{
    public class Client
    {
        IPAddress ip_address;
        IPEndPoint ip_end_point;
        TcpClient client;

        public Client(string ip)
        {
            ip_address = IPAddress.Parse(ip);
            ip_end_point = new IPEndPoint(ip_address, 5000);
        }
        

        public bool connect()
        {
            try
            {
                client = new TcpClient();
                client.Connect(ip_end_point);
                return true;
                //Zdarzenie poprawnego połączenia
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Stan połączenia", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
        }

        public void send(string message)
        {
            int percentage = 0;
            byte[] data_ASCII = System.Text.Encoding.ASCII.GetBytes(message);
            byte[] data_length = BitConverter.GetBytes(data_ASCII.Length);
            byte[] package = new byte[4 + message.Length];
            data_length.CopyTo(package, 0);
            data_ASCII.CopyTo(package, 4);

            NetworkStream ns = client.GetStream();
            int bytes_left = data_ASCII.Length, bytes_send = 0, buffer_size = 1024;

            ns.Write(package, 0, 4);
            Console.Write("Data send: 0%");
            while (bytes_left > 0)
            {
                try
                {
                    int next_package_size = bytes_left > buffer_size ? buffer_size : bytes_left;
                    
                    ns.Write(package, bytes_send+4, next_package_size);
                    bytes_send += next_package_size;
                    bytes_left -= next_package_size;
                    if (percentage > 9)
                    {
                        Console.SetCursorPosition(Console.CursorLeft - 3, Console.CursorTop);
                    }
                    else
                    {
                        Console.SetCursorPosition(Console.CursorLeft - 2, Console.CursorTop);
                    }
                    Console.Write("{0}%", (int)((double)bytes_send / (double)message.Length * 100));
                    percentage = (int)((double)bytes_send / (double)message.Length * 100);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Stan wysyłania", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }
            }
            ns.Flush();
            Console.WriteLine();
            //Zdarzenie - wysłanie całego pakietu danych
        }

        public void send(Users user)
        {
            int percentage = 0;
           
            byte[] user_name = System.Text.Encoding.ASCII.GetBytes(user.getUserName()+"/");
            byte[] password = System.Text.Encoding.ASCII.GetBytes(user.getPassword()+"/");
            byte[] real_name = System.Text.Encoding.ASCII.GetBytes(user.getRealName() + "/");
            byte[] age = System.Text.Encoding.ASCII.GetBytes(user.getAge() + "/");
            byte[] email = System.Text.Encoding.ASCII.GetBytes(user.getEmail() + "/");

            int length = user_name.Length + password.Length + real_name.Length + age.Length + email.Length+5;

            byte[] data_length = BitConverter.GetBytes(length);
            byte[] package = new byte[4 + length];
            data_length.CopyTo(package, 0); 
            user_name.CopyTo(package, 4);
            password.CopyTo(package, 4 + user_name.Length);
            real_name.CopyTo(package, 4 + user_name.Length+password.Length);
            age.CopyTo(package, 4 + user_name.Length + password.Length+real_name.Length);
            email.CopyTo(package, 4 + user_name.Length + password.Length + real_name.Length + age.Length);


            NetworkStream ns = client.GetStream();
            int bytes_left = length;
            int bytes_send = 0, buffer_size = 1024;

            ns.Write(package, 0, 4);
            percentage = (int)((double)bytes_send / (double)length * 100);
            Console.Write("Data send: 0%");
            while (bytes_left > 0)
            {
                try
                {
                    int next_package_size = bytes_left > buffer_size ? buffer_size : bytes_left;
                    ns.Write(package, bytes_send+4, next_package_size);
                    bytes_send += next_package_size;
                    bytes_left -= next_package_size;
                    if (percentage > 9)
                    {
                        Console.SetCursorPosition(Console.CursorLeft - 3, Console.CursorTop);
                    }
                    else
                    {
                        Console.SetCursorPosition(Console.CursorLeft - 2, Console.CursorTop);
                    }
                    Console.Write("{0}%", (int)((double)bytes_send / (double)length * 100));
                    percentage = (int)((double)bytes_send / (double)length * 100);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Stan wysyłania", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }
            }
            Console.WriteLine();
            ns.Flush();
        }

        public void send(string file_name, BinaryReader reader, int size)
        {
            int percentage = 0;
            int buffer_size = 1024;
            byte[] name = System.Text.Encoding.ASCII.GetBytes(file_name);
            byte[] name_length = BitConverter.GetBytes(file_name.Length);
            byte[] data_ASCII = reader.ReadBytes(size);
            byte[] data_length = BitConverter.GetBytes(size);
            byte[] package = new byte[24 + name.Length + size];
            data_length.CopyTo(package, 0);
            name_length.CopyTo(package, 4);
            name.CopyTo(package, 24);
            data_ASCII.CopyTo(package, 24 + name.Length);

            NetworkStream ns = client.GetStream();
            int bytes_left = size+file_name.Length, bytes_send = 0;

            ns.Write(package, 0, 4);
            ns.Write(package, 4, 20);
            percentage =(int)((double)bytes_send / (double)(size+file_name.Length) * 100);
            Console.Write("Data send: 0%");
            while (bytes_left > 0)
            {
                try
                {
                    int next_package_size = bytes_left > buffer_size ? buffer_size : bytes_left;
                    ns.Write(package, bytes_send+24, next_package_size);
                    bytes_send += next_package_size;
                    bytes_left -= next_package_size;
                    if (percentage > 9)
                    {
                        Console.SetCursorPosition(Console.CursorLeft - 3, Console.CursorTop);
                    }
                    else
                    {
                        Console.SetCursorPosition(Console.CursorLeft - 2, Console.CursorTop);
                    }
                    Console.Write("{0}%", (int)((double)bytes_send / (double)(size + file_name.Length) * 100));
                    percentage = (int)((double)bytes_send / (double)(size + file_name.Length) * 100);
                    
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Stan wysyłania", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }
            }
            ns.Flush();
            //Zdarzenie - wysłanie całego pakietu danych
        }

        public string recive()
        {
            string message1 = string.Empty;
            int percentage = 0;
            
            byte[] size = new byte[4];
            NetworkStream ns = client.GetStream();


            int data_recived = 0, data_size, buffer_size = 1024, next_pacakge_size;
            bool exception_flag = false;

            ns.Read(size, 0, 4);
            data_size = BitConverter.ToInt32(size, 0);

            byte[] e_flag = new byte[1];
            ns.Read(e_flag, 0, 1);

            exception_flag = BitConverter.ToBoolean(e_flag, 0);

            byte[] data_ASCII = new byte[data_size];

            percentage = (int)((double)data_recived / (double)data_size * 100);
            Console.Write("Data recived: 0%");
            while (data_size - data_recived > 0)
            {
                try
                {
                    next_pacakge_size = data_size - data_recived > buffer_size ? buffer_size : data_size - data_recived;
                    ns.Read(data_ASCII, 0, next_pacakge_size);
                    data_recived += next_pacakge_size;
                    message1 += System.Text.Encoding.ASCII.GetString(data_ASCII);
                    if (percentage > 9)
                    {
                        Console.SetCursorPosition(Console.CursorLeft - 3, Console.CursorTop);
                    }
                    else
                    {
                        Console.SetCursorPosition(Console.CursorLeft - 2, Console.CursorTop);
                    }
                    Console.Write("{0}%", (int)((double)data_recived / (double)data_size * 100));
                    percentage = (int)((double)data_recived / (double)data_size * 100);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Stan wysyłania", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }
            }

            /*if (exception_flag)
            {
                Exception new_exception =  new Exception(message1);
                throw new_exception;
            }*/

            Console.WriteLine();
            return message1;
        }

    }
}
