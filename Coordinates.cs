using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tanks
{
    public class Coordinates
    {
        private int _cordinateToX;
        private int _cordinateToY;
        public int CordinateToX {
            get => _cordinateToX;
            set => _cordinateToX = value;
        }
        public int CordinateToY
        {
            get => _cordinateToY;
            set => _cordinateToY = value;
        }

        public Coordinates(int CordinateToX, int CordinateToY) {
            this.CordinateToX = CordinateToX;
            this.CordinateToY = CordinateToY;
        }
    }
}
