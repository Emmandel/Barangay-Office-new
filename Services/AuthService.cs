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
            Task.Run(async () => await _Connection.CreateTableAsync<AdminUserInfo>()).Wait();//ensure table exists
            Task.Run(async () => await InitializeDefaultUsers()).Wait();
        }



        private async Task InitializeDefaultUsers()
        {
            var adminExists = await _Connection.Table<AdminUserInfo>().FirstOrDefaultAsync(u => u.Username == "Admin@gmail.com");
            var userExists = await _Connection.Table<AdminUserInfo>().FirstOrDefaultAsync(u => u.Username == "Customer@gmail.com");

            if (adminExists == null) // Insert Admin if not exists
            {
                await _Connection.InsertAsync(new AdminUserInfo
                {
                    Username = "Admin@gmail.com",
                    Password = "Test123",
                    Role = "Admin"
                });
            }

            if (userExists == null) // Insert User if not exists
            {
                await _Connection.InsertAsync(new AdminUserInfo
                {
                    Username = "Customer@gmail.com",
                    Password = "Test123",
                    Role = "Customer"
                });
            }
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
                Preferences.Default.Set("UserRole", user.Role); //store role for future reference
                return user.Role; //return role
            }
            return null;
        }

        //Register new User, ensure that the role is also stored
        public async Task<bool> RegisterAsync(string username, string password, string role)
        {
            var existingUser = await _Connection.Table<AdminUserInfo>().FirstOrDefaultAsync(u => u.Username == username);

            if (existingUser != null) return false; //user already exists

            await _Connection.InsertAsync(new AdminUserInfo { Username = username, Password = password, Role = role });
            return true;
        }



        //Check authentication state from preference
        public async Task<bool> IsAuthenticatedAsync()
        {
            await Task.Delay(500);
            string storedRole = Preferences.Get("UserRole", string.Empty);
            return !string.IsNullOrEmpty(storedRole); //ensure role exists
        }

        //store login state
        public string GetRole()
        {
            return Preferences.Get("UserRole", string.Empty);
        }

        public async Task<(bool IsAuthenticated, string role)> GetAuthenticatedUserRoleAsync()
        {
            bool isAuthenticated = Preferences.Get(AuthStateKey, false);
            string role = Preferences.Get("UserRole", string.Empty);

            return isAuthenticated && !string.IsNullOrEmpty(role) ? (true, role) : (false, null);
        }


        //logout and remove authentication state
        public void LogOut()
        {
            Preferences.Default.Remove(AuthStateKey);
            Preferences.Default.Remove("UserRole");
        }

    }
}
