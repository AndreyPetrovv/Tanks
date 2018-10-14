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
        Image image;
        private int _bulletPositionToX;
        private int _bulletPositionToY;
        public int BulletPositionToX
        {
            get
            {
                return _bulletPositionToX;
            }
            set
            {
                _bulletPositionToX = value;
            }
        }
        public int BulletPositionToY
        {
            get
            {
                return _bulletPositionToY;
            }
            set
            {
                _bulletPositionToY = value;
            }
        }

        public Bullet(int positionToX,int positionToY) {
            BulletPositionToX = positionToX;
            BulletPositionToY = positionToY;
            drawing();
        }

        private void drawing()
        {
            image = new Image();
            image.Width = 9;
            image.Height = 5;
            var uri = new Uri("pack://application:,,,/ImageTank/Bullet.png");
            var bitmap = new BitmapImage(uri);
            image.Source = bitmap;

            //foreach (Window window in Application.Current.Windows)
            //{
            //    if (window.GetType() == typeof(MainWindow))
            //    {
                    
                    
            //        (window as MainWindow).canvas.Children.Add(image);
            //    }
            //}
        }
        public Image getImage()
        {
            return image;
        }

        public void flight(int x, int y) {
            BulletPositionToX += x;
            BulletPositionToY += y;
        }
    }
}
