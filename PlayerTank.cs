using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
namespace Tanks
{
    public class PlayerTank : Tank
    {

        public PlayerTank(string name, int x, int y, string tankOr) : base(name, x, y, tankOr) { }
        public override void Shot(ref List<Bullet> windowBullet)
        {
            if (IsRecgarge())
            {
                RecgargeShot = DateTime.Now;
                windowBullet.Add(new Bullet(this.GetCoordinates.CordinateToX, this.GetCoordinates.CordinateToY, Orient));
                Clip--;
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
            PreviousPositionToY = 0;
            PreviousPositionToX = 0;

            Turn(Orient);
            switch (Orient)
            {
                case "Up":
                    coordinatesTank.CordinateToY -= 5;
                    PreviousPositionToY = 5;
                    break;
                case "Down":
                    coordinatesTank.CordinateToY += 5;
                    PreviousPositionToY = -5;
                    break;
                case "Right":
                    coordinatesTank.CordinateToX += 5;
                    PreviousPositionToX = -5;
                    break;
                case "Left":
                    coordinatesTank.CordinateToX -= 5;
                    PreviousPositionToX = 5;
                    break;
            }
        }
        public override void CancellationMove()
        {
            coordinatesTank.CordinateToX += PreviousPositionToX;
            coordinatesTank.CordinateToY += PreviousPositionToY;
        }
    }
}