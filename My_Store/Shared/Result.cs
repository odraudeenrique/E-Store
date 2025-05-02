namespace My_Store.Shared
{
    public class Result<T>
    {
        private bool _isValid;
        public bool IsValid
        {
            get { return _isValid; }
            private set { _isValid = value; }
        }
        private T? _value;
        public T? Value
        {
            get { return this._value; }
            private set { this._value = value; }
        }
        private string? _error;
        public string? Error
        {

            get { return this._error; }
            private set { this._error = value; }

        }

        public static Result<T> Successful(T value)
        {
            Result<T> Result = new Result<T>();
            Result.Value = value;
            Result.IsValid = true;
            return Result;
        }

        public static Result<T> Failed(string Error)
        {
            Result<T> Result = new Result<T>();
            Result.IsValid = false;
            Result.Error = Error;

            return Result;
        }


    }
}
