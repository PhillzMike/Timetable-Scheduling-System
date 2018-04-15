using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulingSystem {
    //3dO
    public class Timetable {
        
        HashSet<Course> AllCourses;
        HashSet<string> DoW;
        int LoP;
        int NoP;
        HashSet<Venue> allVenues;
        public Timetable(string toExcel, Tuple<DateTime, DateTime> DailyTime,int LengthOfPeriod, HashSet<string> DaysOfWeek ) {
            allVenues = new HashSet<Venue>();
            this.DoW = DaysOfWeek;
            this.LoP = LengthOfPeriod;
            this.NoP = DailyTime.Item2.Hour - DailyTime.Item1.Hour;
            if(DailyTime.Item1.Minute> DailyTime.Item2.Minute) {
                NoP -= 1;
            }
            Input ngine = new Input(toExcel, DailyTime);
            allVenues.UnionWith(ngine.AllVenues);
            AllCourses = new HashSet<Course>(ngine.AllCourses.Values);
            ngine.Save(toExcel + ".timetable");
        }
        public Timetable(Input loaded, Tuple<DateTime, DateTime> DailyTime, int LengthOfPeriod, HashSet<string> DaysOfWeek) {
            allVenues = new HashSet<Venue>();
            allVenues.UnionWith(loaded.AllVenues);
            this.DoW = DaysOfWeek;
            this.LoP = LengthOfPeriod;
            this.NoP = DailyTime.Item2.Hour - DailyTime.Item1.Hour;
            if (DailyTime.Item1.Minute > DailyTime.Item2.Minute) {
                NoP -= 1;
            }
            AllCourses = new HashSet<Course>(loaded.AllCourses.Values);
        }
        public List<List<Dictionary<Venue, Course>>> Generate() {
            List<List<Dictionary<Venue, Course>>> TTv1 = ClusterTogether();
            return TTv1;
        }
        

        List<List<Dictionary<Venue, Course>>> ClusterTogether(){
            Dictionary<string, HashSet<Course>> CourseOnDay = new Dictionary<string, HashSet<Course>>();
            Dictionary<string, Graph<Course>> GraphOfDay = new Dictionary<string, Graph<Course>>();
            Dictionary<Course, HashSet<string>> AssignedDays = new Dictionary<Course, HashSet<string>>();
            List<string> DoneDays = new List<string>();
            List<List<HashSet<Course>>> TTv1 = new List<List<HashSet<Course>>>();
            Random r = new Random();
            foreach (string Day in DoW) {
                CourseOnDay.Add(Day, new HashSet<Course>());
                foreach (Course c in AllCourses) {
                    AssignedDays.Add(c, new HashSet<string>());
                    if (c.ValidDays.Contains(Day.ToLower().Trim())) {
                        int mustShow = (c.WeeklyHours / LoP) - AssignedDays[c].Count;
                        int OutOf = c.ValidDays.Count - c.ValidDays.Intersect(DoneDays).Count();
                        double priority = (double)mustShow / (double)OutOf;
                        if (priority > 1) priority = 1;
                        if (r.Next(10) < (priority * 10)){
                            CourseOnDay[Day].Add(c);
                            
                        }
                    }
                }
                GraphOfDay.Add(Day, new Graph<Course>(CourseOnDay[Day]));
                foreach (Course c1 in GraphOfDay[Day].GetVertices()) {
                    foreach (Course c2 in GraphOfDay[Day].GetVertices()) {
                        if (!c1.Equals(c2))
                            if (c1.Students.Intersect(c2.Students).Count() + c1.Lecturers.Intersect(c2.Lecturers).Count() > 0) {
                                GraphOfDay[Day].SetEdge(c1, c2);
                            }
                    }
                }
                List<Dictionary<Course, Venue>> CanBeTogether = new List<Dictionary<Course, Venue>>();
                Func<Course,HashSet<Course>,bool> h = CanAdd;
                List<HashSet<Course>> init = GraphOfDay[Day].ColorGraph(h,NoP);
                foreach (HashSet<Course> hc in init) {
                    foreach(Course c in hc) {
                        AssignedDays[c].Add(Day);
                    }
                }
                TTv1.Add(init);
                DoneDays.Add(Day.ToLower().Trim());
            }
            return  GenerateVenues(TTv1);
        }
        bool CanAdd(Course c, HashSet<Course> hc) {
            hc = new HashSet<Course>(hc);
            if (hc.Contains(c))
                return false;
            if (hc.Count + 1 > allVenues.Count)
                return false;
            HashSet<Venue> Taken = new HashSet<Venue>();
            hc.Add(c);
            foreach(Course c2 in hc) {
                int bestFit = int.MaxValue;
                Venue best = null;
                bool foundVenue = false;
                foreach(Venue v in allVenues) {
                    if (Taken.Contains(v)) continue;
                    if(v.IsLab == c2.IsLab) {
                        if (v.GetCapacity >= c2.Students.Count) {
                            foundVenue = true;
                            int fit = v.GetCapacity - c2.Students.Count;
                            
                            if (fit < bestFit) {
                                bestFit = fit;
                                best = v;
                            }
                            if (fit == 0)
                                break;
                        }
                    }
                }
                if (!foundVenue)
                    return false;
                Taken.Add(best);
            }
            return true;
        }
        List<List<Dictionary<Venue, Course>>> GenerateVenues(List<List<HashSet<Course>>> Ver1) {
            List<List<Dictionary<Venue, Course>>> ret = new List<List<Dictionary<Venue, Course>>>();
            foreach (List<HashSet<Course>> lhc in Ver1) {
                List<Dictionary<Venue, Course>> ldvc = new List<Dictionary<Venue, Course>>();
                foreach (HashSet<Course> hc in lhc) {
                    ldvc.Add(AssignVenues(hc));
                }
                ret.Add(ldvc);
            }
            return ret;
        }
        Dictionary<Venue, Course> AssignVenues(HashSet<Course> hc) {
            Dictionary<Venue, Course> ret = new Dictionary<Venue, Course>();
            foreach (Course c2 in hc) {
                int bestFit = int.MaxValue;
                Venue best = null;
                bool foundVenue = false;
                foreach (Venue v in allVenues) {
                    if (ret.Keys.Contains(v)) continue;
                    if (v.IsLab == c2.IsLab) {
                        if (v.GetCapacity >= c2.Students.Count) {
                            foundVenue = true;
                            int fit = v.GetCapacity - c2.Students.Count;

                            if (fit < bestFit) {
                                bestFit = fit;
                                best = v;
                            }
                            if (fit == 0)
                                break;
                        }
                    }
                }
                if (!foundVenue)
                    throw new Exception("This shouldn't be. Check logs");
                ret.Add(best, c2);
            }
            return ret;
        }

      
    }

}
