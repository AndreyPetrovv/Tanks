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
        Image Image;
        public MapObject(double width, double height)
        {
            Image = new Image();
            
            Image.Width = width;
            Image.Height = height;
        }
        public MapObject() { }
        public bool IsCheckRange(Tank tankOne, Tank tankTwo, bool isCheck)
        {
            if (isCheck)
            {
                if (tankTwo.GetCoordinates.CordinateToX + 29 >= X && tankOne.GetCoordinates.CordinateToX + 29 <= (X + 102))
                    if (tankOne.GetCoordinates.CordinateToY < Y && Y < tankTwo.GetCoordinates.CordinateToY)
                        return false;
                    else if (tankOne.GetCoordinates.CordinateToY > Y && Y > tankTwo.GetCoordinates.CordinateToY)
                        return false;
            }
            else
            {
                if (tankTwo.GetCoordinates.CordinateToY + 29 >= Y && tankTwo.GetCoordinates.CordinateToY + 29 <= (Y + 102))
                    if (tankOne.GetCoordinates.CordinateToX < X && X < tankTwo.GetCoordinates.CordinateToX)
                        return false;
                    else if (tankOne.GetCoordinates.CordinateToX > X && X > tankTwo.GetCoordinates.CordinateToX)
                        return false;
            }
            return true;
        }
        public bool IsCheckMove(int objectX, int objectY, double n, Tank tank)
        {
            if (Math.Abs((objectX + n) - (X + 55)) >= (55 + n) ||
                Math.Abs((objectY + n) - (Y + 51)) >= (51 + n))
            {
                if (Math.Abs((objectX + n) - (X + 51)) >= (51 + n))
                {
                    tank.Touch = Convert.ToInt32(Math.Abs((objectX + n) - (X + 51)) - (51 + n));
                }
                if (Math.Abs((objectY + n) - (Y + 51)) >= (51 + n))
                {
                    tank.Touch = Convert.ToInt32(Math.Abs((objectY + n) - (Y + 51)) - (51 + n));
                }
                return true;
            }
            return false;
        }// Тестовый вариант !!! НОВОЕ
        public bool IsCheckMove(int objectX, int objectY, double n)
        {
            if (Math.Abs((objectX + n) - (X + 55)) >= (55 + n) ||
                Math.Abs((objectY + n) - (Y + 51)) >= (51 + n))
                return true;
            return false;
        }

        public void SetImage(string type)
        {
            if (type == "0")
                Image.Source = new BitmapImage(new Uri("pack://application:,,,/ImageMap/tileSand2.png"));
            else
                Image.Source = new BitmapImage(new Uri("pack://application:,,,/ImageMap/crateMetal.png"));
        }
        public Image GetImage()
        {
            return Image;
        }

    }
}
