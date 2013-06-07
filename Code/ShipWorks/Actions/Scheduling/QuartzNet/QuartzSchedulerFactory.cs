using Quartz.Impl;
using ShipWorks.Data.Connection;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;


namespace ShipWorks.Actions.Scheduling.QuartzNet
{
    public class QuartzSchedulerFactory : Quartz.ISchedulerFactory
    {
        readonly Quartz.ISchedulerFactory innerFactory;

        public QuartzSchedulerFactory()
        {
            var sqlConfig = new SqlSessionConfiguration();
            sqlConfig.Load();

            var properties = new NameValueCollection {
                { StdSchedulerFactory.PropertyCheckConfiguration,         "true" },
                { StdSchedulerFactory.PropertyDataSourceConnectionString, sqlConfig.GetConnectionString() },
                { StdSchedulerFactory.PropertyTablePrefix,                "Scheduling" }
            };

            this.innerFactory = new StdSchedulerFactory(properties);
        }


        public Quartz.IScheduler GetScheduler()
        {
            return innerFactory.GetScheduler();
        }


        #region Unsupported

        public ICollection<Quartz.IScheduler> AllSchedulers
        {
            get { throw new NotSupportedException(); }
        }

        public Quartz.IScheduler GetScheduler(string schedName)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
