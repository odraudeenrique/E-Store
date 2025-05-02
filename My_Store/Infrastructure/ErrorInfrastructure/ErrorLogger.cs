using Microsoft.OpenApi.MicrosoftExtensions;
using My_Store.Shared;
using My_Store.Infrastructure.DataAccessInfrastructure;

namespace My_Store.Infrastructure.ErrorInfrastructure
{
    public class ErrorLogger
    {

        public static void LogToDatabase(ErrorInfo Error)
        {
            try
            {

                DataAccess Data = new DataAccess();

                Data.ToSetProcedure("StoredToInsertError");

                Data.ToSetParameters("@Title", Error.Tittle);
                Data.ToSetParameters("@Message", Error.Message);
                Data.ToSetParameters("@StackTrace", Error.StackTrace);
                Data.ToSetParameters("@Source", Error.Source);

                Data.ToExecute();

            }
            catch
            {
                throw;
            }
        }

        //public List<ErrorInfo> ToGetAllErrors()
        //{
        //    return new List<ErrorInfo>();
        //}




    }
}
