﻿using System;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace Rejestracja_użytkownikow
{
    public class Serwer
    {
        IPAddress ip_address;
        IPEndPoint ip_end_point;
        TcpListener listener;
        TcpClient client;

        public Serwer(string ip)
        {
            ip_address = IPAddress.Parse(ip);
            ip_end_point = new IPEndPoint(ip_address, 5000);
        }

        

        public Serwer(IPAddress ip)
        {
            ip_address = ip;
            ip_end_point = new IPEndPoint(ip_address, 5000);
        }

        public IPAddress GetIPAddress()
        {
            return ip_end_point.Address;
        }

        public void connect()
        {
            try
            {
                listener = new TcpListener(ip_end_point);
                Console.WriteLine("Waiting for connection...");
                listener.Start();
                client = listener.AcceptTcpClient();
                MessageBox.Show("Połączono z", "Stan połączenia", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //Zdarzenie poprawnego połączenia
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Stan połączenia", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private byte[] toPackage(string message, bool e_flag)
        {
            byte[] data_ASCII = System.Text.Encoding.ASCII.GetBytes(message);
            byte[] data_length = BitConverter.GetBytes(data_ASCII.Length);
            byte[] package = new byte[5 + message.Length];
            data_length.CopyTo(package, 0);
            bool exception_flag = e_flag;
            BitConverter.GetBytes(exception_flag).CopyTo(package, 4);
            data_ASCII.CopyTo(package, 5);
            return package;
        }

        public void send(string message, bool e_flag)
        {
            int percentage = 0;
            byte[] package = toPackage(message, e_flag);

            NetworkStream ns = client.GetStream();
            int bytes_left = package.Length-4, bytes_send = 0, buffer_size = 1024;

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
                    Console.Write("{0}%", (int)((double)bytes_send / ((double)message.Length + 1) * 100));
                    percentage = (int)((double)bytes_send / (double)(message.Length + 1) * 100);
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

        public string recive()
        {
            int percentage = 0;
            int data_recived = 0, data_size, buffer_size = 1024, next_pacakge_size;
            string message = string.Empty;
            
            byte[] size = new byte[4];
            NetworkStream ns = client.GetStream();

            ns.Read(size, 0, 4);
            data_size = BitConverter.ToInt32(size, 0);
            byte[] data_ASCII = new byte[data_size];
            Console.Write("Data recived: 0%");
            while (data_size - data_recived > 0)
            {
                try
                {
                    next_pacakge_size = data_size - data_recived > buffer_size ? buffer_size : data_size - data_recived;
                    ns.Read(data_ASCII, data_recived, next_pacakge_size);
                    data_recived += next_pacakge_size;
                    message += System.Text.Encoding.ASCII.GetString(data_ASCII);
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
            Console.WriteLine();
            return message;
        }

        public void recive_file()
        {
            int data_recived = 0, data_size, buffer_size = 1024, next_pacakge_size;

            int percentage = 0;
            int name_length = 0;

            byte[] size = new byte[4];
            byte[] name_size = new byte[20];
            NetworkStream ns = client.GetStream();

            ns.Read(size, 0, 4);
            data_size = BitConverter.ToInt32(size, 0);
            ns.Read(name_size, 0, 20);
            name_length = BitConverter.ToInt32(name_size, 0);

            byte[] data_ASCII = new byte[data_size];
            Console.Write("Data recived: 0%");
            while (data_size - data_recived > 0)
            {
                try
                {
                    next_pacakge_size = data_size - data_recived > buffer_size ? buffer_size : data_size - data_recived;
                    ns.Read(data_ASCII, data_recived, next_pacakge_size);
                    data_recived += next_pacakge_size;
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
            Console.WriteLine();

            string name = System.Text.Encoding.ASCII.GetString(data_ASCII, 0, name_length);
            FileStream file = File.Create(@"C:\Users\Benio\Desktop\Server_files\"+name);
            BinaryWriter bw = new BinaryWriter(file);
            bw.Write(data_ASCII, name_length, data_ASCII.Length-name_length);

            /*
            FileStream f = File.Create("przeslany.txt");
            var fs = new StreamWriter(f);
            Console.WriteLine(data_ASCII.Length + System.Text.Encoding.ASCII.GetString(data_ASCII).Length);
            fs.Write(System.Text.Encoding.ASCII.GetString(data_ASCII));
            */

            file.Close();
        }

        public Users reciveUser()
        {
            int percentage = 0;
            int data_recived = 0, data_size, buffer_size = 1024, next_pacakge_size;
            string message = string.Empty;
            byte[] size = new byte[4];
            NetworkStream ns = client.GetStream();


            try
            {
                ns.Read(size, 0, 4);
            }
            catch (Exception e)
            {
                //Obsługa błędu
            }
            
            data_size = BitConverter.ToInt32(size, 0);
            byte[] data = new byte[data_size];
            Console.Write("Data recived: 0%");
            percentage = (int)((double)data_recived / (double)data_size * 100);
            while (data_size - data_recived > 0)
            {
                try
                {
                    next_pacakge_size = data_size - data_recived > buffer_size ? buffer_size : data_size - data_recived;
                    ns.Read(data, data_recived, next_pacakge_size);
                    data_recived += next_pacakge_size;
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

            
            message = System.Text.Encoding.ASCII.GetString(data);
            string[] split_data = message.Split('/');
            Users nu = new Users(split_data[0], split_data[1], split_data[2], Convert.ToInt32(split_data[3]), split_data[4]);

            Console.WriteLine();
            return nu;
        }
    }
}
