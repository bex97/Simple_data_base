using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rejestracja_użytkownikow
{
    class DataExchangeSQL
    {
        public static event LoginState Login;
        static public bool GetAccesToAccount(string user_name, string password, System.Data.SqlClient.SqlConnection conn)
        {
            
            string get_info_string = "SELECT Password From Users WHERE UserName='"+user_name+"'";
            var cmd = new System.Data.SqlClient.SqlCommand(get_info_string, conn);
            var result = cmd.ExecuteReader();
            if (result.HasRows && result.GetValue(0).ToString() == password)
            {
                 Login += Succesfull_login;
                 OnLogin(new BadPasswordException(""));
                 Login -= Succesfull_login;
            }
            else
            {
                throw new BadPasswordException("Podano zły login lub hasło");
            }
            return false;
        }

        static public void OnLogin(BadPasswordException e)
        {
            Login?.Invoke(e);
        }
        static public void Succesfull_login(BadPasswordException e)
        {
            MessageBox.Show("Zalogowałeś się!", "Stan logowania", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        static public void NotSuccesfull_login(BadPasswordException e)
        {
            MessageBox.Show(e.Message, "Stan logowania", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

    }
}
