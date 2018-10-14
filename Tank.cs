using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media.Imaging;


namespace Tanks
{
    public class Tank
    {
        Bullet bullet;
        Image image;
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

        private string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public Tank(string name, int x, int y)
        {
            PositionToX = x;
            PositionToY = y;
            _name = name;
        }

        public void Drawing()
        {
            image = new Image();
            image.Width = 60;
            image.Height = 60;
            var uri = new Uri("pack://application:,,,/ImageTank/танк.png");
            var bitmap = new BitmapImage(uri);
            image.Source = bitmap;
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(MainWindow))
                {
                    (window as MainWindow).canvas.Children.Add(image);
                }
            }

        }
        public Image GetImage()
        {
            return image;
        }
        public void Move(int x, int y, Uri uri)
        {
            PositionToX += x;
            PositionToY += y;

            var bitmap = new BitmapImage(uri);
            image.Source = bitmap;
        }
        public void Shot()
        {
            bullet = new Bullet(PositionToX, PositionToY);
        }
        public Image FlightShot()
        {
            bullet.flight(0, -1);
            return bullet.getImage();
        }

        public Bullet GetBulll()
        {
            return bullet;
        }
    }
}
