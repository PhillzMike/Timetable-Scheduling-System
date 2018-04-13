using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulingSystem {
    public partial class Timetable {
        List<Dictionary<Course, Venue>> UtilizeSpace(HashSet<Course> hc) {
            List<Course> courses = new List<Course>(hc);
            List<Venue> venues = new List<Venue>(allVenues);
            venues.Sort();

            var map = new List<Dictionary<Course, Venue>>();

            for (int i = 0; i < courses.Count; i++) {
                map.Add(new Dictionary<Course, Venue>());
                for (int j = i + 1; j < courses.Count; j++) {
                    if (courses[j].Students.Count > venues[0].GetCapacity)
                        continue;
                    var tempVenue = venues[0];
                    for (int k = 1; i < venues.Count; i++) {
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
