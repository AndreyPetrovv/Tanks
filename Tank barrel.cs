using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Tanks
{
    class Tank_barrel : Tank
    {
        private int _directionRelativeToX;
        private int _directionRelativeToY;
        public int DirectionRelativeToX {
            get {
                return _directionRelativeToX;
            }
            set {
                _directionRelativeToX = value;
            }
        }
        public int DirectionRelativeToY {
            get
            {
                return _directionRelativeToY;
            }
            set
            {
                _directionRelativeToY = value;
            }
        }
        private void Changedirection(int x, int y) { }

        public Tank_barrel(int x, int y) : base()
        {
            DirectionRelativeToX = 0;
            DirectionRelativeToY = -1;
            drawing();
        }

        Line line;
        public override void drawing()
        {
            base.drawing();
            line = new Line();
            line.X1 = PositionToX+25;
            line.Y1 = PositionToY+25;
            line.X2 = PositionToX+25;
            line.Y2 = PositionToY+25;
            for (int i = 0; i < 45; i++)
            {
                line.X2 += DirectionRelativeToX;
                line.Y2 += DirectionRelativeToY;
            }
            line.Stroke = Brushes.Blue;
            line.StrokeThickness = 10;
        }

        public Line GetBarrel()
        {
            return line;
        }

        public override void Shot()
        {
            base.Shot();
        }

    }
}
