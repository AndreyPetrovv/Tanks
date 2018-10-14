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

        static Tank tank;

        public MainWindow()
        {
            InitializeComponent();

            FirsInitializanion();
        }

        private void FirsInitializanion()
        {
            tank = new Tank("1", 250, 250);
            tank.Drawing();
            AddCanvas(tank.GetImage());
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
                    tank.Move(0, -2, new Uri("pack://application:,,,/ImageTank/танк.png"));
                    break;
                case Key.S:
                    tank.Move(0, 2, new Uri("pack://application:,,,/ImageTank/танкD.png"));
                    break;
                case Key.A:
                    tank.Move(-2, 0, new Uri("pack://application:,,,/ImageTank/танкL.png"));
                    break;
                case Key.D:
                    tank.Move(2, 0, new Uri("pack://application:,,,/ImageTank/танкR.png"));
                    break;
                case Key.Space:
                    tank.Shot();
                    canvas.Children.Add(tank.FlightShot());

                    Thread thread = new Thread(TankShot);
                    thread.SetApartmentState(ApartmentState.STA);
                    DateTime date1 = new DateTime();
                    thread.Name = "Поток" + date1;
                    thread.Start();
                    break;
            }
            AddCanvas(tank.GetImage());
        }

        public void TankShot()
        {
            //tank.Shot();

            for (int i = 0; i < 30; i++)
            {
                
                Thread.Sleep(100);
                canvas.Dispatcher.Invoke(new ThreadStart(delegate { Canvas.SetLeft(tank.FlightShot(), tank.GetBulll().BulletPositionToX); }));
                canvas.Dispatcher.Invoke(new ThreadStart(delegate { Canvas.SetTop(tank.FlightShot(), tank.GetBulll().BulletPositionToY); }));
                //Dispatcher.CurrentDispatcher.Invoke(new ThreadStart(delegate { Canvas.SetLeft(tank.FlightShot(), tank.GetBulll().BulletPositionToX); }));
                //Dispatcher.CurrentDispatcher.Invoke(new ThreadStart(delegate { Canvas.SetLeft(tank.FlightShot(), tank.GetBulll().BulletPositionToY); }));
                //Canvas.SetLeft(tank.FlightShot(), tank.GetBulll().BulletPositionToX);
                //Canvas.SetTop(tank.FlightShot(), tank.GetBulll().BulletPositionToY);
            }
        }

    }
}
