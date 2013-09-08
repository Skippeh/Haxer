using System;
using System.IO;

namespace ServerCSharp
{
    public class Account
    {
        public string Username { get; private set; }

        /// <summary>Password hash, not the actual password.</summary>
        public string Password { get; private set; }

        public string UniqueID { get; private set; }

        private string hashKey;

        private Account()
        {
            
        }

        public static Account Create(string user, string password)
        {
            var account = new Account();

            account.Username = user;
            string randomKey = CryptoHelper.GetRandomKey();
            account.Password = CryptoHelper.Hash(password, randomKey);
            account.hashKey = randomKey;
            account.UniqueID = CryptoHelper.GetRandomKey();

            account.Save();
            return account;
        }

        public void Save()
        {
            Directory.CreateDirectory("Accounts");
            string filePath = "Accounts/" + CryptoHelper.ToSHA256(Username.ToLower()) + ".user";

            var file = new BinaryWriter(File.Create(filePath));

            file.Write(Username);
            file.Write(Password);
            file.Write(hashKey);
            file.Write(UniqueID);

            file.Close();
        }

        public static Account Load(string user)
        {
            string filePath = "Accounts/" + CryptoHelper.ToSHA256(user.ToLower()) +  ".user";
            var account = new Account();
            var reader = new BinaryReader(File.OpenRead(filePath));

            account.Username = reader.ReadString();
            account.Password = reader.ReadString();
            account.hashKey = reader.ReadString();
            account.UniqueID = reader.ReadString();

            // TODO: Load the rest.

            reader.Close();
            return account;
        }

        public static bool Exists(string username)
        {
            Directory.CreateDirectory("Accounts");
            foreach (var filePath in Directory.GetFiles("Accounts"))
            {
                var fileName = Path.GetFileNameWithoutExtension(filePath);

                if (fileName == CryptoHelper.ToSHA256(username.ToLower()))
                {
                    return true;
                }
            }

            return false;
        }

        public static string GetHashedPassword(string username)
        {
            var file = new BinaryReader(File.OpenRead("Accounts/" + CryptoHelper.ToSHA256(username.ToLower()) + ".user"));

            file.ReadString(); // username
            string password = file.ReadString();
            file.Close();

            return password;
        }

        public static bool TryLogin(string user, string password, out Account account)
        {
            account = null;
            var file = new BinaryReader(File.OpenRead("Accounts/" + CryptoHelper.ToSHA256(user.ToLower()) + ".user"));

            var fileUsername = file.ReadString();
            var filePassword = file.ReadString();
            var fileHash = file.ReadString();
            file.Close();

            if (user.ToLower() != fileUsername.ToLower())
                return false;

            if (CryptoHelper.Hash(password, fileHash) != filePassword)
                return false;

            account = Account.Load(user);
            return true;
        }
    }
}