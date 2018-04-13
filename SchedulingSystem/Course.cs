using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulingSystem
{
   public class Course
    {
        private string code;
        private bool isLab;
        private int weeklyHours;
        private int level;
        private HashSet<Lecturer> lecturers;
        private HashSet<Student> students;
        private DateTime startTimeFrame;
        private DateTime endTimeFrame;
        private List<string> validDays;
        public static DateTime GlobalStart;
        public static DateTime GlobalEnd;
        public static List<string> GlobalDays;
        //TODO
        public bool HasSpecialTime;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="code">The course code</param>
            /// <param name="weeklyClassHours">the number of lecture hours the course should be allocated in a week</param>
            /// <param name="weeklyLabHours">the number of lab hours the course shoould be allocated in a week</param>
            /// <param name="startTimeFrame"></param>
            /// <param name="endTimeFrame"></param>
            /// <param name="validDays">the days the course is valid for</param>
            /// <param name="level">the level the course is studied</param>
            /// <param name="lecturers">the lecturers taking the course</param>
            public Course(string code, bool isLab, int weeklyHours, int level , DateTime startTimeFrame, DateTime endTimeFrame, List<string> validDays)
            {
            this.code = code;
            this.isLab = isLab;
            this.weeklyHours = weeklyHours;
            this.startTimeFrame = startTimeFrame;
            this.endTimeFrame = endTimeFrame;
            HasSpecialTime = (startTimeFrame.Equals(GlobalStart) && endTimeFrame.Equals(GlobalEnd)) ? false : true;
            this.level = level;
            this.lecturers = new HashSet<Lecturer>();
            this.students = new HashSet<Student>();
            this.validDays = new List<string>();
            foreach (var day in validDays){
                this.validDays.Add(day);
            }
        }

        public string Code { get => code; }
        public bool IsLab { get => isLab;  }
        public int WeeklyHours { get => weeklyHours;  }
        public DateTime StartTimeFrame { get => startTimeFrame; }
        public DateTime EndTimeFrame { get => endTimeFrame; }
        public List<string> ValidDays { get => new List<string>(validDays); }
        public int Level { get => level;}
        public HashSet<Lecturer> Lecturers { get => lecturers; }
        public HashSet<Student> Students { get => students; }

    }
}
