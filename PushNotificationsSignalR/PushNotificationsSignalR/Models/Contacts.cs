using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PushNotificationsSignalR.Models
{
    public class Contacts
    {
        public int ContactID { get; set; }
        public string ContactName { get; set; }
        public string ContactNo { get; set; }
        public DateTime AddedOn { get; set; }

        public List<Contacts> Read(DateTime afterDate)
        {
            string Conn = ConfigurationManager.ConnectionStrings["Conn"].ConnectionString;
            List<Contacts> _ResultList = new List<Contacts>();
            Contacts _Contact;
            try
            {
                using (SqlConnection conn=new SqlConnection(Conn))
                {
                    using (SqlCommand cmdRead=new SqlCommand())
                    {
                        cmdRead.Connection = conn;
                        cmdRead.CommandText = @"Select [ContactID],[ContactName],[ContactNo] from [dbo].[Contacts] where [AddedOn]>@AddedOn";
                        cmdRead.Parameters.AddWithValue("@AddedOn", afterDate);

                        //connection open
                        conn.Open();
                        using (SqlDataReader reader=cmdRead.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                _Contact = new Contacts();
                                _Contact.ContactID = Convert.ToInt32(reader["ContactID"]);
                                _Contact.ContactName = Convert.ToString(reader["ContactName"]);
                                _Contact.ContactNo = Convert.ToString(reader["ContactNo"]);
                                

                                _ResultList.Add(_Contact);
                            }
                        }
                    }
                }
                return _ResultList;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
    }
}