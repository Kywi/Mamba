using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PathFinding
{
    public partial class Upload_Form : Form
    {
   
        List<Action> actions;
        public Upload_Form(List<Action> actions)
        {
            InitializeComponent();
            this.actions = actions;
        }

        private void button6_Click(object sender, EventArgs e) //Завантажити лабіринт з зображення
        {
            actions[0].Invoke();
           // parent.Load_From_Image();
            Close();
        }

        private void button1_Click(object sender, EventArgs e) //Завантажити лабіринт з файлу
        {
            actions[1].Invoke();
            //parent.Read_from_File();
            Close();
        }

        private void Upload_Form_Load(object sender, EventArgs e)
        {
            (new ToolTip()).SetToolTip(button1, "Завантажити лабіринт з файлу формату .LMAP");
            (new ToolTip()).SetToolTip(button6, "Лабіринт буде розпізнано з зображення");

        }
    }
}
