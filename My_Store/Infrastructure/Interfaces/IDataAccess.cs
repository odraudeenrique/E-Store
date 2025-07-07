using Microsoft.Data.SqlClient;

namespace My_Store.Infrastructure.Interfaces
{
    public interface IDataAccess
    {
        //Acá tengo que definir lo métodos y aplicarle la inyección de dependencia.
        void  ToSetProcedure(string Procedure);
        void ToSetQuery(string Query);
        void ToSetParameters(string Name, Object Value);
        Task<SqlDataReader> ToRead();
        Task ToExecute();
        Task<SqlDataReader> ToExecuteWithResult(); 
        Task<int> ToExecuteScalarInt();
        Task<string>ToExecuteScalarString();
        ValueTask DisposeAsync();



    }
}
