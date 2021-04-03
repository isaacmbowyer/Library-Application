using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LibraryLove.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LibraryLove.Pages.Librarian
{
    public class EditModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public Book BookRecord { get; set; }


        // This is used to get the wwwwroot folder path - inject this
        public readonly IWebHostEnvironment _env;

        public EditModel(IWebHostEnvironment env)
        {
            _env = env;
        }

        public void OnGet(int? id)  
        {
            // Connect to Database
            DBConnection dbstring = new DBConnection();
            string DbConnection = dbstring.DbString();
            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();

            BookRecord = new Book(); 

            // SQL Query
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = @"SELECT * FROM BOOK WHERE Id = @BId";
                command.Parameters.AddWithValue("@BId", id);


                SqlDataReader reader = command.ExecuteReader(); 


                while (reader.Read())
                {
                    BookRecord.Id = reader.GetInt32(0);
                    BookRecord.Title = reader.GetString(1);
                    BookRecord.AuthorFirstName = reader.GetString(2);
                    BookRecord.AuthorLastName = reader.GetString(3);
                    BookRecord.ISBN = reader.GetInt32(4);
                    BookRecord.Image = reader.GetString(5);
                    BookRecord.Genre = (BookGenre)reader.GetInt32(6);
                }
               
                reader.Close(); // finished reading 
            }
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
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
                    command.CommandText = @"UPDATE Book SET Title = @BTitle, AuthorFirstName = @AFName, AuthorLastName = @ALName, ISBN = @BISBN, Image = @BImage, Genre = @BGenre WHERE Id = @BId";
                    command.Parameters.AddWithValue("@BId", BookRecord.Id);
                    // Edit the Book details to database
                    command.Parameters.AddWithValue("@BTitle", BookRecord.Title);
                    command.Parameters.AddWithValue("@AFName", BookRecord.AuthorFirstName);
                    command.Parameters.AddWithValue("@ALName", BookRecord.AuthorLastName);
                    command.Parameters.AddWithValue("@BISBN", BookRecord.ISBN);

                    // Librarian has selected a new file to upload - upload it
                    if (BookRecord.UploadImage != null)
                    {
                        // Path to the wwwroot folder and images folder
                        string folder = Path.Combine(_env.WebRootPath, "Images", BookRecord.UploadImage.FileName);

                        // Copy the uploaded file to the folder -> we need a filestream to copy the contents of the file
                        BookRecord.UploadImage.CopyTo(new FileStream(folder, FileMode.Create)); // FileMode.Create because the file needs to be created on the server

                        // Update the Image in the database 
                        command.Parameters.AddWithValue("@BImage", BookRecord.UploadImage.FileName);

                    }

                    else
                    {
                        // Librarian did not update the Book, keep the orginal image details
                        command.Parameters.AddWithValue("@BImage", BookRecord.Image);
                    }


                    command.Parameters.AddWithValue("@BGenre", BookRecord.Genre);

                    // Update the book object inside the database
                    command.ExecuteNonQuery();

                    return RedirectToPage("Book", "Get", new { id = BookRecord.Id });
                }
            }

            return Page();
            
        }

    }
}
