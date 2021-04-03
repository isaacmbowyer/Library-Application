using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using LibraryLove;
using LibraryLove.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminProject.Pages.ViewLibrarians
{
    public class ViewLibrariansModel : PageModel
    {
        public List<Member> UserList { get; set; }


        [BindProperty(SupportsGet = true)]
        public string SearchData { get; set; }

        public bool NoMatch { get; set; }

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
                command.CommandText = @"SELECT * FROM Member WHERE ROLE = 'Librarian'";

                if (!string.IsNullOrEmpty(SearchData))
                {
                    if (SearchData.Contains('@'))
                    {
                        command.CommandText += " AND Email LIKE '%' + @User + '%'";

                    }
                    else
                    {
                        command.CommandText += " AND Username LIKE '%' + @User + '%'";
                    }

                    command.Parameters.AddWithValue("@User", SearchData);
                }

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



            if (UserList.Count == 0)
            {
                NoMatch = true;
            }
            else
            {
                NoMatch = false;
            }

        }
        public void OnPost()
        {
            // Connect to Database
            DBConnection dbstring = new DBConnection();
            string DbConnection = dbstring.DbString();
            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();

            using (SqlCommand command = new SqlCommand())
            {
                // Delete the User
                command.Connection = conn;
                command.CommandText = @"DELETE Member WHERE Id = @UId";
                command.Parameters.AddWithValue("@UId", Id);

                command.ExecuteNonQuery();
            }

            OnGet(); // get a new list of librarians

        }

    }
 }

