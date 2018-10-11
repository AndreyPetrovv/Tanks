using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Tanks 
{
    public class Tank 
    {

        private string _name;
        private int _positionToX;
        private int _positionToY;
        public int PositionToX
        {
            get
            {
                return _positionToX;
            }
            set
            {
                _positionToX = value;
            }
        }
        public int PositionToY
        {
            get
            {
                return _positionToY;
            }
            set
            {
                _positionToY = value;
            }
        }

        private string Name {
            get
            {
                return _name;
            }
            set {
                _name = value;
            }
        }

        BodyTank tankBo ;
        Tank_barrel tankBa ;

        public Tank(string name, int x, int y) {
            PositionToX = x;
            PositionToY = y;
            _name = name;
            tankBo = new BodyTank(x,y);
            tankBa = new Tank_barrel(x, y);
           
        }
        public Tank() { }

        public Rectangle drawingTankBo() {
            
            return tankBo.GetBody();
        }
        public Line drawingTankBa() {
            
            return tankBa.GetBarrel();
        }

        public virtual void drawing() { }

        public void Move(int x, int y) {
            PositionToX += x;
            PositionToY += y;
            tankBo.drawing();
            tankBa.drawing();
        }

        public void barrelRotation(int x, int y) {
            tankBa.DirectionRelativeToX = x;
            tankBa.DirectionRelativeToY = y;
            tankBa.drawing();
        }
        public virtual void Shot() { }

    }
}
