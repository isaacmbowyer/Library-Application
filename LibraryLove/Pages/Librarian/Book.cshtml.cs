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
    public class BookModel : PageModel
    {
        [BindProperty]
        public Book BookRecord { get; set; }

        public void OnGet(int id)
        {
            // Connect to Database
            DBConnection dbstring = new DBConnection();
            string DbConnection = dbstring.DbString();
            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;

                // SQL Query
                command.CommandText = @"SELECT * FROM Book WHERE Id = @BId";
                command.Parameters.AddWithValue("@BId", id);
                SqlDataReader reader = command.ExecuteReader(); // read records 

                BookRecord = new Book();

                while (reader.Read())
                {
                    BookRecord.Id = reader.GetInt32(0);
                    BookRecord.Title = reader.GetString(1);
                    BookRecord.AuthorFirstName = reader.GetString(2);
                    BookRecord.AuthorLastName = reader.GetString(3);
                    BookRecord.ISBN = reader.GetInt32(4);
                    BookRecord.Image = reader.GetString(5);

                    if (reader.IsDBNull(7))
                    {
                        // Let the Librarian know that this book is new in the system 
                        BookRecord.Quantity = -1;
                    }
                    else
                    {
                        // The Quanity it is not null, get its actual value
                        BookRecord.Quantity = reader.GetInt32(7);
                    }

                }

                reader.Close();
            }
        }

    }
}
