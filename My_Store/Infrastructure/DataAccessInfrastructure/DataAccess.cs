using Microsoft.Data.SqlClient;
using My_Store.Shared;
using Microsoft.Extensions.Configuration;
using System.IO;
using My_Store.Shared.Helper;



namespace My_Store.Infrastructure.DataAccessInfrastructure
{
    public class DataAccess : IDisposable
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
            Result<string> Aux = Helper.ToValidateIfStringValid(ConnectionString);

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
            Result<string> Aux = Helper.ToValidateIfStringValid(Procedure);

            if (!Aux.IsValid || Command == null)
            {
                return;
            }
            Command.CommandType = System.Data.CommandType.StoredProcedure;
            Command.CommandText = Aux.Value;
        }


        public void ToSetQuery(string Query)
        {
            Result<string> Aux = Helper.ToValidateIfStringValid(Query);
            if (!Aux.IsValid || Command == null)
            {
                return;
            }
            Command.CommandType = System.Data.CommandType.Text;
            Command.CommandText = Aux.Value;
        }


        public void ToSetParameters(string Name, object Value)
        {
            Result<string> ParameterName = Helper.ToValidateIfStringValid(Name);

            if (!ParameterName.IsValid || Value == null)
            {
                return;
            }

            Command.Parameters.AddWithValue(ParameterName.Value, Value);
        }



        public SqlDataReader ToRead()
        {
            if (Connection == null || Command == null)
            {
                return null;
            }

            Command.Connection = Connection;
            try
            {
                Connection.Open();
                Reader = Command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                return Reader;
            }
            catch
            {
                throw;
            }
            finally
            {
                if (Reader == null)
                {
                    Dispose();
                }
            }

        }


        public void ToExecute()
        {
            if (Connection == null || Command == null)
            {
                return;
            }

            Command.Connection = Connection;
            try
            {
                Connection.Open();
                Command.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
            finally
            {
                Dispose();
            }
        }


        public int ToExecuteScalarInt()
        {
            if (Connection == null || Command == null)
            {
                int InvalidValue = -1;
                return InvalidValue;
            }


            Command.Connection = Connection;
            try
            {
                Connection.Open();
                return int.Parse(Command.ExecuteScalar().ToString());

            }
            catch
            {
                throw;
            }
            finally
            {
                Dispose();
            }
        }

        public string ToExecuteScalarString()
        {
            if (Connection == null || Command == null)
            {
                string InvalidValue = "";
                return InvalidValue;
            }


            Command.Connection = Connection;
            try
            {
                Connection.Open();
                string QueryResult = Command.ExecuteScalar().ToString();
                return QueryResult != null ? QueryResult : "";

            }
            catch
            {
                throw;
            }
            finally
            {
                Dispose();
            }
        }

        public void Dispose()
        {
            if (Reader != null)
            {
                Reader.Close();
            }
            if (Command != null)
            {
                Command.Dispose();
            }
            if (Connection != null)
            {
                Connection.Close();
            }
        }


    }
}
