using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace Tanks
{
    public class BotTank : Tank
    {
        private bool _moveShot = true;

        public bool MoveShot {
            get {
                return _moveShot;
            }
            set {
                _moveShot = value;
            }
        }    
        public int PreviousOrientationMove = 0;
        public int RepetitionOfMovement = 0;
        public double passed = 0;
        public int OrientationMove = 1;
        public DateTime ShotDelay;
        public bool motionСancellation = false;


        public BotTank(string name, int x, int y, string tankOr) : base(name, x, y, tankOr) { }
        public BotTank() : base() { }
        public override void Shot(ref List<Bullet> windowBullet)
        {

            if (IsRecgarge() && IsShotDelayBot())
            {
                windowBullet.Add(new Bullet(this.GetCoordinates.CordinateToX, this.GetCoordinates.CordinateToY, Orient));
                RecgargeShot = DateTime.Now;
                ShotDelay = DateTime.Now;
                Clip--;
            }
        }
        private bool IsShotDelayBot()
        {
            DateTime time = DateTime.Now;
            if (time.Second + 60 * (time.Minute - ShotDelay.Minute) - ShotDelay.Second < 1)
                return false;
            return true;
        }
        public override void Turn(string orient)
        {
            Orient = orient;
            switch (Orient)
            {
                case "Up":
                    image.Source = new BitmapImage(new Uri("pack://application:,,,/ImageTank/EvilTank.png"));
                    break;
                case "Down":
                    image.Source = new BitmapImage(new Uri("pack://application:,,,/ImageTank/EvilTankD.png"));
                    break;
                case "Right":
                    image.Source = new BitmapImage(new Uri("pack://application:,,,/ImageTank/EvilTankR.png"));
                    break;
                case "Left":
                    image.Source = new BitmapImage(new Uri("pack://application:,,,/ImageTank/EvilTankL.png"));
                    break;
            }

        }
        public bool IsRangeShot(int Y0, int Y1) {
            if (Math.Abs(Y0 - Y1) < 470 || Health != 4)
                return true;
            return false;
        }
        public bool IsMove() {
            if (passed >= 110 || motionСancellation)
                return false;
            return true;
        }
        
    }
}
