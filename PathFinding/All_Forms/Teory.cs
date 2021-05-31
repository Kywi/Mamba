using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PathFinding
{
    public partial class Teory : Form
    {
        private string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\WebsiteFor_Kurs\index.html";
        
        public Teory()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Teory_on_open_state();
            Close();
        }

        private void Teory_on_open_state()
        {
            using (FileStream stream = new FileStream("Open_Teory_State.PF", FileMode.Create))
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write(checkBox1.Checked);
                    writer.Close();
                }
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
             Process.Start(path);
        }

        private void Teory_FormClosed(object sender, FormClosedEventArgs e)
        {
            Teory_on_open_state();
        }

        private void Teory_Load(object sender, EventArgs e)
        {          
            BinaryReader fin = new BinaryReader(new FileStream("Open_Teory_State.PF", FileMode.Open));
            while (fin.BaseStream.Position != fin.BaseStream.Length)
            {
                checkBox1.Checked = fin.ReadBoolean();
            }
            fin.Close();
        }

  
    }
}
