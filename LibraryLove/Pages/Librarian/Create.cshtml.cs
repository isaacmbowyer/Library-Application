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
    public class CreateModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public Book BookRecord { get; set; }

       

        // This is used to get the wwwwroot folder path - inject this
        public readonly IWebHostEnvironment _env;

        public CreateModel(IWebHostEnvironment env)
        {
            _env = env;
        }

        public void OnGet()
        {
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


                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = conn;
                    command.CommandText = @"INSERT INTO Book (Title, AuthorFirstName, AuthorLastName, ISBN, Image, Genre) VALUES (@BTitle, @AFName, @ALName, @BISBN, @BImage, @BGenre)";


                    // Add book to database
                    command.Parameters.AddWithValue("@BTitle", BookRecord.Title);
                    command.Parameters.AddWithValue("@AFName", BookRecord.AuthorFirstName);
                    command.Parameters.AddWithValue("@ALName", BookRecord.AuthorLastName);
                    command.Parameters.AddWithValue("@BISBN", BookRecord.ISBN);

                    
                    if (BookRecord.UploadImage  != null)
                    {
                        // Upload the image If the Librarian requested to

                        // Path to the wwwroot folder and images folder
                        string path = Path.Combine(_env.WebRootPath, "Images", BookRecord.UploadImage.FileName);

                        // Copy the uploaded file to the folder -> we need a filestream to copy the contents of the file
                        BookRecord.UploadImage.CopyTo(new FileStream(path, FileMode.Create)); // FileMode.Create because the file needs to be created on the server

                        // Save Image name to the database 
                        command.Parameters.AddWithValue("@BImage", BookRecord.UploadImage.FileName);
                    }
                    else
                    {
                        // Librarian did not upload an image, use default image instead
                        command.Parameters.AddWithValue("@BImage", "default.jpg");
                    }


                    command.Parameters.AddWithValue("@BGenre", BookRecord.Genre);
                    
                    command.ExecuteNonQuery();
                }

                return RedirectToPage("View");
            }
            else
            {
                return Page();
            }
        }
    }
}

