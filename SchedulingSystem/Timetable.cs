using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulingSystem {
    //3dO
    public class Timetable {
        public Timetable(string toExcel, Tuple<DateTime, DateTime> DailyTime) {
            Input ngine = new Input(toExcel, DailyTime);
            HashSet<Course> AllCourses = new HashSet<Course>(ngine.AllCourses.Values.ToList());
            Graph ical = new Graph(ngine.AllCourses.Values.ToList());
            foreach(Course c1 in )
        }
    }
}
