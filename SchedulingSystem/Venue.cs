using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
namespace SchedulingSystem {
    //Derry
    class Venue {
        private string VenueName;
        private int VenueCapacity;
        private bool IsLab;

        public Venue( String Name, int Capacity, bool Lab ){
        this.VenueName = Name;
        this.VenueCapacity = Capacity;
        this.IsLab = Lab;
        
        
}    
        public string getName {get {  return this.VenueName;}}
        
        public int getCapacity{get {return this.VenueCapacity;}}

         public bool isLab {get {return this.isLab;}}

        

    }
}
