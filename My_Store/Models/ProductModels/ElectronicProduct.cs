namespace My_Store.Models.ProductModels
{
    public class ElectronicProduct : BaseProduct
    {

        private int _ram;
        public int Ram
        {
            get { return _ram; }
            set { _ram = value; }
        }

        private int _processor;
        public int Processor
        {
            get { return _processor; }
            set { _processor = value; }
        }

        private int _storagge;
        public int Storagge
        {
            get { return _storagge; }
            set { _storagge = value; }
        }

        private string _screen;
        public string Screen
        {
            get { return _screen; }
            set { _screen = value; }
        }

        private float _baterry;
        public float Baterry
        {
            get { return _baterry; }
            set { _baterry = value; }
        }

        private bool _hasAccessories;
        public bool HasAccessories
        {
            get { return _hasAccessories; }
            set { _hasAccessories = value; }
        }
        private int _numberOfAccessories;
        public int NumberOfAccessories
        {
            get { return _numberOfAccessories; }
            set { _numberOfAccessories = value; }
        }

        public ElectronicProduct(int CategoryType, string Serial) : base(CategoryType, Serial)
        {

        }






    }
}
