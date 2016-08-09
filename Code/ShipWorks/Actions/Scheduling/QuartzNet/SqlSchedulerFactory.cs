using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Quartz.Impl;
using ShipWorks.Data.Connection;


namespace ShipWorks.Actions.Scheduling.QuartzNet
{
    public class SqlSchedulerFactory : Quartz.ISchedulerFactory
    {
        readonly Quartz.ISchedulerFactory innerFactory;

        public SqlSchedulerFactory()
        {
            var sqlConfig = new SqlSessionConfiguration();
            sqlConfig.Load();

            var properties = new NameValueCollection {
                { "quartz.checkConfiguration",                    "true"                                              },
                { "quartz.jobStore.type",                         "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz"        },
                { "quartz.jobStore.driverDelegateType",           "Quartz.Impl.AdoJobStore.SqlServerDelegate, Quartz" },
                { "quartz.jobStore.dataSource",                   "shipWorks"                                         },
                { "quartz.dataSource.shipWorks.provider",         "SqlServer-20"                                      },
                { "quartz.dataSource.shipWorks.connectionString", sqlConfig.GetConnectionString()                     },
                { "quartz.jobStore.tablePrefix",                  "Scheduling_"                                       },
                { "quartz.jobStore.useProperties",                "true"                                              },
                { "quartz.threadPool.threadCount",                "2"                                                 },
                { "quartz.serializer.type",                       typeof(VersionAgnosticObjectSerializer).AssemblyQualifiedName  },
            };

            innerFactory = new StdSchedulerFactory(properties);
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
