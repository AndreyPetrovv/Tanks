using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Threading;
using System.Diagnostics;

namespace Tanks
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        static List<Tank> windowTank = new List<Tank>();
        static List<Bullet> windowBullet = new List<Bullet>();
        static List<MapObject> mapObjects = new List<MapObject>();

        Thread objectMappingTank;
        Thread objectMappingBullet;
        Thread objectTankControl;
        static Tank tank;

        public MainWindow()
        {
            InitializeComponent();

            FirsInitializanion();

            windowTank.Add(new Tank("2", 200, 50, "EvilTank"));
            //windowTank[1].AddUri(new Uri("pack://application:,,,/ImageTank/EvilTankD.png"));

            objectMappingTank = new Thread(ThreadMapingTank);
            objectMappingBullet = new Thread(ThreadMapingBullet);
            objectTankControl = new Thread(ThreadTankControl);
            objectMappingTank.SetApartmentState(ApartmentState.STA);
            objectMappingBullet.SetApartmentState(ApartmentState.STA);
            objectMappingTank.Start();
            objectMappingBullet.Start();
            objectTankControl.Start();
        }


        void ThreadTankControl()
        {

            while (true) {

                this.TurnTheTank();
                this.TankShot();
            }
        }
        void ThreadMapingTank()
        {

            while (true)
            {
                Thread.Sleep(1);
                for (int i = 0; i < windowTank.Count; i++)
                {
                    try
                    {
                        for (int j = 0; j < windowTank.Count; j++)
                        {
                            if (windowTank[i] != windowTank[j])
                            {

                                if (Math.Abs(windowTank[i].PositionToX - windowTank[j].PositionToX) > 58 ||
                                    Math.Abs(windowTank[i].PositionToY - windowTank[j].PositionToY) > 58)
                                {

                                    Dispatcher.Invoke(() => Canvas.SetTop(windowTank[i].GetImage(), windowTank[i].PositionToY));
                                    Dispatcher.Invoke(() => Canvas.SetLeft(windowTank[i].GetImage(), windowTank[i].PositionToX));

                                }
                                else
                                {
                                    windowTank[i].PositionToY = windowTank[i].PreviousPositionToY;
                                    windowTank[i].PositionToX = windowTank[i].PreviousPositionToX;
                                }

                            }
                            else {
                                Dispatcher.Invoke(() => Canvas.SetTop(windowTank[i].GetImage(), windowTank[i].PositionToY));
                                Dispatcher.Invoke(() => Canvas.SetLeft(windowTank[i].GetImage(), windowTank[i].PositionToX));
                            }
                        }


                    }
                    catch { }
                    if (windowTank[i].Health == 0)
                    {
                        Dispatcher.Invoke(() => canvas.Children.Remove(windowTank[i].GetImage()));

                        windowTank.Remove(windowTank[i]);
                        Dispatcher.Invoke(() => Num.Text = (Convert.ToInt32(Num.Text) - 1).ToString());
                    }
                }

            }
        }

        void ThreadMapingBullet()
        {
            while (true)
            {
                Thread.Sleep(17);
                for (int i = 0; i < windowBullet.Count; i++)
                {
                    windowBullet[i].flight();
                    Dispatcher.Invoke(() => Canvas.SetLeft(windowBullet[i].GetImage(), windowBullet[i].BulletPositionToX));
                    Dispatcher.Invoke(() => Canvas.SetTop(windowBullet[i].GetImage(), windowBullet[i].BulletPositionToY));
                    for (int j = 0; j < windowTank.Count; j++)
                    {
                        try
                        {
                            if (ChargeDestructionCheck(windowTank[j], windowBullet[i]))
                            {
                                windowTank[j].Damage();
                                Dispatcher.Invoke(() => canvas.Children.Remove(windowBullet[i].GetImage()));
                                windowBullet.Remove(windowBullet[i]);
                            }
                        }
                        catch { }
                    }
                    if (i <= windowBullet.Count - 1)
                        if ((windowBullet[i].BulletPositionToY > 450 || windowBullet[i].BulletPositionToY < 0) ||
                            (windowBullet[i].BulletPositionToX > 800 || windowBullet[i].BulletPositionToX < 0))
                        {
                            Dispatcher.Invoke(() => canvas.Children.Remove(windowBullet[i].GetImage()));
                            windowBullet.Remove(windowBullet[i]);
                        }
                }
            }
        }

        private void TurnTheTank()
        {
            DateTime StartTime = DateTime.Now;
           
            for (int i = 1; i < windowTank.Count; i++)
            {
                //sw = new Stopwatch();
                //sw.Start();
                if (windowTank[i].TimeTurn >= 500)
                {

                    if (windowTank[0].PositionToY < windowTank[i].PositionToY)
                    {
                        Dispatcher.Invoke(() => windowTank[1].AddUri(new Uri("pack://application:,,,/ImageTank/EvilTank.png")));
                        Dispatcher.Invoke(() => windowTank[1].Orient = "Up");
                    }
                    else if (windowTank[0].PositionToY > windowTank[i].PositionToY)
                    {
                        Dispatcher.Invoke(() => windowTank[1].AddUri(new Uri("pack://application:,,,/ImageTank/EvilTankD.png")));
                        Dispatcher.Invoke(() => windowTank[1].Orient = "Down");
                    }
                    if (windowTank[0].PositionToX > windowTank[i].PositionToX)
                    {
                        Dispatcher.Invoke(() => windowTank[1].AddUri(new Uri("pack://application:,,,/ImageTank/EvilTankR.png")));
                        Dispatcher.Invoke(() => windowTank[1].Orient = "Right");
                    }
                    else if (windowTank[0].PositionToX < windowTank[i].PositionToX)
                    {
                        Dispatcher.Invoke(() => windowTank[1].AddUri(new Uri("pack://application:,,,/ImageTank/EvilTankLpng.png")));
                        Dispatcher.Invoke(() => windowTank[1].Orient = "Left");
                    }
                   
                    windowTank[i].TimeTurn = 0;
                }
                //TimeSpan elapsed = DateTime.Now - StartTime;
                //sw.Stop();
                double f = (DateTime.Now - StartTime).TotalMilliseconds/1000;
                windowTank[i].TimeTurn += (DateTime.Now - StartTime).TotalMilliseconds;
            }

        }
        private void TankShot()
        {

            for (int i = 1; i < windowTank.Count; i++)
            {

                if ( (Math.Abs(windowTank[0].PositionToX - windowTank[i].PositionToX) < 29 ||
                                Math.Abs(windowTank[0].PositionToY - windowTank[i].PositionToY) < 29))
                {
                    if (Math.Abs(windowTank[0].PositionToY - windowTank[i].PositionToY) < 250)
                    {
                        Dispatcher.Invoke(() => windowTank[i].Shot(ref windowBullet));
                        Thread.Sleep(1000);
                    }
                }

            }

        }

        private static bool ChargeDestructionCheck(Tank tank, Bullet bullet)
        {
            if ((bullet.BulletPositionToY > tank.PositionToY) && (bullet.BulletPositionToY < tank.PositionToY + 62) &&
                                                                 ((bullet.BulletPositionToX > tank.PositionToX) &&
                                                                 (bullet.BulletPositionToX < tank.PositionToX + 58)))
                return true;
            return false;

        }

        private void FirsInitializanion()
        {
            tank = new Tank("1", 250, 250,"танк");
            windowTank.Add(tank);
        }

        private void AddCanvas(Image image)
        {
            Canvas.SetLeft(image, tank.PositionToX);
            Canvas.SetTop(image, tank.PositionToY);
        }

        private void Button_Click()
        {
            Random r = new Random();
            windowTank.Add(new Tank("2", r.Next(50,600),r.Next(50,340), "EvilTank"));
            Num.Text = (Convert.ToInt32(Num.Text) + 1 ).ToString();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
         {
            
            switch (e.Key)
            {
                case Key.W:
                    tank.Move("Up",0, -5, new Uri("pack://application:,,,/ImageTank/танк.png"));
                    break;
                case Key.S:
                    tank.Move("Down",0, 5, new Uri("pack://application:,,,/ImageTank/танкD.png"));
                    break;
                case Key.A:
                    tank.Move("Left",-5, 0, new Uri("pack://application:,,,/ImageTank/танкL.png"));
                    break;
                case Key.D:
                    tank.Move("Right",5, 0, new Uri("pack://application:,,,/ImageTank/танкR.png"));
                    break;
                case Key.Space:
                    tank.Shot(ref windowBullet);
                    break;
                case Key.Q: Button_Click(); break;
            }
        }

        private void myWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            objectMappingBullet.Abort();
            objectMappingTank.Abort();
        }
    }
}
