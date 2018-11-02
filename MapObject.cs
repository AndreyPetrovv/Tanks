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

        public bool IsdCheckRange(int x, int y) {
            if ((x >= X && x <= rectangle.Width) || (y >= Y && y <= rectangle.Height))
                return false;
            return true;
        }
        public bool IsCheckMove(int objectX, int objectY)
        {
            //if (((Math.Abs(objectX - X) < 60) && (Math.Abs(objectY - Y) < 60)))
            //{
            if (Math.Abs((objectX +29) - (X + 60)) >= 89 || Math.Abs((objectY + 29) - (Y + 60)) >= 89)
                return true;
            return false;
            //}
            //return true;
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
