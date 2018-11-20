using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;


namespace Tanks
{
    public class Tank
    {

        protected Image image;
        private string _orient = "Up";
        private string _name;
        private int _health;
        private int _clip;
        private int _previousPositionX;
        private int _previousPositionY;

        public DateTime RecgargeShot;
        protected Coordinates coordinatesTank;

        public int Clip
        {
            get
            {
                return _clip;
            }
            set
            {
                _clip = value;
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
        protected int PreviousPositionToX
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
        protected int PreviousPositionToY
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
        public string Name
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
            coordinatesTank = new Coordinates(x, y);
            Health = 4;
            Clip = 3;
            PreviousPositionToX = coordinatesTank.CordinateToX;
            PreviousPositionToY = coordinatesTank.CordinateToY;
            Name = name;
            Drawing(tankOr);
        }
        public Tank() { }
        public void Damage()
        {
            Health--;
        }// молучение урона
        public void Drawing(string tankOr)
        {
            image = new Image();
            image.Width = 58;
            image.Height = 58;
            var uri = new Uri("pack://application:,,,/ImageTank/" + tankOr + ".png");
            var bitmap = new BitmapImage(uri);
            image.Source = bitmap;
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(MainWindow))
                {
                    (window as MainWindow).canvas.Children.Add(image);
                }
            }

        } // отрисовка и настройка обекта
        public Image GetImage() {
            return image;
        }
        public Coordinates GetCoordinates
        {
            get => coordinatesTank;
        }// возвращает кординаты
        public virtual void Move(string Orient)
        {
            PreviousPositionToX = coordinatesTank.CordinateToX;
            PreviousPositionToY = coordinatesTank.CordinateToY;
            Turn(Orient);
            switch (Orient) {
                case "Up":
                    coordinatesTank.CordinateToY -= 5;
                    break;
                case "Down":
                    coordinatesTank.CordinateToY += 5;
                    break;
                case "Right":
                    coordinatesTank.CordinateToX += 5;
                    break;
                case "Left":
                    coordinatesTank.CordinateToX -= 5;
                    break;
            }
        } // Изменение кординат танка и имейджа
        public virtual void Shot(ref List<Bullet> windowBullet) {} // Выстрел танка
        public bool IsRecgarge()
        {
            DateTime time = DateTime.Now;
            if (Clip == 0)
                if (RecgargeShot.Minute == time.Minute && time.Second - RecgargeShot.Second < 10)
                {
                    return false;
                }
                else if ((time.Second + 60 * (time.Minute - RecgargeShot.Minute)) - RecgargeShot.Second < 10)
                    return false;
                else
                {
                    Clip = 3;
                }
            return true;
        }// Перезарядка
        public virtual void Turn(string orient) {}// Поворот танка
        public virtual void CancellationMove() {
            coordinatesTank.CordinateToX = PreviousPositionToX;
            coordinatesTank.CordinateToY = PreviousPositionToY;
        }
    }
}
