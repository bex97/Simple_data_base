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

            NetworkStream ns = client.GetStream();

            try
            {
                ns.Write(data_ASCII, 0, data_ASCII.Length);
                //Poprawne wysłanie danych
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Stan wysyłania", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
           
        }

        public string ReciveString()
        {
            byte[] data_ASCII = new byte[256];
            NetworkStream ns = client.GetStream();
            ns.Read(data_ASCII, 0, data_ASCII.Length);

            return Convert.ToBase64String(data_ASCII);
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

        public string ReciveString()
        {
            byte[] data_ASCII = new byte[256];
            NetworkStream ns = client.GetStream();
            ns.Read(data_ASCII, 0, data_ASCII.Length);

            return Convert.ToBase64String(data_ASCII);
        }
    }
}
