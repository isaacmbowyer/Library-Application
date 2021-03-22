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
    public class ManageModel : PageModel
    {
        [BindProperty]
        public List<Book> Books { get; set; }

        [BindProperty]
        public QuantityBook BookRecord { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PenaltyPrice { get; set; }

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
                command.CommandText = @"SELECT Id, Title, ISBN, Quantity FROM Book ORDER BY Title";
                SqlDataReader reader = command.ExecuteReader(); // read records 
                Books = new List<Book>();

                while (reader.Read())
                {
                    Book record = new Book();
                    record.Id = reader.GetInt32(0);
                    record.Title = reader.GetString(1);
                    record.ISBN = reader.GetInt32(2);
                    if (reader.IsDBNull(3))
                    {
                        // Quantity of this book is 0 
                        record.Quantity = 0;
                    }
                    else
                    {
                        // Get its actual value
                        record.Quantity = reader.GetInt32(3);
                    }
                    Books.Add(record);
                }

                reader.Close();

            }

        }

        public void OnPost()
        {
            // Connect to Database
            DBConnection dbstring = new DBConnection();
            string DbConnection = dbstring.DbString();
            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();

            // SQL Query
            using (SqlCommand command = new SqlCommand())
            {
                // Add the Added Book Stock to the Current quantity
                BookRecord.CurrentBooks = BookRecord.CurrentBooks + BookRecord.AddedBooks;

                command.Connection = conn;
                command.CommandText = @"UPDATE Book SET Quantity = @BQuantity WHERE Id = @BId";
                command.Parameters.AddWithValue("@BId", BookRecord.BookId);

                // Edit the Book details to database
                command.Parameters.AddWithValue("@BQuantity", BookRecord.CurrentBooks);

                command.ExecuteNonQuery();
            }
            // Update the page 
            OnGet();
        }
    }
}

