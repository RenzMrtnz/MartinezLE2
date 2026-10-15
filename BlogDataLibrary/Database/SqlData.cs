using BlogDataLibrary.Models;
using System.Collections.Generic;

namespace BlogDataLibrary.Database
{
    public class SqlData
    {
        private readonly ISqlDataAccess _db;
        private const string connectionStringName = "SqlDb";

        public SqlData(ISqlDataAccess db)
        {
            _db = db;
        }

        public List<UserModel> GetUsers()
        {
            return _db.LoadData<UserModel, dynamic>(
                "spUsers_List",
                new { },
                connectionStringName);
        }
    }
}
