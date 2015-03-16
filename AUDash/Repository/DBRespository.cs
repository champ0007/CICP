﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using AUDash.Models;
using System.Configuration;
using System.Web.Configuration;
namespace AUDash.Repository
{
    public class DBRepository
    {
        string connectionString;
        public DBRepository()
        {
            //Configuration rootWebConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("/");
            ConnectionStringSettings connString;

            ConnectionStringsSection connSection = (ConnectionStringsSection)WebConfigurationManager.GetSection("connectionStrings");

            if (connSection.ConnectionStrings.Count > 0)
            {
                connString = connSection.ConnectionStrings["AUDashboardAzureConnection"];
                if (connString != null)
                    connectionString = connString.ConnectionString;
                else
                    connectionString = string.Empty;
            }


        }

        private SqlConnection GetConnection()
        {
            //string azureConnection = "Server=tcp:e5r6frgs22.database.windows.net,1433;Database=audashbAMiWR6WOt;User ID=tushar@e5r6frgs22;Password=India@123;Trusted_Connection=False;Encrypt=True;Connection Timeout=50;";
            //SqlConnection conn = new SqlConnection(azureConnection);

            //SqlConnection conn = new SqlConnection("Data Source=USHYDTUSHARMA4\\Sqlexpress;Initial Catalog=AUDashboard;Integrated Security = true");
            SqlConnection conn = new SqlConnection(connectionString);


            return conn;
        }

        public void AddResource(string resource)
        {
            SqlCommand cmd = new SqlCommand("Insert into resource(resourcedata) values('" + resource + "')", GetConnection());
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
        }

        public List<Resource> GetAllResources()
        {
            List<Resource> resources = new List<Resource>();
            SqlCommand cmd = new SqlCommand("select resourceid, resourcedata from resource", GetConnection());
            cmd.Connection.Open();
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                Resource resource = new Resource();
                resources.Add(new Resource()
                {
                    ResourceId = rdr.GetInt32(0),
                    ResourceData = rdr.GetString(1)
                });
            }

            cmd.Connection.Close();

            return resources;
        }

        internal void EditResource(string resource, int resourceId)
        {
            SqlCommand cmd = new SqlCommand("update resource set resourcedata = '" + resource + "' where resourceId = " + resourceId, GetConnection());
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
        }

        internal string GetReferenceData(string storageId)
        {
            string storageData = string.Empty;
            SqlCommand cmd = new SqlCommand("select StorageData from ReferenceData where StorageId = '" + storageId + "'", GetConnection());
            cmd.Connection.Open();
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                storageData = HttpUtility.HtmlDecode(rdr.GetString(0));
            }

            cmd.Connection.Close();

            return storageData;
        }


        internal void SetReferenceData(string storageId, string storageData)
        {
            SqlCommand cmd = new SqlCommand("update ReferenceData set StorageData = '" + HttpUtility.HtmlEncode(storageData) + "' where StorageId = '" + storageId + "'", GetConnection());
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
        }


        internal Dictionary<string, string> GetDashboardCounts()
        {
            Dictionary<string, string> dashboardCounts = new Dictionary<string, string>();
            SqlCommand cmd = new SqlCommand("select * from ReferenceData where StorageId in ('Invoices','NewToDoItems','GSSResources','Projects')", GetConnection());
            cmd.Connection.Open();
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                dashboardCounts.Add(rdr.GetString(0), HttpUtility.HtmlDecode(rdr.GetString(1)));
            }

            cmd.Connection.Close();

            return dashboardCounts;

        }
    }
}