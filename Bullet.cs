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
        private string _orient;
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
        public string Orient
        {
            get
            {
                return _orient;
            }
            set
            {
                _orient = value;
            }
        }

        public Bullet(int positionToX,int positionToY,string orient) {
            BulletPositionToX = positionToX + 25;
            BulletPositionToY = positionToY;
            Orient = orient;
            switch (Orient)
            {
                case "Up":
                    BulletPositionToX = positionToX + 25;
                    BulletPositionToY = positionToY;
                    break;
                case "Down":
                    BulletPositionToX = positionToX + 25;
                    BulletPositionToY = positionToY + 62; 
                    break;
                case "Left":
                    BulletPositionToX = positionToX;
                    BulletPositionToY = positionToY + 25;
                    break;
                case "Right":
                    BulletPositionToX = positionToX +58 ;
                    BulletPositionToY = positionToY + 25;
                    break;
            }
            drawing();
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


        public Image GetImage()
        {
            return image;
        }

        public void flight() {
            int i = 3;
            switch (Orient)
            {
                case "Up":
                    BulletPositionToY -= i;
                    break;
                case "Down":
                    BulletPositionToY += i;
                    break;
                case "Left":
                    BulletPositionToX -= i;
                    break;
                case "Right":
                    BulletPositionToX += i;
                    break;
            }

        }
    }
}
