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
        private static bool GAME = false;
        static Stopwatch Stopwatch = null;
        static List<MapObject> listMapObjects = new List<MapObject>();
        static List<Tank> windowTank = new List<Tank>();
        static List<Bullet> windowBullet = new List<Bullet>();
        static Random _rand = new Random(Environment.TickCount);
        static Tank tank;
        Thread objectMappingTank;
        Thread objectMappingBullet;
        Thread objectTankControl;

        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void InitializanionMap()
        {
            if (!GAME)
            {
                for (int i = 0; i < 11; i++)
                {
                    mapGrid.ColumnDefinitions.Add(new ColumnDefinition());
                }
                for (int i = 0; i < 7; i++)
                {
                    mapGrid.RowDefinitions.Add(new RowDefinition());
                }
            }
            string[] mapp;
            if (map.Text == "1")
                mapp = new string[7]
        {
                "00000000000",
                "01010101010",
                "00000000000",
                "01010101010",
                "00000000000",
                "01010101010",
                "00000000000"
        };
            else
                mapp = new string[7]
            {
                "00001100000",
                "00010010000",
                "00010010000",
                "00010010000",
                "01000001100",
                "10000100001",
                "01110001110"
            };
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    MapObject mapObject = new MapObject(111, 104);
                    Grid.SetColumn(mapObject.GetImage(), j);
                    Grid.SetRow(mapObject.GetImage(), i);

                    if (mapp[i][j] == '1')
                    {
                        mapObject.SetImage("1");
                        mapObject.X = j * 110;
                        mapObject.Y = i * 102;
                        listMapObjects.Add(mapObject);
                    }
                    else
                    {
                        mapObject.SetImage("0");
                    }

                    mapGrid.Children.Add(mapObject.GetImage());

                }
            }
            mapGrid.ShowGridLines = true;
        } // Отрисовка карты
        private void InitializanionTank()
        {
            tank = new PlayerTank("1", 295, 5, "танк");
            windowTank.Add(tank);
            if (colTank.Text == "1")
                windowTank.Add(new BotTank("2", 225, 650, "EvilTank"));
            else if (colTank.Text == "2")
            {
                windowTank.Add(new BotTank("2", 225, 650, "EvilTank"));
                windowTank.Add(new BotTank("2", 485, 650, "EvilTank"));
            }
            else {
                windowTank.Add(new BotTank("2", 225, 650, "EvilTank"));
                windowTank.Add(new BotTank("2", 485, 650, "EvilTank"));
                windowTank.Add(new BotTank("2", 665, 650, "EvilTank"));
            }
        } // Добавление танков на карту
        private void InitializanionThread()
        {
            objectMappingTank = new Thread(ThreadMapingTank);
            objectMappingBullet = new Thread(ThreadMapingBullet);
            objectTankControl = new Thread(ThreadTankControl);
            objectMappingTank.SetApartmentState(ApartmentState.STA);
            objectMappingBullet.SetApartmentState(ApartmentState.STA);
            objectMappingTank.Start();
            objectMappingBullet.Start();
            objectTankControl.Start();
        } // Запуск потоков

        void ThreadTankControl()
        {

            while (true)
            {
                try
                {
                    for (int i = 1; i < windowTank.Count; i++)
                    {
                        СonditionTurnTheTank((BotTank)windowTank[i]);
                        if (((BotTank)windowTank[i]).MoveShot || ! ((BotTank)windowTank[i]).IsRecgarge() )
                            BotTankMove((BotTank)windowTank[i]);
                        ((BotTank)windowTank[i]).MoveShot = true;
                    }
                }
                catch { }
                Thread.Sleep(50);
            }
        }// Поток контроля вражеского
        void ThreadMapingTank()
        {
            while (true)
            {
                if (windowTank.Count == 1 || tank.Health == 0)
                    Dispatcher.Invoke(()=> GameOver());
                
                Thread.Sleep(17);
                for (int i = 0; i < windowTank.Count; i++)
                {
                    DateTime time = DateTime.Now;
                    try
                    {
                        Tank tk = windowTank[i];
                        Dispatcher.Invoke(() => PlayerTankInfo());
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
        }// Поток отрисовки танков
        void ThreadMapingBullet()
        {
            while (true)
            {
                for (int i = 0; i < windowBullet.Count; i++)
                {
                    windowBullet[i].flight();
                    Dispatcher.Invoke(() => Canvas.SetLeft(windowBullet[i].GetImage, windowBullet[i].GetCoordinates.CordinateToX));
                    Dispatcher.Invoke(() => Canvas.SetTop(windowBullet[i].GetImage, windowBullet[i].GetCoordinates.CordinateToY));
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
        }// Поток отрисовки пуль

        private void PlayerTankInfo() {
            string xp = "", oboim = "";
            ((PlayerTank)tank).Info(ref xp, ref oboim);
            Xp.Text = xp;
            Oboim.Text = oboim;
        }

        private void BotTankMove(BotTank botTank)
        {
            if (botTank.IsMove() && IsBeingOnTheMap(tank) && Dispatcher.Invoke(() => CheckListObjectMap(tank)))
                switch (botTank.OrientationMove)
                {
                    case 1:
                        Dispatcher.Invoke(() => botTank.Move("Up"));
                        Dispatcher.Invoke(() => botTank.passed += 5);
                        break;
                    case 2:
                        Dispatcher.Invoke(() => botTank.Move("Down"));
                        Dispatcher.Invoke(() => botTank.passed += 5);
                        break;
                    case 3:
                        Dispatcher.Invoke(() => botTank.Move("Right"));
                        Dispatcher.Invoke(() => botTank.passed += 5);
                        break;
                    case 4:
                        Dispatcher.Invoke(() => botTank.Move("Left"));
                        Dispatcher.Invoke(() => botTank.passed += 5);
                        break;
                }
            else
            {
                NotMoveBotTank(botTank);
                
            }
        }// Движеие согласно выбранному направлению
        private void NotMoveBotTank(BotTank botTank)
        {
            Dispatcher.Invoke(() => botTank.passed = 0);
            Dispatcher.Invoke(() => botTank.OrientationMove = Roll());
            botTank.motionСancellation = false;
        }// отмена бвижения танка
        public static int Roll()
        {
            return _rand.Next(1, 5);
        }// Получение случайного направления танка
        private void СonditionTurnTheTank(BotTank tank)
        {
            bool IsRangeTankShotX = tank.IsRangeShot(windowTank[0].GetCoordinates.CordinateToX, tank.GetCoordinates.CordinateToX);
            bool IsRangeTankShotY = tank.IsRangeShot(windowTank[0].GetCoordinates.CordinateToY, tank.GetCoordinates.CordinateToY);
            if (Math.Abs(windowTank[0].GetCoordinates.CordinateToX - tank.GetCoordinates.CordinateToX) < 33)
            {
                if (windowTank[0].GetCoordinates.CordinateToY > tank.GetCoordinates.CordinateToY &&
                    Dispatcher.Invoke(() => CheckListRange(tank, true)) && IsRangeTankShotY)

                {
                    Dispatcher.Invoke(() => tank.MoveShot = false);
                    Dispatcher.Invoke(() => tank.Turn("Down"));
                    Dispatcher.Invoke(() => tank.Shot(ref windowBullet));
                }
                else if (windowTank[0].GetCoordinates.CordinateToY < tank.GetCoordinates.CordinateToY &&
                    Dispatcher.Invoke(() => CheckListRange(tank, true)) && IsRangeTankShotY)
                {
                    Dispatcher.Invoke(() => tank.MoveShot = false);
                    Dispatcher.Invoke(() => tank.Turn("Up"));
                    Dispatcher.Invoke(() => tank.Shot(ref windowBullet));
                }
            }
            else if (Math.Abs(windowTank[0].GetCoordinates.CordinateToY - tank.GetCoordinates.CordinateToY) < 33)
            {
                if (windowTank[0].GetCoordinates.CordinateToX > tank.GetCoordinates.CordinateToX && 
                    IsRangeTankShotX && Dispatcher.Invoke(() => CheckListRange(tank, false)))

                {
                    Dispatcher.Invoke(() => tank.MoveShot = false);
                    Dispatcher.Invoke(() => tank.Turn("Right"));
                    Dispatcher.Invoke(() => tank.Shot(ref windowBullet));
                }
                else if (windowTank[0].GetCoordinates.CordinateToX < tank.GetCoordinates.CordinateToX && 
                    IsRangeTankShotX && Dispatcher.Invoke(() => CheckListRange(tank, false)))
                {
                    Dispatcher.Invoke(() => tank.MoveShot = false);
                    Dispatcher.Invoke(() => tank.Turn("Left"));
                    Dispatcher.Invoke(() => tank.Shot(ref windowBullet));
                }
            }
        }//Условия поворота танка и выстрела

        private void Collision(Tank tnk)
        {
            for (int j = 0; j < windowTank.Count; j++)
            {
                if (tnk != windowTank[j])
                {

                    if (IsThereACollision(tnk, windowTank[j]) && IsBeingOnTheMap(tnk) && Dispatcher.Invoke(() => CheckListObjectMap(tnk)))
                    {
                        PositionСhange(tnk);
                    }
                    else
                    {
                        //Dispatcher.Invoke(() => CheckListObjectMap(tank));
                        if (tnk.GetType() == new BotTank().GetType())
                            ((BotTank)tnk).motionСancellation = true; 
                        tnk.CancellationMove();
                        
                    }
                }
            }
        } // Вызов метода отрисовки при соблюдение всех услоовий

        private void DeleteBullet(Bullet bullet)
        {
            Dispatcher.Invoke(() => canvas.Children.Remove(bullet.GetImage));
            windowBullet.Remove(bullet);
        } // Удаление пули

        private bool IsThereACollision(Tank tankOne, Tank tankTwo)
        {
            if ((Math.Abs(tankOne.GetCoordinates.CordinateToX - tankTwo.GetCoordinates.CordinateToX) >= 58 || Math.Abs(tankOne.GetCoordinates.CordinateToY - tankTwo.GetCoordinates.CordinateToY) >= 58))
                return true;
            return false;
        } // Проверка на столкновение танков
        private bool IsBeingOnTheMap(Tank tank)
        {
            if ((tank.GetCoordinates.CordinateToY >= 656 || tank.GetCoordinates.CordinateToY <= 0) || (tank.GetCoordinates.CordinateToX >= 1162 || tank.GetCoordinates.CordinateToX <= 0))
                return false;
            return true;
        } // Проверка танка на выход за границы карты
        private bool IsBeingOnTheMap(Bullet bullet)
        {
            if ((bullet.GetCoordinates.CordinateToY > 704 || bullet.GetCoordinates.CordinateToY < 0) || (bullet.GetCoordinates.CordinateToX > 1144 || bullet.GetCoordinates.CordinateToX < 0))
                return true;
            return false;
        } // Проверка пули на выход за границы карты
        private bool ChargeDestructionCheck(Tank tank, Bullet bullet)
        {
            if (Math.Abs((tank.GetCoordinates.CordinateToX + 29) - (bullet.GetCoordinates.CordinateToX + 4.5)) < 33.5 &&
                Math.Abs((tank.GetCoordinates.CordinateToY + 29) - (bullet.GetCoordinates.CordinateToY + 4.5)) < 33.5)
                return true;
            return false;

        } // Проверка на попадание пули в танк
        private bool CheckListRange(Tank tank, bool isCheck)
        {
            foreach (var item in listMapObjects)
            {
                if (item.IsCheckRange(windowTank[0], tank, isCheck))
                    continue;
                else
                    return false;
            }
            return true;
        } // Проверка на видимость танка игрока, танком противника
        private bool CheckListObjectMap(Tank tank)
        {
            foreach (var item in listMapObjects)
            {
                if (item.IsCheckMove(tank.GetCoordinates.CordinateToX, tank.GetCoordinates.CordinateToY, 29))
                    continue;
                else
                    return false;
            }
            return true;
        } // Проверка на столкновение танка с объектом карты
        private bool CheckListObjectMap(Bullet bullet)
        {
            foreach (var item in listMapObjects)
            {
                if (item.IsCheckMove(bullet.GetCoordinates.CordinateToX, bullet.GetCoordinates.CordinateToY, 4.5))
                    continue;
                else
                    return false;
            }
            return true;
        }// Проверка на столкновение пули с объектом карты

        private void PositionСhange(Tank tank)
        {
            Dispatcher.Invoke(() => Canvas.SetTop(tank.GetImage(), tank.GetCoordinates.CordinateToY));
            Dispatcher.Invoke(() => Canvas.SetLeft(tank.GetImage(), tank.GetCoordinates.CordinateToX));
        } // Перемещение танка
        private void PositionСhange(Bullet bullet)
        {
            Dispatcher.Invoke(() => Canvas.SetTop(bullet.GetImage, bullet.GetCoordinates.CordinateToY));
            Dispatcher.Invoke(() => Canvas.SetLeft(bullet.GetImage, bullet.GetCoordinates.CordinateToX));
        } // Перемещение пули

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {

            switch (e.Key)
            {
                case Key.W:
                    tank.Move("Up");
                    break;
                case Key.S:
                    tank.Move("Down");
                    break;
                case Key.A:
                    tank.Move("Left");
                    break;
                case Key.D:
                    tank.Move("Right");
                    break;
                case Key.Space:
                    tank.Shot(ref windowBullet);
                    break;
            }
            //tank.Turn();
        } // Отлавливание события нажатия на клавишу
        private void myWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (GAME)
            {
                objectMappingBullet.Abort();
                objectMappingTank.Abort();
                objectTankControl.Abort();
            }
        } // Отлавливание события закрытия приложения
        private void GameOver() {
            Stopwatch.Stop();
            objectMappingBullet.Abort();
            objectMappingTank.Abort();
            objectTankControl.Abort();
            MessageBox.Show("Игра окончена");
            foreach (var item in windowBullet)
            {
                canvas.Children.Remove(item.GetImage);
            }
            foreach (var item in windowTank)
            {
                canvas.Children.Remove(item.GetImage());
            }
            foreach (var item in listMapObjects)
            {
                mapGrid.Children.Remove(item.GetImage());
            }
            windowTank.Clear();
            windowBullet.Clear();
            listMapObjects.Clear();
            TimeGame.Text = Stopwatch.Elapsed.ToString();
            Game.Visibility = Visibility.Collapsed;
            MenuPostGame.Visibility = Visibility.Visible;
        } // Конец игры, остановка всех потоков 

        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            menu.Visibility = Visibility.Collapsed;
            MenuNewGame.Visibility = Visibility.Visible;
        }

        private void GameGame_Click(object sender, RoutedEventArgs e)
        {
            MenuNewGame.Visibility = Visibility.Collapsed;
            Game.Visibility = Visibility.Visible;
            InitializanionMap();
            InitializanionTank();
            InitializanionThread();
            GAME = true;
            Stopwatch = new Stopwatch();
            Stopwatch.Start();
        }

        private void SaveTimeGame_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OpenMenu_Click(object sender, RoutedEventArgs e)
        {
            MenuPostGame.Visibility = Visibility.Collapsed;
            menu.Visibility = Visibility.Visible;
        }
    }
}
