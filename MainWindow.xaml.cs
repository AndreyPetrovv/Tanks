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

namespace Tanks
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        static List<Tank> windowTank = new List<Tank>();
        static List<Bullet> windowBullet = new List<Bullet>();


        static Tank tank;

        public MainWindow()
        {
            InitializeComponent();

            FirsInitializanion();

            windowTank.Add(new Tank("2", 200, 50,"Eтанк"));

            Thread objectMappingTank = new Thread(ThreadMapingTank);
            Thread objectMappingBullet = new Thread(ThreadMapingBullet);
            objectMappingTank.Start();
            objectMappingBullet.Start();

        }

        void ThreadMapingTank()
        {
            //windowTank.Add(new Tank("2", 200, 50, "Eтанк"));

            while (true)
            {
                Thread.Sleep(1);
                for (int i = 0; i < windowTank.Count; i++)
                {

                    Dispatcher.Invoke(() => Canvas.SetLeft(windowTank[i].GetImage(), windowTank[i].PositionToX));
                    Dispatcher.Invoke(() => Canvas.SetTop(windowTank[i].GetImage(), windowTank[i].PositionToY));

                    if (windowTank[i].Health == 0)
                    {
                        Dispatcher.Invoke(() => canvas.Children.Remove(windowTank[i].GetImage()));
                        windowTank.Remove(windowTank[i]);
                    }
                }

            }
        }

        void ThreadMapingBullet()
        {
            while (true)
            {
                Thread.Sleep(5);
                for (int i = 0; i < windowBullet.Count; i++)
                {
                    windowBullet[i].flight();
                    Dispatcher.Invoke(() => Canvas.SetLeft(windowBullet[i].GetImage(), windowBullet[i].BulletPositionToX));
                    Dispatcher.Invoke(() => Canvas.SetTop(windowBullet[i].GetImage(), windowBullet[i].BulletPositionToY));
                    for (int j = 0; j < windowTank.Count; j++)
                    {
                        if ( windowBullet[i].BulletPositionToY == windowTank[j].PositionToY)
                            windowTank[j].Damage();
                    }
                    if ((windowBullet[i].BulletPositionToY > 450 || windowBullet[i].BulletPositionToY < 0) ||
                        (windowBullet[i].BulletPositionToX > 800 || windowBullet[i].BulletPositionToX < 0))
                    {
                        Dispatcher.Invoke(() => canvas.Children.Remove(windowBullet[i].GetImage()));
                        windowBullet.Remove(windowBullet[i]);
                    }
                }
            }
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
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
            }
        }

    }
}
