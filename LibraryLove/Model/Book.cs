using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryLove.Model
{
    public class Book : IValidatableObject
    {
        public int Id { get; set; }

        [Required (ErrorMessage ="Book Title is required")]
        [Display(Name = "Book Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Author First Name is required")]
        [Display(Name = "Author First Name")]
        public string AuthorFirstName { get; set; }
        
        [Required(ErrorMessage = "Author Last Name is required")]
        [Display(Name = "Author Last Name")]
        public string AuthorLastName { get; set; }

        [Required(ErrorMessage = "Book ISBN is required")]
        [Display(Name = "Book ISBN")]
        [RegularExpression(@"^97.{4}",
            ErrorMessage = "Invalid ISBN")]
        public int? ISBN { get; set; }

        public string Image { get; set; }

        [Display(Name = "Book Image")]
        public IFormFile UploadImage { get; set; }

        [Required(ErrorMessage = "Select a Genre from the drop down list")]
        [Display(Name = "Book Genre")]
        public BookGenre? Genre { get; set; }

        public int Quantity { get; set; }
       
        public string GetFullName()
        {
            return AuthorFirstName + " " + AuthorLastName;
        }
 
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {

            // Connect to Database
            DBConnection dbstring = new DBConnection();
            string DbConnection = dbstring.DbString();
            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                Int32 count;
                if (Id == 0)
                {
                    // User is on the Create Page

                    // Check Title is Unique 
                    command.CommandText = @"SELECT COUNT(Title) FROM Book WHERE Title = @BTitle";
                    command.Parameters.AddWithValue("@BTitle", Title);

                    count = (Int32)command.ExecuteScalar();

                    if (count == 1)
                    {
                        // Title exists show error
                        yield return new ValidationResult("This Title already exists", new[] { "Title" });
                    }

                    // Check if the ISBN is unique 
                    command.CommandText = @"SELECT COUNT(ISBN) FROM Book WHERE ISBN = @BISBN";
                    command.Parameters.AddWithValue("@BISBN", ISBN);

                    count = (Int32)command.ExecuteScalar();

                    if (count == 1)
                    {
                        yield return new ValidationResult("This ISBN already exists", new[] { "ISBN" });
                    }
                }

                else
                {
                    // User is on Edit Page

                    // Check if the user changed their Title
                    command.CommandText = @"SELECT Title FROM Book WHERE Id = @BId";
                    command.Parameters.AddWithValue("@BId", Id);
                    SqlDataReader reader = command.ExecuteReader();
           
                    string databaseTitle = "";
                    while (reader.Read())
                    {
                        // Get the Title from database
                       databaseTitle = reader.GetString(0);
                    }

                    reader.Close();

                    // Check if the Title is the same or not
                    if (!databaseTitle.Equals(Title))
                    {
                        // Check Title is Unique 
                        command.CommandText = @"SELECT COUNT(Title) FROM Book WHERE Title = @BTitle";
                        command.Parameters.AddWithValue("@BTitle", Title);

                        count = (Int32)command.ExecuteScalar();

                        if (count == 1)
                        {
                            // Title exists show error
                            yield return new ValidationResult("This Title already exists", new[] { "Title" });
                        }

                    }

                    // Check if user changed the ISBN 
                    command.CommandText = @"SELECT ISBN FROM Book WHERE Id = " + Id;
                    reader = command.ExecuteReader(); // read records 

                    int OldISBN = 0;
                    while (reader.Read())
                    {
                        OldISBN = reader.GetInt32(0);
                    }

                    reader.Close();


                    if (!OldISBN.Equals(ISBN))
                    {
                        // Check if the ISBN is unique 
                        command.CommandText = @"SELECT COUNT(ISBN) FROM Book WHERE ISBN = @BISBN";
                        command.Parameters.AddWithValue("@BISBN", ISBN);

                        count = (Int32)command.ExecuteScalar();

                        if (count == 1)
                        {
                            yield return new ValidationResult("This ISBN already exists", new[] { "ISBN" });
                        }
                    }
                }
            
            }

            // Check first that the User uploaded a new image
            if (UploadImage != null)
            {
                // Check if the User requested an image that already exists
                if (File.Exists(@"C:\Users\Isaac Bowyer\source\repos\LibraryLove\LibraryLove\wwwroot\Images\" + UploadImage.FileName))
                {
                    yield return new ValidationResult("This Book Image already exists", new[] { "UploadImage" });

                }
            }

        }

    }
}
