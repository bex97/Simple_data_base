using System;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace Rejestracja_użytkownikow
{
    public class Client
    {
        string file_path;
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
            byte[] data_ASCII = System.Text.Encoding.ASCII.GetBytes(message);
            byte[] data_length = BitConverter.GetBytes(data_ASCII.Length);
            byte[] package = new byte[4 + message.Length];
            data_length.CopyTo(package, 0);
            data_ASCII.CopyTo(package, 4);

            NetworkStream ns = client.GetStream();
            int bytes_left = data_ASCII.Length, bytes_send = 0, buffer_size = 1024;

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

        public void send(Users user)
        {
            byte[] user_name = System.Text.Encoding.ASCII.GetBytes(user.getUserName()+"/");
            byte[] password = System.Text.Encoding.ASCII.GetBytes(user.getPassword()+"/");
            byte[] real_name = System.Text.Encoding.ASCII.GetBytes(user.getRealName() + "/");
            byte[] age = BitConverter.GetBytes(user.getAge() + '/');
            byte[] email = System.Text.Encoding.ASCII.GetBytes(user.getEmail() + "/");

            byte[] data_length = BitConverter.GetBytes(user_name.Length + password.Length + real_name.Length + age.Length+email.Length+5);
            byte[] package = new byte[4 + data_length.Length];
            data_length.CopyTo(package, 0);
            user_name.CopyTo(package, 4);
            password.CopyTo(package, 4 + user_name.Length);
            real_name.CopyTo(package, 4 + user_name.Length+password.Length);
            age.CopyTo(package, 4 + user_name.Length + password.Length+real_name.Length);
            email.CopyTo(package, 4 + user_name.Length + password.Length + real_name.Length + age.Length);


            NetworkStream ns = client.GetStream();
            int bytes_left = data_length.Length, bytes_send = 0, buffer_size = 1024;

            ns.Write(package, 0, 4);
            while (bytes_left > 0)
            {
                try
                {
                    int next_package_size = bytes_left > buffer_size ? buffer_size : bytes_left;
                    ns.Write(package, bytes_send+4, next_package_size);
                    bytes_send += next_package_size;
                    bytes_left -= next_package_size;
                    if (bytes_send / data_length.Length * 100 > 9) Console.Write("\b\b\b\b\b\bData sent: {0}%", bytes_send / data_length.Length * 100);
                    else Console.Write("\b\b\b\bData sent: {0}%", bytes_send / data_length.Length * 100);
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

        public void send(BinaryReader reader, int size)
        {
            
            int buffer_size = 1024;
            byte[] data_ASCII = reader.ReadBytes(buffer_size);
            byte[] data_length = BitConverter.GetBytes(size);
            byte[] package = new byte[4 + size];
            data_length.CopyTo(package, 0);
            data_ASCII.CopyTo(package, 4);

            NetworkStream ns = client.GetStream();
            int bytes_left = size, bytes_send = 0;

            ns.Write(package, 0, 4);
            while (bytes_left > 0)
            {
                try
                {
                    int next_package_size = bytes_left > buffer_size ? buffer_size : bytes_left;
                    ns.Write(package, bytes_send, next_package_size);
                    bytes_send += next_package_size;
                    bytes_left -= next_package_size;
                    if (bytes_send / size * 100 > 9) Console.Write("/b/b/b/b/b/bData sent: {0}%", bytes_send / size * 100);
                    else Console.Write("/b/b/b/bData sent: {0}%", bytes_send / size * 100);
                    data_ASCII = reader.ReadBytes(next_package_size);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Stan wysyłania", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }
            }
            //Zdarzenie - wysłanie całego pakietu danych
        }

        public string recive()
        {
            string message1 = string.Empty;
            
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
            while (data_size - data_recived > 0)
            {
                try
                {
                    next_pacakge_size = data_size - data_recived > buffer_size ? buffer_size : data_size - data_recived;
                    ns.Read(data_ASCII, 0, next_pacakge_size);
                    data_recived += next_pacakge_size;
                    message1 += System.Text.Encoding.ASCII.GetString(data_ASCII);
                    if (data_recived / data_size * 100 > 9) Console.Write("\b\b\b\b\b\bData recived: {0}%", data_recived / data_size * 100);
                    else Console.Write("\b\b\b\bData recived: {0}%", data_recived / data_size * 100);
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

    public class RecivingData
    {
        string file_path;
        IPAddress ip_address;
        IPEndPoint ip_end_point;
        TcpListener listener;
        TcpClient client;

        RecivingData(string ip)
        {
            ip_address = IPAddress.Parse(ip);
            ip_end_point = new IPEndPoint(ip_address, 50000);
        }

        public void connect()
        {
            try
            {
                listener = new TcpListener(ip_end_point);
                listener.Start();
                client = listener.AcceptTcpClient();
                //Zdarzenie poprawnego połączenia
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Stan połączenia", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void send(string message)
        {
            byte[] data_ASCII = System.Text.Encoding.ASCII.GetBytes(message);
            byte[] data_length = BitConverter.GetBytes(data_ASCII.Length);
            byte[] package = new byte[4 + message.Length];
            data_length.CopyTo(package, 0);
            data_ASCII.CopyTo(package, 4);

            NetworkStream ns = client.GetStream();
            int bytes_left = data_ASCII.Length, bytes_send = 0, buffer_size = 1024;

            ns.Write(package, 0, 4);
            while (bytes_left > 0)
            {
                try
                {
                    int next_package_size = bytes_left > buffer_size ? buffer_size : bytes_left;
                    ns.Write(package, bytes_send, next_package_size);
                    bytes_send += next_package_size;
                    bytes_left -= next_package_size;
                    if (bytes_send / message.Length * 100 > 9) Console.Write("/b/b/b/b/b/bData sent: {0}%", bytes_send / message.Length * 100);
                    else Console.Write("/b/b/b/bData sent: {0}%", bytes_send / message.Length * 100);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Stan wysyłania", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }
            }
            //Zdarzenie - wysłanie całego pakietu danych
        }

        public string recive()
        {
            string message = string.Empty;
            byte[] data_ASCII = new byte[1024];
            byte[] size = new byte[4];
            NetworkStream ns = client.GetStream();


            int data_recived = 0, data_size, buffer_size = 1024, next_pacakge_size;
            ns.Read(size, 0, 4);
            data_size = BitConverter.ToInt32(size, 0);
            while (data_size - data_recived > 0)
            {
                try
                {
                    next_pacakge_size = data_size - data_recived > buffer_size ? buffer_size : data_size - data_recived;
                    ns.Read(data_ASCII, data_recived, next_pacakge_size);
                    data_recived += next_pacakge_size;
                    message += System.Text.Encoding.ASCII.GetString(data_ASCII);
                    if (data_recived / data_size * 100 > 9) Console.Write("/b/b/b/b/b/bData recived: {0}%", data_recived / data_size * 100);
                    else Console.Write("/b/b/b/bData recived: {0}%", data_recived / data_size * 100);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Stan wysyłania", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }
            }

            return message;
        }
    }
}
