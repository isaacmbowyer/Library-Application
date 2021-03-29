using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AdminProject.Data;
using AdminProject.Models;
using Microsoft.Data.SqlClient;

namespace AdminProject.Pages.Members
{
    public class DeleteModel : PageModel
    {
        [BindProperty]
        public AllMembers AllMembers { get; set;}
        public void OnGet(int? id) //Pass Id from browser that will be deleted
        {
            string dbconnection = @"Data Source = (localdb)\MSSQLLocalDB; Initial Catalog = AllMembers; Integrated Security = True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False";


            SqlConnection conn = new SqlConnection(dbconnection);
            conn.Open(); //Database connection

            AllMembers = new AllMembers();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = @"SELECT * FROM UsersTable WHERE Id = " + id; //Select the record to be deleted

                SqlDataReader read = command.ExecuteReader();

                while (read.Read())
                {
                    AllMembers.Username = read.GetString(1);
                    AllMembers.FirstName = read.GetString(2);
                    AllMembers.LastName = read.GetString(3); //Read all the information
                    AllMembers.Email = read.GetString(4);
                    AllMembers.Password = read.GetString(5);
                    AllMembers.Role = read.GetString(6);

                }
                read.Close();
            }
        }
        public IActionResult OnPost(int?id)
        {
            string dbconnection = @"Data Source = (localdb)\MSSQLLocalDB; Initial Catalog = AllMembers; Integrated Security = True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False";


            SqlConnection conn = new SqlConnection(dbconnection);
            conn.Open(); //Database connection

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = @"DELETE UsersTable WHERE Id = " + id; //Delete the record

                command.ExecuteNonQuery();
            }
            conn.Close();
            return RedirectToPage("Index");
        }
    }
}
