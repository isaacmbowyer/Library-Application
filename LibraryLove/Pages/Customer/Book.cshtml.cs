using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using LibraryLove.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LibraryLove.Pages.Customer
{
    public class BookModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public Book BookRecord { get; set; }

        [BindProperty]
        public DateTime Date { get; set; }

        [BindProperty]
        public DateTime ReturnDate { get; set; }

        // At first we do not know if they loaned the book or if they preloaned it
        [BindProperty]
        public bool PreLoanBook { get; set; } = false;

        [BindProperty]
        public bool LoanBook { get; set; } = false;
        public PreLoanedBook PreLoanedRecord { get; set; }

        public int Id;
        public const string SessionKeyName1 = "userID";

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

                // SQL Query to read Book Details
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
                        // Let the Customer know that this book has not got any quanity so its only for preloan 
                        BookRecord.Quantity = 0;
                    }
                    else
                    {
                        // The Quanity it is not null, get its actual value
                        BookRecord.Quantity = reader.GetInt32(7);
                    }

                }

                reader.Close();

                // Check if the user has the book
                Id = (int)HttpContext.Session.GetInt32(SessionKeyName1);


           
                command.Parameters.AddWithValue("@UID", Id);


                if (BookRecord.Quantity == 0)
                {
                    // SQL Query to see if the Customer preloans the book
                    command.CommandText = @"SELECT DatePreLoaned FROM PreLoanedBook WHERE UsernameId = @UID AND BookId = @BId";

                    reader = command.ExecuteReader(); // read records 

                    while (reader.Read())
                    {
                        Date = reader.GetDateTime(0);
                        PreLoanBook = true;  // user has preloaned the book
                    }

                    reader.Close();

                }
         
             
                // If the Book is not quanitty 0 that means no one can pre-loan the book
                if(!PreLoanBook)
                {
                    // This means that the User might have loaned the book instead
                    command.CommandText = @"SELECT DateLoaned, DateReturned FROM LoanedBook WHERE UsernameId = @UID AND BookId = @BId";
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Date = reader.GetDateTime(0);
                        ReturnDate = reader.GetDateTime(1);
                        LoanBook = true; // user has the book
                    }

                    reader.Close();
                }




            }
        }

        public IActionResult OnPostLoan()
        {

            // Connect to Database
            DBConnection dbstring = new DBConnection();
            string DbConnection = dbstring.DbString();
            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;

                // SQL Query to read Book Details
                command.CommandText = @"INSERT INTO LoanedBook(UsernameId, BookId, DateLoaned, DateReturned) VALUES(@UserID, @BookID, @DLoaned, @DReturned)";
                command.Parameters.AddWithValue("@UserId", Id = (int)HttpContext.Session.GetInt32(SessionKeyName1));
                command.Parameters.AddWithValue("@BookId", BookRecord.Id);

                Date = DateTime.Now;
                ReturnDate = Date.AddDays(7);
                BookRecord.Quantity -= 1; 
                command.Parameters.AddWithValue("@DLoaned", Date);
                command.Parameters.AddWithValue("@DReturned", ReturnDate);
                command.ExecuteNonQuery();


                // SQL Query to add the quanity of the book
                command.CommandText = @"UPDATE Book SET Quantity = @BQuantity WHERE Id = @BookId";
                command.Parameters.AddWithValue("@BQuantity", BookRecord.Quantity);

                command.ExecuteNonQuery();


                
            }

            LoanBook = true;
            PreLoanBook = false;
            return Page();


        }


        public IActionResult OnPostReturn()
        {
            // Connect to Database
            DBConnection dbstring = new DBConnection();
            string DbConnection = dbstring.DbString();
            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;

                // SQL Query to delete book details
                command.CommandText = @"DELETE FROM LoanedBook WHERE BookId = @BookId AND UsernameId = @UserId";
                command.Parameters.AddWithValue("@UserId", Id = (int)HttpContext.Session.GetInt32(SessionKeyName1));
                command.Parameters.AddWithValue("@BookId", BookRecord.Id);

                command.ExecuteNonQuery();

                // SQL Query to let the user who currently preloans the book to get it the book

                if(BookRecord.Quantity == 0)
                {
                    // First read PreloanBook Table as we need to know if any customers have preloaned the book  as they will instantly loan the book
                    command.CommandText = @"SELECT TOP(1) UsernameId FROM PreLoanedBook WHERE BookId = @BookId ORDER BY DatePreLoaned";

                    SqlDataReader reader = command.ExecuteReader(); // read records 

                    PreLoanedRecord = new PreLoanedBook();

                    // Get each username who pre-loan the book
                    while (reader.Read())
                    {
                        PreLoanedRecord.CustomerId = reader.GetInt32(0);
                    }

                    reader.Close();

                    // If the Book is empty then no one has pre-loaned that book
                    if (PreLoanedRecord.CustomerId != 0)
                    {
                        // Delete the record from the table for that book as the users who have the earliest date take priority
                        command.CommandText = @"DELETE FROM PreLoanedBook WHERE Id IN(SELECT TOP(1) Id FROM PreLoanedBook WHERE BookId = @BId ORDER BY DatePreLoaned)";
                        command.ExecuteNonQuery();


                        // Add those records to LoanedBook Table & add the Dates
                        command.CommandText = @"INSERT INTO LoanedBook (UsernameId, BookId, DateLoaned, DateReturned) VALUES (@CId, @BId, @DLoaned, @DReturned)";
                        command.Parameters.AddWithValue("@CId", PreLoanedRecord.CustomerId);


                        DateTime currentDate = DateTime.Now; // get the current time

                        command.Parameters.AddWithValue("@DLoaned", currentDate);  // they now loan the book as of now
                        command.Parameters.AddWithValue("@DReturned", currentDate.AddDays(7)); // this is the returned due date

                        command.ExecuteNonQuery();

                        // Returned book + 1, Preloaned the book -1 == cancel out
                    
                    }

                    else
                    {
                        // No users pre-loan the book add quantity
                        AddQuantity(command, Id);
                    
                    }

                }
                else
                {
                    // No users preloan the book add quantity
                      AddQuantity(command, Id);
                }

                // User has just returned the book
                LoanBook = false;
                PreLoanBook = false;
                return Page();



            }
        }

        public IActionResult OnPostPreLoan()
        {
            // Connect to Database
            DBConnection dbstring = new DBConnection();
            string DbConnection = dbstring.DbString();
            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;

                // SQL Query to insert preloaned book details
                command.CommandText = @"INSERT INTO PreLoanedBook(UsernameId, BookId, DatePreLoaned) VALUES(@UserID, @BookID, @DPLoaned)";
                command.Parameters.AddWithValue("@UserId", Id = (int)HttpContext.Session.GetInt32(SessionKeyName1));
                command.Parameters.AddWithValue("@BookId", BookRecord.Id);
                Date = DateTime.Now;
                command.Parameters.AddWithValue("@DPLoaned", Date);

                command.ExecuteNonQuery();


            }

            PreLoanBook = true;
            LoanBook = false;
            return Page();

        }

    
        public IActionResult OnPostReturnAndPay()
        {
            return Page();
        }

        public void AddQuantity(SqlCommand command, int Id)
        {
            // SQL Query to add the quanity of the book
            command.CommandText = @"UPDATE Book SET Quantity = @BQuantity WHERE Id = @BookId";
            command.Parameters.AddWithValue("@BQuantity", BookRecord.Quantity + 1);
            command.ExecuteNonQuery();
        }

    }
}