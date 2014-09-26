﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using AUDash.Models;

namespace AUDash.Repository
{
    public class DBRepository
    {
        private SqlConnection GetConnection()
        {
            //string azureConnection = "Server=tcp:e5r6frgs22.database.windows.net,1433;Database=audashbAMiWR6WOt;User ID=tushar@e5r6frgs22;Password=India@123;Trusted_Connection=False;Encrypt=True;Connection Timeout=50;";
            //SqlConnection conn = new SqlConnection(azureConnection);

            SqlConnection conn = new SqlConnection("Data Source=.;Initial Catalog=AUDashboard;User ID=sa;Password=India@123");
            return conn;
        }

        public void AddResource(string resource)
        {
            SqlCommand cmd = new SqlCommand("Insert into resources(resourcedata) values('" + resource + "')", GetConnection());
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
        }

        public List<Resource> GetAllResources()
        {
            List<Resource> resources = new List<Resource>();
            SqlCommand cmd = new SqlCommand("select resourceid, resourcedata from resources", GetConnection());
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
            SqlCommand cmd = new SqlCommand("update resources set resourcedata = '" + resource + "' where resourceId = " + resourceId , GetConnection());
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
        }
    }
}