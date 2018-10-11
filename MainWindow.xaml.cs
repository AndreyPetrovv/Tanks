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

namespace Tanks
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Tank tankOne;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void addCanvas() {
            canvas.Children.Add(tankOne.drawingTankBo());
            canvas.Children.Add(tankOne.drawingTankBa());
            Canvas.SetLeft(tankOne.drawingTankBo(), tankOne.PositionToX);
            Canvas.SetTop(tankOne.drawingTankBo(), tankOne.PositionToY);
            Canvas.SetLeft(tankOne.drawingTankBa(), tankOne.PositionToX);
            Canvas.SetTop(tankOne.drawingTankBa(), tankOne.PositionToY);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Random r = new Random();
            tankOne = new Tank("1", 250, 250);
            addCanvas();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key) {
                case Key.W:
                    canvas.Children.Remove(tankOne.drawingTankBo());
                    canvas.Children.Remove(tankOne.drawingTankBa());
                    tankOne.Move(0,-3);
                    addCanvas();
                    break;
                case Key.S:
                    canvas.Children.Remove(tankOne.drawingTankBo());
                    canvas.Children.Remove(tankOne.drawingTankBa());
                    tankOne.Move(0, 3);
                    addCanvas();
                    break;
                case Key.A:
                    canvas.Children.Remove(tankOne.drawingTankBo());
                    canvas.Children.Remove(tankOne.drawingTankBa());
                    tankOne.Move(-3, 0);
                    addCanvas();
                    break;
                case Key.D:
                    canvas.Children.Remove(tankOne.drawingTankBo());
                    canvas.Children.Remove(tankOne.drawingTankBa());
                    tankOne.Move(3, 0);
                    addCanvas();
                    break;
                case Key.Left:
                    canvas.Children.Remove(tankOne.drawingTankBo());
                    canvas.Children.Remove(tankOne.drawingTankBa());
                    tankOne.barrelRotation(-1,0);
                    addCanvas();
                    break;
                case Key.Right:
                    canvas.Children.Remove(tankOne.drawingTankBa());
                    canvas.Children.Remove(tankOne.drawingTankBo());
                    tankOne.barrelRotation(1, 0);
                    addCanvas();
                    break;
                case Key.Up:
                    canvas.Children.Remove(tankOne.drawingTankBa());
                    canvas.Children.Remove(tankOne.drawingTankBo());
                    tankOne.barrelRotation(0, -1);
                    addCanvas();
                    break;
                case Key.Down:
                    canvas.Children.Remove(tankOne.drawingTankBa());
                    canvas.Children.Remove(tankOne.drawingTankBo());
                    tankOne.barrelRotation(0, 1);
                    addCanvas();
                    break;
            }
        }
    }
}
