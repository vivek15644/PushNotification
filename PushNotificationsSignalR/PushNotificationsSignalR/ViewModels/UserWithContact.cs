using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PushNotificationsSignalR.Models;
using System.Data.SqlClient;
using PushNotificationsSignalR.Utility;

namespace PushNotificationsSignalR.ViewModels
{
    public class UserWithContact
    {
        public Contacts Contact { get; set; } 
        public User User { get; set; }


        /// <summary>
        /// read all the user with their contact
        /// </summary>
        /// <param name="afterDate"></param>
        /// <returns></returns>
        public List<UserWithContact> Read(DateTime afterDate)
        {
            List<UserWithContact> _Result = new List<UserWithContact>();
            UserWithContact _User;
            try
            {
                using (SqlConnection conn=DatabaseHandler.GetConnection())
                {
                    using (SqlCommand cmdRead=new SqlCommand())
                    {
                        cmdRead.Connection = conn;
                        cmdRead.CommandText = "USP_GetUserWithContact";
                        cmdRead.CommandType = System.Data.CommandType.StoredProcedure;
                        cmdRead.Parameters.AddWithValue("@afterdate", afterDate);

                        //connection open
                        conn.Open();

                        SqlDataReader reader = cmdRead.ExecuteReader();
                        while (reader.Read())
                        {
                            _User = new UserWithContact();
                            _User.User = new User();
                            _User.Contact = new Contacts();
                            _User.User.Id = Convert.ToInt32(reader["UserId"]);
                            _User.User.Name = Convert.ToString(reader["Name"]);
                            _User.User.City = Convert.ToString(reader["City"]);

                            _User.Contact.ContactID = Convert.ToInt32(reader["ContactID"]);
                            _User.Contact.ContactName = _User.User.Name;
                            _User.Contact.ContactNo = Convert.ToString(reader["ContactNo"]);

                            _Result.Add(_User);
                        }

                        //connection close
                        conn.Close();
                    }
                }
                return _Result;
            }
            catch (Exception ex)
            {
                return null;
                
            }
        }
    }
}