using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace SchedulingSystem
{  
    
    //Teni
    class Input
    {
        public Tuple<DateTime, DateTime> timeFrame;
        public String InfoTable;
        public Dictionary<String, Course> AllCourses;
        public List<Venue> AllVenues;
        public List<Student> AllStudents;
        public List<Lecturer> AllLecturers;
        public Input(string Datatable, Tuple<DateTime, DateTime> timeSchedule)
        {
            this.InfoTable = Datatable;
            this.timeFrame = timeSchedule;
            GenerateCourses(ReadInput(2));

            AllStudents = GenerateStudents(ReadInput(1));
           
            AllLecturers = GenerateLecturers(ReadInput(3));
            AllVenues =  GenerateVenues(ReadInput(4));
        }
        
        public  Excel.Range ReadInput( int sheetnumber)
        {
            Excel.Application Table = new Excel.Application();
            Excel.Workbook  TableWorkBook = Table.Workbooks.Open(InfoTable);
            Excel._Worksheet  TableWorkSheet = TableWorkBook.Sheets[sheetnumber];
            Excel.Range PartUsed = TableWorkSheet.UsedRange;

            return PartUsed;
        }
        

        public List<Student> GenerateStudents(Excel.Range sp)
        {
            List<Student> studentList = new List<Student>();
           for (int i =3;  i<= sp.Rows.Count; i++)
            {
                //checks if student name or level was omitted and skips the row
                if (!string.IsNullOrWhiteSpace(sp.Cells[i, 1].Value2 +"") || !string.IsNullOrWhiteSpace(sp.Cells[i, 2].Value2 + ""))
                {
                    string name = sp.Cells[i, 1].Value2;
                    int level = (int) sp.Cells[i, 2].Value2;
                    List<Course> coursesOffered = new List<Course>();
                    for (int j = 1; j <= sp.Columns.Count; j++)
                    {       //check if a course cell is empty
                        if (sp.Cells[i, j] != null && !string.IsNullOrWhiteSpace(sp.Cells[i, j].Value2 + "")  )
                        {      //checks the if the courses in all the list of courses 
                            if (AllCourses.ContainsKey(sp.Cells[i, j].Value2))
                            {
                                coursesOffered.Add(AllCourses[sp.Cells[i, j].Value2]);
                                
                            }
                            else
                            {
                                Error("For Student " + name + " , there is no course" + sp.Cells[i, j].Value2);
                            }
                        }
                    }
                    //create a Student objets and adds it to the list of Students
                    Student nStu = new Student(name, coursesOffered, level);
                    studentList.Add(nStu);
                    foreach (Course c in coursesOffered)
                        c.Students.Add(nStu);
                }
                else
                {
                    Error("Row " + i + " had an error and was skipped");
                }
            }
            return studentList;
        }
        public void GenerateCourses(Excel.Range sp)
        {  
         
            for (int i = 5; i <= sp.Rows.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(sp.Cells[i, 1].Value2 +"") || string.IsNullOrWhiteSpace(sp.Cells[i, 2].Value2 + "")
                    || string.IsNullOrWhiteSpace(sp.Cells[i, 3].Value2 + ""))
                    continue;

                   string code = sp.Cells[i, 1];
                   int level = (int)sp.Cells[i, 2];
                   int wCH = (int)sp.Cells[i, 3];
                int wLH;
                DateTime sTF;
                DateTime eTF;
                string vD;
                List<String> nvD;
                if (string.IsNullOrWhiteSpace(sp.Cells[i, 4].Value2 + ""))
                    wLH = 0;
                else
                    wLH= (int)sp.Cells[i, 4];

                if (string.IsNullOrWhiteSpace(sp.Cells[i, 5].Value2 + ""))
                    sTF = timeFrame.Item1;
                else
                    sTF = DateTime.ParseExact(sp.Cells[i, 5], "hh:mm", System.Globalization.CultureInfo.CurrentCulture);

                if (string.IsNullOrWhiteSpace(sp.Cells[i, 6].Value2 + ""))
                    eTF = timeFrame.Item2;
                else
                    eTF = DateTime.ParseExact(sp.Cells[i, 6], "hh:mm", System.Globalization.CultureInfo.CurrentCulture);

                if (string.IsNullOrWhiteSpace(sp.Cells[i, 7].Value2 + ""))
                    nvD = new List<String>() { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };
                else
                {
                    vD = sp.Cells[i, 7];
                    nvD = vD.Split(',').ToList();
                }

                Course courseinfo = new Course(code, wCH, wLH, level, sTF, eTF, nvD);
                if (AllCourses.ContainsKey(code))
                    Error("Course code aready exist and so was skipped");
                else
                  AllCourses.Add(code, courseinfo);
            }
            
        }

        public List<Lecturer> GenerateLecturers(Excel.Range sp)
        {
            List<Lecturer> AllLecturers = new List<Lecturer>();
            for (int i = 3; i < sp.Rows.Count; i++)
            {
                if (!string.IsNullOrWhiteSpace(sp.Cells[i, 1].Value2 + ""))
                {
                    string lecturername = sp.Cells[i, 2];
                    List<Course> LecturerCourses = new List<Course>();
                 
                    for (int j = 1; j <= sp.Columns.Count; j++)
                    {       //check if a course cell is empty
                        if (sp.Cells[i, j] != null && !string.IsNullOrWhiteSpace(sp.Cells[i, j].Value2 + ""))
                        {      //checks the if the courses in all the list of courses 
                            if (AllCourses.ContainsKey(sp.Cells[i, j].Value2))
                            {
                                LecturerCourses.Add(sp.Cells[i, j].Value2);
                            }
                            else
                            {
                                Error("For Lecturer" + lecturername + " , there is no course" + sp.Cells[i, j].Value2);
                            }
                        }
                    }
                    //create a Student objets and adds it to the list of Students
                    Lecturer lect = new Lecturer(lecturername, LecturerCourses);
                    AllLecturers.Add(lect);
                    foreach (Course l in LecturerCourses)
                        l.Lecturers.Add(lect);
                }
                else
                {
                    Error("Row " + i + " had an error and was skipped");
                }
            }
            return AllLecturers;
         }
               
        public List<Venue> GenerateVenues(Excel.Range sp)
        {

            List<Venue> Venues= new List<Venue>();
            for (int i = 3; i < sp.Rows.Count; i++)

            {
                bool isLab = false;
                if (string.IsNullOrWhiteSpace(sp.Cells[i, 1].Value2 + "") || string.IsNullOrWhiteSpace(sp.Cells[i, 2].Value2 + ""))
                   continue;
                 string VenueName= sp.Cells[i, 1];
                 int VenueCapacity = (int)sp.Cells[i, 2];
                 string LabType = "";

             if (!string.IsNullOrWhiteSpace(sp.Cells[i, 1].Value2 + "")) { 
                    LabType = sp.Cells[i, 3];
                }
               LabType =  LabType.ToLower().Trim();
                    if (LabType == "true")
                        isLab = true;
                    else
                        isLab = false;

                Venues.Add(new Venue(VenueName, VenueCapacity, isLab));
            }
            return Venues;
        }
           
        //public List<Course> GetALlCourses { get => li}
        public void Error(string errortype)
        {

        }
    }

//   cleanup
//            GC.Collect();
//    GC.WaitForPendingFinalizers();

//            rule of thumb for releasing com objects:
//              never use two dots, all COM objects must be referenced and released individually
//              ex: [somthing].[something].[something] is bad

//release com objects to fully kill excel process from running in the background
//            Marshal.ReleaseComObject(xlRange);
//            Marshal.ReleaseComObject(xlWorksheet);

//            close and release
//            xlWorkbook.Close();
//    Marshal.ReleaseComObject(xlWorkbook);

//            quit and release
//            xlApp.Quit();
//    Marshal.ReleaseComObject(xlApp);
}
