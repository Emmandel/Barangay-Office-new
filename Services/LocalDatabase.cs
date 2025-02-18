using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Barangay_Office.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using SQLite;

namespace Barangay_Office.Services
{
    public class LocalDatabase
    {

        private readonly SQLiteAsyncConnection _database;
        private readonly string _databaseName = "ThisSQL.db";

        public LocalDatabase()
        {
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), _databaseName);
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<AdminUserInfo>();
        }

        public Task<AdminUserInfo> Authenticate(string username, string password)
        {
            var user = _database.Table<AdminUserInfo>()
                .FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
            return Task.FromResult(user);
        }
    }
}
