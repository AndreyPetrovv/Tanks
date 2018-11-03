using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;


namespace Tanks
{
    public class MapObject 
    {
        private int _x;
        private int _y;
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
        Rectangle rectangle;
        public MapObject(int x, int y, int width, int height)
        {
            rectangle = new Rectangle();
            X = x ;
            Y = y ;
            rectangle.Width = width;
            rectangle.Height = height;
            drawing();
        }

        public bool IsCheckRange(Tank tankOne, Tank tankTwo, bool isCheck)
        {
            if (isCheck)
            {
                if (tankTwo.PositionToX + 29 >= X && tankOne.PositionToX + 29 <= (X + rectangle.Width))
                    if (tankOne.PositionToY < Y && Y < tankTwo.PositionToY)
                        return false;
                    else if (tankOne.PositionToY > Y && Y > tankTwo.PositionToY)
                        return false;
            }
            else
            {
                if (tankTwo.PositionToY + 29 >= Y && tankTwo.PositionToY + 29 <= (Y + rectangle.Height))
                    if (tankOne.PositionToX < X && X < tankTwo.PositionToX)
                        return false;
                    else if (tankOne.PositionToX > X && X > tankTwo.PositionToX)
                        return false;
            }
            return true;
        }
        public bool IsCheckMove(int objectX, int objectY,double n)
        {
            if (Math.Abs((objectX + n) - (X + rectangle.Width / 2)) >= (rectangle.Width / 2 + n) || Math.Abs((objectY + n) - (Y + rectangle.Height/2)) >= (rectangle.Height/2 + n))
                return true;
            return false;
        }
        private void drawing() {

            rectangle.Fill = Brushes.Green;

            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(MainWindow))
                {
                    (window as MainWindow).canvas.Children.Add(rectangle);
                }
            }
        }

        public Rectangle GetRectangle() {
            return rectangle;
        }
      
    }
}
