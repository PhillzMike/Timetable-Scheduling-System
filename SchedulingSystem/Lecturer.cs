using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SchedulingSystem {
    //Seyi
    public class Lecturer {
        private string name;
        private List<Course> CourseTaken;

        public Lecturer(string name, List<Course> CTaken) {
            this.name = name;
            this.CourseTaken = new List<Course>();
            for (int i = 0; i < CTaken.Count; i++) {
                CourseTaken.Add(CTaken[i]);
            }
        }

        public string LecturerName {
            get => name;
        }
        public List<Course> Courses {
            get => new List<Course>(CourseTaken);
        }
    }
}
