using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Timi401 {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e) {

        }
        Back bck;
        private void button1_Click(object sender, EventArgs e) {
            try {
                bck.Login(textBox1.Text, textBox2.Text);
            }catch (Exception ex) {
                label3.Visible = true;
                label3.Text = ex.Message;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e) {
            label3.Visible = false;
        }

        private void button5_Click(object sender, EventArgs e) {

        }
        Account ac;
        private void button8_Click(object sender, EventArgs e) {
            int.TryParse(textBox3.Text, out int script);
            int.TryParse(textBox4.Text, out int no);
            ac.Update(script, no);
        }
    }
}
