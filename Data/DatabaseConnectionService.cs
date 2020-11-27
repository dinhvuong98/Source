using Dapper;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Common.Dependency;
using Utilities.Configuation;

namespace Data
{
    public class DatabaseConnectService
    {
        private static readonly AppSettings AppSettings = SingletonDependency<IOptions<AppSettings>>.Instance.Value;

        public IDbConnection Connection{ get; set;}

        private IDbTransaction Transaction { get; set; }
       

        public bool IsConnected => this.Connection.State == ConnectionState.Open;

        public DatabaseConnectService()
        {
            this.Connection = new SqlConnection(AppSettings.ConnectionString);
            ConnectionOpen();
        }

        public void ConnectionOpen()
        {
            if (!this.IsConnected)
            {
                this.Connection.Open();
            }
        }

        public void ConnectionClose()
        {
            if (this.IsConnected)
            {
                this.Connection.Close();
            }
        }

        public IDbTransaction BeginTransaction()
        {
            return this.Connection.BeginTransaction(IsolationLevel.Serializable);
        }

        public List<T> Select<T>(string query, object parameters, IDbTransaction transaction = null)
            where T : class
        {
            return this.Connection.Query<T>(query, parameters, transaction).ToList();
        }

        public List<T> Select<T>(string query)
            where T : class
        {
            return this.Connection.Query<T>(query).ToList();
        }

        public async Task<List<T>> SelectAsync<T>(string query, object parameters, IDbTransaction transaction = null)
          where T : class
        {
            return (await this.Connection.QueryAsync<T>(query, parameters, transaction)).ToList();
        }

        public void Execute(string query)
        {
            this.Connection.Execute(query);
        }

        public async Task ExecuteAsync(string query)
        {
            await this.Connection.ExecuteAsync(query);
        }

        public void Execute(string query, object parameters)
        {
            this.Connection.Execute(query, parameters);
        }

        public async Task ExecuteAsync(string query, object parameters)
        {
            await this.Connection.ExecuteAsync(query, parameters);
        }

        public void Execute(string query, object parameters, IDbTransaction transaction)
        {
            this.Connection.Execute(query, parameters, transaction);
        }

        public async Task ExecuteAsync(string query, object parameters, IDbTransaction transaction)
        {
            await this.Connection.ExecuteAsync(query, parameters, transaction);
        }

        public void Dispose()
        {
            ConnectionClose();
        }
    }
}
