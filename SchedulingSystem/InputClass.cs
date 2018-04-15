using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace SchedulingSystem
{  
    
    //Teni
    [Serializable]
    public class Input
    {
        public Tuple<DateTime, DateTime> timeFrame;
        public String InfoTable;
        public Dictionary<String, Course> AllCourses;
        public List<Venue> AllVenues;
        public List<Student> AllStudents;
        public List<Lecturer> AllLecturers;
        public Input(string Datatable, Tuple<DateTime, DateTime> timeSchedule)
        {
            errors = new List<string>();
            this.InfoTable = Datatable;
            this.timeFrame = timeSchedule;
            AllCourses = new Dictionary<string, Course>();
            Excel.Application Table = new Excel.Application();
            Excel.Workbook TableWorkBook = Table.Workbooks.Open(InfoTable);


            Excel._Worksheet CourseSheet = TableWorkBook.Sheets[2];
            Excel.Range CourseUsed = CourseSheet.UsedRange;
            GenerateCourses(CourseUsed);
            

            Excel._Worksheet Stu_Sheet = TableWorkBook.Sheets[1];
            Excel.Range Stu_Used = Stu_Sheet.UsedRange;
            AllStudents = GenerateStudents(Stu_Used);
           

            Excel._Worksheet Lect_Sheet = TableWorkBook.Sheets[3];
            Excel.Range Lect_Used = Lect_Sheet.UsedRange;
            AllLecturers = GenerateLecturers(Lect_Used);
            

            Excel._Worksheet Ven_Sheet = TableWorkBook.Sheets[4];
            Excel.Range Ven_Used = Ven_Sheet.UsedRange;
            AllVenues = GenerateVenues(Ven_Used);
            
           

            GC.Collect();
            GC.WaitForPendingFinalizers();

            Marshal.ReleaseComObject(CourseUsed);
            Marshal.ReleaseComObject(CourseSheet);
            Marshal.ReleaseComObject(Stu_Used);
            Marshal.ReleaseComObject(Stu_Sheet);
            Marshal.ReleaseComObject(Lect_Used);
            Marshal.ReleaseComObject(Lect_Sheet);
            Marshal.ReleaseComObject(Ven_Used);
            Marshal.ReleaseComObject(Ven_Sheet);

            TableWorkBook.Close();
            Marshal.ReleaseComObject(TableWorkBook);
            Table.Quit();
            Marshal.ReleaseComObject(Table);
        }
        public void Save(string path) {
            try {
                File.SetAttributes(path, File.GetAttributes(path) & ~FileAttributes.ReadOnly);
            } catch { }
            try {
                using (FileStream fsout = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None)) {
                    new BinaryFormatter().Serialize(fsout, this);
                    File.SetAttributes(path, (File.GetAttributes(path) | FileAttributes.ReadOnly));
                }
            } catch {
                throw new Exception("An error Occured, Could not save input Details");
            }
        }
        public static Input Load(string Path) {
            Input l;
            try {
                using (FileStream fsin = new FileStream(Path, FileMode.Open, FileAccess.Read, FileShare.None)) {
                    l = (Input)(new BinaryFormatter().Deserialize(fsin));
                }
            } catch (FileNotFoundException) {
                throw new FileNotFoundException("The file doesn't contain a valid Serialized Input");
            }
            return l;
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
                    for (int j = 3; j <= sp.Columns.Count; j++)
                    {       //check if a course cell is empty
                        if (sp.Cells[i, j] != null && !string.IsNullOrWhiteSpace(sp.Cells[i, j].Value2 + "")  )
                        {      //checks the if the courses in all the list of courses 
                            if (AllCourses.ContainsKey(sp.Cells[i, j].Value2+""))
                            {
                                coursesOffered.Add(AllCourses[sp.Cells[i, j].Value2]);
                                if (AllCourses.ContainsKey(sp.Cells[i, j].Value2 + "l(4)b")) {
                                    coursesOffered.Add(AllCourses[sp.Cells[i, j].Value2 + "l(4)b"]);
                                }
                            } else if (AllCourses.ContainsKey(sp.Cells[i, j].Value2 + "l(4)b")) {
                                coursesOffered.Add(AllCourses[sp.Cells[i, j].Value2 + "l(4)b"]);
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
                if (string.IsNullOrWhiteSpace(sp.Cells[i, 1].Value2 +"") || string.IsNullOrWhiteSpace(sp.Cells[i, 2].Value2 + ""))
                    continue;

                   string code = sp.Cells[i, 1].Value2;
                code = code.Trim().ToLower();
                   int level = (int)sp.Cells[i, 2].Value2;
                int wCH; 
                int wLH;
                DateTime sTF;
                DateTime eTF;
                string vD;
                List<String> nvD;
                if (string.IsNullOrWhiteSpace(sp.Cells[i, 3].Value2 + ""))
                    wCH = 0;
                else
                    wCH = (int)sp.Cells[i, 3].Value2;
                if (string.IsNullOrWhiteSpace(sp.Cells[i, 4].Value2 + ""))
                    wLH = 0;
                else
                    wLH= (int)sp.Cells[i, 4].Value2;

                if (string.IsNullOrWhiteSpace(sp.Cells[i, 5].Value2 + ""))
                    sTF = timeFrame.Item1;
                else
                    sTF = DateTime.ParseExact(sp.Cells[i, 5].Value2, "hh:mm", System.Globalization.CultureInfo.CurrentCulture);

                if (string.IsNullOrWhiteSpace(sp.Cells[i, 6].Value2 + ""))
                    eTF = timeFrame.Item2;
                else
                    eTF = DateTime.ParseExact(sp.Cells[i, 6].Value2, "hh:mm", System.Globalization.CultureInfo.CurrentCulture);

                if (string.IsNullOrWhiteSpace(sp.Cells[i, 7].Value2 + ""))
                    nvD = new List<String>() { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };
                else
                {
                    vD = sp.Cells[i, 7].Value2;
                    nvD = vD.Split(',').ToList();
                }
                if(wCH + wLH > 0) {
                    Course courseinfo;
                    if (wCH > 0) {
                        courseinfo = new Course(code, false, wCH, level, sTF, eTF, nvD);
                        if (AllCourses.ContainsKey(code))
                            Error("Course code aready exist and so was skipped");
                        else
                            AllCourses.Add(code, courseinfo);
                    }
                    if (wLH > 0) {
                        courseinfo = new Course(code, true, wLH, level, sTF, eTF, nvD);
                        if (AllCourses.ContainsKey(code+"l(4)b"))
                            Error("Course code aready exist and so was skipped"+code);
                        else
                            AllCourses.Add(code + "l(4)b", courseinfo);
                    }
                    
                }
                 
               
            }
            
        }

        public List<Lecturer> GenerateLecturers(Excel.Range sp)
        {
            List<Lecturer> AllLecturers = new List<Lecturer>();
            for (int i = 3; i < sp.Rows.Count; i++)
            {
                if (!string.IsNullOrWhiteSpace(sp.Cells[i, 1].Value2 + ""))
                {
                    string lecturername = sp.Cells[i, 1].Value2;
                    List<Course> LecturerCourses = new List<Course>();
                 
                    for (int j = 2; j <= sp.Columns.Count; j++)
                    {       //check if a course cell is empty
                        if (sp.Cells[i, j] != null && !string.IsNullOrWhiteSpace(sp.Cells[i, j].Value2 + ""))
                        {      //checks the if the courses in all the list of courses 
                            if (AllCourses.ContainsKey(sp.Cells[i, j].Value2+""))
                            {
                                LecturerCourses.Add(AllCourses[sp.Cells[i, j].Value2]);
                                if (AllCourses.ContainsKey(sp.Cells[i, j].Value2 + "l(4)b")) {
                                    LecturerCourses.Add(AllCourses[sp.Cells[i, j].Value2 + "l(4)b"]);
                                }
                            } else if (AllCourses.ContainsKey(sp.Cells[i, j].Value2 + "l(4)b")) {
                                LecturerCourses.Add(AllCourses[sp.Cells[i, j].Value2 + "l(4)b"]);
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
                 string VenueName= sp.Cells[i, 1].Value2;
                 int VenueCapacity = (int)sp.Cells[i, 2].Value2;
                 string LabType = "";

             if (!string.IsNullOrWhiteSpace(sp.Cells[i, 3].Value2 + "")) { 
                    LabType = sp.Cells[i, 3].Value2+"";
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
        public List<String> errors;
        public void Error(string errortype)
        {
            errors.Add(errortype);
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
