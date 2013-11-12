using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Configuration;

namespace RSSPollingService
{
    public partial class RSSPollingService : ServiceBase
    {
        private Timer _timer;
        private DateTime _lastRun = DateTime.Now;


        public RSSPollingService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            PerformTask(null);

            int pollTimeInMinutes = ConfigurationManager.AppSettings["PollTime"].ToIntegerOrDefault(10);
            int pollTimeInMS = pollTimeInMinutes * 60 * 1000;
            //int pollTimeInMS = 7000;

            _timer = new System.Threading.Timer(PerformTask, null, 1000, pollTimeInMS);

            //_timer = new Timer(); // every 10 minutes
            //_timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
        }

        protected override void OnStop()
        {
            //_lastRun = DateTime.Now;
            //_timer.Stop();
        }

        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            // ignore the time, just compare the date
            //if (_lastRun.Date < DateTime.Now)
            //{
                // stop the timer while we are running the cleanup task
                //_timer.Stop();

                PerformTask(null);

                //_lastRun = DateTime.Now;
                //_timer.Start();
            //}
        }

        private void PerformTask(object source)
        {
            DiabloNewsRSSConsumer consumer = new DiabloNewsRSSConsumer();
            consumer.OnConsumeDone += new DiabloNewsRSSConsumer.ConsumeDone(consumer_OnConsumeDone);
            consumer.Consume();

        }

        void consumer_OnConsumeDone(List<DAL.News> news)
        {
            int newsItemsAdded = (news != null) ? news.Count : 0;
            StringBuilder eventLogEntry = new StringBuilder(String.Format("RSS Polling retreived {0} new records", newsItemsAdded));
            for (int i = 0; i < newsItemsAdded; i++)
            {
                eventLogEntry.AppendLine(String.Format("{0} of type {1} published at {2}", news[i].Subject, news[i].ArticleTypeID, news[i].DateTime));

            }

            D3RSSEventLogs.WriteEntry(eventLogEntry.ToString(), EventLogEntryType.Information);
        }

    }
}
