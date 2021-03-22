using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using LibraryLove.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LibraryLove.Pages.Librarian
{
    public class SearchModel : PageModel
    {
        [BindProperty]
        public string SearchTerm { get; set; }

        public int NoMatch { get; set; } 

        [BindProperty(SupportsGet = true)]
        public List<Book> Books { get; set; }

    
        public void OnPost()
        {
            if (SearchTerm != null)
            {
                // Connect to Database
                DBConnection dbstring = new DBConnection();
                string DbConnection = dbstring.DbString();
                SqlConnection conn = new SqlConnection(DbConnection);
                conn.Open();


                Books = new List<Book>();

                using (SqlCommand command = new SqlCommand())
                {



                    command.Connection = conn;

                    try
                    {
                        // If this code exectues correctly, the search will be ISBN
                        int ISBN = Int32.Parse(SearchTerm);
                        command.CommandText = @"SELECT Id, Title, AuthorFirstName, AuthorLastName, Image  FROM Book WHERE ISBN = @BSearch";
                        command.Parameters.AddWithValue("@BSearch", ISBN);
                    }
                    catch (FormatException e)
                    {

                        // The search is Title
                        command.CommandText = @"SELECT Id, Title, AuthorFirstName, AuthorLastName, Image  FROM Book WHERE Title LIKE '%' + @BSearch + '%' ORDER BY Title";
                        command.Parameters.AddWithValue("@BSearch", SearchTerm);
                    }



                    SqlDataReader reader = command.ExecuteReader(); // read reecords


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
 
            if(Books.Count == 0)
            {
                 NoMatch = -1;
            }

        }
    }
}
