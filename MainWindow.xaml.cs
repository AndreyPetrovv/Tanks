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
        static List<MapObject> listMapObjects = new List<MapObject> {
            new MapObject(85,70,120,120),
            new MapObject(295,70,120,120),
            new MapObject(495,70,120,120),
            new MapObject(695,70,120,120),
            new MapObject(895,70,120,120),
        };
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
            InitializanionMap();

           //windowTank.Add(new Tank("2", 298, 180, "EvilTank"));

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
                                if (CheckListObjectMap(windowTank[i]))
                                {
                                    Dispatcher.Invoke(() => Canvas.SetTop(windowTank[i].GetImage(), windowTank[i].PositionToY));
                                    Dispatcher.Invoke(() => Canvas.SetLeft(windowTank[i].GetImage(), windowTank[i].PositionToX));
                                }
                            }
                            else
                            {
                                windowTank[i].PositionToY = windowTank[i].PreviousPositionToY;
                                windowTank[i].PositionToX = windowTank[i].PreviousPositionToX;
                            }

                        }
                        else
                        {
                            if (CheckListObjectMap(windowTank[i]))
                            {
                                Dispatcher.Invoke(() => Canvas.SetTop(windowTank[i].GetImage(), windowTank[i].PositionToY));
                                Dispatcher.Invoke(() => Canvas.SetLeft(windowTank[i].GetImage(), windowTank[i].PositionToX));
                            }
                            else {
                                    CheckListObjectMap(windowTank[i]);
                                windowTank[i].PositionToY = windowTank[i].PreviousPositionToY;
                                windowTank[i].PositionToX = windowTank[i].PreviousPositionToX;
                            }
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
                        if ((windowBullet[i].BulletPositionToY > 686 || windowBullet[i].BulletPositionToY < 0) ||
                            (windowBullet[i].BulletPositionToX > 1180 || windowBullet[i].BulletPositionToX < 0) || CheckListObjectMap(windowBullet[i]))
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
                
                if (windowTank[i].TimeTurn >= 1000)
                {

                    if (windowTank[0].PositionToY > windowTank[i].PositionToY && 
                        Math.Abs(windowTank[0].PositionToX - windowTank[i].PositionToX) <= Math.Abs(windowTank[0].PositionToY - windowTank[i].PositionToY))
                    {
                        Dispatcher.Invoke(() => windowTank[i].AddUri(new Uri("pack://application:,,,/ImageTank/EvilTankD.png")));
                        Dispatcher.Invoke(() => windowTank[i].Orient = "Down");
                    }
                    else if (windowTank[0].PositionToY < windowTank[i].PositionToY &&
                        Math.Abs(windowTank[0].PositionToX - windowTank[i].PositionToX) <= Math.Abs(windowTank[0].PositionToY - windowTank[i].PositionToY))
                    {
                        Dispatcher.Invoke(() => windowTank[i].AddUri(new Uri("pack://application:,,,/ImageTank/EvilTank.png")));
                        Dispatcher.Invoke(() => windowTank[i].Orient = "Up");
                    }
                    else if (windowTank[0].PositionToX > windowTank[i].PositionToX &&
                        Math.Abs(windowTank[0].PositionToX - windowTank[i].PositionToX) >= Math.Abs(windowTank[0].PositionToY - windowTank[i].PositionToY))
                    {
                        Dispatcher.Invoke(() => windowTank[i].AddUri(new Uri("pack://application:,,,/ImageTank/EvilTankR.png")));
                        Dispatcher.Invoke(() => windowTank[i].Orient = "Right");
                    }
                    else if (windowTank[0].PositionToX < windowTank[i].PositionToX &&
                        Math.Abs(windowTank[0].PositionToX - windowTank[i].PositionToX) >= Math.Abs(windowTank[0].PositionToY - windowTank[i].PositionToY))
                    {
                        Dispatcher.Invoke(() => windowTank[i].AddUri(new Uri("pack://application:,,,/ImageTank/EvilTankLpng.png")));
                        Dispatcher.Invoke(() => windowTank[i].Orient = "Left");
                    }

                    windowTank[i].TimeTurn = 0;
                }
                for (int j= 0; j < 500; j++) { }
                try
                {
                    windowTank[i].TimeTurn += (DateTime.Now - StartTime).TotalMilliseconds;
                }
                catch { }
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
            if ((bullet.BulletPositionToY > tank.PositionToY) && (bullet.BulletPositionToY < tank.PositionToY + 58) &&
                                                                 ((bullet.BulletPositionToX > tank.PositionToX) &&
                                                                 (bullet.BulletPositionToX < tank.PositionToX + 58)))
                return true;
            return false;

        }
        private bool CheckListObjectMap(Tank tank) {
            foreach (var item in listMapObjects)
            {
                if (item.IsCheckMove(tank.PositionToX, tank.PositionToY))
                    continue;
                else
                    return false;
            }
            return true;
        }
        private bool CheckListObjectMap(Bullet bullet)
        {
            foreach (var item in listMapObjects)
            {
                if (item.IsCheckMove(bullet.BulletPositionToX, bullet.BulletPositionToY))
                    continue;
                else
                    return false;
            }
            return true;
        }
        private void InitializanionMap()
        {
            foreach (var item in listMapObjects)
            {
                canvas.Children.Add(item.GetRectangle());
                Canvas.SetTop(item.GetRectangle(), item.Y);
                Canvas.SetLeft(item.GetRectangle(), item.X);
            }

        }
        private void FirsInitializanion()
        {
            tank = new Tank("1", 295, -20,"танк");
            windowTank.Add(tank);
        }
        private void Button_Click()
        {
            Random r = new Random();
            windowTank.Add(new Tank("2", r.Next(300,800),r.Next(300,1200), "EvilTank"));
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
            objectTankControl.Abort();
        }
    }
}
