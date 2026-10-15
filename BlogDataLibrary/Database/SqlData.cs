using System.Collections.Generic;
using System.Linq;
using BlogDataLibrary.Models;

namespace BlogDataLibrary.Database
{
    public class SqlData : ISqlData
    {
        private readonly ISqlDataAccess _db;
        private const string connectionStringName = "SqlDb";

        public SqlData(ISqlDataAccess db)
        {
            _db = db;
        }

        public UserModel? Authenticate(string username, string password)
        {
            return _db.LoadData<UserModel, dynamic>(
                "dbo.spUsers_Authenticate",
                new { username, password },
                connectionStringName
            ).FirstOrDefault();
        }

        public void Register(
            string username,
            string firstName,
            string lastName,
            string password)
        {
            _db.SaveData(
                "dbo.spUsers_Register",
                new { userName = username, firstName, lastName, password },
                connectionStringName);
        }

        public void AddPost(PostModel post)
        {
            _db.SaveData(
                "dbo.spPosts_Insert",
                new
                {
                    userId = post.UserId,
                    title = post.Title,
                    body = post.Body,
                    dateCreated = post.DateCreated
                },
                connectionStringName);
        }

        public List<ListPostModel> ListPosts()
        {
            return _db.LoadData<ListPostModel, dynamic>(
                "dbo.spPosts_List",
                new { },
                connectionStringName);
        }

        public ListPostModel? ShowPostDetails(int id)
        {
            return _db.LoadData<ListPostModel, dynamic>(
                "dbo.spPosts_Detail",
                new { id },
                connectionStringName
            ).FirstOrDefault();
        }
    }
}
