using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Extensions.Configuration;
using System.IO;
using My_Store.Shared.Helper;
using My_Store.Models.UserModels;
using Microsoft.AspNetCore.Authentication;



namespace My_Store.Infrastructure.DataAccessInfrastructure
{
    public class DataAccess : IAsyncDisposable
    {
        private SqlConnection Connection { get; set; }
        private SqlCommand Command { get; set; }
        private SqlDataReader Reader { get; set; }


        public DataAccess()
        {

            IConfiguration StringConfiguration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            string ConnectionString = StringConfiguration.GetConnectionString("MyStoreConnectionString");
            Result<string> Aux = Helper.ToValidateString(ConnectionString);

            if (Aux.IsValid)
            {
                Connection = new SqlConnection(Aux.Value);
                Command = new SqlCommand();
            }
            else
            {
                Connection = null;
                Command = null;
            }
        }



        public void ToSetProcedure(string Procedure)
        {
            Result<string> Aux = Helper.ToValidateString(Procedure);

            if (!Aux.IsValid || Command == null)
            {
                return;
            }
            Command.CommandType = System.Data.CommandType.StoredProcedure;
            Command.CommandText = Aux.Value;
        }


        public void ToSetQuery(string Query)
        {
            Result<string> Aux = Helper.ToValidateString(Query);
            if (!Aux.IsValid || Command == null)
            {
                return;
            }
            Command.CommandType = System.Data.CommandType.Text;
            Command.CommandText = Aux.Value;
        }


        public void ToSetParameters(string Name, object Value)
        {
            Result<string> ParameterName = Helper.ToValidateString(Name);

            if (!ParameterName.IsValid || Value == null)
            {
                return;
            }

            Command.Parameters.AddWithValue(ParameterName.Value, Value);
        }



        public async Task<SqlDataReader> ToRead()
        {
            if (Connection == null || Command == null)
            {
                return null;
            }

            Command.Connection = Connection;
            try
            {
                await Connection.OpenAsync();
                Reader = await Command.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                return Reader;
            }
            catch
            {
                throw;
            }
         

        }

        public async Task ToExecute()
        {
            if (Connection == null || Command == null)
            {
                return;
            }

            Command.Connection = Connection;
            try
            {
                await Connection.OpenAsync();
                await Command.ExecuteNonQueryAsync();
            }
            catch
            {
                throw;
            }
          
        }

        public async Task<SqlDataReader> ToExecuteWithResult()
        {
            if (Connection == null || Command == null)
            {
                return null;
            }

            Command.Connection = Connection;
            try
            {
                await Connection.OpenAsync();
                Reader=await Command.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                return Reader;
            }
            catch
            {
                throw;
            }
           
        }


        public async Task<int> ToExecuteScalarInt()
        {
            const int InvalidValue = -1;

            if (Connection == null || Command == null)
            {
                return InvalidValue;
            }


            Command.Connection = Connection;
            try
            {
                await Connection.OpenAsync();
                var Scalar = await Command.ExecuteScalarAsync();
                int Aux = InvalidValue;
                int.TryParse(Scalar?.ToString(), out Aux);
                
                return Aux>0 ? Aux : InvalidValue;

            }
            catch
            {
                throw;
            }
          
        }

        public async Task<string> ToExecuteScalarString()
        {
            const string InvalidValue = "";

            if (Connection == null || Command == null)
            {
                return InvalidValue;
            }
            

            Command.Connection = Connection;
            try
            {
                await Connection.OpenAsync();
                var  Scalar = await Command.ExecuteScalarAsync();
                
                return Scalar != null ? Scalar.ToString() : InvalidValue;

            }
            catch
            {
                throw;
            }
          
        }
    

        public async ValueTask DisposeAsync()
        {
            if (Reader != null)
            {
                await Reader.CloseAsync();
            }
            if (Command != null)
            {
                await Command.DisposeAsync();
            }
            if (Connection != null)
            {
                await Connection.CloseAsync();
            }
        }


    }
}
