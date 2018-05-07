using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using DataAccess.Models;
using Microsoft.Data.Sqlite;

namespace DataAccess.Sqlite
{
    public class SqliteDataUserService : IDataUserService
    {
        private readonly IDataConnectionFactory connectionFactory;
        private readonly ISqliteHelperService helperService;

        public SqliteDataUserService(
            IDataConnectionFactory connectionFactory,
            ISqliteHelperService helperService)
        {
            this.connectionFactory = connectionFactory;
            this.helperService = helperService;
        }

        public User Register(string username, string password)
        {
            using (var connection = connectionFactory.CreateConnection())
            {
                var salt = CreateSalt();
                var passwordHash = Sha256(Sha256(password) + salt);

                helperService.ExequtePlain(connection,
                    "INSERT INTO users(Username, Password, Salt, CreationDate)" +
                    "VALUES($username, $password, $salt, CURRENT_TIMESTAMP)",
                    new Parameter("username", username),
                    new Parameter("password", passwordHash),
                    new Parameter("salt", salt));

                return new User
                {
                    Id = helperService.GetScalar<long>(connection, "SELECT last_insert_rowid()"),
                    Username = username
                };
            }
        }

        public User Get(string username, string password)
        {
            using (var connection = connectionFactory.CreateConnection())
            {
                (connection as SqliteConnection).CreateFunction("sha256", (string text, string salt) => Sha256(text + salt));

                return helperService.GetScalarMapped<User>(connection,
                    "SELECT rowid, *" +
                    "FROM users " +
                    "WHERE Username=$username AND Password=sha256($hash, Salt)",
                    new Parameter("username", username),
                    new Parameter("hash", Sha256(password)));
            }
        }

        private static readonly Random random = new Random();
        private static string CreateSalt()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 16)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private static string Sha256(string text)
        {
            var crypt = new SHA256Managed();
            var hash = new StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(text));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }
    }
}