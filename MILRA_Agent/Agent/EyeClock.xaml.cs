using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace Agent
{
    /// <summary>
    /// Interaction logic for MILRA_Clock.xaml
    /// </summary>
    public partial class MILRA_Clock : UserControl
    {
        System.Timers.Timer timer = new System.Timers.Timer(1000);

        public MILRA_Clock()
        {
            InitializeComponent();

            IsraelCalendar mdCalendar = new IsraelCalendar();
            DateTime date = DateTime.Now;
            TimeZone time = TimeZone.CurrentTimeZone;
            TimeSpan difference = time.GetUtcOffset(date);
            //uint currentTime = mdCalendar.Time() + (uint)difference.TotalSeconds;
            uint currentTime = (uint)mdCalendar.GetIsraelTime().Second + (uint)difference.TotalSeconds;
            christianityCalendar.Content = mdCalendar.GetIsraelTime();

            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            timer.Enabled = true;
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //http://thispointer.spaces.live.com/blog/cns!74930F9313F0A720!252.entry?_c11_blogpart_blogpart=blogview&_c=blogpart#permalink
            this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
            {
                secondHand.Angle = DateTime.Now.Second * 6; 
                minuteHand.Angle = DateTime.Now.Minute * 6; 
                hourHand.Angle = (DateTime.Now.Hour * 30) + (DateTime.Now.Minute * 0.5); 
            }));
        }

        private void rootLayout_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
           // this.DragMove();
        }
    }

    internal class IsraelCalendar
    {
        public DateTime GetIsraelTime()
        {
            return (GetIsraelTime(DateTime.UtcNow));
        }

        // input: UTC DateTime object
        public DateTime GetIsraelTime(DateTime d)
        {
            d = d.AddHours(2);           // Israel is at GMT+2

            // April 2nd, 2:00 AM
            DateTime DSTStart = new DateTime(d.Year, 4, 2, 2, 0, 0);
            while (DSTStart.DayOfWeek != DayOfWeek.Friday)
                DSTStart = DSTStart.AddDays(-1);

            CultureInfo jewishCulture = CultureInfo.CreateSpecificCulture("he-IL");
            System.Globalization.HebrewCalendar cal =
                  new System.Globalization.HebrewCalendar();
            jewishCulture.DateTimeFormat.Calendar = cal;
            // Yom HaKipurim, at the start of the next Jewish year, 2:00 AM
            DateTime DSTFinish =
                 new DateTime(cal.GetYear(DSTStart) + 1, 1, 10, 2, 0, 0, cal);
            while (DSTFinish.DayOfWeek != DayOfWeek.Sunday)
                DSTFinish = DSTFinish.AddDays(-1);

            if (d > DSTStart && d < DSTFinish)
                d = d.AddHours(1);

            return (d);
        }
    }
}
