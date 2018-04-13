using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulingSystem {
    //3dO
    public class Timetable {
        Graph<Course> icarly;
        public Timetable(string toExcel, Tuple<DateTime, DateTime> DailyTime) {
            Input ngine = new Input(toExcel, DailyTime);
            HashSet<Course> AllCourses = new HashSet<Course>(ngine.AllCourses.Values);
             icarly = new Graph<Course>(ngine.AllCourses.Values);
            foreach(Course c1 in AllCourses) {
                foreach(Course c2 in AllCourses) {
                    if(!c1.Equals(c2))
                    if(c1.Students.Intersect(c2.Students).Count()+ c1.Lecturers.Intersect(c2.Lecturers).Count() > 0) {
                            icarly.SetEdge(c1, c2);
                    }
                }
            }
            ClusterTogether();
        }
        List<HashSet<Course>> CanBeTogether;
        Dictionary<Course, List<int>> Course_sets;
        Dictionary<Course, int> HoursLeft;
        Graph<HashSet<Course>> Diff_days;
        List<List<HashSet<Course>>> Days;
        void ClusterTogether(){
            List<HashSet<Course>> init = icarly.ColorGraph();
            foreach(HashSet<Course> hc in init) {
                CanBeTogether.AddRange(UtilizeSpace(hc));
            }
        }
        void Separate(List<HashSet<HashSet<Course>>> TT) {
            List<HashSet<HashSet<Course>>> Sep = new List<HashSet<HashSet<Course>>>();
            Random r = new Random();

        }
        List<HashSet<Course>> UtilizeSpace(HashSet<Course> hc) {
            throw new NotImplementedException();
        }
        
    }

}
