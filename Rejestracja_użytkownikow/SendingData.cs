using System;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace Rejestracja_użytkownikow
{
    public class Serwer
    {
        string file_path;
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
            byte[] package = toPackage(message, e_flag);

            NetworkStream ns = client.GetStream();
            int bytes_left = package.Length-4, bytes_send = 0, buffer_size = 1024;

            ns.Write(package, 0, 4);
            while (bytes_left > 0)
            {
                try
                {
                    int next_package_size = bytes_left > buffer_size ? buffer_size : bytes_left;
                    ns.Write(package, bytes_send+4, next_package_size);
                    bytes_send += next_package_size;
                    bytes_left -= next_package_size;
                    if (bytes_send / message.Length * 100 > 9) Console.Write("\b\b\b\b\b\bData sent: {0}%", bytes_send / message.Length * 100);
                    else Console.Write("\b\b\b\bData sent: {0}%", bytes_send / message.Length * 100);
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
            int data_recived = 0, data_size, buffer_size = 1024, next_pacakge_size;
            string message = string.Empty;
            
            byte[] size = new byte[4];
            NetworkStream ns = client.GetStream();

            ns.Read(size, 0, 4);
            data_size = BitConverter.ToInt32(size, 0);
            byte[] data_ASCII = new byte[data_size];
            while (data_size - data_recived > 0)
            {
                try
                {
                    next_pacakge_size = data_size - data_recived > buffer_size ? buffer_size : data_size - data_recived;
                    ns.Read(data_ASCII, data_recived, next_pacakge_size);
                    data_recived += next_pacakge_size;
                    message += System.Text.Encoding.ASCII.GetString(data_ASCII);
                    if (data_recived / data_size * 100 > 9) Console.Write("\b\b\b\b\b\bData recived: {0}%", data_recived / data_size * 100);
                    else Console.Write("\b\b\b\bData recived: {0}%", data_recived / data_size * 100);
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

            byte[] size = new byte[4];
            NetworkStream ns = client.GetStream();

            ns.Read(size, 0, 4);
            data_size = BitConverter.ToInt32(size, 0);
            byte[] data_ASCII = new byte[data_size];
            while (data_size - data_recived > 0)
            {
                try
                {
                    next_pacakge_size = data_size - data_recived > buffer_size ? buffer_size : data_size - data_recived;
                    ns.Read(data_ASCII, data_recived, next_pacakge_size);
                    data_recived += next_pacakge_size;

                    if (data_recived / data_size * 100 > 9) Console.Write("\b\b\b\b\b\bData recived: {0}%", data_recived / data_size * 100);
                    else Console.Write("\b\b\b\bData recived: {0}%", data_recived / data_size * 100);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Stan wysyłania", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }
            }
            Console.WriteLine();

            FileStream f = File.Create("przeslany.txt");
            var fs = new StreamWriter(f);
            fs.WriteLine(System.Text.Encoding.ASCII.GetString(data_ASCII));

            f.Close();
        }

        public Users reciveUser()
        {
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
            while (data_size - data_recived > 0)
            {
                try
                {
                    next_pacakge_size = data_size - data_recived > buffer_size ? buffer_size : data_size - data_recived;
                    ns.Read(data, data_recived + 4, next_pacakge_size);
                    data_recived += next_pacakge_size;
                    message += System.Text.Encoding.ASCII.GetString(data);
                    if (data_recived / data_size * 100 > 9) Console.Write("\b\b\b\b\b\bData recived: {0}%", data_recived / data_size * 100);
                    else Console.Write("\b\b\b\bData recived: {0}%", data_recived / data_size * 100);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Stan wysyłania", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }
            }

            string[] split_data = message.Split('/');
            foreach (var str in split_data)
            {
                Console.WriteLine(str);
            }
            Users nu = new Users(split_data[0], Security.stringToSecureString(split_data[1]), split_data[2], Convert.ToInt32(split_data[3]), split_data[4]);

            Console.WriteLine();
            return nu;
        }
    }
}
