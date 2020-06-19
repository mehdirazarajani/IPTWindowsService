using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace IPTWindowsService
{
    class LogSender
    {

        private static List<SearchLogModel> getSearchLog()
        {
            List<SearchLogModel> searchLogs = new List<SearchLogModel>();
            SqlConnection dbConnection = new SqlConnection(ConfigurationManager.AppSettings["SqlDBConn"].ToString());
            try
            {
                string query = ConfigurationManager.AppSettings["SelectQuery"].ToString();
                using (SqlCommand command = new SqlCommand(query, dbConnection))
                {
                    dbConnection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        searchLogs.Add(new SearchLogModel(reader));
                    }
                }
            }
            catch (Exception e)
            {
                return searchLogs;
            }
            return searchLogs;
        }

        private static void markLogsDone()
        {
            SqlConnection dbConnection = new SqlConnection(ConfigurationManager.AppSettings["SqlDBConn"].ToString());
            try
            {
                string query = ConfigurationManager.AppSettings["UpdateQuery"].ToString();
                using (SqlCommand command = new SqlCommand(query, dbConnection))
                {
                    dbConnection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
            }
        }

        public static void Email()
        {
            var searchLogs = getSearchLog();
            EmailSender sender = new EmailSender();
            var recipentEmail = ConfigurationManager.AppSettings["recipentEmail"];
            sender.Send(recipentEmail, SearchLogModel.Printify(searchLogs));
            markLogsDone();
        }

    }
}
