using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supabase;

namespace Barangay_Office.Services
{
    public class PostgreSqlService
    {
        private readonly Client _supabaseClient;

        //env
        private const string supabase_url = "https://ukewluuwhquxypuwttci.supabase.co";

        private const string supabase_key = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InVrZXdsdXV3aHF1eHlwdXd0dGNpIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDE1MDcxNjcsImV4cCI6MjA1NzA4MzE2N30.nnTE11pjpbNVb0WkQxHAqZY_ItOPu8eqFK4Oig7xAuI";

        public PostgreSqlService()
        {
            var options = new SupabaseOptions
            {
                AutoRefreshToken = true,
                AutoConnectRealtime = true
            };

            _supabaseClient = new Client(supabase_url, supabase_key, options);
        }
        public Client GetClient() => _supabaseClient;
    }
}
