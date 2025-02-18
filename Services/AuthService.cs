using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Admin.Directory.directory_v1.Data;
using SQLite;

namespace Barangay_Office.Services
{
    public class AuthService
    {
        private readonly SQLiteAsyncConnection _connection;
        private const string AuthStateKey = "AuthState";
        private const string UserRoleKey = "UserRole";

        public AuthService(LocalDatabase DbService)
        {
            _connection = DbService.GetConnection();
        }



        public async Task<bool> LoginAsync(string username, string password)
        {
            var user = await _connection.Table<UsersInfo>()
                .Where(u => u.Username == username && u.Password == password)
                .FirstOrDefaultAsync();

            if(user != null)
            {
                //user is authenticated and store their role
                //Login(user.Role);
                Preferences.Default.Set(AuthStateKey, true);
                Preferences.Default.Set(UserRoleKey, user.Role);
                return true;
            }

            return false;
            //return Preferences.Default.Get<bool>(AuthStateKey, false);

        }

        public void Logout()
        {
            Preferences.Default.Remove(AuthStateKey);
            Preferences.Default.Remove(UserRoleKey);
        }


        public bool IsAuthenticated()
        {
            return Preferences.Default.Get<bool>(AuthStateKey, false);
        }

        public string GetUserRole()
        {
            return Preferences.Default.Get<string>(UserRoleKey, "customer"); //Default to customer
        }
        //public void Login(string role)
        //{
        //    Preferences.Default.Set(AuthStateKey, true);
        //    Preferences.Default.Set(UserRoleKey, role);
        //}

    }
}
