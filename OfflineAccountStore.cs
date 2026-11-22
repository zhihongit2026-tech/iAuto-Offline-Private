// Authentication helper for an authorized offline launcher build.
// Source-only prototype. Does not modify or bypass the original executable.
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace L
{
    internal static class OfflineAccountStore
    {
        private const string Version = "IAUTO-OFFLINE-1";
        private const int Iterations = 200000;

        private static string AccountPath
        {
            get
            {
                return Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "iAutoOffline", "account.dat");
            }
        }

        public static void Create(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || username.Trim().Length > 150)
                throw new ArgumentException("Username must contain 1-150 characters.");
            if (password == null || password.Length < 10)
                throw new ArgumentException("Password must contain at least 10 characters.");
            if (File.Exists(AccountPath))
                throw new InvalidOperationException("Local account already exists.");

            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
                rng.GetBytes(salt);
            byte[] hash = Hash(password, salt);
            string record = Version + "\n"
                + Convert.ToBase64String(Encoding.UTF8.GetBytes(username.Trim())) + "\n"
                + Convert.ToBase64String(salt) + "\n"
                + Convert.ToBase64String(hash) + "\n";
            Directory.CreateDirectory(Path.GetDirectoryName(AccountPath));
            using (var file = new FileStream(AccountPath, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            using (var writer = new StreamWriter(file, new UTF8Encoding(false)))
                writer.Write(record);
        }

        public static bool Verify(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrEmpty(password))
                return false;
            try
            {
                string[] lines = File.ReadAllLines(AccountPath, Encoding.UTF8);
                if (lines.Length != 4 || lines[0] != Version)
                    return false;
                string saved = Encoding.UTF8.GetString(Convert.FromBase64String(lines[1]));
                if (!string.Equals(username.Trim(), saved, StringComparison.OrdinalIgnoreCase))
                    return false;
                byte[] salt = Convert.FromBase64String(lines[2]);
                byte[] expected = Convert.FromBase64String(lines[3]);
                if (salt.Length != 16 || expected.Length != 32)
                    return false;
                byte[] actual = Hash(password, salt);
                int diff = 0;
                for (int i = 0; i < expected.Length; i++)
                    diff |= expected[i] ^ actual[i];
                return diff == 0;
            }
            catch (IOException) { return false; }
            catch (UnauthorizedAccessException) { return false; }
            catch (FormatException) { return false; }
            catch (CryptographicException) { return false; }
        }

        private static byte[] Hash(string password, byte[] salt)
        {
            // .NET Framework 4.5 Rfc2898DeriveBytes: PBKDF2-HMAC-SHA1.
            using (var derive = new Rfc2898DeriveBytes(password, salt, Iterations))
                return derive.GetBytes(32);
        }
    }
}
