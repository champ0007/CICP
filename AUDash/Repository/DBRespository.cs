using System;
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
            SqlConnection conn = new SqlConnection("Data Source=.;Initial Catalog=AUDashboard;User ID=sa;Password=India@123");
            return conn;
        }

        public void AddResource(string resource)
        {
            SqlCommand cmd = new SqlCommand("Insert into resources values('" + resource + "')", GetConnection());
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
        }

        public List<string> GetAllResources()
        {
            List<string> resources = new List<string>();
            SqlCommand cmd = new SqlCommand("select resourcedata from resources", GetConnection());
            cmd.Connection.Open();
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                resources.Add(rdr.GetString(0));
            }

            cmd.Connection.Close();

            return resources;
        }


    }
}