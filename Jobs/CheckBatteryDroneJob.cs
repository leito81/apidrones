using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Microsoft.Extensions.Logging;
using apidrones.Data;

namespace apidrones.Jobs
{
    [DisallowConcurrentExecution]
    public class CheckBatteryDroneJob : IJob
    {
        private readonly ILogger<CheckBatteryDroneJob> Logger;
        private readonly IServiceProvider _serviceProvider;

        public CheckBatteryDroneJob(ILogger<CheckBatteryDroneJob> logger, IServiceProvider serviceProvider)
        {
            Logger = logger;
            _serviceProvider = serviceProvider;
        }

        public Task Execute(IJobExecutionContext context)
        {
            
            using (var scope = _serviceProvider.CreateScope())
            {
                var _context = scope.ServiceProvider.GetService<ApidronesContext>();

                var levelBattery = _context.Drones.Select(S => new { S.Serial_number, S.Battery_capacity }).ToList();

                string LevelBatteryInfo = String.Empty;

                foreach (var ite in levelBattery)
                {
                    LevelBatteryInfo += String.Format("Drone {0} with battery level {1}. DateTime: {2} \n", ite.Serial_number, ite.Battery_capacity, DateTime.Now.ToString());
                }

                String exeroute = Directory.GetCurrentDirectory();
                StreamWriter fileLogs = new StreamWriter(exeroute + "/CheckBatteryLevelsLogs.txt", true);
                fileLogs.WriteLine((LevelBatteryInfo == String.Empty) ? "There are not drones in the database." : LevelBatteryInfo);
                fileLogs.Close();

                Logger.Log(LogLevel.Information, "Battery levels checked");
            }

            return Task.CompletedTask;
        }
    }
}
