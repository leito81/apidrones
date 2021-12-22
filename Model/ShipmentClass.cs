using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apidrones.Model
{
    public class ShipmentClass
    {
        public int IdShipment { get; set; }

        //relations
        public String DroneId { get; set; }
        public DroneClass Drone { get; set; }

        public ICollection<ShipmentDetails> Details { get; set; }

    }
}
