using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Tanks
{
    public class MapObject : Shape
    {
        private int _width;
        private int _height;
        private int _x;
        private int _y;
        public int WidthRange
        {
            get {
                return _width;
            }
            set {
                _width = value;
            }
        }
        public int HeightRange
        {
            get
            {
                return _height;
            }
            set
            {
                _height = value;
            }
        }
        public int X
        {
            get
            {
                return _x;
            }
            set
            {
                _x = value;
            }
        }
        public int Y
        {
            get
            {
                return _y;
            }
            set
            {
                _y = value;
            }
        }
        Rectangle rectangle = new Rectangle();
        MapObject(int x, int y)
        {
            X = x;
            Y = y;
        }

        void PlottingWidthAndHeight(int n, int width, int height)
        {
            rectangle.Width = width;
            rectangle.Height = height;
        }

        private bool IsdCheckRange(int x, int y) {
            if (x >= X && x <= rectangle.Width)
                return false;
            if (y >= Y && y <= rectangle.Height)
                return false;
            return true;
        }

        protected override Geometry DefiningGeometry => throw new NotImplementedException();
    }
}
