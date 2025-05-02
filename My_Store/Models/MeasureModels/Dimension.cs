using My_Store.Shared;
using My_Store.Shared.Helper;

namespace My_Store.Models.MeasureModels
{
    public class Dimension
    {
        private float _depth;
        public float Depth
        {
            get { return _depth; }
            set { _depth = value; }
        }
        private float _width;
        public float Width
        {
            get { return _width; }
            set { _width = value; }
        }
        private float _height;
        public float Height
        {
            get { return _height; }
            set { _height = value; }
        }




        public Dimension(float Depth, float Width, float Height)
        {
            ToCreateDimension(Depth, Width, Height);
        }




        private void ToCreateDimension(float Depth, float Width, float Height)
        {

            Result<float> AuxDepth = Helper.ToValidateNumberMayorThanZero(Depth);
            Result<float> AuxWidth = Helper.ToValidateNumberMayorThanZero(Width);
            Result<float> AuxHeight = Helper.ToValidateNumberMayorThanZero(Height);

            this.Depth = AuxDepth.IsValid ? AuxDepth.Value : 0;
            this.Width = AuxWidth.IsValid ? AuxWidth.Value : 0;
            this.Height = AuxHeight.IsValid ? AuxHeight.Value : 0;

        }



    }
}
