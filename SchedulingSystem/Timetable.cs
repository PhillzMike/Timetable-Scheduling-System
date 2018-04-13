using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulingSystem {
    //3dO
    public class Timetable {
        Graph<Course> icarly;
        HashSet<Course> AllCourses;
        HashSet<string> DoW;
        int LoP;
        int NoP;
        public Timetable(string toExcel, Tuple<DateTime, DateTime> DailyTime,int LengthOfPeriod, HashSet<string> DaysOfWeek ) {
            this.DoW = DaysOfWeek;
            this.LoP = LengthOfPeriod;
            this.NoP = DailyTime.Item2.Hour - DailyTime.Item1.Hour;
            if(DailyTime.Item1.Minute> DailyTime.Item2.Minute) {
                NoP -= 1;
            }
            Input ngine = new Input(toExcel, DailyTime);
            AllCourses = new HashSet<Course>(ngine.AllCourses.Values);
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
        void Diffy() {
            Diff_days = new Graph<HashSet<Course>>(CanBeTogether);
            foreach(HashSet<Course> hc in CanBeTogether) {
                foreach (HashSet<Course> hc2 in CanBeTogether) {
                    if (hc.Intersect(hc2).Count() > 0) {
                        Diff_days.SetEdge(hc, hc2);
                    }
                }
            }
            GetBest(Diff_days.ColorGraph(NoP));
        }
        Dictionary<HashSet<int>,int> ScoreCombs;
        void GetBest(List<HashSet<HashSet<Course>>> TT) {
            foreach(HashSet<int> comb in Combin(TT.Count, DoW.Count)) {
                List<HashSet<HashSet<Course>>> Sep = new List<HashSet<HashSet<Course>>>();
                foreach (int n in comb) {
                    Sep.Add(TT[n]);
                }
                ScoreCombs.Add(comb,Score(Sep));
            }
            //Assume all courses are 1 hour long
            


        }

        void Separate(List<HashSet<HashSet<Course>>> TimeT) {
            List<List<HashSet<Course>>> FinalTT = new List<List<HashSet<Course>>>();
        }
        int Score(List<HashSet<HashSet<Course>>> TimeT) {
            throw new NotImplementedException();
        }
        List<HashSet<int>> Combin(int tot,int count) {
            throw new NotImplementedException();
        }
        List<HashSet<Course>> UtilizeSpace(HashSet<Course> hc) {
            throw new NotImplementedException();
        }
        
    }

}
