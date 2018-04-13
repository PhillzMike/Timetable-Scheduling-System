using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulingSystem
{
    //Peace
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

        public String getName()
        {
            return name;
        }
        public int getLevel()
        {
            return level;
        }
        public List<Course> getCoursesRegistered()
        {
            return coursesRegistered;
        }
        
    }
}