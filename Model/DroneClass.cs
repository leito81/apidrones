using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace apidrones.Model
{
    public enum DroneStateId : int
    {
        IDLE = 1,
        LOADING = 2,
        LOADED = 3,
        DELIVERING = 4,
        DELIVERED = 5,
        RETURNING = 6
    };

    public enum DroneModelId : int
    {
        Lightweight = 1,
        Middleweight = 2,
        Cruiserweight = 3,
        Heavyweight = 4
    };

    public class DroneState
    {
        public DroneStateId DroneStateId { get; set; }
        public string StateName { get; set; }

        public ICollection<DroneClass> Drones { get; set; }
    }

    public class DroneModel
    {
        public DroneModelId DroneModelId { get; set; }
        public string ModelName { get; set; }

        public ICollection<DroneClass> Drones { get; set; }
    }

    
    public class DroneClass
    {
        [StringLength(100, ErrorMessage ="The length of the serial number must be up to 100 characters.")]
        public String Serial_number { get; set; } 

        [Range ( 1, 500, ErrorMessage ="The weight limit is 500")]
        public int Weight_limit { get; set; }
        public int Battery_capacity { get; set; } //Percentage

        //relations
        public DroneStateId droneStateId { get; set; }
        public DroneState droneState { get; set; }

        public DroneModelId droneModelId { get; set; }
        public DroneModel droneModel { get; set; }

        public ICollection<ShipmentClass> Shipments { get; set; }



    }
}
