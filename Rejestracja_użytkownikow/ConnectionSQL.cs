using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rejestracja_użytkownikow
{
    


    class ConnectionSQL
    {
        public static event Connection ConnectionEvent;

        //Łączenie z bazą danych
        public static System.Data.SqlClient.SqlConnection connectToDatabase(string connection_string)
        {
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(connection_string);
            try
            {
                conn.Open();
                ConnectionArgs args = new ConnectionArgs();
                args.message = "Połączono!";
                args.connect_to = conn.ServerVersion;
                ConnectionEvent += ConnectionSQLConnectionEvent;
                ConnectionEvent?.Invoke(args);
                ConnectionEvent -= ConnectionSQLConnectionEvent;
            }
            catch (Exception e)
            {
                ConnectionEvent += ErrorConnectionSQLConnectionEvent;
                ConnectionArgs args = new ConnectionArgs();
                args.message = e.Message;
                ConnectionEvent -= ErrorConnectionSQLConnectionEvent;
            }
            return conn;
        }

        private static void ConnectionSQLConnectionEvent(ConnectionArgs args)
        {
            System.Windows.Forms.MessageBox.Show("Connected to: " + args.connect_to, "Connection state", System.Windows.Forms.MessageBoxButtons.OK);
        }

        private static void ErrorConnectionSQLConnectionEvent(ConnectionArgs args)
        {
            System.Windows.Forms.MessageBox.Show(args.message, "Connection state", System.Windows.Forms.MessageBoxButtons.OK);
        }
        
    }
}
