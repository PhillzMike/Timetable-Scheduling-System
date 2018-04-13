using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulingSystem {
    //3dO
    public partial class Timetable {
        Graph<Course> icarly;
        HashSet<Course> AllCourses;
        HashSet<string> DoW;
        int LoP;
        int NoP;
        HashSet<Venue> allVenues;
        public Timetable(string toExcel, Tuple<DateTime, DateTime> DailyTime,int LengthOfPeriod, HashSet<string> DaysOfWeek ) {
            //
            //public Timetable(string toExcel, Tuple<DateTime, DateTime> DailyTime)
            //{
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
        Dictionary<Course, Tuple<int,int>> HoursLeft;
        Graph<HashSet<Course>> Diff_days;
        List<List<HashSet<Course>>> Days;
        void ClusterTogether(){
            List<HashSet<Course>> init = icarly.ColorGraph();
            foreach(HashSet<Course> hc in init) {
              //  CanBeTogether.AddRange(UtilizeSpace(hc));
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
            List<List<List<Course>>> FinalTT = new List<List<List<Course>>>();
            foreach(HashSet<HashSet<Course>> DayPeriods in TimeT) {
                
                foreach(HashSet<Course> SinglePeriod in DayPeriods) {
                    foreach(Course c in SinglePeriod) {
                        if (HoursLeft.ContainsKey(c)) {
                            HoursLeft[c] = new Tuple<int, int>(c.WeeklyHours, HoursLeft[c].Item2 + 1);
                        } else {
                            HoursLeft.Add(c,new Tuple<int,int>(c.WeeklyHours, 1));
                        }
                    }
                }
            }
            Random r = new Random();
            foreach (HashSet<HashSet<Course>> DayPeriods in TimeT) {
                List < List < Course > > Periods = new List<List<Course>>();
                foreach (HashSet<Course> SinglePeriod in DayPeriods) {
                    List<Course> thisPeriod = new List<Course>();
                    foreach (Course c in SinglePeriod) {
                        if (r.Next(10) < (((double)HoursLeft[c].Item1 / (double)HoursLeft[c].Item2) * 10)) {
                            thisPeriod.Add(c);
                            HoursLeft[c] = new Tuple<int, int>(HoursLeft[c].Item1 - 1, HoursLeft[c].Item2 - 1);
                        } else {
                            HoursLeft[c] = new Tuple<int, int>(HoursLeft[c].Item1, HoursLeft[c].Item2 - 1);
                        }
                    }
                    Periods.Add(thisPeriod);
                }
                FinalTT.Add(Periods);
            }
        }
        int Score(List<HashSet<HashSet<Course>>> TimeT) {
            Dictionary<Course, int> Remaining = new Dictionary<Course, int>();
            foreach (HashSet<HashSet<Course>> DayPeriods in TimeT) {

                foreach (HashSet<Course> SinglePeriod in DayPeriods) {
                    foreach (Course c in SinglePeriod) {
                        if (Remaining.ContainsKey(c)) {
                            Remaining[c]--;
                            if (Remaining[c] <= 0) {
                                Remaining.Remove(c);
                            }
                        }
                    }
                }
            }
            int ret = 0;
            foreach(Course left in Remaining.Keys) {
                ret += Remaining[left];
            }
            return ret;
        }
        List<HashSet<int>> Combin(int tot,int count) {
            CombinC = new List<HashSet<int>>();
            CombinCode(tot, count, new HashSet<int>());
            return CombinC;
        }
        List<HashSet<int>> CombinC;
        void CombinCode(int tot,int count,HashSet<int> prev) {
            for(int i = 0; i < tot; i++) {
                if (!prev.Contains(i)) {
                    prev.Add(i);
                    if (count == 1) {
                        CombinC.Add(new HashSet<int>(prev));
                    } else {
                        CombinCode(tot, count - 1, prev);
                    }
                    prev.Remove(i);
                }
            }
        }
    }

}
