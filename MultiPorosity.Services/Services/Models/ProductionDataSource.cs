using System.Text.Json.Serialization;
using System.Threading.Tasks;

using Engineering.DataSource.Services.DataAccess;
using Engineering.DataSource.Tools;

using Microsoft.Data.Analysis;

using PostgreSql;

namespace MultiPorosity.Services.Models
{
    public abstract class ProductionDataSource
    {
    }

    public sealed class DatabaseDataSource : ProductionDataSource
    {
        [JsonPropertyName(nameof(Host))]
        public string Host { get; set; }

        [JsonPropertyName(nameof(Port))]
        public int Port { get; set; }

        [JsonPropertyName(nameof(DatabaseName))]
        public string DatabaseName { get; set; }

        [JsonPropertyName(nameof(Username))]
        public string Username { get; set; }

        private byte[] EncryptedPassword;

        [JsonPropertyName(nameof(Password))]
        public string Password
        {
            get { return Encryption.Decrypt(EncryptedPassword, Encryption.GetUid(), Encryption.GetUserId()); }
            set { EncryptedPassword = Encryption.Encrypt(value, Encryption.GetUid(), Encryption.GetUserId()); }
        }
        
        public DatabaseDataSource()
        {
            Host         = string.Empty;
            Port         = 0;
            DatabaseName = string.Empty;
            Username     = string.Empty;
            Password     = string.Empty;
        }

        public DatabaseDataSource(byte[] encryptedPassword,
                                  string host,
                                  int    port,
                                  string databaseName,
                                  string username)
        {
            EncryptedPassword = encryptedPassword;
            Host              = host;
            Port              = port;
            DatabaseName      = databaseName;
            Username          = username;
        }
    }

    public sealed class FileDataSource : ProductionDataSource
    {
        [JsonPropertyName(nameof(Path))]
        public string Path { get; set; }

        public FileDataSource(string path)
        {
            Path = path;
        }
    }
}