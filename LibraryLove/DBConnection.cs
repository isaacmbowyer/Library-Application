using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryLove
{
    public class DBConnection
    {
        public string DbString()
        {
            string dbString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Isaac Bowyer\source\repos\LibraryLove\LibraryLove\Data\LibraryDB.mdf;Integrated Security=True;Connect Timeout=30";
            return dbString;
        }
    }
}
