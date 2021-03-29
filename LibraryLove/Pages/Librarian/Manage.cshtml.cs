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
        public List<PreLoanedBook> PreLoanedBooks { get; set; }


        [BindProperty]
        public QuantityBook BookRecord { get; set; }

        [BindProperty]
        public PreLoanedBook PreLoanedBook { get; set; }

        [BindProperty]
        public LoanedBook LoanedBook { get; set; }

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
                        // Book only for pre-loan
                        record.Quantity = null;
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
                command.Connection = conn;
               

                // First read which customers have pre-loaned the book as they will instantly loan the book
                command.CommandText = @"SELECT TOP(@BFirst) UsernameId FROM PreLoanedBook WHERE BookId = @BId ORDER BY DatePreLoaned";
                command.Parameters.AddWithValue("@BFirst", BookRecord.AddedBooks);
                command.Parameters.AddWithValue("@BId", BookRecord.BookId);

                SqlDataReader reader = command.ExecuteReader(); // read records 

                PreLoanedBooks = new List<PreLoanedBook>();

                // Get each username who pre-loan the book
                while (reader.Read())
                {
                    PreLoanedBook PreLoanedBook = new PreLoanedBook();
                    PreLoanedBook.CustomerId = reader.GetInt32(0);

                    PreLoanedBooks.Add(PreLoanedBook);
                }

                reader.Close();

                // If the Book List is empty then no one has pre-loaned that book
                if (PreLoanedBooks.Count != 0)
                {
                    // Delete the records from the table for that book as the users who have the earliest date take priority
                    command.CommandText = @"DELETE FROM PreLoanedBook WHERE Id IN(SELECT TOP(@BFirst) Id FROM PreLoanedBook WHERE BookId = @BId ORDER BY DatePreLoaned)";
                    command.ExecuteNonQuery();

                    for (int i = 0; i < PreLoanedBooks.Count; i++)
                    {
                        // Add those records to LoanedBook Table & add the Dates
                        command.CommandText = @"INSERT INTO LoanedBook (UsernameId, BookId, DateLoaned, DateReturned) VALUES (@CId" + i + ", @BId, @DLoaned" + i + ", @DReturned" + i + ")";
                        command.Parameters.AddWithValue("@CId" + i, PreLoanedBooks[i].CustomerId);

                        // already defined BookId earlier on, so automatically added

                        DateTime currentDate = DateTime.Now; // get the current time

                        command.Parameters.AddWithValue("@DLoaned" + i, currentDate);  // they now loan the book as of now
                        command.Parameters.AddWithValue("@DReturned" + i, currentDate.AddDays(7)); // this is the returned due date

                        command.ExecuteNonQuery();

                    }

                    // change the value - people who preloan the book take priorty 
                    BookRecord.CurrentBooks = BookRecord.AddedBooks - PreLoanedBooks.Count;
                }


                else
                {
                    // Update the Added Book Stock to the Current quantity
                    BookRecord.CurrentBooks = BookRecord.CurrentBooks + BookRecord.AddedBooks;
                }

                // Update the quantity 
                command.CommandText = @"UPDATE Book SET Quantity = @BQuantity WHERE Id = @BookId";
                command.Parameters.AddWithValue("@BookId", BookRecord.BookId);
                command.Parameters.AddWithValue("@BQuantity", BookRecord.CurrentBooks);

                command.ExecuteNonQuery();


            }
            // Update the page 
            OnGet();


        }
    }
}

