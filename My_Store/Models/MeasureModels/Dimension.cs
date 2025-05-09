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
            Result<float> ValidatedDepth = Helper.IsGreaterThanZero(Depth);
            Result<float> ValidatedWidth = Helper.IsGreaterThanZero(Width);
            Result<float> ValidatedHeight = Helper.IsGreaterThanZero(Height);

            if ((!ValidatedDepth.IsValid) || (!ValidatedWidth.IsValid) || (!ValidatedHeight.IsValid))
            {
                this.Depth = 0;
                this.Width = 0;
                this.Height = 0;
            }
            s
            this.Depth = ValidatedDepth.Value;
            this.Width = ValidatedWidth.Value;
            this.Height = ValidatedHeight.Value;

        }



    }
}
