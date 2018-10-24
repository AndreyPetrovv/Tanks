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
using System.Windows.Threading;
using System.Threading;


namespace Tanks
{
    public class Tank
    {
        Image image;
        
        private string _orient="Up";
        private string _name;
        private int _health;
        private int _positionToX;
        private int _positionToY;

        public int Health
        {
            get
            {
                return _health;
            }
            set
            {
                _health = value;
            }
        }
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

        public Tank(string name, int x, int y, string tankOr)
        {
            Health = 1;
            PositionToX = x + 31;
            PositionToY = y + 29;
            _name = name;
            Drawing(tankOr);
        }
        public void Damage()
        {
            Health--;
            if (Health == 0)
            {

                //MessageBox.Show("Вы его убили!");
                //DeadTank();
            }
        }
        private void DeadTank()
        {
            var uri = new Uri("pack://application:,,,/ImageTank/Dead.png");
            var bitmap = new BitmapImage(uri);
            image.Source = bitmap;
        }
        public void Drawing(string tankOr)
        {
            image = new Image();
            image.Width = 60;
            image.Height = 60;
            var uri = new Uri("pack://application:,,,/ImageTank/"+tankOr+".png");
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
        public void Move(string orient,int x, int y, Uri uri)
        {
            PositionToX += x;
            PositionToY += y;
            Orient = orient;
            var bitmap = new BitmapImage(uri);
            image.Source = bitmap;
        }
        public void Shot(ref List<Bullet> windowBullet)
        {
            windowBullet.Add(new Bullet(PositionToX, PositionToY,Orient));
        }
    }
}
