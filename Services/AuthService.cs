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
            var adminExists = await _Connection.Table<AdminUserInfo>().FirstOrDefaultAsync(u => u.Email == "Admin@gmail.com");
            var CustomerExists = await _Connection.Table<AdminUserInfo>().FirstOrDefaultAsync(u => u.Email == "Customer@gmail.com");

            if (adminExists == null) // Insert Admin if not exists
            {
                await _Connection.InsertAsync(new AdminUserInfo
                {
                    Email = "Admin@gmail.com",
                    Password ="Test123",
                    //Password = BCrypt.Net.BCrypt.HashPassword("Test@123"),
                    Role = "Admin"
                });
            }

            if (CustomerExists == null) // Insert User if not exists
            {
                await _Connection.InsertAsync(new AdminUserInfo
                {
                    Email = "Customer@gmail.com",
                    Password ="Test123",
                    //Password = BCrypt.Net.BCrypt.HashPassword("Test@123"),
                    Role = "Customer"
                });
            }
        }


        //authenticate user and password
        public async Task<AdminUserInfo> GetAdminUserInfoAsync(string email, string password)
        {
            return await _Connection.Table<AdminUserInfo>()
                .FirstOrDefaultAsync(e => e.Email == email && e.Password == password);
        }


        //check if user is authenticated from sqlite
        public async Task<string?> LoginAsync(string email, string password)
        {
            var user = await GetAdminUserInfoAsync(email, password);
            if (user != null && user.Password == password)
            {
                //"UserID"
                Preferences.Set(AuthStateKey, true); //store login state
                Preferences.Set("UserRole", user.Role); //store role for future reference
                return user.Role; //return role
            }
            return null;
        }

        //Register new User, ensure that the role is also stored
        public async Task<bool> RegisterAsync(string email, string password, string role)
        {
            if (!email.Contains("@") || !email.Contains(".")) return false; //simple email validation

            var existingUser = await _Connection.Table<AdminUserInfo>().FirstOrDefaultAsync(u => u.Email == email);
            if (existingUser != null) return false; //user already exists

            //string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password); 

            //Password = hashedPassword
            await _Connection.InsertAsync(new AdminUserInfo { Email = email, Password = password, Role = role });
            return true;
        }

        //for biometrics, get userrole by email
        public async Task<string?> GetUserRoleByEmail(string email)
        {
            var user = await _Connection.Table<AdminUserInfo>()
                .FirstOrDefaultAsync(u => u.Email == email);

            return user?.Role; // Returns role if user exists, otherwise returns null
        }



        //Check authentication state from preference
        public async Task<bool> IsAuthenticatedAsync()
        {
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

        //method for resetting the password
        public async Task<bool> ResetPasswordAsync(string email, string newPassword)
        {
            var user = await _Connection.Table<AdminUserInfo>().FirstOrDefaultAsync(u => u.Email == email);
            if (user != null)
            {
                user.Password = newPassword; // Ideally, hash this password
                await _Connection.UpdateAsync(user);
                return true;
            }
            return false;
        }


        //check if the email exist
        public async Task<bool> IsEmailRegisteredAsync(string email)
        {
            var user = await _Connection.Table<AdminUserInfo>().FirstOrDefaultAsync(u => u.Email == email);
            return user != null;
        }



        //logout and remove authentication state
        public void LogOut()
        {
            // Preserve last role for biometric login
            string lastRole = Preferences.Get("UserRole", string.Empty);
            Preferences.Set("LastUserRole", lastRole);

            //"UserID"
            Preferences.Default.Remove(AuthStateKey);
            Preferences.Default.Remove("UserRole");
        }

    }
}
