using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SchedulingSystem;
namespace Timetable {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }
        

        private void Button2_Click(object sender, EventArgs e) {
            OpenFileDialog fd= new OpenFileDialog();
            DialogResult a = fd.ShowDialog();
            textBox1.Text = fd.FileName;
        }

        private void Button1_Click(object sender, EventArgs e) {
            string st = numericUpDown1.Value + ":" + numericUpDown2.Value;
            string nd = numericUpDown4.Value + ":" + numericUpDown3.Value;
            DateTime start = new DateTime(2000, 01, 01, (int)numericUpDown1.Value, (int)numericUpDown2.Value, 0);
            DateTime end = new DateTime(2000, 01, 01, (int)numericUpDown4.Value, (int)numericUpDown3.Value, 0);
            List<string> Days = new List<string>();
            foreach(string day in checkedListBox1.CheckedItems) {
                Days.Add(day);
            }
            SchedulingSystem.Timetable tt = new SchedulingSystem.Timetable(textBox1.Text, new Tuple<DateTime, DateTime>(start, end), 1, Days);
            List<List<Dictionary<Venue,Course>>> Gen = tt.Generate();
            Output outp = new Output(@"C:\Users\Opsi Jay\Documents\Visual Studio 2017\Projects\Timetable\Timetable\output.xlsx", Gen, new Tuple<DateTime, DateTime>(start, end), Days, 60);
        }

        private void Button3_Click(object sender, EventArgs e) {
            Input npt = Input.Load(textBox1.Text + ".timetable");
            string st = numericUpDown1.Value + ":" + numericUpDown2.Value;
            string nd = numericUpDown4.Value + ":" + numericUpDown3.Value;
            DateTime start = new DateTime(2000, 01, 01, (int)numericUpDown1.Value, (int)numericUpDown2.Value, 0);
            DateTime end = new DateTime(2000, 01, 01, (int)numericUpDown4.Value, (int)numericUpDown3.Value, 0);
            List<string> Days = new List<string>();
            foreach (string day in checkedListBox1.CheckedItems) {
                Days.Add(day);
            }
            SchedulingSystem.Timetable tt = new SchedulingSystem.Timetable(npt, new Tuple<DateTime, DateTime>(start, end), 1, Days);
            List<List<Dictionary<Venue, Course>>> Gen = tt.Generate();
            Output outp = new Output(@"C:\Users\Opsi Jay\Documents\Visual Studio 2017\Projects\Timetable\Timetable\output.xlsx", Gen, new Tuple<DateTime, DateTime>(start, end), Days, 60);
        }
    }
}
