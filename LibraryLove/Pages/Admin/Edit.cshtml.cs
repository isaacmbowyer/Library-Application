using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AdminProject.Data;
using AdminProject.Models;
using Microsoft.Data.SqlClient;

namespace AdminProject.Pages.Members
{
    public class EditModel : PageModel
    {
        [BindProperty]
        public AllMembers AllMembers { get; set; }

        public void OnGet(int? id) //Retrieve the Id of the record to be edited
        {
            string dbconnection = @"Data Source = (localdb)\MSSQLLocalDB; Initial Catalog = AllMembers; Integrated Security = True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False";


            SqlConnection conn = new SqlConnection(dbconnection);
            conn.Open(); //Database connection

            AllMembers = new AllMembers();

            using(SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = @"SELECT Id, Username, FirstName, LastName, Email, Password, Role FROM UsersTable WHERE Id = " + id;

                SqlDataReader read = command.ExecuteReader(); //Select all the data where the Id matches

                while(read.Read())
                {
                    AllMembers.Username = read.GetString(1);
                    AllMembers.FirstName = read.GetString(2);
                    AllMembers.LastName = read.GetString(3);
                    AllMembers.Email = read.GetString(4); //Read the information (note password will not be displayed on page)
                    AllMembers.Password = read.GetString(5);
                    AllMembers.Role = read.GetString(6);

                }
                read.Close();
            }
        }

        public IActionResult OnPost(int? id)
        {
            string dbconnection = @"Data Source = (localdb)\MSSQLLocalDB; Initial Catalog = AllMembers; Integrated Security = True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False";


            SqlConnection conn = new SqlConnection(dbconnection);
            conn.Open(); //Database connection

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = @"UPDATE UsersTable SET Username = @UName, FirstName = @FName, LastName = @LName, Email = @Email, @Pass = Password, Role = @Role WHERE id = " + id;

                if (ModelState.IsValid)
                {
                    command.Parameters.AddWithValue("@UName", AllMembers.Username);
                    command.Parameters.AddWithValue("@FName", AllMembers.FirstName);
                    command.Parameters.AddWithValue("@LName", AllMembers.LastName);
                    command.Parameters.AddWithValue("@Email", AllMembers.Email); //Update the database with the fields entered
                    command.Parameters.AddWithValue("@Pass", AllMembers.Password);
                    command.Parameters.AddWithValue("@Role", AllMembers.Role);
                }
                else
                {
                    return Page(); //Return back if error occured
                }

                command.ExecuteNonQuery();
            }
            return RedirectToPage("Index"); //If no errors occured, back to index
            
        }
  
    }
}

