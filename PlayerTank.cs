using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using System.Diagnostics;

namespace Tanks
{
    public class PlayerTank : Tank
    {
        private Stopwatch StopwatchRecrage = new Stopwatch();
        public PlayerTank(string name, int x, int y, string tankOr) : base(name, x, y, tankOr) { }
        public override void Shot(ref List<Bullet> windowBullet)
        {
            if (IsRecgarge())
            {
                RecgargeShot = DateTime.Now;
                windowBullet.Add(new Bullet(this.GetCoordinates.CordinateToX, this.GetCoordinates.CordinateToY, Orient));
                Clip--;
                StopwatchRecrage.Reset();
            }
            if (Clip == 0) {
                StopwatchRecrage.Start();
            }
            
        }
        public override void Turn(string orient)
        {
            Orient = orient;
            switch (Orient)
            {
                case "Up":
                    image.Source = new BitmapImage(new Uri("pack://application:,,,/ImageTank/танк.png"));
                    break;
                case "Down":
                    image.Source = new BitmapImage(new Uri("pack://application:,,,/ImageTank/танкD.png"));
                    break;
                case "Right":
                    image.Source = new BitmapImage(new Uri("pack://application:,,,/ImageTank/танкR.png"));
                    break;
                case "Left":
                    image.Source = new BitmapImage(new Uri("pack://application:,,,/ImageTank/танкL.png"));
                    break;
            }

        }
        public void Info(ref string xp, ref string clip) {
            IsRecgarge();
            xp = Health.ToString();
            clip = Clip.ToString();
        }
        public override void Move(string Orient)
        {

            PreviousPositionToY = coordinatesTank.CordinateToY;
            PreviousPositionToX = coordinatesTank.CordinateToX;
            int i = 4;
            Turn(Orient);
            switch (Orient)
            {
                case "Up":
                    coordinatesTank.CordinateToY -= i;
                    //PreviousPositionToY = i;
                    break;
                case "Down":
                    coordinatesTank.CordinateToY += i;
                    // PreviousPositionToY = -i;
                    break;
                case "Right":
                    coordinatesTank.CordinateToX += i;
                    // PreviousPositionToX = -i;
                    break;
                case "Left":
                    coordinatesTank.CordinateToX -= i;
                    // PreviousPositionToX = i;
                    break;
            }
        }

        public string IsStopwatch() {

            return (10 - StopwatchRecrage.Elapsed.Seconds).ToString() ;
        }
      
        public override void CancellationMove()
        {
            coordinatesTank.CordinateToX = PreviousPositionToX;
            coordinatesTank.CordinateToY = PreviousPositionToY;
        }
    }
}