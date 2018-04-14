using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulingSystem {
    public partial class Timetable {
        List<Dictionary<Course, Venue>> UtilizeSpace(HashSet<Course> hc) {
            var venues = new List<Venue>(allVenues);
            var courses = new List<Course>(hc);
            var map = new List<Dictionary<Course, Venue>>();
            for(int i = 0; i < hc.Count; i++)
            {
                var dict = new Dictionary<Course, Venue>();
                RecursiveGuy(venues, courses, dict);
                courses.RemoveAt(0);
                map.Add(dict);
            }
            return map;
        }
        private void RecursiveGuy(List<Venue> venues,List<Course> allcourses, Dictionary<Course, Venue> sol)
        {
            for(int j = 0;j<allcourses.Count;j++)
            {

                for (int i = 0; i < venues.Count; i++)
                {
                    if(allcourses[j].Students.Count < venues[i].GetCapacity)
                    {
                        sol.Add(allcourses[j], venues[i]);
                        var venue = venues[i];
                        var course = allcourses[j];
                        venues.RemoveAt(i);
                        allcourses.RemoveAt(j);
                        RecursiveGuy(venues,allcourses, sol);
                        venues.Insert(i, venue);
                        allcourses.Insert(j, course);

                    }
                        
                }
            }
        }

    }
}
