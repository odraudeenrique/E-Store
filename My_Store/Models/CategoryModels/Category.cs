namespace My_Store.Models.CategoryModels
{
    public class Category
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private TypeOfCategory _categoryType;

        private TypeOfCategory CategoryType
        {
            get { return _categoryType; }
            set { _categoryType = value; }
        }

        public Category(int Category)
        {
            ToSetCategory(Category);
        }

        private void ToSetCategory(int Aux)
        {
            switch (Aux)
            {
                case 2:
                    Name = "Electronics";
                    CategoryType = TypeOfCategory.Electronics;
                    break;
                case 3:
                    Name = "Toys";
                    CategoryType = TypeOfCategory.Toys;
                    break;
                case 4:
                    Name = "Clothes";
                    CategoryType = TypeOfCategory.Clothes;
                    break;
                case 5:
                    Name = "Forniture";
                    CategoryType = TypeOfCategory.Forniture;
                    break;
                default:
                    Name = "Default";
                    CategoryType = TypeOfCategory.Default;
                    break;
            }
        }


    }
}
