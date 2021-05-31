using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PathFinding.All_Forms
{
    public partial class SaveForm : Form
    {
        List<Action> actions;
        public SaveForm(List<Action> actions)
        {
            InitializeComponent();
            // parent = form1;
            this.actions = actions;
        }

        private void button6_Click(object sender, EventArgs e) //Зберегти лабіринт у файл
        {
            actions[0].Invoke();
            Close();
        }

        private void button1_Click(object sender, EventArgs e) //Зберегти лабіринт у вигляді зображення
        {
            actions[1].Invoke();
            Close();
        }

        private void SaveForm_Load(object sender, EventArgs e)
        {
            (new ToolTip()).SetToolTip(button6, "Зберегти лабіринт у файл формату .LMAP");
            (new ToolTip()).SetToolTip(button1, "Зберегти лабіринт у формат зображення (лабіринт буде без сітки і чорно-білий)");

        }
    }
}
