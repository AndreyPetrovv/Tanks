using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Tanks
{
    class BodyTank : Tank 
    {
        private int _health ;
        public int Health {

            get {
                return _health;

            }
            set {
                _health = value;
            }
        }

        Rectangle rectangle;

        public BodyTank(int x, int y) : base()
        {
            drawing();
        }

        public override  void drawing()
        {
            base.drawing();
            rectangle = new Rectangle();
            rectangle.Width = 50;
            rectangle.Height = 50;
            rectangle.Fill = Brushes.DimGray;
            rectangle.StrokeThickness = 3;
            rectangle.Stroke = Brushes.Black;
            rectangle.StrokeDashArray = new DoubleCollection {  };
        }

        public Rectangle GetBody() {
            return rectangle;
        }

    }
}
