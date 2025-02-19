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
        private const string AuthStateKey = "AuthState";


        //authenticate the user
        public async Task<bool> IsAuthenticatedAsync()
        {
            await Task.Delay(1000);

            var authState = Preferences.Default.Get(AuthStateKey, false);

            return authState;
            Console.WriteLine("sample");
        }

        public void LogIn()
        {
            Preferences.Default.Set(AuthStateKey, true);
        }

        public void LogOut()
        {
            Preferences.Default.Remove(AuthStateKey);
        }


    }
}
