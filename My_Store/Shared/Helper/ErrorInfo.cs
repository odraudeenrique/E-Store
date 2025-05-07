namespace My_Store.Shared.Helper
{
    public class ErrorInfo
    {
        private readonly string _id;
        private string Id { get { return _id; } }
        private string _tittle;
        public string Tittle { get { return _tittle; } private set { _tittle = value; } }
        private string _message;
        public string Message { get { return _message; } private set { _message = value; } }
        private string _stackTrace;
        public string StackTrace { get { return _stackTrace; } private set { _stackTrace = value; } }
        private string _source;
        public string Source { get { return _source; } private set { _source = value; } }
        private DateTime _dateCreated;
        public DateTime DateCreated { get { return _dateCreated; } }

        private ErrorInfo() { }


        public static ErrorInfo FromException(Exception Ex, string CustomTittle = "Unhandled Exception")
        {
            return new ErrorInfo
            {
                Tittle = CustomTittle,
                Message = Ex.Message,
                StackTrace = Ex.StackTrace,
                Source = Ex.Source,
            };

        }
    }
}
