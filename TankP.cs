using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tanks
{
    class TankP : Tank
    {
        public DateTime ShotDelay;

        public TankP(string name, int x, int y, string tankOr) : base (name,x,y,tankOr)
        {
            Health = 4;
            PositionToX = x;
            PositionToY = y;
            PreviousPositionToX = PositionToX;
            PreviousPositionToY = PositionToY;
            Name = name;
        }
    }
}
