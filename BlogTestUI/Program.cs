using System;
using System.Collections.Generic;
using BlogDataLibrary.Database;
using BlogDataLibrary.Models;
using Microsoft.Extensions.Configuration;

namespace BlogTestUI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            ISqlDataAccess dataAccess = new SqlDataAccess(config);
            SqlData db = new SqlData(dataAccess);

            bool running = true;

            while (running)
            {
                Console.Clear();
                Console.WriteLine("==================================");
                Console.WriteLine("  MARTINEZ BLOG MANAGEMENT SYSTEM");
                Console.WriteLine("==================================");
                Console.WriteLine("1. Login");
                Console.WriteLine("2. Register New User");
                Console.WriteLine("3. Add Post");
                Console.WriteLine("4. List All Posts");
                Console.WriteLine("5. Show Post Details");
                Console.WriteLine("6. Exit");
                Console.Write("\nChoose an option: ");

                string choice = Console.ReadLine() ?? "";

                try
                {
                    switch (choice)
                    {
                        case "1":
                            Login(db);
                            break;
                        case "2":
                            Register(db);
                            break;
                        case "3":
                            AddPost(db);
                            break;
                        case "4":
                            ListPosts(db);
                            break;
                        case "5":
                            ShowPostDetails(db);
                            break;
                        case "6":
                            running = false;
                            continue;
                        default:
                            Console.WriteLine("Invalid option.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }

                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine();
            }
        }

        static UserModel? GetCurrentUser(SqlData db)
        {
            Console.Write("Username: ");
            string username = Console.ReadLine() ?? "";

            Console.Write("Password: ");
            string password = Console.ReadLine() ?? "";

            return db.Authenticate(username, password);
        }

        static void Login(SqlData db)
        {
            var user = GetCurrentUser(db);

            if (user == null)
                Console.WriteLine("Invalid username or password.");
            else
                Console.WriteLine($"Welcome, {user.FirstName} {user.LastName}!");
        }

        static void Register(SqlData db)
        {
            Console.Write("Username: ");
            string username = Console.ReadLine() ?? "";

            Console.Write("First name: ");
            string firstName = Console.ReadLine() ?? "";

            Console.Write("Last name: ");
            string lastName = Console.ReadLine() ?? "";

            Console.Write("Password: ");
            string password = Console.ReadLine() ?? "";

            db.Register(username, firstName, lastName, password);
            Console.WriteLine("User registered successfully!");
        }

        static void AddPost(SqlData db)
        {
            Console.WriteLine("--- Login to create a post ---");

            var user = GetCurrentUser(db);

            if (user == null)
            {
                Console.WriteLine("Invalid login. Cannot add post.");
                return;
            }

            Console.Write("Post title: ");
            string title = Console.ReadLine() ?? "";

            Console.Write("Post body: ");
            string body = Console.ReadLine() ?? "";

            db.AddPost(new PostModel
            {
                UserId = user.Id,
                Title = title,
                Body = body,
                DateCreated = DateTime.Now
            });

            Console.WriteLine("Post successfully added!");
        }

        static void ListPosts(SqlData db)
        {
            List<ListPostModel> posts = db.ListPosts();

            if (posts.Count == 0)
            {
                Console.WriteLine("No posts available.");
                return;
            }

            foreach (var post in posts)
            {
                string preview = post.Body.Length > 20
                    ? post.Body.Substring(0, 20) + "..."
                    : post.Body;

                Console.WriteLine(
                    $"{post.Id}. {post.Title} by {post.UserName} " +
                    $"[{post.DateCreated:yyyy-MM-dd}]");

                Console.WriteLine(preview);
                Console.WriteLine();
            }
        }

        static void ShowPostDetails(SqlData db)
        {
            Console.Write("Enter post ID: ");

            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid post ID.");
                return;
            }

            var post = db.ShowPostDetails(id);

            if (post == null)
            {
                Console.WriteLine("Post not found.");
                return;
            }

            Console.WriteLine($"\nTitle: {post.Title}");
            Console.WriteLine(
                $"Author: {post.FirstName} {post.LastName} ({post.UserName})");
            Console.WriteLine($"Date: {post.DateCreated:MMMM dd, yyyy}");
            Console.WriteLine($"\n{post.Body}");
        }
    }
}
