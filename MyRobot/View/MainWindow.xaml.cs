using MyRobot.Model;
using MyRobot.Model.Network;
using MyRobot.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyRobot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        RobotViewModel vm;
        public MainWindow()
        {
            InitializeComponent();
            vm = new RobotViewModel(new MyRobotModel(new MyTelnetClient("0",0)));
            DataContext = vm;
        }

        private void Start(object sender, RoutedEventArgs e)
        {
            MyServer server = new MyServer(1234, MyServer.MyClientHandler, 3);

            server.start();
            /*
            DateTime m_start = DateTime.Now;

            string error = "";
            // Indexer.postingFilesPath = m_postingFilesPath + "\\";
            //  Indexer.documentsPath = m_documentsPath + "\\";

            // if (!Directory.Exists(m_documentsPath))

            //    isValid = false;
            error = "Path for dataset is missing. Please choose a folder.\n";
            //   isValid = false;
            error += "Path for posting files is missing. Please choose a folder\n";
            error += "Stop words file is missing in the folder.\n";



            DateTime m_end = DateTime.Now;
            string m_time = (m_end - m_start).ToString();


            //    MessageBoxResult mbr = System.Windows.MessageBox.Show("Running Time : " + m_time + "\n" + "Number of indexed documents: " + ReadFile.totalDocs + "\n" + "Number of unique terms: " + Indexer.amountOfUnique, "Output", MessageBoxButton.OK, MessageBoxImage.None);
            */


        }

        private bool dragStarted = false;

        private void Slider_DragCompleted(object sender, DragCompletedEventArgs e)
        {
           // DoWork(((Slider)sender).Value);
            this.dragStarted = false;
            vm.VM_Azimuth = (int)(((Slider)sender).Value);
        }

        private void Slider_DragStarted(object sender, DragStartedEventArgs e)
        {
           // vm.VM_Azimuth = (int)(((Slider)sender).Value);
            this.dragStarted = true;
        }

        private void Slider_ValueChanged(
            object sender,
            RoutedPropertyChangedEventArgs<double> e)
        {
            //   if (!dragStarted)
            //DoWork(e.NewValue);
            //   vm.VM_Azimuth = (int)(((Slider)sender).Value);
            progressBar1.Value = e.NewValue;
        }

      

    }
}
