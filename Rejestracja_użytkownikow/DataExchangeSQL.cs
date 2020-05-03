using System;
using System.Security;
using System.Net;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rejestracja_użytkownikow
{
    class DataExchangeSQL
    {
        static public event LoginState Login;
        static public event RegistrationState Register;

        static public bool insertIntoUsersTable(Users new_user, System.Data.SqlClient.SqlConnection conn)
        {
            string insert = "INSERT INTO Users_info (UserName, Password, RealName, Age, Email) " +
                                "VALUES (@UserName, @Password, @RealName, @Age, @Email)";

            var com = new System.Data.SqlClient.SqlCommand(insert, conn);
            com.Parameters.AddWithValue("@UserName", new_user.getUserName());
            com.Parameters.AddWithValue("@Password", new_user.getPassword());
            com.Parameters.AddWithValue("@RealName", new_user.getRealName());
            com.Parameters.AddWithValue("@Age", new_user.getAge());
            com.Parameters.AddWithValue("@Email", new_user.getEmail());
            try
            {
                com.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        static public bool insertIntoUsersInfoTable(Users new_user, System.Data.SqlClient.SqlConnection conn)
        {
            string insert2 = "INSERT INTO Users (UserName, Password) " +
                                 "VALUES (@UserName, @Password)";
            var com = new System.Data.SqlClient.SqlCommand(insert2, conn);
            com.Parameters.AddWithValue("@UserName", new_user.getUserName());
            com.Parameters.AddWithValue("@Password", new_user.getPassword());
            try
            {
                com.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        static public bool GetAccesToAccount(string user_name, string password, System.Data.SqlClient.SqlConnection conn)
        {
            
            string get_info_string = "SELECT Password From Users WHERE UserName='"+user_name+"'";
            var cmd = new System.Data.SqlClient.SqlCommand(get_info_string, conn);
            var result = cmd.ExecuteReader();
            if (result.HasRows) 
            {
                result.Read();
                if(result.GetValue(0).ToString().Equals(password))
                {
                    result.Close();
                    return true;
                }
                else
                {
                    result.Close();
                    throw new BadPasswordException("Podano złe hasło");
                }
            }
            else
            {
                result.Close();
                throw new BadPasswordException("Podano zły login");
            }
        }

        static public bool newUserRegister(Users new_user, System.Data.SqlClient.SqlConnection conn)
        {
            string get_info_string = "SELECT UserName From Users WHERE UserName='" + new_user.getUserName() + "'";
            var cmd = new System.Data.SqlClient.SqlCommand(get_info_string, conn);
            System.Data.SqlClient.SqlDataReader result = cmd.ExecuteReader();

            if (result.HasRows)
            {
                result.Close();
                throw new UserNameAlreadyExistException();
            }
            else
            {
                result.Close();
                if (!insertIntoUsersTable(new_user, conn)) return false;
                if (!insertIntoUsersInfoTable(new_user, conn)) return false;
                
                return true;
            }
        }
    }
}
