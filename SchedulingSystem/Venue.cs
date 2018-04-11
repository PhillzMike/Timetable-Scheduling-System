using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulingSystem {
    //Derry
    class Venue {
        private string VenueName;
        private int VenueCapacity;
        private bool _isLab;

        public Venue( String Name, int Capacity, bool Lab ){
        this.VenueName = Name;
        this.VenueCapacity = Capacity;
        this._isLab = Lab;
        
        
}    
        public string GetName {get {  return this.VenueName;}}
        
        public int GetCapacity{get {return this.VenueCapacity;}}

         public bool IsLab {get {return this._isLab;}}

        

    }
}
