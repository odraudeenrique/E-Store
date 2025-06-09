using My_Store.Models.MeasureModels;
using My_Store.Models.UserModels;
using My_Store.Shared.SecurityHelper;
using System.Diagnostics.Metrics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;


namespace My_Store.Shared.Helper
{
    public static class Helper
    {
        public static Result<T> IsGreaterThanZero<T>(T Value)
        {
            try
            {
                decimal Number = Convert.ToDecimal(Value);

                if (!(Number > 0))
                {
                    return Result<T>.Failed("The number is minor than zero");
                }

                return Result<T>.Successful(Value);
            }
            catch (InvalidCastException)
            {
                return Result<T>.Failed("The convertion failed");
            }
            catch (FormatException)
            {
                return Result<T>.Failed("The format is invalid");
            }
            catch (OverflowException)
            {
                return Result<T>.Failed("Overflow exception");
            }
        }

        public static Result<string> ToValidateString(string Text)
        {


            if (string.IsNullOrWhiteSpace(Text))
            {
                return Result<string>.Failed("The string is empty");
            }
            return Result<string>.Successful(Text);

        }

        public static Result<(float Depth, float Width, float Height)> ToValidateDimension(Dimension Dimension)
        {
            Result<float> ValidatedDepth = IsGreaterThanZero(Dimension.Depth);
            Result<float> ValidatedWidth = IsGreaterThanZero(Dimension.Width);
            Result<float> ValidatedHeight = IsGreaterThanZero(Dimension.Height);

            if (!ValidatedDepth.IsValid)
            {
                return Result<(float, float, float)>.Failed("The Depth is minor than zero");
            }
            if (!ValidatedWidth.IsValid)
            {
                return Result<(float, float, float)>.Failed("The width is minor than zero");
            }
            if (!ValidatedHeight.IsValid)
            {
                return Result<(float, float, float)>.Failed("The Height is minor than zero");
            }


            return Result<(float, float, float)>.Successful((ValidatedDepth.Value, ValidatedWidth.Value, ValidatedHeight.Value));


        }

        public static Result<(float WeightNumber, string WeightUnit)> ToValidateWeight(Weight Weight)
        {
            Result<float> ValidatedWeightNumber = IsGreaterThanZero(Weight.WeightNumber);
            Result<string> ValidatedWeightUnit = ToValidateString(Weight.WeightUnit);

            if (!ValidatedWeightNumber.IsValid)
            {
                return Result<(float, string)>.Failed("The weight number is minor than zero");
            }
            if (!ValidatedWeightUnit.IsValid)
            {
                return Result<(float, string)>.Failed("The weight unit is not valid");
            }

            return Result<(float, string)>.Successful((ValidatedWeightNumber.Value, ValidatedWeightUnit.Value));

        }

        public static Result<(string UserEmail, string PasswordHash)> ToValidateUserCredentials(string Email, string Password)
        {
            Result<string> ValidatedEmail = ToValidateString(Email);
            Result<string> ValidatedPassword = ToValidateString(Password);

            if (!ValidatedEmail.IsValid)
            {
                return Result<(string, string)>.Failed("The email is not valid");
            }
            if (ValidatedEmail.Value.Length > 255)
            {
                return Result<(string, string)>.Failed("The email has more than 255 characters");
            }
            if (!ValidatedPassword.IsValid)
            {
                return Result<(string, string)>.Failed("The password is not valid");
            }

            if ((ValidatedPassword.IsValid) && (ValidatedPassword.Value.Length < 8))
            {
                return Result<(string, string)>.Failed("The password must have 8 or more characteres");
            }

            return Result<(string, string)>.Successful((ValidatedEmail.Value, ValidatedPassword.Value));

        }


        public static Result<string?> ToValidateUserName(Object? Aux)
        {
            if(Aux is null or DBNull)
            {
                return Result<string?>.Successful(null);

            }

            string? NameValue=Aux.ToString();
            if (string.IsNullOrWhiteSpace(NameValue))
            {
                return Result<string?>.Successful(null);
            }


            if (NameValue.Length > 100)
            {
                return Result<string?>.Failed("It should have less than 100 characters");
            }
             
            return Result<string?>.Successful(NameValue);

        }

