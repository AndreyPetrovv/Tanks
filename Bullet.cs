using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Threading;

namespace Tanks
{
    public class Bullet 
    {
        private Image image;
        private  Coordinates CoordinatesBullet;
        private int _flightBulletToX = 0;
        private int _flightBulletToY = 0;
        public Bullet(int positionToX, int positionToY, string orient)
        {
            CoordinatesBullet = new Coordinates(positionToX + 25, positionToY);
            findingOrintation(orient);
            drawing();
        }
        private void findingOrintation(string orient) {
            int i = 13;
            switch (orient)
            {
                case "Up":
                    CoordinatesBullet.CordinateToY = CoordinatesBullet.CordinateToY - 10;
                    _flightBulletToY = -i;
                    break;
                case "Down":
                    CoordinatesBullet.CordinateToY = CoordinatesBullet.CordinateToY + 62;
                    _flightBulletToY = i;
                    break;
                case "Left":
                    CoordinatesBullet.CordinateToX = CoordinatesBullet.CordinateToX - 29;
                    CoordinatesBullet.CordinateToY = CoordinatesBullet.CordinateToY + 25;
                    _flightBulletToX = -i;
                    break;
                case "Right":
                    CoordinatesBullet.CordinateToX = CoordinatesBullet.CordinateToX + 29;
                    CoordinatesBullet.CordinateToY = CoordinatesBullet.CordinateToY + 25;
                    _flightBulletToX = i;
                    break;
            }
        }
        private void drawing()
        {
            image = new Image();
            image.Width = 9;
            image.Height = 9;
            var uri = new Uri("pack://application:,,,/ImageTank/Bullet.png");
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
        public Coordinates GetCoordinates {
            get =>  CoordinatesBullet;
        }
        public Image GetImage
        {
            get => image;
        }
        public void flight() {
            CoordinatesBullet.CordinateToX += _flightBulletToX;
            CoordinatesBullet.CordinateToY += _flightBulletToY;
        }
    }
}
