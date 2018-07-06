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
using System.Windows.Shapes;
using LiveCharts.Wpf;
using LiveCharts.Configurations;
using Wpf.Annotations;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Wpf.Gauges;
using Wpf.CartesianChart;
using System.Threading;

namespace Agent
{
    /// <summary>
    /// Interaction logic for AgentBoard.xaml
    /// </summary>
    public partial class AgentBoard : Window
    {

        public delegate void PercentageHandler(double percentage);
        private static PercentageHandler handler;

        private static DataServiceManager DSManger;
        private Thread uiTask;

        private Gauge180 _gaugeView;
        private ConstantChangesChart _chartView;

        public int PredictionAlertValue { get; set; }
        public double RatingValue { get; set; }
        public double RatingCount { get; set; }
        private bool isClockShown = true;

        private double predictionValue;

        public AgentBoard()
        {
            InitializeComponent();

            DSManger = new DataServiceManager();

            handler = new PercentageHandler(updatePercentage);

            GaugeView = new Gauge180();
            ChartView = new ConstantChangesChart();
            DataContext = this;

            titleText.Text = "Hello " + Environment.UserName + ", let's check if you are realy how you say you are";
            uiTask = new Thread(CheckPercentage);
            uiTask.Start();
        }

        private void UIElement_OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MenuToggleButton.IsChecked = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Gauge180 GaugeView
        {
            get { return _gaugeView; }
            set
            {
                _gaugeView = value;
                OnPropertyChanged();
            }
        }

        public ConstantChangesChart ChartView
        {
            get { return _chartView; }
            set
            {
                _chartView = value;
                OnPropertyChanged();
            }
        }

        

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null) PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void CheckPercentage()
        {
            while (true)
            {
                if (GaugeView == null)
                    break;
                //System.Random rnd = new System.Random();
                //ProfileHonestyRate.Rate = rnd.NextDouble() * rnd.Next(0, 100);
                ProfileHonestyRate.Rate = DSManger.getPredictionFromService();
                handler(ProfileHonestyRate.Rate);
                Thread.Sleep(1000);
            }
        }


        public void updatePercentage(double percentage)
        {
            if(_gaugeView != null)
                _gaugeView.Value = percentage;

            if (_chartView != null)
                _chartView.InjectPrediction(percentage);
        }

        private void lockerToggled(object sender, RoutedEventArgs e)
        {
            if(lockTog.IsChecked == false)
            {
                this.userProp.IsEnabled = false;
                lockTog.ToolTip = "Click To Unlock";
                this.lockIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Lock;
            }
            else
            {
                this.userProp.IsEnabled = true;
                lockTog.ToolTip = "Click To Lock";
                this.lockIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.LockOpen;
            }
            
        }


        private void BasicRatingBar_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            double prevVal = RatingValue * RatingCount++;
            RatingValue = (BasicRatingBar.Value + prevVal) / RatingCount;
        }

        private void BasicRatingBar_TargetUpdated(object sender, DataTransferEventArgs e)
        {
            double prevVal = RatingValue * RatingCount++;
            RatingValue = (BasicRatingBar.Value + prevVal) / RatingCount;
        }

        private void ListBox_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as ListBox).SelectedItem;
            if (item != null)
            {
                if (item == subscribe)
                    subscribeOp();
                else if (item == closeClock)
                    closeClockOp();
                else if (item == exit)
                    exitOp();
                else if (item == onePager)
                    onePagerOp();
            }
        }

        private void onePagerOp()
        {
           // throw new NotImplementedException();
        }

        private void exitOp()
        {
            this.Close();
        }

        private void closeClockOp()
        {
            if (isClockShown)
            {
                this.MILRA_Clock.Visibility = Visibility.Hidden;
                this.closeClock.Content = "Show Clock";
                isClockShown = false;
            }
            else
            {
                this.MILRA_Clock.Visibility = Visibility.Visible;
                this.closeClock.Content = "Close Clock";
                isClockShown = true;
            }
        }

        private void subscribeOp()
        {
            //throw new NotImplementedException();
        }

        private void Rate_Button_Click(object sender, RoutedEventArgs e)
        {
            
            RatingValue = ((RatingValue * RatingCount) + this.BasicRatingBar.Value) / ++RatingCount;

        }


        //    public void updatePercentage(double percentage)
        //    {
        //        var r = new Random();
        //        foreach (var series in Series)
        //        {
        //            if (series.Values.Count > 10) series.Values.RemoveAt(0);
        //            series.Values.Add(new PredictionViewModel
        //            {
        //               Prediction = r.NextDouble() * 30,
        //                DateTime = DateTime.Now
        //            });
        //        }
        //   }

        //    public SeriesCollection Series { get; set; }
        //      public Func<double, string> YFormatter { get; set; }
        //     public Func<double, string> XFormatter { get; set; }


    }
}
