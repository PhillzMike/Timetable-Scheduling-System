﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulingSystem
{
   public class Course
    {
        private string code;
        private int weeklyClassHours;
        private int weeklyLabHours;
        private int level;
        private HashSet<Lecturer> lecturers;
        private HashSet<Student> students;
        private DateTime startTimeFrame;
        private DateTime endTimeFrame;
        private List<string> validDays;
        

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
        public Course(string code, int weeklyClassHours, int weeklyLabHours, int level , DateTime startTimeFrame, DateTime endTimeFrame, List<string> validDays)
        {
            this.code = code;
            this.weeklyClassHours = weeklyClassHours;
            this.weeklyLabHours = weeklyLabHours;
            this.startTimeFrame = startTimeFrame;
            this.endTimeFrame = endTimeFrame;
            this.level = level;
            this.lecturers = new HashSet<Lecturer>();
            this.validDays = new List<string>();
            foreach (var day in validDays)
            {
                this.validDays.Add(day);
            }
            foreach (var lecturer in lecturers)
            {
                this.lecturers.Add(lecturer);
            }
        }

        public string Code { get => code; }
        public int WeeklyClassHours { get => weeklyClassHours;  }
        public int WeeklyLabHours { get => weeklyLabHours; }
        public DateTime StartTimeFrame { get => startTimeFrame; }
        public DateTime EndTimeFrame { get => endTimeFrame; }
        public List<string> ValidDays { get => new List<string>(validDays); }
        public int Level { get => level;}
        public HashSet<Lecturer> Lecturers { get => new HashSet<Lecturer>(lecturers);}
        public HashSet<Student> Students { get => new HashSet<Student>(students); }

    }
}
