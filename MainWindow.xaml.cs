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
        static List<MapObject> listMapObjects = new List<MapObject>();
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

            InitializanionMap();
            InitializanionTank();
            InitializanionThread();
        }

        private void InitializanionMap() {

            for (int i = 0; i < 11; i++)
            {
                mapGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int i = 0; i < 7; i++)
            {
                mapGrid.RowDefinitions.Add(new RowDefinition());
            }
            for (int i = 1; i < 11; i += 2)
            {
                for (int j = 1; j < 7; j += 2)
                {
                    MapObject mapObject = new MapObject(110, 103);
                    mapObject.X = i * 110;
                    mapObject.Y = j * 103;
                    mapGrid.Children.Add(mapObject.GetRectangle());
                    Grid.SetColumn(mapObject.GetRectangle(), i);
                    Grid.SetRow(mapObject.GetRectangle(), j);
                    listMapObjects.Add(mapObject);
                }
            }
            mapGrid.ShowGridLines = true;
        }
        private void InitializanionTank()
        {
            tank = new Tank("1", 295, 5, "танк");
            windowTank.Add(tank);
            //windowTank.Add(new Tank("2", 200, 5, "EvilTank"));
            windowTank.Add(new Tank("2", 225, 658, "EvilTank"));
        }
        private void InitializanionThread() {
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
                try
                {
                    for (int i = 1; i < windowTank.Count; i++)
                    {

                        СonditionTurnTheTank(windowTank[i]);
                        BotTankMove(windowTank[i]);
                    }
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
                    DateTime time = DateTime.Now;
                    try
                    {
                        Collision(windowTank[i]);
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
                                DeleteBullet(windowBullet[i]);
                            }
                        }
                        catch { }
                    }
                    if (i <= windowBullet.Count - 1 && (IsBeingOnTheMap(windowBullet[i]) || !Dispatcher.Invoke(() => CheckListObjectMap(windowBullet[i]))))
                        {
                            DeleteBullet(windowBullet[i]);
                        }

                }
                Thread.Sleep(17);
            }
        }

        private void BotTankMove(Tank tank)
        {
            switch (tank.OrientationMove)
            {
                case "Up":
                    if (IsBeingOnTheMap(tank) && Dispatcher.Invoke(() => CheckListObjectMap(tank)) && tank.passed < 211)
                    {
                        Dispatcher.Invoke(() => tank.Move("Up", 0, -1, new Uri("pack://application:,,,/ImageTank/EvilTank.png")));
                        Dispatcher.Invoke(() => tank.passed++);
                    }
                    else
                    {
                        Dispatcher.Invoke(() => tank.OrientationMove = "Right");
                        Dispatcher.Invoke(() => tank.passed = 0);
                    }
                    break;
                case "Down":
                    if (IsBeingOnTheMap(tank) && Dispatcher.Invoke(() => CheckListObjectMap(tank)) && tank.passed < 211)
                    {
                        Dispatcher.Invoke(() => tank.Move("Down", 0, 1, new Uri("pack://application:,,,/ImageTank/танкD.png")));
                        Dispatcher.Invoke(() => tank.passed++);

                    }
                    else
                    {
                        Dispatcher.Invoke(() => tank.OrientationMove = "Up");
                        Dispatcher.Invoke(() => tank.passed = 0);
                    }
                    break;
                case "Left":
                    if (IsBeingOnTheMap(tank) && Dispatcher.Invoke(() => CheckListObjectMap(tank)) && tank.passed < 211)
                    {
                        Dispatcher.Invoke(() => tank.Move("Left", -1, 0, new Uri("pack://application:,,,/ImageTank/танкL.png")));
                        Dispatcher.Invoke(() => tank.passed++);

                    }
                    else
                    {
                        Dispatcher.Invoke(() => tank.OrientationMove = "Up");
                        Dispatcher.Invoke(() => tank.passed = 0);
                    }
                    break;
                case "Right":
                    if (IsBeingOnTheMap(tank) && Dispatcher.Invoke(() => CheckListObjectMap(tank)) && tank.passed < 211)
                    {
                        Dispatcher.Invoke(() => tank.Move("Right", 1, 0, new Uri("pack://application:,,,/ImageTank/танкR.png")));
                        Dispatcher.Invoke(() => tank.passed++);

                    }
                    else
                    {
                        Dispatcher.Invoke(() => tank.OrientationMove = "Down");
                        Dispatcher.Invoke(() => tank.passed = 0);
                    }
                    break;
            }
        }
        private void СonditionTurnTheTank(Tank tank)
        {
            if ((Math.Abs(windowTank[0].PositionToX - tank.PositionToX) < 58 &&
                windowTank[0].PositionToY > tank.PositionToY))
            {
                if ((Math.Abs(windowTank[0].PositionToY - tank.PositionToY) < 270 ||
                    tank.Health != 4) && Dispatcher.Invoke(() => CheckListRange(tank, true)))
                {
                    Turn(tank, new Uri("pack://application:,,,/ImageTank/EvilTankD.png"), "Down");
                    this.TankShot(tank);
                }
            }
            else if ((Math.Abs(windowTank[0].PositionToX - tank.PositionToX) < 58 &&
                windowTank[0].PositionToY < tank.PositionToY))
            {
                if ((Math.Abs(windowTank[0].PositionToY - tank.PositionToY) < 270 ||
                    tank.Health != 4) && Dispatcher.Invoke(() => CheckListRange(tank, true)))
                {
                    Turn(tank, new Uri("pack://application:,,,/ImageTank/EvilTank.png"), "Up");
                    this.TankShot(tank);
                }
            }
            else if ((Math.Abs(windowTank[0].PositionToY - tank.PositionToY) < 58 &&
                windowTank[0].PositionToX > tank.PositionToX))
            {
                if ((Math.Abs(windowTank[0].PositionToX - tank.PositionToX) < 270 ||
                    tank.Health != 4) && Dispatcher.Invoke(() => CheckListRange(tank, false)))
                {
                    Turn(tank, new Uri("pack://application:,,,/ImageTank/EvilTankR.png"), "Right");
                    this.TankShot(tank);
                }
            }
            else if ((Math.Abs(windowTank[0].PositionToY - tank.PositionToY) < 58 &&
                windowTank[0].PositionToX < tank.PositionToX))
            {
                if ((Math.Abs(windowTank[0].PositionToX - tank.PositionToX) < 270 ||
                    tank.Health != 4) && Dispatcher.Invoke(() => CheckListRange(tank, false)))
                {
                    Turn(tank, new Uri("pack://application:,,,/ImageTank/EvilTankLpng.png"), "Left");
                    this.TankShot(tank);
                }
            }


        }
        private void Turn(Tank tank, Uri uri, string orient) {

            Dispatcher.Invoke(() => tank.AddUri(uri));
            Dispatcher.Invoke(() => tank.Orient = orient);
        }
        private void TankShot(Tank tank)
        {

            if ((Math.Abs(windowTank[0].PositionToX - tank.PositionToX) < 34 ||
                            Math.Abs(windowTank[0].PositionToY - tank.PositionToY) < 34) && IsRecgarge(tank) && ShotDelayBot(tank))
            {
                Dispatcher.Invoke(() => tank.Shot(ref windowBullet));
                tank.RecgargeShot = DateTime.Now;
                tank.ShotDelay = DateTime.Now;
                tank.Clip--;
            }

        }

        private bool IsRecgarge(Tank tank)
        {
            DateTime time = DateTime.Now;
            if (tank.Clip == 0)
                if (tank.RecgargeShot.Minute == time.Minute && time.Second - tank.RecgargeShot.Second < 10)
                {
                    return false;
                }
                else if ((time.Second + 60 * (time.Minute - tank.RecgargeShot.Minute)) - tank.RecgargeShot.Second < 10)
                    return false;

            return true;
        }
        private bool ShotDelayBot(Tank tank)
        {
            DateTime time = DateTime.Now;
            if (time.Second + 60 * (time.Minute - tank.ShotDelay.Minute) - tank.ShotDelay.Second < 1)
                    return false;
            return true;
        }
        private void Collision(Tank tank)
        {
            for (int j = 0; j < windowTank.Count; j++)
            {
                if (tank != windowTank[j])
                {

                    if (IsThereACollision(tank, windowTank[j]) && IsBeingOnTheMap(tank) && Dispatcher.Invoke(() => CheckListObjectMap(tank)))
                    {
                        PositionСhange(tank);
                    }
                    else
                    {
                        tank.PositionToY = tank.PreviousPositionToY;
                        tank.PositionToX = tank.PreviousPositionToX;
                    }

                }
                else
                {
                    if (Dispatcher.Invoke(() => CheckListObjectMap(tank)) && IsBeingOnTheMap(tank))
                    {
                        Dispatcher.Invoke(() => Canvas.SetTop(tank.GetImage(), tank.PositionToY));
                        Dispatcher.Invoke(() => Canvas.SetLeft(tank.GetImage(), tank.PositionToX));
                    }
                    else
                    {
                        tank.PositionToY = tank.PreviousPositionToY;
                        tank.PositionToX = tank.PreviousPositionToX;
                    }
                }
            }
        }
        private void DeleteBullet(Bullet bullet) {
            Dispatcher.Invoke(() => canvas.Children.Remove(bullet.GetImage()));
            windowBullet.Remove(bullet);
        }

        private bool IsThereACollision(Tank tankOne, Tank tankTwo) {
            if ((Math.Abs(tankOne.PositionToX - tankTwo.PositionToX) > 58 || Math.Abs(tankOne.PositionToY -tankTwo.PositionToY) > 58))
                return true;
            return false;
        }
        private bool IsBeingOnTheMap(Tank tank) {
            if ((tank.PositionToY > 721 || tank.PositionToY < 0) || (tank.PositionToX > 1220 || tank.PositionToX < 0))
                return false;
            return true;
        }
        private bool IsBeingOnTheMap(Bullet bullet) {
            if ((bullet.BulletPositionToY > 704 || bullet.BulletPositionToY < 0) || (bullet.BulletPositionToX > 1144 || bullet.BulletPositionToX < 0))
                return true;
            return false;
        }
        private bool ChargeDestructionCheck(Tank tank, Bullet bullet)
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

        private void PositionСhange(Tank tank) {
            Dispatcher.Invoke(() => Canvas.SetTop(tank.GetImage(), tank.PositionToY));
            Dispatcher.Invoke(() => Canvas.SetLeft(tank.GetImage(), tank.PositionToX));
        }
        private void PositionСhange(Bullet bullet)
        {
            Dispatcher.Invoke(() => Canvas.SetTop(bullet.GetImage(), bullet.BulletPositionToY));
            Dispatcher.Invoke(() => Canvas.SetLeft(bullet.GetImage(), bullet.BulletPositionToX));
        }

        private void Button_Click()
        {
            Random r = new Random();
            windowTank.Add(new Tank("2", r.Next(225,800),r.Next(300,1200), "EvilTank"));
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
                    if (IsRecgarge(tank))
                    {
                        tank.RecgargeShot = DateTime.Now;
                        tank.Shot(ref windowBullet);
                        tank.Clip--;
                    }
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
