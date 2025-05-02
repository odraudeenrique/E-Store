using My_Store.Shared;
using My_Store.Shared.Helper;

namespace My_Store.Models.MeasureModels
{
    public class Weight
    {
        private float _weightNumber;
        public float WeightNumber
        {
            get { return _weightNumber; }
            private set { _weightNumber = value; }
        }

        private string _weightUnit;
        public string WeightUnit
        {
            get { return _weightUnit; }
            private set { _weightUnit = value; }
        }

        public Weight(float Number, string Unit)
        {
            ToCreateWeight(Number, Unit);
        }

        private void ToCreateWeight(float WeightNumber, string WeightUnit)
        {
            Result<float> AuxNumber = Helper.ToValidateNumberMayorThanZero(WeightNumber);
            this.WeightNumber = AuxNumber.IsValid ? AuxNumber.Value : 0;

            Result<string> AuxUnit = Helper.ToValidateIfStringValid(WeightUnit);

            if (ToGetUnit(AuxUnit.Value) != "" && AuxUnit.IsValid)
            {
                this.WeightUnit = AuxUnit.Value;
            }
            else
            {
                this.WeightUnit = "";
            }


        }
        private string ToGetUnit(string Unit)
        {
            switch (Unit)
            {
                case "lb":
                    return Unit;
                case "oz":
                    return Unit;
                case "ton":
                    return Unit;
                case "long ton":
                    return Unit;
                case "kg":
                    return Unit;
                case "g":
                    return Unit;
                default:
                    return "";
            }

        }


        public override string ToString()
        {
            if (!(WeightNumber > 0 || WeightUnit != ""))
            {
                return "";
            }
            return $"{WeightNumber.ToString()} {WeightUnit} ";
        }

    }
}
