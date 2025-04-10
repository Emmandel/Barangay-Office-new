using Barangay_Office.Models;
using Barangay_Office.Utilities;
using BCrypt.Net;
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
            var SuperadminExists = await _Connection.Table<AdminUserInfo>().FirstOrDefaultAsync(u => u.Email == "SuperAdmin@gmail.com");
            var CustomerExists = await _Connection.Table<AdminUserInfo>().FirstOrDefaultAsync(u => u.Email == "Customer@gmail.com");

            if (adminExists == null) // Insert Admin if not exists
            {
                string encryptedpassword = PasswordEncryptionManager.Encrypt("SampleTest@123");
                await _Connection.InsertAsync(new AdminUserInfo
                {
                    Email = "Admin@gmail.com",
                    //Password ="SampleTest@123",
                    Password = encryptedpassword,//stored hashpassword
                    Role = "Admin"
                });
            }

            if (CustomerExists == null) // Insert User if not exists
            {
                string encryptedpassword = PasswordEncryptionManager.Encrypt("SampleTest@123");
                await _Connection.InsertAsync(new AdminUserInfo
                {
                    Email = "Customer@gmail.com",
                    //Password ="SampleTest@123",
                    Password = encryptedpassword,//stored hashpassword
                    Role = "Customer"
                });
            }
            
            if (SuperadminExists == null) // Insert User if not exists
            {
                string encryptedPassword = PasswordEncryptionManager.Encrypt("SampleTest@123");
                Console.WriteLine($"[InitializeDefaultUsers] Encrypted Password (Admin): {encryptedPassword}");
                await _Connection.InsertAsync(new AdminUserInfo
                {
                    Email = "SuperAdmin@gmail.com",
                    //Password ="SampleTest@123",
                    Password = encryptedPassword,//stored hashpassword
                    Role = "SuperAdmin"
                });
            }
        }


        //authenticate user and password
        //public async Task<AdminUserInfo> GetAdminUserInfoAsync(string email, string password)
        //{
        //    return await _Connection.Table<AdminUserInfo>()
        //        .FirstOrDefaultAsync(e => e.Email == email && e.Password == password);
        //}


        //check if user is authenticated from sqlite
        public async Task<string?> LoginAsync(string email, string password)
        {

            try
            {
                AdminUserInfo user = await _Connection.Table<AdminUserInfo>().FirstOrDefaultAsync(e => e.Email == email);

                if (user != null)
                {
                    string decryptedPasssword = PasswordEncryptionManager.Decrypt(user.Password ?? string.Empty);

                    if (decryptedPasssword != password)
                    {
                        // Add more detailed error logging
                        Console.WriteLine($"[LoginAsync] Password verification failed for user: {email}");
                        return null;
                    }
                        Preferences.Set(AuthStateKey, true);
                        Preferences.Set("UserRole", user.Role);
                        return user.Role;
                }
                else
                {
                    Console.WriteLine($"[LoginAsync] User: {email} not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[LoginAsync] Exception: {ex}");
            }
            return null;
        }

        //Register new User, ensure that the role is also stored
        public async Task<bool> RegisterAsync(string email, string password, string role)
        {
            if (!email.Contains("@") || !email.Contains(".")) return false; //simple email validation

            var existingUser = await _Connection.Table<AdminUserInfo>().FirstOrDefaultAsync(u => u.Email == email);
            if (existingUser != null) return false; //user already exists

            string encryptedPassword = PasswordEncryptionManager.Encrypt(password);//hash the password

            //Password = hashedPassword
            await _Connection.InsertAsync(new AdminUserInfo { Email = email, Password = encryptedPassword, Role = role });
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
            try
            {
                string storedRole = Preferences.Get("UserRole", string.Empty);
                //return !string.IsNullOrEmpty(storedRole); //ensure role
                if (string.IsNullOrEmpty(storedRole)) return false;


                //check of it exist on database
                var user = await _Connection.Table<AdminUserInfo>()
                    .FirstOrDefaultAsync(u => u.Role == storedRole);

                return user != null;
            }
            catch
            {
                return false;
            }
        }

        //store login state
        public static string GetRole()
        {
            return Preferences.Get("UserRole", string.Empty);
        }


        //get the authenticated roles
        public async Task<(bool IsAuthenticated, string role)> GetAuthenticatedUserRoleAsync()
        {
            return await Task.Run(() =>
            {
                bool isAuthenticated = Preferences.Get(AuthStateKey, false);
                string role = Preferences.Get("UserRole", string.Empty);

                return isAuthenticated && !string.IsNullOrEmpty(role) ? (true, role) : (false, string.Empty);
            });
        }

        //method for resetting the password
        public async Task<bool> ResetPasswordAsync(string email, string newPassword)
        {
            var user = await _Connection.Table<AdminUserInfo>().FirstOrDefaultAsync(u => u.Email == email);
            if (user != null)
            {
                string encryptPassword = PasswordEncryptionManager.Encrypt(newPassword);
                //user.Password = newPassword; 
                user.Password = encryptPassword;
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
