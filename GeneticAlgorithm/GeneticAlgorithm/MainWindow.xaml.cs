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
		List<AlgorithmResult> ar;

        public MainWindow()
        {
            InitializeComponent();

            // load file.
            links = FileHelper.ReadFile("Data\\network.txt");
           // ConfigurePlot();
        }

        private void buttonRun_Click(object sender, RoutedEventArgs e)
        {
            resultBox.Items.Clear();
            //points.Clear();
            
            Algorithm a = new Algorithm(links,
                Convert.ToInt32(inputGenerations.Text),
                Convert.ToInt32(inputPopulation.Text),
                Convert.ToDouble(inputCrossover.Text),
                Convert.ToDouble(inputMutation.Text));

			ar = a.Run().ToList();

            int x = 0;

            maxPoints.Clear();
            avgPoints.Clear();

            foreach (var item in ar)
            {
                maxPoints.Add(new DataPoint(x, item.MaxFitness));
                avgPoints.Add(new DataPoint(x, item.AverageFitness));
				resultBox.Items.Add(item.ToString());
                x++;
            }

            plotter.InvalidatePlot(true); 
        }


        /// <summary>
        /// Do we need this?
        /// </summary>
        void ConfigurePlot()
        {
            plotter.LegendTitle = "Legend";
            plotter.LegendOrientation = LegendOrientation.Horizontal;
            plotter.LegendPlacement = LegendPlacement.Outside;
            plotter.LegendPosition = LegendPosition.TopRight;
            plotter.LegendBackground = Colors.LimeGreen;
            plotter.LegendBorder = Colors.Black;

        }

        public static List<DataPoint> maxPoints = new List<DataPoint>();

        public static List<DataPoint> avgPoints = new List<DataPoint>();

		/// <summary>
		/// Handles the Click event of the btnSaveData control.
		/// This method was created to diagnose a bug found when running the algorithm.
		/// Perhaps there's a bug in the code causing the population to die out, or a bad random number generator?
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
		private void btnSaveData_Click(object sender, RoutedEventArgs e)
		{
			FileHelper.WriteFile("Chromosomes.txt", ar);
		}
	}

	public class MainViewModel
    {
        public MainViewModel()
        {
            this.Title = "Overlay Networks using a Genetic Algorithm";


            MainWindow.maxPoints = new List<DataPoint>
            {
                //new DataPoint(0, 4),
                //new DataPoint(10, 13),
                //new DataPoint(20, 15),
                //new DataPoint(30, 16),
                //new DataPoint(40, 12),
                //new DataPoint(50, 12)
            };

            MainWindow.avgPoints = new List<DataPoint>();

            this.MaxPoints = MainWindow.maxPoints;
            this.AvgPoints = MainWindow.avgPoints;
        }

        public string Title { get; private set; }

        public IList<DataPoint> MaxPoints { get; private set; }
        public IList<DataPoint> AvgPoints { get; private set; }
    }
}
