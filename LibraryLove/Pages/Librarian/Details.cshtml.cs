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
        [BindProperty]
        public List<LoanedBook> LoanedBooks { get; set; }
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
                // SQL Query
                command.CommandText = @"SELECT Username, FirstName, LastName, Title, LoanedDate FROM CustomerBook INNER JOIN Member ON UsernameId = Member.Id INNER JOIN Book ON Book.Id = LoanedBookId WHERE LoanedBookId = @BId";
                command.Parameters.AddWithValue("@BId", Id);

                SqlDataReader reader = command.ExecuteReader(); // read records 
                LoanedBooks = new List<LoanedBook>();

                while (reader.Read())
                {
                    LoanedBook CustomerBook = new LoanedBook();
                    CustomerBook.CustomerUsername = reader.GetString(0);
                    CustomerBook.CustomerFirstName = reader.GetString(1);
                    CustomerBook.CustomerLastName = reader.GetString(2);
                    CustomerBook.BookTitle = reader.GetString(3);
                    CustomerBook.LoanedDate = reader.GetDateTime(4);  // get the date when the customer loaned the book

                    CustomerBook.ReturnDate = CustomerBook.LoanedDate.AddDays(7); // get the date when the book should be returned

                    LoanedBooks.Add(CustomerBook);
                }

                reader.Close();
            }
        }
    }
}
