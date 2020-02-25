using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rejestracja_użytkownikow
{
    //Delegaty
    public delegate void Connection(ConnectionArgs args);

    class ConnectionSQL
    {
        public static event Connection ConnectionEvent;

        public static System.Data.SqlClient.SqlConnection connect_to_database(string connection_string)
        {
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(connection_string);
            try
            {
                conn.Open();
                ConnectionArgs args = new ConnectionArgs();
                args.message = "Połączono!";
                args.connect_to = conn.ServerVersion;
                ConnectionEvent += ConnectionSQL_ConnectionEvent;
                ConnectionEvent?.Invoke(args);
                ConnectionEvent -= ConnectionSQL_ConnectionEvent;
            }
            catch (Exception e)
            {
                ConnectionEvent += ErrorConnectionSQL_ConnectionEvent;
                ConnectionArgs args = new ConnectionArgs();
                args.message = e.Message;
                ConnectionEvent -= ErrorConnectionSQL_ConnectionEvent;
            }
            return conn;
        }

        private static void ConnectionSQL_ConnectionEvent(ConnectionArgs args)
        {
            System.Windows.Forms.MessageBox.Show(args.message + "\nConnected to: " + args.connect_to, "Connection state", System.Windows.Forms.MessageBoxButtons.OK);
        }

        private static void ErrorConnectionSQL_ConnectionEvent(ConnectionArgs args)
        {
            System.Windows.Forms.MessageBox.Show(args.message, "Connection state", System.Windows.Forms.MessageBoxButtons.OK);
        }
        
    }
}
