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
            new MapObject(68,68,116,116),
            new MapObject(300,68,116,116),
            new MapObject(522,68,116,116),
            new MapObject(744,68,116,116),
            new MapObject(967,68,116,116),
            new MapObject(68,300,116,116),
            new MapObject(300,300,116,116),
            new MapObject(522,300,116,116),
            new MapObject(744,300,116,116),
            new MapObject(967,300,116,116),
            new MapObject(68,522,116,116),
            new MapObject(300,522,116,116),
            new MapObject(522,522,116,116),
            new MapObject(744,522,116,116),
            new MapObject(967,522,116,116)
        };
        static List<Tank> windowTank = new List<Tank>();
        static List<Bullet> windowBullet = new List<Bullet>();
        static List<MapObject> mapObjects = new List<MapObject>();
        static List<Image> images = new List<Image>() {
            new Image()

        };

        Thread objectMappingTank;
        Thread objectMappingBullet;
        Thread objectTankControl;
        static Tank tank;

        public MainWindow()
        {
            InitializeComponent();

            FirsInitializanion();
            InitializanionMap();
            Griding();

            windowTank.Add(new Tank("2", 200, 358, "EvilTank"));

            objectMappingTank = new Thread(ThreadMapingTank);
            objectMappingBullet = new Thread(ThreadMapingBullet);
            objectTankControl = new Thread(ThreadTankControl);
            objectMappingTank.SetApartmentState(ApartmentState.STA);
            objectMappingBullet.SetApartmentState(ApartmentState.STA);
            objectMappingTank.Start();
            objectMappingBullet.Start();
            objectTankControl.Start();
        }

        private void Griding() {

            for (int i = 0; i < 11; i++)
            {
                mapGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int i = 0; i < 7; i++)
            {
                mapGrid.RowDefinitions.Add(new RowDefinition());
            }
            for (int i = 1; i < 11; i+=2)
            {
                for (int j = 1; j < 7; j+=2)
                {
                    Rectangle rectangle = new Rectangle();
                    rectangle.Fill = Brushes.Blue;
                    rectangle.Width = 111;
                    rectangle.Height = 103;
                    mapGrid.Children.Add(rectangle);
                    Grid.SetColumn(rectangle, i);
                    Grid.SetRow(rectangle, j);
                }
            }
            mapGrid.ShowGridLines = true;
        }

        void ThreadTankControl()
        {

            while (true) {
                try
                {
                    this.TurnTheTank();
                }
                catch { }
                Thread.Sleep(50);
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

                                if ((Math.Abs(windowTank[i].PositionToX - windowTank[j].PositionToX) > 58 ||
                                    Math.Abs(windowTank[i].PositionToY - windowTank[j].PositionToY) > 58) && IsBeingOnTheMap(windowTank[i]))
                                {
                                    if (Dispatcher.Invoke(() => CheckListObjectMap(windowTank[i])))
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
                                if (Dispatcher.Invoke(() => CheckListObjectMap(windowTank[i])) && IsBeingOnTheMap(windowTank[i]))
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
                        if (IsBeingOnTheMap(windowBullet[i]) || !Dispatcher.Invoke(() => CheckListObjectMap(windowBullet[i])) )
                        {
                            Dispatcher.Invoke(() => canvas.Children.Remove(windowBullet[i].GetImage()));
                            windowBullet.Remove(windowBullet[i]);
                        }
                    
                }
                Thread.Sleep(17);
            }
        }

        private void TurnTheTank()
        {

            for (int i = 1; i < windowTank.Count; i++)
            {
                if ((Math.Abs(windowTank[0].PositionToX - windowTank[i].PositionToX) < 58 && 
                    windowTank[0].PositionToY > windowTank[i].PositionToY))
                {
                    if ((Math.Abs(windowTank[0].PositionToY - windowTank[i].PositionToY) < 270 || 
                        windowTank[i].Health != 4) && Dispatcher.Invoke(() => CheckListRange(windowTank[i], true)))
                    {
                        Turn(windowTank[i], new Uri("pack://application:,,,/ImageTank/EvilTankD.png"), "Down");
                        this.TankShot();
                    }
                }
                else if ((Math.Abs(windowTank[0].PositionToX - windowTank[i].PositionToX) < 58 && 
                    windowTank[0].PositionToY < windowTank[i].PositionToY))
                {
                    if ((Math.Abs(windowTank[0].PositionToY - windowTank[i].PositionToY) < 270 || 
                        windowTank[i].Health != 4) && Dispatcher.Invoke(() => CheckListRange(windowTank[i], true)))
                    {
                        Turn(windowTank[i], new Uri("pack://application:,,,/ImageTank/EvilTank.png"), "Up");
                        this.TankShot();
                    }
                }
                else if ((Math.Abs(windowTank[0].PositionToY - windowTank[i].PositionToY) < 58 && 
                    windowTank[0].PositionToX > windowTank[i].PositionToX))
                {
                    if ((Math.Abs(windowTank[0].PositionToX - windowTank[i].PositionToX) < 270 || 
                        windowTank[i].Health != 4) && Dispatcher.Invoke(() => CheckListRange(windowTank[i], false)))
                    {
                        Turn(windowTank[i], new Uri("pack://application:,,,/ImageTank/EvilTankR.png"), "Right");
                        this.TankShot();
                    }
                }
                else if ((Math.Abs(windowTank[0].PositionToY - windowTank[i].PositionToY) < 58 && 
                    windowTank[0].PositionToX < windowTank[i].PositionToX))
                {
                    if ((Math.Abs(windowTank[0].PositionToX - windowTank[i].PositionToX) < 270 || 
                        windowTank[i].Health != 4) && Dispatcher.Invoke(() => CheckListRange(windowTank[i], false)))
                    {
                        Turn(windowTank[i], new Uri("pack://application:,,,/ImageTank/EvilTankLpng.png"), "Left");
                        this.TankShot();
                    }
                }
            }

        }
        private void Turn(Tank tank, Uri uri, string orient) {

            Dispatcher.Invoke(() => tank.AddUri(uri));
            Dispatcher.Invoke(() => tank.Orient = orient);
        }
        private void TankShot()
        {

            for (int i = 1; i < windowTank.Count; i++)
            {

                if ( (Math.Abs(windowTank[0].PositionToX - windowTank[i].PositionToX) < 34 ||
                                Math.Abs(windowTank[0].PositionToY - windowTank[i].PositionToY) < 34))
                {
                        Dispatcher.Invoke(() => windowTank[i].Shot(ref windowBullet));
                        Thread.Sleep(1000);
                }

            }

        }

        private static bool IsBeingOnTheMap(Tank tank) {
            if ((tank.PositionToY > 663 || tank.PositionToY < 0) || (tank.PositionToX > 1098 || tank.PositionToX < 0))
                return false;
            return true;
        }
        private static bool IsBeingOnTheMap(Bullet bullet) {
            if ((bullet.BulletPositionToY > 704 || bullet.BulletPositionToY < 0) || (bullet.BulletPositionToX > 1144 || bullet.BulletPositionToX < 0))
                return true;
            return false;
        }
        private static bool ChargeDestructionCheck(Tank tank, Bullet bullet)
        {
            if(Math.Abs((tank.PositionToX + 29) - (bullet.BulletPositionToX + 4.5)) < 33.5 &&
                Math.Abs((tank.PositionToY + 29) - (bullet.BulletPositionToY + 4.5)) < 33.5)
                return true;
            return false;

        }

        private bool CheckListRange(Tank tank, bool isCheck) {
            foreach (var item in listMapObjects)
            {
                if (item.IsCheckRange(windowTank[0], tank, isCheck))
                    continue;
                else
                    return false;
            }
            return true;
        }
        private bool CheckListObjectMap(Tank tank)
        {
            foreach (var item in listMapObjects)
            {
                if (item.IsCheckMove(tank.PositionToX, tank.PositionToY, 29))
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
                if (item.IsCheckMove(bullet.BulletPositionToX, bullet.BulletPositionToY, 4.5))
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
            tank = new Tank("1", 295, 5,"танк");
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
