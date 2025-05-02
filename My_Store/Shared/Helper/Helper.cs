using My_Store.Models.MeasureModels;

namespace My_Store.Shared.Helper
{
    public static class Helper
    {
        private static bool IsMayorThanZero<T>(T Value)
        {
            try
            {
                decimal number = Convert.ToDecimal(Value);
                return number >= 0 ? true : false;
            }
            catch (InvalidCastException)
            {
                return false;
            }
            catch (FormatException)
            {
                return false;
            }
            catch (OverflowException)
            {
                return false;
            }
        }
        private static bool ToValidateString(string Text)
        {
            if (string.IsNullOrEmpty(Text))
            {
                return false;
            }
            return true;

        }


        public static Result<T> ToValidateNumberMayorThanZero<T>(T Number)
        {
            if (IsMayorThanZero(Number))
            {
                return Result<T>.Successful(Number);

            }
            else
            {
                return Result<T>.Failed("Something wrong happened; please try again");
            }

        }
        public static Result<string> ToValidateIfStringValid(string Text)
        {
            if (!ToValidateString(Text))
            {
                return Result<string>.Failed("The string is not valid");
            }

            return Result<string>.Successful(Text);
        }

        public static Result<Dimension> ToValidateDimension(Dimension Dimension)
        {

            if (!(IsMayorThanZero(Dimension.Depth) && IsMayorThanZero(Dimension.Width) && IsMayorThanZero(Dimension.Height)))
            {
                return Result<Dimension>.Failed("One of the values is not greater than zero");
            }


            return Result<Dimension>.Successful(Dimension);
        }

        public static Result<Weight> ToValidateWeight(Weight Weight)
        {
            if (IsMayorThanZero(Weight.WeightNumber) && ToValidateString(Weight.WeightUnit))
            {
                return Result<Weight>.Failed("One of the values is not valid");
            }

            return Result<Weight>.Successful(Weight);
        }


    }
}

