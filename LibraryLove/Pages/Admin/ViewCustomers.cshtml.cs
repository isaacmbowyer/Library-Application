using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using LibraryLove;
using LibraryLove.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminProject.Pages.ViewCustomers
{
    public class ViewCustomersModel : PageModel
    {
        [BindProperty]
        public List<Member> UserList { get; set; }


        [BindProperty(SupportsGet = true)]
        public string SearchData { get; set; }

        [BindProperty]
        public int Id { get; set; }


        public void OnGet()
        {
            // Connect to Database
            DBConnection dbstring = new DBConnection();
            string DbConnection = dbstring.DbString();
            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();


            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = @"SELECT * FROM Member WHERE ROLE = 'Customer'";
                command.Parameters.AddWithValue("@User", SearchData);
              

                SqlDataReader read = command.ExecuteReader();

                UserList = new List<Member>();

                while (read.Read())
                {
                    Member record = new Member();
                    record.Id = read.GetInt32(0);
                    record.Username = read.GetString(1);
                    record.FirstName = read.GetString(2);
                    record.LastName = read.GetString(3);
                    record.Email = read.GetString(4);

                    UserList.Add(record);
                }
                read.Close();

            }

        }

    }
}
