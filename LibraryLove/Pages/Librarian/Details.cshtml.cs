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
    public class DetailsModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int? Quantity { get; set; }

        [BindProperty]
        public List<LoanedBook> LoanedBooks { get; set; }

        [BindProperty]
        public List<PreLoanedBook> PreLoanedBooks { get; set; }

        public void OnGet(int Id)
        {
            // Connect to Database
            DBConnection dbstring = new DBConnection();
            string DbConnection = dbstring.DbString();
            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;

                SqlDataReader reader;
                LoanedBooks = new List<LoanedBook>();
                PreLoanedBooks = new List<PreLoanedBook>();

                if (Quantity != null)
                {
                    // Book is available for Loan at any form 

                    // SQL Query
                    command.CommandText = @"SELECT Username, FirstName, LastName, Title, DateLoaned, DateReturned FROM LoanedBook INNER JOIN Member ON UsernameId = Member.Id INNER JOIN Book ON Book.Id = BookId WHERE BookId = @BId";
                    command.Parameters.AddWithValue("@BId", Id);

                    reader = command.ExecuteReader(); // read records 

                    while (reader.Read())
                    {
                        LoanedBook CustomerBook = new LoanedBook();
                        CustomerBook.CustomerUsername = reader.GetString(0);
                        CustomerBook.CustomerFirstName = reader.GetString(1);
                        CustomerBook.CustomerLastName = reader.GetString(2);
                        CustomerBook.BookTitle = reader.GetString(3);
                        CustomerBook.LoanedDate = reader.GetDateTime(4);  // get the date when the customer got the book
                        CustomerBook.ReturnDate = reader.GetDateTime(5); // get the date when the book should be returned

                        LoanedBooks.Add(CustomerBook);
                    }

                    reader.Close();
                }

                // Get all the Books that the user could have preloaned 
                command.CommandText = @"SELECT Username, FirstName, LastName, Title, DatePreLoaned FROM PreLoanedBook INNER JOIN Member ON UsernameId = Member.Id INNER JOIN Book ON Book.Id = BookId WHERE BookId = @BookId";
                command.Parameters.AddWithValue("@BookId", Id);

                reader = command.ExecuteReader(); // read records 
  
                while (reader.Read())
                {
                    PreLoanedBook CustomerBook = new PreLoanedBook();
                    CustomerBook.CustomerUsername = reader.GetString(0);
                    CustomerBook.CustomerFirstName = reader.GetString(1);
                    CustomerBook.CustomerLastName = reader.GetString(2);
                    CustomerBook.BookTitle = reader.GetString(3);
                    CustomerBook.PreLoanedDate = reader.GetDateTime(4);  // get the date when the customer got the book

                    PreLoanedBooks.Add(CustomerBook);
                }

                reader.Close();
            }
        }
    }
}
