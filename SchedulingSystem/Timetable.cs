using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulingSystem {
    //3dO
    public class Timetable {
        Graph<Course> icarly;
        HashSet<Venue> allVenues;
        public Timetable(string toExcel, Tuple<DateTime, DateTime> DailyTime) {
            allVenues = new HashSet<Venue>();
            Input ngine = new Input(toExcel, DailyTime);
            allVenues.UnionWith(ngine.AllVenues);
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
        List<Dictionary<Course,Venue>> UtilizeSpace(HashSet<Course> hc) {
            List<Course> courses = new List<Course>(hc);
            List<Venue> venues = new List<Venue>(allVenues);
            venues.Sort();

            var map = new List<Dictionary<Course,Venue>>();
            
            for(int i=0;i<courses.Count;i++)
            {
                map.Add(new Dictionary<Course, Venue>());
                for (int j = i + 1; j < courses.Count; j++)
                {
                    if (courses[j].Students.Count > venues[0].GetCapacity)
                        continue;
                    var tempVenue = venues[0];
                    for (int k = 1; i < venues.Count; i++)
                    {
                        if (courses[j].Students.Count > venues[i].GetCapacity)
                            break;
                        tempVenue = venues[i];
                    }
                    venues.Remove(tempVenue);
                }
                

            }
            
        }
        
    }

}
