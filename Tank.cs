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
        
        private string _orient="Down";
        private string _name;
        private int _health;
        private double _timeTurn;
        private int _positionToX;
        private int _positionToY;
        private int _previousPositionX;
        private int _previousPositionY;

        public double TimeTurn {
            get {
                return _timeTurn;
            }
            set {
                _timeTurn = value;
            }
        }
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
        public int PreviousPositionToX
        {
            get
            {
                return _previousPositionX;
            }
            set
            {
                _previousPositionX = value;
            }
        }
        public int PreviousPositionToY
        {
            get
            {
                return _previousPositionY;
            }
            set
            {
                _previousPositionY = value;
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
            Health = 4;
            PositionToX = x + 31;
            PositionToY = y + 29;
            PreviousPositionToX = PositionToX;
            PreviousPositionToY = PositionToY;
            _name = name;
            Drawing(tankOr);
        }
        public void Damage()
        {
            Health--;
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
            foreach (Window window in  Application.Current.Windows)
            {
                if (window.GetType() == typeof(MainWindow))
                {
                    (window as MainWindow).canvas.Children.Add(image);
                }
            }

        }
        public void AddUri(Uri uri) {
            image.Source = new BitmapImage(uri);
        }
        public Image GetImage()
        {
            return image;
        }
        public void Move(string orient,int x, int y, Uri uri)
        {
            PreviousPositionToX = PositionToX;
            PreviousPositionToY = PositionToY;
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
