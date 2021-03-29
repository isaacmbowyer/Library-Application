using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AdminProject.Models;
using Microsoft.Data.SqlClient;
using System.Data.SqlClient;

namespace AdminProject.Pages.Members
{

    public class CreateModel : PageModel
    {
        [BindProperty]
        public AllMembers UserRecord { get; set; }

        public void OnGet()
        {

        }
        public IActionResult OnPost()
        {
           
            string dbconnection = @"Data Source = (localdb)\MSSQLLocalDB; Initial Catalog = AllMembers; Integrated Security = True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False";
            SqlConnection con = new SqlConnection(dbconnection);
            con.Open();//Establish a connection to the database and table

            using (SqlCommand command = new SqlCommand())//Allows us to use SQL commands in the code
            {
                command.Connection = con;
                command.CommandText = @"INSERT INTO UsersTable (Username, FirstName, LastName, Email, Password, Role) VALUES (@UName, @FName, @LName, @Email, @Pass, @Role)";//Insert everything they enter into Users table

                if (ModelState.IsValid)
                {
                    command.Parameters.AddWithValue("@UName", UserRecord.Username);
                    command.Parameters.AddWithValue("@FName", UserRecord.FirstName);
                    command.Parameters.AddWithValue("@LName", UserRecord.LastName);//The data they enter and their fields:
                    command.Parameters.AddWithValue("@Email", UserRecord.Email);
                    command.Parameters.AddWithValue("@Pass", UserRecord.Password);
                    command.Parameters.AddWithValue("@Role", UserRecord.Role);
                }
                else
                {
                    return Page();//If it is not validated correctly, reload the page
                }

                command.ExecuteNonQuery();
            }
            return RedirectToPage("Index");
        }
    }
}
