using GeneticAlgorithm.HomeworkLib.GA;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
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

namespace GeneticAlgorithm
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Link> links;

        public MainWindow()
        {
            InitializeComponent();

            // load file.
            links = FileHelper.ReadFile("Data\\network.txt");

        }

        private void buttonRun_Click(object sender, RoutedEventArgs e)
        {
            //resultBox.Items.Clear();
            //points.Clear();
            
            Algorithm a = new Algorithm(links,
                Convert.ToInt32(inputGenerations.Text),
                Convert.ToInt32(inputPopulation.Text),
                Convert.ToDouble(inputCrossover.Text),
                Convert.ToDouble(inputMutation.Text));

            List<AlgorithmResult> ar = a.Run().ToList();
            int x = 0;
            points.Clear();
            foreach (var item in ar)
            {
                points.Add(new DataPoint(x, item.AverageFitness));
                x++;
            }

            plotter.InvalidatePlot(true);
        }

        public static List<DataPoint> points = new List<DataPoint>();
    }

    public class MainViewModel
    {
        public MainViewModel()
        {
            this.Title = "Example 2";
            MainWindow.points = new List<DataPoint>
            {
                new DataPoint(0, 4),
                new DataPoint(10, 13),
                new DataPoint(20, 15),
                new DataPoint(30, 16),
                new DataPoint(40, 12),
                new DataPoint(50, 12)
            };

            this.Points = MainWindow.points;
        }

        public string Title { get; private set; }

        public IList<DataPoint> Points { get; private set; }
    }
}
