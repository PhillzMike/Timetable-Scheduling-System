using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
namespace SchedulingSystem {
    public class Output {
        Excel.Range XL;
        List<string> Periods;
        List<Venue> Venues;
        public Output(string pathToFile, List<List<Dictionary<Venue, Course>>> Timetable, Tuple<DateTime, DateTime> timeSchedule,List<string> Days, int LoP) {
            Periods = GeneratePeriods(LoP, timeSchedule);
            Venues = new List<Venue>();
            foreach(List<Dictionary<Venue, Course>> tt in Timetable) {
                foreach(Dictionary<Venue,Course> VC in tt) {
                    foreach(Venue v in VC.Keys) {
                        if (!Venues.Contains(v)) {
                            Venues.Add(v);
                        }
                    }
                }
            }
            Excel.Application Table = new Excel.Application();
            Excel.Workbook TableWorkBook = Table.Workbooks.Open(pathToFile);


            Excel._Worksheet CourseSheet = TableWorkBook.Sheets[1];
            XL = CourseSheet.UsedRange;
            int r = 1;
            int c = 1;
            for(int i = 0; i < Days.Count; i++) {
                Write(Days[i], Timetable[i],r,c);
                c += Periods.Count + 2;
                if ((i + 1) % 3 == 0) {
                    c = 1;
                    r += Venues.Count + 3;
                }
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();

            Marshal.ReleaseComObject(XL);
            Marshal.ReleaseComObject(CourseSheet);

            TableWorkBook.Close();
            Marshal.ReleaseComObject(TableWorkBook);
            Table.Quit();
            Marshal.ReleaseComObject(Table);
        }
        List<string> GeneratePeriods(int LoP, Tuple<DateTime, DateTime> timeSchedule) {
            List<string> ret = new List<string>();
            int h = timeSchedule.Item1.Hour;
            int m = timeSchedule.Item2.Minute;
            int hInc = LoP / 60;
            int mInc = LoP % 60;
            int m2,h2;
            m2 = m + mInc;
            int ov = 0;
            if (m2 >= 60) {
                ov = 1;
                m2 -= 60;
            }
            h2 = h + hInc + ov;
            while (IsLess(h2, m2, timeSchedule.Item2)) {
                ret.Add(h + ":" + (m < 10 ? "0" + m : m + "") + " - " + h2 + ":" + (m2 < 10 ? "0" + m2 : m2 + ""));
                m = m2;
                h = h2;
                m2 = m + mInc;
                ov = 0;
                if (m2 >= 60) {
                    ov = 1;
                    m2 -= 60;
                }
                h2 = h + hInc + ov;
            }
            return ret;
        }
        bool IsLess(int h,int m, DateTime old) {
            if (h < old.Hour)
                return true;
            if (h == old.Hour)
                if (m <= old.Minute)
                    return true;
            return false;
        }
        void Write(string Day,List<Dictionary<Venue,Course>> Periods,int r,int c) {
            Borders( Day, r, c);
            for(int i = 0; i < Periods.Count; i++) {
                PrintPeriod(Periods[i], r + 2, c + 1 + i);
            }
        }
        void PrintPeriod(Dictionary<Venue,Course> Period, int r,int c) {
            for(int i = 0; i < Venues.Count; i++) {
                if (Period.ContainsKey(Venues[i])) {
                    XL.Cells[r +i, c ].Value2 = Period[Venues[i]].ToString();
                }
            }
        }
        void Borders( string Name, int r, int c) {
            XL.Cells[r, c].Value2 = Name;
            for(int i = 1; i <= Periods.Count; i++) {
                XL.Cells[r+1, c+i].Value2 = Periods[i-1];
            }
            for (int i = 0; i <Venues.Count; i++) {
                XL.Cells[r + i+2 , c ].Value2 = Venues[i].ToString();
            }
        }
    }
}
