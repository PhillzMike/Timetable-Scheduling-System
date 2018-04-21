using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace ConsoleApp1 {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine("Input dept codes and number of levels");
            string inp = Console.ReadLine();
            Dictionary<string, int> deptLev = new Dictionary<string, int>();
            while (!inp.Equals("lol")) {
                int.TryParse(Console.ReadLine(), out int lvl);
                if (deptLev.ContainsKey(inp)) {
                    deptLev[inp] = lvl;
                } else {
                    deptLev.Add(inp, lvl);
                }
                inp = Console.ReadLine();
            }
            List<string> names = new List<string>();
            inp = Console.ReadLine();
            while (!inp.Equals("lol")) {
                names.Add(inp);
                inp = Console.ReadLine();
            }
            Dictionary<string, Dictionary<int, List<string>>> DeptCours = new Dictionary<string, Dictionary<int, List<string>>>();
            Dictionary<string,List<string>> Course = new Dictionary<string, List<string>>();
            Random r = new Random();
            foreach(string dept in deptLev.Keys) {
                DeptCours.Add(dept,new Dictionary<int, List<string>>());
                for(int i = 0; i < deptLev[dept]; i++) {
                    DeptCours[dept].Add(i+1, new List<string>());
                    DeptCours[dept][i + 1].Add(dept);
                    DeptCours[dept][i + 1].Add((i+1)+"");
                    int units = r.Next(15, 22);
                    bool final = true;
                    while (units > 0) {
                        int thisUnit;
                        
                        if (i == deptLev[dept] - 1&&final) {
                            final = false;
                            thisUnit = 6;
                            DeptCours[dept][i+1].Add(GenerateCourse(dept, i+1,thisUnit,Course));
                            units -= thisUnit;
                        }else if (units < 4) {
                            thisUnit = units;
                            DeptCours[dept][i + 1].Add(GenerateCourse(dept, i+1, thisUnit, Course));
                            units -= thisUnit;
                        } else {
                            thisUnit = r.Next(2, 5);
                            DeptCours[dept][i + 1].Add(GenerateCourse(dept, i+1, thisUnit, Course));
                            units -= thisUnit;
                        }
                    }
                }
            }
            Dictionary<string, List<string>> Students = new Dictionary<string, List<string>>();
            int ck = 0;
            foreach(string dept in DeptCours.Keys) {
                foreach (int lvl in DeptCours[dept].Keys) {
                    for(int i=0;i< r.Next(20, 150); i++) {
                        string name="";
                        do {
                            if (ck < names.Count-1)
                                name = names[ck++] + " " + names[ck++];
                            else
                                name = names[r.Next(names.Count)] + " " + names[r.Next(names.Count)];
                        } while (Students.ContainsKey(name));
                        Students.Add(name, new List<string>());
                        Students[name].Add(name);
                        Students[name].Add(lvl+"");
                        Students[name].Add(dept);
                        if (lvl != 1) {
                            int extra = 0, minus = 0;
                            int x = r.Next(10);
                            if (x >= 7) {
                                extra = x - 6;
                            }
                            x = r.Next(10);
                            if (x >= 8) {
                                minus = x - 7;
                            }
                            for (int d = 0; d < extra; d++) {
                                if (DeptCours[dept][lvl - 1].Count > d)
                                    Students[name].Add(DeptCours[dept][lvl - 1][d+2]);
                            }
                            for (int d = 0; d < minus; d++) {
                                if (DeptCours[dept][lvl].Count > d)
                                    Students[name].Add("~"+DeptCours[dept][lvl ][d+2]);
                            }
                        }
                    }
                }
            }
            Dictionary<string, List<string>> Lecturers = new Dictionary<string, List<string>>();
            
            foreach(string dept in DeptCours.Keys) {
                List<string> lid = new List<string>();
                for (int i = 0; i < r.Next(6, 13); i++) {
                    string name = "";
                    do {
                        int bases = r.Next(4);
                        name = (bases == 0) ? "Mr " : (bases == 1) ? "Mrs " : (r.Next(3) == 1) ? "Prof " : "Dr ";
                        if (ck < names.Count-1)
                            name += names[ck++] + " " + names[ck++];
                        else
                            name += names[r.Next(names.Count)] + " " + names[r.Next(names.Count)];
                    } while (Lecturers.ContainsKey(name));
                    lid.Add(name);
                    Lecturers.Add(name, new List<string>());
                    Lecturers[name].Add(name);
                }
                foreach(int lvl in DeptCours[dept].Keys) {
                    int inc=0;
                    foreach(string cos in DeptCours[dept][lvl]) {
                        inc++;
                        if(inc>2)
                            Lecturers[lid[r.Next(lid.Count)]].Add(cos);
                    }
                }

            }
            Excel.Application Table = new Excel.Application();
            Excel.Workbook TableWorkBook = Table.Workbooks.Open(@"C:\Users\Opsi Jay\Documents\Visual Studio 2017\Projects\Timetable\Timetable\SampleGuide.xlsx");

            Excel._Worksheet Stu_Sheet = TableWorkBook.Sheets[1];
            Excel.Range Stu_Used = Stu_Sheet.UsedRange;
            int row = 50;
            int incr = 0;
            foreach (string student in Students.Keys) {
                for(int jc =1;jc<=Students[student].Count;jc++) {
                    Stu_Used.Cells[row+incr, jc].Value2 = Students[student][jc - 1];
                }
                incr++;
            }
            row=10;
            incr = 0;
            Excel._Worksheet Dept_Sheet = TableWorkBook.Sheets[2];
            Excel.Range Dept_Used = Dept_Sheet.UsedRange;
            foreach(string dept in DeptCours.Keys) {
                foreach (int lvl in DeptCours[dept].Keys) {
                    for(int jc = 1; jc <= DeptCours[dept][lvl].Count; jc++) {
                        Dept_Used.Cells[row + incr, jc].Value2 = DeptCours[dept][lvl][jc - 1];
                    }
                    incr++;
                }
            }
            row = 40;
            incr = 0;
            Excel._Worksheet CourseSheet = TableWorkBook.Sheets[3];
            Excel.Range CourseUsed = CourseSheet.UsedRange;
            foreach(string cos in Course.Keys) {
                for(int jc = 1; jc < Course[cos].Count; jc++) {
                    CourseUsed.Cells[row + incr, jc].Value2 = Course[cos][jc - 1];
                }
                incr++;
            }

            row = 25;
            incr = 0;
            
            Excel._Worksheet Lect_Sheet = TableWorkBook.Sheets[4];
            Excel.Range Lect_Used = Lect_Sheet.UsedRange;
            foreach (string lect in Lecturers.Keys) {
                for (int jc = 1; jc <= Lecturers[lect].Count; jc++) {
                    Lect_Used.Cells[row + incr, jc].Value2 = Lecturers[lect][jc - 1];
                }
                incr++;
            }




            GC.Collect();
            GC.WaitForPendingFinalizers();

            Marshal.ReleaseComObject(CourseUsed);
            Marshal.ReleaseComObject(CourseSheet);
            Marshal.ReleaseComObject(Stu_Used);
            Marshal.ReleaseComObject(Stu_Sheet);
            Marshal.ReleaseComObject(Lect_Used);
            Marshal.ReleaseComObject(Lect_Sheet);
            Marshal.ReleaseComObject(Dept_Used);
            Marshal.ReleaseComObject(Dept_Sheet);

            TableWorkBook.Close();
            Marshal.ReleaseComObject(TableWorkBook);
            Table.Quit();
            Marshal.ReleaseComObject(Table);
        }
        static string GenerateCourse(string code, int lvl, int units, Dictionary<string, List<string>> C) {
            Random r = new Random();
            int lst;
            string last;
            do {
                lst = r.Next(50);
                if (lst <= 30)
                    lst /= 10;
                else lst -= 20;
                last = (lst < 10) ? "0" + lst : lst+"";
            } while (C.ContainsKey(code + "" + lvl + "" + last));
            string cod = code + "" + lvl + "" + last;
            C.Add(cod, new List<string>());
            C[cod].Add(cod);
            C[cod].Add(lvl + "");
            C[cod].Add(units + "");
            if (r.Next(10) == 9) {
                C[cod].Add( r.Next(1, 4)+"");

            } else {
                C[cod].Add("");
            }
            int prev=8;
            if (r.Next(40) == 29) {
                prev = r.Next(8, 18);
                C[cod].Add(prev + ":00");
            } else {
                C[cod].Add("");
            }
            if (r.Next(40) == 19) {
                C[cod].Add(r.Next(prev, 19) + ":00");
            } else {
                C[cod].Add("");
            }


            if (r.Next(40) == 19) {
                foreach(string day in new List<string>() { "monday", "tuesday", "wednesday", "thursday", "friday" }) {
                    if (r.Next(2) == 0) {
                        C[cod].Add(day);
                    } else {
                        C[cod].Add("");
                    }
                }
                if (r.Next(3) == 1) {
                    C[cod].Add("saturday");
                }
                if (r.Next(10) == 5) {
                    C[cod].Add("sunday");
                }
            } else {
                C[cod].AddRange( new List<string>() { "", "", "", "", "", "", "" });
            }
            return cod;
        }


    }
}
