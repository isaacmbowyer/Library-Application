using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using LibraryLove.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LibraryLove.Pages.Librarian
{
    public class ViewModel : PageModel
    {

        [BindProperty(SupportsGet = true)]
        public Book BookRecord { get; set; }

        [BindProperty]
        public List<Book> Books { get; set; }
        public void OnGet()
        {
            // Connect to Database 
            DBConnection dbstring = new DBConnection();
            string DbConnection = dbstring.DbString();
            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();

            using (SqlCommand command = new SqlCommand())
            {
                // Select every book to be displayed to the user
                command.Connection = conn;
                command.CommandText = @"SELECT Id, Title, AuthorFirstName, AuthorLastName, Image FROM Book ORDER BY Title";

                SqlDataReader reader = command.ExecuteReader(); // read reecords

                Books = new List<Book>();

                while (reader.Read())
                {
                    Book record = new Book();
                    record.Id = reader.GetInt32(0);
                    record.Title = reader.GetString(1);
                    record.AuthorFirstName = reader.GetString(2);
                    record.AuthorLastName = reader.GetString(3);
                    record.Image = reader.GetString(4);

                    Books.Add(record);
                }
              
                reader.Close();
            }
        }
        public void OnPost()
        {
            // If the User selected a genre display those revelant books


            if (BookRecord.Genre != null) 
            {
                // Connect to Database
                DBConnection dbstring = new DBConnection();
                string DbConnection = dbstring.DbString();
                SqlConnection conn = new SqlConnection(DbConnection);
                conn.Open();

                // Get the value chosen 
                int value = BookRecord.Genre.GetHashCode();

                using (SqlCommand command = new SqlCommand())
                {

                    command.Connection = conn;
                    command.CommandText = @"SELECT Id, Title, AuthorFirstName, AuthorLastName, Image  FROM Book WHERE Genre = @BGenre ORDER BY Title";
                    command.Parameters.AddWithValue("@BGenre", value);

                    SqlDataReader reader = command.ExecuteReader(); // read reecords

                    Books = new List<Book>();

                    while (reader.Read())
                    {
                        Book record = new Book();
                        record.Id = reader.GetInt32(0);
                        record.Title = reader.GetString(1);
                        record.AuthorFirstName = reader.GetString(2);
                        record.AuthorLastName = reader.GetString(3);
                        record.Image = reader.GetString(4);

                        Books.Add(record);
                    }

                    reader.Close();
                }
            }

            else
            {
                // The user wishes to veiw all the books instead 
                OnGet();
            }

            
        }


    }
}


   