using My_Store.Shared.Helper;
using My_Store.Models.CategoryModels;

namespace My_Store.Models.ProductModels
{
    public abstract class BaseProduct
    {
        private readonly string _id;

        public string Id
        {
            get { return _id; }
        }
        private string _serialNumber;
        protected string SerialNumber
        {
            get { return _serialNumber; }
            set { _serialNumber = value; }
        }

        private string _brand;
        protected string Brand
        {
            get { return _brand; }
            set { _brand = value; }
        }
        private string _name;
        protected string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _model;
        protected string Model
        {
            get { return _model; }
            set { _model = value; }
        }

        private string _description;
        protected string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        private string _color;
        protected string Color
        {
            get { return _color; }
            set { _color = value; }
        }
        private string _material;
        protected string Material
        {
            get { return _material; }
            set { _material = value; }
        }

        private int _unitPerPackage;
        protected int UnitPerPackage
        {
            get { return _unitPerPackage; }
            set { _unitPerPackage = value; }
        }

        private bool _guarantee;
        protected bool Guarantee
        {
            get { return _guarantee; }
            set { _guarantee = value; }
        }

        private float _guaranteeTime;
        protected float GuaranteeTime
        {
            get { return _guaranteeTime; }
            set
            {
                _guaranteeTime = value;

            }
        }

        private decimal _price;

        protected decimal Price
        {
            get
            {
                return _price;
            }
            set
            {
                _price = value;
            }
        }
        
        private Category _category;
        public Category Category
        {
            get { return _category; }
            set { _category = value; }
        }
        private int _categoryNumber;
        public int CategoryNumber
        {
            get { return _categoryNumber; }
            set
            {
                _categoryNumber = value;
            }

        }


        public BaseProduct(int CategoryType, string Serial)
        {
            Category = new Category(CategoryType);

            Result<string> AuxSerial = Helper.ToValidateString(Serial);

            if (!AuxSerial.IsValid)
            {
                SerialNumber=AuxSerial.Error;
            }
            SerialNumber = AuxSerial.Value;


        }








    }
}
