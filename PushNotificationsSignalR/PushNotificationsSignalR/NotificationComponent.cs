using Microsoft.AspNet.SignalR;
using PushNotificationsSignalR.Models;
using PushNotificationsSignalR.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PushNotificationsSignalR
{
    public class NotificationComponent
    {
        public void RegisterNotification(DateTime CurrentTime)
        {
            string Constr = ConfigurationManager.ConnectionStrings["Conn"].ConnectionString;
            using (SqlConnection conn=new SqlConnection(Constr))
            {
                using (SqlCommand cmdRead=new SqlCommand())
                {
                    cmdRead.Connection = conn;
                    cmdRead.CommandText = @"Select [ContactID],[ContactName],[ContactNo] from [dbo].[Contacts] where [AddedOn]>@AddedOn";
                    cmdRead.Parameters.AddWithValue("@AddedOn", CurrentTime);
                    if(conn.State!=System.Data.ConnectionState.Open)
                    {
                        conn.Open();
                    }
                    cmdRead.Notification = null;
                    SqlDependency sqlDep = new SqlDependency(cmdRead);
                    sqlDep.OnChange += sqlDep_OnChange;

                    using (SqlDataReader reader=cmdRead.ExecuteReader())
                    {
                        //
                    }


                }
            }
        }

        private void sqlDep_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if(e.Type==SqlNotificationType.Change)
            {
                SqlDependency sqlDep = sender as SqlDependency;
                sqlDep.OnChange -= sqlDep_OnChange;

                var notificationHub = GlobalHost.ConnectionManager.GetHubContext<notificationHub>();
                notificationHub.Clients.All.Notify("added");

                RegisterNotification(DateTime.Now);

            }
        }

        public List<Contacts> GetContacts(DateTime afterDate)
        {
            Contacts _Contact = new Contacts();
            return _Contact.Read(afterDate);
        }

        public List<UserWithContact> GetUser(DateTime afterDate)
        {
            UserWithContact _User = new UserWithContact();
            return _User.Read(afterDate);
        }
    }
}