using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace AdminProject.Pages.ViewCustomers
{
    public class IndexModel : PageModel
    {
        //[BindProperty]
        public List<AllMembers> UserList { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SearchData { get; set; }

        public void OnGet()
        {
            if (SearchData!= null)
            {
                string dbconnection = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=AllMembers;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";


                SqlConnection conn = new SqlConnection(dbconnection);
                conn.Open();

                //  AllMembers = new AllMembers();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = conn;
                    command.CommandText = @"SELECT * FROM UsersTable WHERE Role = 'Customer' AND Username = '" + SearchData +"'";

                    SqlDataReader read = command.ExecuteReader();

                    UserList = new List<AllMembers>();

                    while (read.Read())
                    {
                        AllMembers record = new AllMembers();
                        record.Id = read.GetInt32(0);
                        record.Username = read.GetString(1);
                        record.FirstName = read.GetString(2);
                        record.LastName = read.GetString(3);
                        record.Email = read.GetString(4);
                        record.Password = read.GetString(5);
                        record.Role = read.GetString(6);

                        UserList.Add(record);
                    }
                    read.Close();
                    //conn.Close();
                }
            }
            else if (SearchData == null)
            {
                string dbconnection = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=AllMembers;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";


                SqlConnection conn = new SqlConnection(dbconnection);
                conn.Open();

                //  AllMembers = new AllMembers();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = conn;
                    command.CommandText = @"SELECT * FROM UsersTable WHERE Role = 'Customer'";

                    SqlDataReader read = command.ExecuteReader();

                    UserList = new List<AllMembers>();

                    while (read.Read())
                    {
                        AllMembers record = new AllMembers();
                        record.Id = read.GetInt32(0);
                        record.Username = read.GetString(1);
                        record.FirstName = read.GetString(2);
                        record.LastName = read.GetString(3);
                        record.Email = read.GetString(4);
                        record.Password = read.GetString(5);
                        record.Role = read.GetString(6);

                        UserList.Add(record);
                    }
                    read.Close();
                    //conn.Close();
                }
            }
          
        }
    }
}
