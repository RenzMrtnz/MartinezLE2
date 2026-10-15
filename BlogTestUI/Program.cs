using System;
using BlogDataLibrary.Database;

namespace BlogTestUI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString =
                @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=BlogDB;Integrated Security=True;";

            ISqlDataAccess dataAccess = new SqlDataAccess();

            SqlData db = new SqlData(dataAccess, connectionString);

            var users = db.GetUsers();

            foreach (var user in users)
            {
                Console.WriteLine(
                    $"ID: {user.Id} | Name: {user.FirstName} {user.LastName} | Username: {user.UserName}");
            }

            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();
        }
    }
}
