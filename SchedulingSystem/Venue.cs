using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulingSystem {
    //Derry
    [Serializable]
   public class Venue {
        private string VenueName;
        private int VenueCapacity;
        private bool _isLab;
        private static int noOfVenues;

        public Venue( String Name, int Capacity, bool Lab ){
            this.VenueName = Name;
            this.VenueCapacity = Capacity;
            this._isLab = Lab;
            noOfVenues++;
        
}    
        public string GetName {get {  return this.VenueName;}}
        
        public int GetCapacity{get {return this.VenueCapacity;}}

         public bool IsLab {get {return this._isLab;}}

        public static int NoOfVenues
        {
            get => noOfVenues;
        }
        public override string ToString() {
            return VenueName;

        }

    }
}
