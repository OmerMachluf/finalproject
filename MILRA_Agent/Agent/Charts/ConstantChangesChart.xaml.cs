using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System.Collections.Generic;

namespace Wpf.CartesianChart
{
    /// <summary>
    /// Interaction logic for ConstantChangesChart.xaml
    /// </summary>
    public partial class ConstantChangesChart
    {
        public ConstantChangesChart()
        {
            InitializeComponent();

            Buffer = new ChartValues<ObservableValue>();

            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Values = Buffer
                }
            };

            

            Timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(1000)
            };

            Timer.Tick += TimerOnTick;

            IsDataInjectionRunning = false;

            R = new Random();

            DataContext = this;

            predictions = new List<double>();
            predictionsHistory = new List<double>();
        }

        private Object listLocker = new Object();
        private Object historyLocker = new Object();
        private List<double> predictions;
        private List<double> predictionsHistory;
        public SeriesCollection SeriesCollection { get; set; }
        public ChartValues<ObservableValue> Buffer { get; set; }
        public DispatcherTimer Timer { get; set; }
        public bool IsDataInjectionRunning { get; set; }
        public Random R { get; set; }

        public void InjectPrediction(double value)
        {
            lock (listLocker)
            {
                predictions.Add(value);
            }
        }

        public List<double> GetPredictionHistoryList(double value)
        {
            var predC = predictions.Count;
            List<double> listToReturn;
            lock (historyLocker)
            {
                listToReturn = new List<double>(predictionsHistory) ;
            }

            return listToReturn;
        }

        private void RunDataOnClick(object sender, RoutedEventArgs e)
        {
            if (!IsDataInjectionRunning)
            {
                Timer.Start();
                IsDataInjectionRunning = true;
                this.runB.IsEnabled = false;
                this.stopB.IsEnabled = true;
            }
        }

        private void TimerOnTick(object sender, EventArgs eventArgs)
        {
            List<double> tempList = new List<double>();

           // Buffer.Add(new ObservableValue(R.Next(0, 10)));
           

            lock(listLocker)
            {
                foreach (var val in predictions)
                {
                    tempList.Add(val);
                    Buffer.Add(new ObservableValue(val));
                }

                predictions.Clear();
            }

            lock(historyLocker)
            {
                predictionsHistory.AddRange(tempList);
            }

            if (Buffer.Count > 10) Buffer.RemoveAt(0);
        }

        private void StopDataOnClick(object sender, RoutedEventArgs e)
        {
            if (IsDataInjectionRunning)
            {
                Timer.Stop();
                IsDataInjectionRunning = false;
                this.runB.IsEnabled = true;
                this.stopB.IsEnabled = false;
            }
        }
    }
}
