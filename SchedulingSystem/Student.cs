using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulingSystem
{
    //Peace
    [Serializable]
    public class Student
    {
        private String name;
        private int level;
        private List<Course> coursesRegistered;
        public Student(String name, List<Course> coursesRegistered, int level)
        {
            this.name = name;
            this.coursesRegistered = coursesRegistered;
            this.level = level;
        }

        public String GetName
        {
            get => name;
        }
        public int GetLevel
        {
            get => level;
        }
        public List<Course> Courses
        {
             get => coursesRegistered;
        }
        public override string ToString() {
            return name;

        }

    }
}