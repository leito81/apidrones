using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apidrones.Jobs
{
    public class CheckBatteryDroneJobSchedule
    {
        public CheckBatteryDroneJobSchedule(Type jobType, string cronExpression)
        {
            JobType = jobType;
            _cronExpression = cronExpression;
        }

        public Type JobType { get; }
        public string _cronExpression { get; }
    }

}
