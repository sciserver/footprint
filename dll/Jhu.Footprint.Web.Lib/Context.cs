using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Jhu.Footprint.Web.Lib
{
    public class Context : IDisposable
    {
        private string connectionString;
        private SqlConnection connection;
        private SqlTransaction transaction;
        private string user;

        public string ConnectionString
        {
            get { return connectionString; }
            set { connectionString = value; }
        }

        public SqlConnection Connection
        {
            get
            {
                if (connection == null)
                {
                    OpenConnection();
                }

                return connection;
            }
        }

        public SqlTransaction Transaction
        {
            get
            {
                if (transaction == null)
                {
                    BeginTransaction();
                }

                return transaction;
            }
        }

        public string User
        {
            get { return user; }
            set { user = value;  }
        }

        public Context()
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {
            //this.connectionString = ConfigurationManager.ConnectionStrings["Jhu.Footprint"].ConnectionString;
            this.connectionString = "";
            this.connection = null;
            this.transaction = null;
            this.user = "";
        }

        public void Dispose()
        {
            if (transaction != null)
            {
                CommitTransaction();
            }

            if (connection != null)
            {
                CloseConnection();
            }
        }

        public void OpenConnection()
        {
            if (connection != null)
            {
                throw new InvalidOperationException("Connection is already open.");
            }

            connection = new SqlConnection(connectionString);
            connection.Open();
        }

        private void CloseConnection()
        {
            if (connection == null)
            {
                throw new InvalidOperationException("Connection is not open.");
            }

            connection.Close();
        }

        private void BeginTransaction()
        {
            if (connection == null)
            {
                throw new InvalidOperationException("Connection is not open.");
            }

            if (transaction != null)
            {
                throw new InvalidOperationException("There's already a transaction.");
            }

            transaction = connection.BeginTransaction();
        }

        private void CommitTransaction()
        {
            if (transaction == null)
            {
                throw new InvalidOperationException("There's no transaction.");
            }

            transaction.Commit();
            transaction = null;
        }

        private void RollbackTransaction()
        {
            if (transaction == null)
            {
                throw new InvalidOperationException("There's no transaction.");
            }

            transaction.Rollback();
            transaction = null;
        }


    }
}
