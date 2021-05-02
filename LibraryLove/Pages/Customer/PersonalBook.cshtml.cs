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
    public class PersonalBookModel : PageModel
    {


        [BindProperty(SupportsGet = true)]
        public Book BookRecord { get; set; }

        [BindProperty]
        public List<Book> LoanedBooks { get; set; }

        [BindProperty]
        public List<Book> PreLoanedBooks { get; set; }

        public const string SessionKeyName1 = "userID";

        public void OnGet()
        {
            // Connect to Database 
            DBConnection dbstring = new DBConnection();
            string DbConnection = dbstring.DbString();
            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();

            using (SqlCommand command = new SqlCommand())
            {

                // Select every loaned book to be displayed to the user
                command.Connection = conn;

             
                command.CommandText = @"SELECT BookId, Title, AuthorFirstName, AuthorLastName, Image FROM LoanedBook INNER JOIN Member ON UsernameId = Member.Id INNER JOIN Book ON Book.Id = BookId WHERE UsernameId = @UID ORDER BY Title";
                

                command.Parameters.AddWithValue("@UID", (int)HttpContext.Session.GetInt32(SessionKeyName1));


                SqlDataReader reader = command.ExecuteReader(); // read reecords

                LoanedBooks = new List<Book>();

                while (reader.Read())
                {
                    Book record = new Book();
                    record.Id = reader.GetInt32(0);
                    record.Title = reader.GetString(1);
                    record.AuthorFirstName = reader.GetString(2);
                    record.AuthorLastName = reader.GetString(3);
                    record.Image = reader.GetString(4);

                    LoanedBooks.Add(record);
                }

                reader.Close();

                // Select every preloaned book to be displayed to the user
         
            
                command.CommandText = @"SELECT BookId, Title, AuthorFirstName, AuthorLastName, Image FROM PreLoanedBook INNER JOIN Member ON UsernameId = Member.Id INNER JOIN Book ON Book.Id = BookId WHERE UsernameId = @UID ORDER BY Title";
               


                reader = command.ExecuteReader(); // read reecords

                PreLoanedBooks = new List<Book>();

                while (reader.Read())
                {
                    Book record = new Book();
                    record.Id = reader.GetInt32(0);
                    record.Title = reader.GetString(1);
                    record.AuthorFirstName = reader.GetString(2);
                    record.AuthorLastName = reader.GetString(3);
                    record.Image = reader.GetString(4);

                    PreLoanedBooks.Add(record);
                }

                reader.Close();

            }
        }

    }
}
