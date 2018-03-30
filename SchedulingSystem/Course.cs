using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulingSystem
{
    abstract class Course
    {
        private string code;
        private int weeklyClassHours;
        private int weeklyLabHours;
        private int startTimeFrame;
        private int endTimeFrame;
        private List<string> validDays;
        private int level;
        private List<Lecturer> lecturers;

        public Course(string code, int weeklyClassHours, int weeklyLabHours, int startTimeFrame, int endTimeFrame, List<string> validDays, int level, List<Lecturer> lecturers)
        {
            this.code = code;
            this.weeklyClassHours = weeklyClassHours;
            this.weeklyLabHours = weeklyLabHours;
            this.startTimeFrame = startTimeFrame;
            this.endTimeFrame = endTimeFrame;
            this.validDays = validDays;
            this.level = level;
            this.lecturers = lecturers;
        }

        public string Code { get => code; }
        public int WeeklyClassHours { get => weeklyClassHours;  }
        public int WeeklyLabHours { get => weeklyLabHours; }
        public int StartTimeFrame { get => startTimeFrame; }
        public int EndTimeFrame { get => endTimeFrame; }
        public List<string> ValidDays { get => validDays; }
        public int Level { get => level; set => level = value; }
        internal List<Lecturer> Lecturers { get => lecturers;}

    }
}
