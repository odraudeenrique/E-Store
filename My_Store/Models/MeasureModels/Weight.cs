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
            Result<float> AuxNumber = Helper.IsGreaterThanZero(WeightNumber);
            Result<string> AuxUnit = Helper.ToValidateString(WeightUnit);

            if (!AuxNumber.IsValid)
            {
                return;
            }

            if ((!ToGetUnit(AuxUnit.Value)) && (AuxUnit.IsValid))
            {
                return;
            }
            this.WeightUnit = AuxUnit.Value;
            this.WeightNumber = AuxNumber.Value;


        }
        private bool ToGetUnit(string Unit)
        {
            switch (Unit)
            {
                case "lb":
                    this.WeightUnit = "lb";
                    return true;
                case "oz":
                    this.WeightUnit = "oz";
                    return true;
                case "ton":
                    this.WeightUnit = "ton";
                    return true;
                case "long ton":
                    this.WeightUnit = "long ton";
                    return true;
                case "kg":
                    this.WeightUnit = "kg";
                    return true;
                case "g":
                    this.WeightUnit = "g";
                    return true;
                default:
                    this.WeightUnit = "";
                    return false;
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
