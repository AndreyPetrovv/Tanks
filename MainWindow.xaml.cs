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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Random r = new Random();
            Tank tankOne = new Tank("1", r.Next(50, 760), r.Next(50, 390));
            canvas.Children.Add(tankOne.drawingTankBo());
            canvas.Children.Add(tankOne.drawingTankBa());
            Canvas.SetLeft(tankOne.drawingTankBo(), tankOne.PositionToX);
            Canvas.SetTop(tankOne.drawingTankBo(), tankOne.PositionToY);
            Canvas.SetLeft(tankOne.drawingTankBa(), tankOne.PositionToX);
            Canvas.SetTop(tankOne.drawingTankBa(), tankOne.PositionToY);
        }
    }
}
