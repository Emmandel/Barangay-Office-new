using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Barangay_Office.Models;
using SQLite;

namespace Barangay_Office.Services
{
    public class AuthService
    {
        private const string AuthStateKey = "AuthState";
        private readonly SQLiteAsyncConnection _Connection;

        public AuthService()
        {
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "barangay_office.db");
            _Connection = new SQLiteAsyncConnection(dbPath);
            _Connection.CreateTableAsync<AdminUserInfo>().Wait();//ensure table exists
        }

        //authenticate user and password
        public async Task<AdminUserInfo> GetAdminUserInfoAsync(string username, string password)
        {
            return await _Connection.Table<AdminUserInfo>()
                .FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
        }




        //check if user is authenticated from sqlite
        public async Task<string> LoginAsync(string username, string password)
        {
            var user = await GetAdminUserInfoAsync(username, password);

            if (user != null && user.Password == password)
            {
                Preferences.Default.Set(AuthStateKey, true); //store login state
                return user.Role; // return role
            }

            return null;
        }

        //Register new user
        public async Task<bool> RegisteredAsync(string username, string password, string role)
        {
            var ExistingUser = await _Connection.Table<AdminUserInfo>().FirstOrDefaultAsync(u => u.Username == username);

            if (ExistingUser != null) return false;

            await _Connection.InsertAsync(new AdminUserInfo
            {
                Username = username,
                Password = password,
                Role = role
            });
            return true;
        }
        
        //check authentication state from preference
        public async Task<bool> IsAuthenticatedAsync()
        {

            await Task.Delay(500);
            return Preferences.Get(AuthStateKey, false);
        }

        public string GetRole()
        {
            return Preferences.Get(UserRoleKey, "customer"); //Default to customer
        }

        public void Logout()
        {
            Preferences.Default.Remove(AuthStateKey);
            Preferences.Default.Remove(UserRoleKey);
        }

        //public void Login(string role)
        //{
        //    Preferences.Default.Set(AuthStateKey, true);
        //    Preferences.Default.Set(UserRoleKey, role);
        //}

    }
}
