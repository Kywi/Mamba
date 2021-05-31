using Newtonsoft.Json;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace PathFinding
{
    class Read_Write_F
    {
        private Form1 form1;

        public Read_Write_F(Form1 form1)
        {
            this.form1 = form1;

        }

        public void Read_From_file(string filename)
        {
            try
            {
                string ser = "";
                BinaryReader fin = new BinaryReader(new FileStream(filename, FileMode.Open));
                while (fin.BaseStream.Position != fin.BaseStream.Length)
                {
                    ser = fin.ReadString();
                }
                fin.Close();
                Drowing temp = new Drowing(form1);
                temp = JsonConvert.DeserializeObject<Drowing>(ser);
                form1.cColunm_X.Value = temp.NColumnX;
                form1.cRows_Y.Value = temp.NRowsY;
                form1.size_cell.Value = temp.CellSize;
                form1.Draw = temp;
                form1.Draw.Form1 = form1;
                form1.DrawPanel.Image = new Bitmap(form1.DrawPanel.Width, form1.DrawPanel.Height);
                form1.Draw.Anim = new Animations(form1, form1.Draw);
                form1.A = 3;
                form1.DrawPanel.Invalidate();
                form1.DrawPanel.Update();
            }
            catch (IOException x)
            {
                MessageBox.Show("Файл пошкоджено! Оберіть інший файл.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                form1.A = 1;
                form1.DrawPanel.Invalidate();
                form1.DrawPanel.Update();
            }
        }

        public void write_to_file(string fname)
        {
            string serialized_obj = JsonConvert.SerializeObject(form1.Draw);
            using (FileStream stream = new FileStream(fname, FileMode.Create))
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write(serialized_obj);
                    writer.Close();
                }
            }
            form1.DrawPanel.Image.Save("MazePicture");
        }

        public static bool Check_Teory_Form_State()//чи показувати форму документації спочатку
        {
            bool F_state = true;
            BinaryReader fin = new BinaryReader(new FileStream("Open_Teory_State.PF", FileMode.Open));
            while (fin.BaseStream.Position != fin.BaseStream.Length)
            {
                F_state = fin.ReadBoolean();
            }
            fin.Close();
            return F_state;
        }
    }
}