        public static Result<string?> ToValidateProfilePicture(Object? Picture)
        {
            if(Picture is null or DBNull)
            {
                return Result<string?>.Successful(null);
            }

            string? PictureValue = Picture.ToString();
            if (string.IsNullOrWhiteSpace(PictureValue))
            {
                return Result<string?>.Successful(null);
            }
            
            if (PictureValue.Length > 500)
            {
                return Result<string?>.Failed("The URL picture should have less than 500 characters");
            }

            return Result<string?>.Successful(PictureValue); 
        }

        public static Result<DateTime?> ToValidateUserBirthday(Object? Date)
        {
            if (Date is null or DBNull)
            {
                return Result<DateTime?>.Successful(null);
            }

            if (Date is not DateTime DateValue)
            {
                return Result<DateTime?>.Failed("Invalid date format");
            }
        
            if (DateValue > DateTime.Now)
            {
                return Result<DateTime?>.Failed("The date is greater than the actual date");
            }
            if (DateValue < DateTime.Now.AddYears(-105))
            {
                return Result<DateTime?>.Failed("The date is not valid because it's too long ago");
            }
            if (DateValue > DateTime.Now.AddYears(-5))
            {
                return Result<DateTime?>.Failed("The date is too soon to be able to register");
            }

            return Result<DateTime?>.Successful(DateValue);
        }

        public static Result<string> ToValidateUserEmail(string Email)
        {
            Result<string>ValidatedEmail= ToValidateString(Email);
            if (!ValidatedEmail.IsValid)
            {
                return Result<string>.Failed("The email is empty");
            }

            if ((!ValidatedEmail.Value.Contains("@") )|| (!ValidatedEmail.Value.Contains(".com")))
            {
                return Result<string>.Failed("The email format is not valid");
            }

            return Result<string>.Successful(Email);
        }
      
        
        //Aca tengo que separar la creación del hash en otro método y mantener la validación de usuario separada de este
        public static Result<string> ToValidateUserCredentials(string Password)
        {
            Result<string> ValidatedPassword = ToValidateString(Password);
            if (!ValidatedPassword.IsValid)
            {
                return Result<string>.Failed("The password field is empty ");
            }
            if ((ValidatedPassword.IsValid) && (ValidatedPassword.Value.Length < 8))
            {
                return Result<string>.Failed("The password must have 8 or more characteres");
            }

            Result<string> HashPassword = SecurityHelper.SecurityHelper.ToGetPasswordHash(ValidatedPassword.Value);
            if (!HashPassword.IsValid)
            {
                return Result<string>.Failed(HashPassword.Error);
            }

            return Result<string>.Successful(HashPassword.Value);
        }

        public static Result<TypeOfUser> GetUserType(int UserType)
        {
            if (!(UserType > 0))
            {
                return Result<TypeOfUser>.Failed("Invalid user type");   
            }


            switch (UserType)
            {
                case 1:
                    return Result<TypeOfUser>.Successful(TypeOfUser.Regular);
                case 2:
                    return Result<TypeOfUser>.Successful(TypeOfUser.Admin);
                case 3:
                    return Result<TypeOfUser>.Successful(TypeOfUser.Master);
                default:
                    return Result<TypeOfUser>.Successful(TypeOfUser.Invalid);

            }
        }
        public static Result<TypeOfUser>ToValidateUserType(TypeOfUser UserType)
        {

            if (UserType == null)
            {
                return Result<TypeOfUser>.Failed("The user's type is null");
            }
            switch (UserType)
            {
                case TypeOfUser.Regular:
                    return Result<TypeOfUser>.Successful(TypeOfUser.Regular);
                case TypeOfUser.Admin:
                    return Result<TypeOfUser>.Successful(TypeOfUser.Admin);
                case TypeOfUser.Master:
                    return Result<TypeOfUser>.Successful(TypeOfUser.Master);
                default:
                    return Result<TypeOfUser>.Successful(TypeOfUser.Invalid);

            }
        }


    }
}

