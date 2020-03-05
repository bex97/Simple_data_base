using System;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rejestracja_użytkownikow
{
    public class SendingData
    {
        string file_path;
        IPAddress ip_address;
        IPEndPoint ip_end_point;
        TcpClient client;

        SendingData(string ip)
        {
            ip_address = IPAddress.Parse(ip);
            ip_end_point = new IPEndPoint(ip_address, 50000);
        }

        public void Connect()
        {
            try
            {
                client = new TcpClient(ip_end_point);
                //Zdarzenie poprawnego połączenia
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Stan połączenia", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void Send(string message)
        {
            byte[] data_ASCII = System.Text.Encoding.ASCII.GetBytes(message);
            byte[] data_length = BitConverter.GetBytes(data_ASCII.Length);
            byte[] package = new byte[4 + message.Length];
            data_length.CopyTo(package, 0);
            data_ASCII.CopyTo(package, 4);

            NetworkStream ns = client.GetStream();
            int bytes_left = data_ASCII.Length, bytes_send = 0, buffer_size = 1024;

            ns.Write(package, 0, 4);
            while (bytes_left>0)
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

        public string Recive()
        {
            string message = string.Empty;
            byte[] data_ASCII = new byte[1024];
            byte[] size = new byte[4];
            NetworkStream ns = client.GetStream();
            

            int data_recived=0, data_size, buffer_size = 1024, next_pacakge_size;
            ns.Read(size, 0, 4);
            data_size = BitConverter.ToInt32(size, 0);
            while(data_size-data_recived>0)
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

        public void Connect()
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

        public void Send(string message)
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

        public string Recive()
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
