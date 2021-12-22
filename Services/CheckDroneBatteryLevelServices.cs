using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Spi;
using apidrones.Jobs;
using System.IO;

namespace apidrones.Services
{
    public class CheckDroneBatteryLevelServices : IHostedService
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IJobFactory _jobFactory;
        private readonly CheckBatteryDroneJobSchedule _jobDroneBatteryCheckLevel;

        public CheckDroneBatteryLevelServices(ISchedulerFactory schedulerFactory, IJobFactory jobFactory, CheckBatteryDroneJobSchedule jobSchedule)
        {
            _schedulerFactory = schedulerFactory;
            _jobDroneBatteryCheckLevel = jobSchedule;
            _jobFactory = jobFactory;
        }
        public IScheduler Scheduler { get; set; }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Scheduler =  await _schedulerFactory.GetScheduler(cancellationToken);
            Scheduler.JobFactory = _jobFactory;

            var job = JobBuilder.Create(_jobDroneBatteryCheckLevel.JobType).WithIdentity(_jobDroneBatteryCheckLevel.JobType.FullName).WithDescription(_jobDroneBatteryCheckLevel.JobType.Name).Build();
            var trigger = TriggerBuilder.Create().WithIdentity($"{_jobDroneBatteryCheckLevel.JobType.FullName}.trigger").WithCronSchedule(_jobDroneBatteryCheckLevel._cronExpression).WithDescription(_jobDroneBatteryCheckLevel._cronExpression).Build();
            await Scheduler.ScheduleJob(job, trigger, cancellationToken);

            await Scheduler.Start(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Scheduler?.Shutdown(cancellationToken);
        }
    }
}
