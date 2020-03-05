﻿using System;
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
            com.Parameters.AddWithValue("@UserName", new_user.get_user_name());
            com.Parameters.AddWithValue("@Password", new NetworkCredential("", new_user.get_password()).Password);
            com.Parameters.AddWithValue("@RealName", new_user.get_real_name());
            com.Parameters.AddWithValue("@Age", new_user.get_age());
            com.Parameters.AddWithValue("@Email", new_user.get_email());
            try
            {
                com.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message+ "\nSpróbuj ponownie", "Stan rejestracji", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return false;
        }

        static public bool insertIntoUsers_infoTable(Users new_user, System.Data.SqlClient.SqlConnection conn)
        {
            string insert2 = "INSERT INTO Users (UserName, Password) " +
                                 "VALUES (@UserName, @Password)";
            var com = new System.Data.SqlClient.SqlCommand(insert2, conn);
            com.Parameters.AddWithValue("@UserName", new_user.get_user_name());
            com.Parameters.AddWithValue("@Password", new NetworkCredential("", new_user.get_password()).Password);
            try
            {
                com.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message + "\nSpróbuj ponownie", "Stan rejestracji", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return false;
        }

        static public bool GetAccesToAccount(string user_name, SecureString password, System.Data.SqlClient.SqlConnection conn)
        {
            
            string get_info_string = "SELECT Password From Users WHERE UserName='"+user_name+"'";
            var cmd = new System.Data.SqlClient.SqlCommand(get_info_string, conn);
            var result = cmd.ExecuteReader();
            if (result.HasRows) 
            {
                result.Read();
                if(Security.IsEqual(new NetworkCredential("",result.GetValue(0).ToString()).SecurePassword,password))
                {
                    
                    Login += Succesfull_login;
                    OnLogin();
                    Login -= Succesfull_login;
                    result.Close();
                    return true;
                }
                else
                {
                    result.Close();
                    throw new BadPasswordException("Podano zły login lub hasło");
                }
            }
            else
            {
                result.Close();
                throw new BadPasswordException("Podano zły login lub hasło");
            }
        }

        static public bool New_user_register(Users new_user, System.Data.SqlClient.SqlConnection conn)
        {
            string get_info_string = "SELECT UserName From Users WHERE UserName='" + new_user.get_user_name() + "'";
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
                if (!insertIntoUsers_infoTable(new_user, conn)) return false;

                Register += Succesfull_register;
                OnRegister();
                Register -= Succesfull_register;
                return true;
            }
        }

        static public void OnLogin()
        {
            Login?.Invoke();
        }

        static public void OnRegister()
        {
            Register?.Invoke();
        }

        static public void Succesfull_login()
        {
            MessageBox.Show("Zalogowałeś się!", "Stan logowania", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        static public void Succesfull_register()
        {
            MessageBox.Show("Zarejestrowałeś się!\nMożesz się teraz zalogować!", "Stan rejstracji", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

    }
}