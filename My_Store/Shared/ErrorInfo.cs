namespace My_Store.Shared
{
    public  class ErrorInfo
    {
        private readonly string _id;
        private string Id { get { return this._id; } }
        private string _tittle;
        public string Tittle {  get { return this._tittle; } private set { this._tittle = value; } }
        private string _message;
        public string Message { get { return this._message; } private set { this._message = value; } }
        private string _stackTrace;
        public string StackTrace { get { return this._stackTrace; } private set { this._stackTrace = value; } }
        private string _source;
        public string Source { get { return this._source; } private set { this._source = value; } }
        private DateTime _dateCreated;
        public DateTime DateCreated{ get { return this._dateCreated; }}

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
