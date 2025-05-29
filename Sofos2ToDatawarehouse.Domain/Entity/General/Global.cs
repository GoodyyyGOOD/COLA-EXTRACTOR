using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sofos2ToDatawarehouse.Domain.Entity.General
{
    public class Global
    {
        private static string _server;
        private static string _sourceDB;
        private static string _userName;
        private static string _password;

        public Global(string server, string sourceDB, string userName, string password)
        {
            _server = server;
            _sourceDB = sourceDB;
            _userName = userName;
            _password = PasswordDecode(password);
        }

        public string GetSourceDatabase()
        {
            return $"Server={_server};Database={_sourceDB};Username={_userName};Password={_password};";
        }

        public string GetSourceHanaDatabase()
        {
            return $"Server={_server};Username={_userName};Password={_password};";
        }

        public string PasswordDecode(string passwordEncoded)
        {

            var passwordDecoded = Convert.FromBase64String(passwordEncoded);
            return Encoding.UTF8.GetString(passwordDecoded);
        }
    }
}
