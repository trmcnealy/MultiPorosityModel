using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Engineering.DataSource.Tools;

using Prism.Mvvm;

namespace MultiPorosity.Presentation.Models
{
    public sealed class DatabaseDataSource : BindableBase
    {
        private string _Host;
        public string Host {
            get { return _Host; }
            set { SetProperty(ref _Host, value); }
        }

        private int _Port;
        public int Port {
            get { return _Port; }
            set { SetProperty(ref _Port, value); }
        }
        
        private string _DatabaseName;
        public string DatabaseName {
            get { return _DatabaseName; }
            set { SetProperty(ref _DatabaseName, value); }
        }
        
        private string _Username;
        public string Username {
            get { return _Username; }
            set { SetProperty(ref _Username, value); }
        }
        
        private byte[] EncryptedPassword;
        private string _PasswordMask;
        private string _Password;
        public string Password
        {
            get { return _PasswordMask; }
            set
            {
                if(SetProperty(ref _Password, value))
                {
                    EncryptedPassword = Encryption.Encrypt(_Password, Encryption.GetUid(), Encryption.GetUserId());

                    _PasswordMask = string.Empty;
                    for (int i = 0; i < _Password.Length; ++i)
                    {
                        _PasswordMask += "*";
                    }
                    _Password     = string.Empty;
                }
            }
        }

        public DatabaseDataSource(string host,
                                  int    port,
                                  string databaseName,
                                  string username,
                                  string password)
        {
            Host             = host;
            Port             = port;
            DatabaseName     = databaseName;
            Username         = username;
            Password         = password;
        }

        public DatabaseDataSource(MultiPorosity.Services.Models.DatabaseDataSource databaseDataSource)
        {
            Host         = databaseDataSource.Host;
            Port         = databaseDataSource.Port;
            DatabaseName = databaseDataSource.DatabaseName;
            Username     = databaseDataSource.Username;
            Password     = databaseDataSource.Password;
        }

        public static explicit operator DatabaseDataSource(MultiPorosity.Services.Models.DatabaseDataSource databaseDataSource)
        {
            return new(databaseDataSource);
        }

        public static implicit operator MultiPorosity.Services.Models.DatabaseDataSource(DatabaseDataSource databaseDataSource)
        {
            return new(databaseDataSource.EncryptedPassword, databaseDataSource.Host, databaseDataSource.Port, databaseDataSource.DatabaseName, databaseDataSource.Username);
        }
    }
}
