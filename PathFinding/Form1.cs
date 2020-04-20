using System;
using System.Drawing;
using Newtonsoft.Json;
using System.Windows;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PathFinding
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //-----------------------------------------------------------------------------------------------Global variables
        int a = 0;
        private int y;
        private int flag_radioButtons;
        private int time_delay;
        private int x;
        private Drowing draw;
        private Thread demostr;
        private List<RadioButton> radiobuttons_list;
        private List<Button> buttons_list;
        private List<NumericUpDown> numericUpDowns_list;
        private Load_Form form_progres;
   
        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
        public int A { get => a; set => a = value; }
        public Thread Demostr { get => demostr; set => demostr = value; }
        internal Drowing Draw { get => draw; set => draw = value; }
        public int Flag_radioButtons { get => flag_radioButtons; set => flag_radioButtons = value; }
        public int Time_delay { get => time_delay; set => time_delay = value; }
        public List<RadioButton> Radiobuttons_list { get => radiobuttons_list; set => radiobuttons_list = value; }
        public List<Button> Buttons_list { get => buttons_list; set => buttons_list = value; }
        public List<NumericUpDown> NumericUpDowns_list { get => numericUpDowns_list; set => numericUpDowns_list = value; }
        public Load_Form Form_progres { get => form_progres; set => form_progres = value; }


        //-----------------------------------------------------------------------------------------------------------------


        //--------------------------------------------------------FORM1 settings
        private void Form1_Load(object sender, EventArgs e)
        {
            basic_sizes_AND_settings();
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }
        //----------------------------------------------------------


        //-----------------------------------------------------------MENU basic settings and events
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Pen pen = new Pen(Color.LightGray, 1);
            e.Graphics.DrawLine(pen, button2.Width + 10, panel1.Height - 5, button2.Width + 10, 5);
            e.Graphics.DrawLine(pen, size_cell.Location.X + size_cell.Width + 10, panel1.Height - 5, size_cell.Location.X + size_cell.Width + 10, 5);
            e.Graphics.DrawLine(pen, button4.Location.X + button4.Width + 10, panel1.Height - 5, button4.Location.X + button4.Width + 10, 5);
            e.Graphics.DrawLine(pen, radioButton1.Location.X + radioButton1.Width + 10, panel1.Height - 5, radioButton1.Location.X + radioButton1.Width + 10, 5);
            e.Graphics.DrawLine(pen, button5.Location.X + button5.Width + 10, panel1.Height - label10.Height - 5, button5.Location.X + button5.Width + 10, 5);
            e.Graphics.DrawLine(pen, button7.Location.X + button7.Width + 10, panel1.Height - 5, button7.Location.X + button7.Width + 10, 5);
        }
        //--------------------------------------------------------------------------------


        //-----------------------------------------------------------Events for drawing
        private void button1_Click(object sender, EventArgs e)
        {
            A = 1;
            DrawPanel.Enabled = true;
            DrawPanel.Invalidate();

        }

        private void DrawPanel_Paint(object sender, PaintEventArgs e)
        {
            switch (A)
            {
                case 1:
                    //Повне перермалювання робочої області при зміні розміру комірки
                    Draw = new Drowing(this);
                    Draw.init_drowing(A);
                    A = 0;
                    break;
                case 2:
                    //Повне перермалювання робочої області при зміні розмірку кількості комірки
                    Draw = new Drowing(this);
                    Draw.init_drowing(A);
                    A = 0;
                    break;
                case 3:
                    //Перемалювання робочої області з заданими парпметрами
                    Draw.Anim.reDraw_Screen();
                    A = 0;
                    break;
                case 4:
                    //Перерисовка робочої області без зміни робочої області
                    Draw.Anim.update_drow_panel();
                    A = 0;
                    break;
                case 5:
                    //Перемалювання 1 комірки
                    Draw.Anim.reDraw_cell(X, Y, 1);
                    break;
            }
        }

        private void cColunm_X_ValueChanged(object sender, EventArgs e)
        {
            A = 2;
            DrawPanel.Invalidate();
            DrawPanel.Enabled = true;
        }

        private void cRows_Y_ValueChanged(object sender, EventArgs e)
        {
            A = 2;
            DrawPanel.Invalidate();
            DrawPanel.Enabled = true;
        }

        private void size_cell_ValueChanged(object sender, EventArgs e)
        {
            A = 1;
            DrawPanel.Invalidate();
            DrawPanel.Enabled = true;
        }

        private void DrawPanel_MouseDown(object sender, MouseEventArgs e)
        {
            Draw.cell_clicker(e.X, e.Y, e.Button, 1);
            A = 3;
            DrawPanel.Invalidate();
        }

        private void DrawPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Draw.cell_clicker(e.X, e.Y, e.Button, 2);
                A = 5;
                DrawPanel.Invalidate();
            }
        }

        //Гененрація лабіринту
        private void button7_Click(object sender, EventArgs e)
        {
            DrawPanel.Enabled = true;

            switch (comboBox2.SelectedIndex)
            {
                case 0:
                    Deep_Generation();
                    break;
                case 1:                 
                    Dividion();
                    break;
            }
        }

        //Прохожденя лабиринта
        private void button5_Click(object sender, EventArgs e)
        {
            DrawPanel.Enabled = true;
            Show_load_form();
            switch(comboBox1.SelectedIndex)
            {
                case 0:
                    Wave();
                    break;
                case 1:
                    A_star();
                    break;
                case 2:
                    Best_First_Search();
                    break;
            }
        }

        //Возобновления потока
        private void button9_Click(object sender, EventArgs e)
        {
            button9.Enabled = false;
            demostr.Resume();
        }

   
        //Очиста області від промальованого шляху
        private void button4_Click(object sender, EventArgs e)
        {
            A = 3;
            DrawPanel.Enabled = true;
            DrawPanel.Invalidate();
        }

        //-----------------------------------------------------------


        //-----------------------------------------------------------Some_functions

        //Базові розміри і значання начальних змінних
        private void basic_sizes_AND_settings()
        {
            Draw = new Drowing(this);
            Draw.init_drowing(1);
            Flag_radioButtons = 1;
            Form_progres = new Load_Form();

            size_cell.Maximum = Math.Min(DrawPanel.Width, DrawPanel.Height);

            cColunm_X.Maximum = DrawPanel.Width / Convert.ToInt32(size_cell.Minimum);
            cRows_Y.Maximum = DrawPanel.Height / Convert.ToInt32(size_cell.Minimum);

            CheckForIllegalCrossThreadCalls = false;
      
            radiobuttons_list = new List<RadioButton>();
                radiobuttons_list.Add(radioButton1);
                radiobuttons_list.Add(radioButton2);
                radiobuttons_list.Add(radioButton3);

            buttons_list = new List<Button>();
                buttons_list.Add(button1);
                buttons_list.Add(button2);
                buttons_list.Add(button3);
                buttons_list.Add(button4);
                buttons_list.Add(button5);
                buttons_list.Add(button7);

            NumericUpDowns_list = new List<NumericUpDown>();
            numericUpDowns_list.Add(size_cell);
            numericUpDowns_list.Add(cColunm_X);
            numericUpDowns_list.Add(cRows_Y);

            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;

            if (!Read_Write_F.Check_Teory_Form_State())
            {
                Teory teory_Form = new Teory();
                teory_Form.ShowDialog();
            }
        }

        //Перемалювання робочої області
        public void update_screen()
        {
           A = 3;
           DrawPanel.Invalidate();
           DrawPanel.Update();
        }

        //Стирання шляху який малюєтся алгоритмами проходження
        public void update_panel_without_redraw()
        {
            A = 4;
            DrawPanel.Invalidate();
            DrawPanel.Update();
        }

        //------------------------------------------------------------Функції та методи для роботи з файлами
        private void write_to_file(string fname, string serialized_obj)
        {
            using (FileStream stream = new FileStream(fname, FileMode.Create))
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write(serialized_obj);
                    writer.Close();
                }
            }
        }   

        public void Read_from_File()
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (openFileDialog1.FileName == null) return;
                Read_Write_F read_Write_ = new Read_Write_F(this);
                read_Write_.Read_From_file(openFileDialog1.FileName);
            }
        }

        public void Load_From_Image()
        {
            try
            {
                if (openFileDialog2.ShowDialog() == DialogResult.OK)
                {
                    Recognition_pict recognition = new Recognition_pict(this, draw);
                    recognition.recogn_Picture(openFileDialog2.FileName.ToString());
                }
            }
            catch (IOException x)
            {
                MessageBox.Show("Файл пошкоджено! Оберіть інший файл.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                A = 1;
                DrawPanel.Invalidate();
                DrawPanel.Update();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (saveFileDialog1.FileName == null) return;
                Read_Write_F read_Write_ = new Read_Write_F(this);
                read_Write_.write_to_file(saveFileDialog1.FileName);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Upload_Form upload = new Upload_Form(this);
            upload.ShowDialog();
        }
        //----------------------------------------------------------------------------------

        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            trackBar1.Enabled = false;
            if (radioButton1.Checked) Flag_radioButtons = 1;
            else if (radioButton2.Checked)
            {
                Flag_radioButtons = 2;
            }
            else
            {
                Flag_radioButtons = 3;
                trackBar1.Enabled = true;
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (demostr == null)
            {
                return;
            }
            if (demostr.ThreadState == ThreadState.Suspended)
            {
                demostr.Resume();
            }
            demostr.Abort();
        }

        //Зміна швидкості стану потоку
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
           Time_delay = trackBar1.Value*100;
        }

        //Перевірка розмврва полів, для деяких алгоритмів генерації потрібно деяке початкове мінімальне значення
        private void Check_fields_size()
        {
            if ((cColunm_X.Value % 2) == 0) cColunm_X.Value -= 1;
            if ((cRows_Y.Value % 2) == 0) cRows_Y.Value -= 1;
            if (cColunm_X.Value <= 3) cColunm_X.Value = 3;
            if (cRows_Y.Value <= 3) cRows_Y.Value = 3;
        }

        //Тип візуалізації алгоритму
        public void visualisation_mode(int mode, Form1 form1, int draw_m = 0)
        {
            switch (mode)
            {
                case 2:  
                    form1.button9.Enabled = true;
                    update_screen();
                    Thread.CurrentThread.Suspend();
                    break;
                case 3:
                    update_screen();
                    Thread.Sleep(form1.Time_delay);
                    break;
            }
        }

        //Блокування кнопок на формі
        public void lock_buttons()
        {
            foreach(Button button in buttons_list)
            {
                button.Enabled = !button.Enabled;
            }

            foreach(NumericUpDown numericUpDown in numericUpDowns_list)
            {
                numericUpDown.Enabled = !numericUpDown.Enabled;
            }

            checkBox1.Enabled = !checkBox1.Enabled;
            checkBox2.Enabled = !checkBox2.Enabled;

            DrawPanel.Enabled = !DrawPanel.Enabled;

            comboBox1.Enabled = !comboBox1.Enabled;
            comboBox2.Enabled = !comboBox2.Enabled;
        }

        //---------------------------------------------------Algorithm functions(тут розміщені функції для запуску потока з необхідним алгоритмом)
        private void Wave()
        {
            update_screen();
            DrawPanel.Update();
            lock_buttons();
            PathFind_Int algorithm_Wave = new Algorithm_Wave(this, Draw);

            Demostr = new Thread(() =>
            {
                algorithm_Wave.Path_Finding();
            });
            demostr.Start();
        }
         
        private void A_star()
        {
            update_screen();
            DrawPanel.Update();
            lock_buttons();
            PathFind_Int algorithm_A = new Algorithm_A(this, Draw);

            Demostr = new Thread(() =>
            {
                algorithm_A.Path_Finding();
            });
            demostr.Start();
        }

        private void Best_First_Search()
        {
            update_screen();
            DrawPanel.Update();
            lock_buttons();
            PathFind_Int Best_F = new Algorithm_Best_F(this, Draw);

            Demostr = new Thread(() =>
            {
                Best_F.Path_Finding();
            });
            demostr.Start();
        }

        private void Dividion()
        {
            update_screen();
            Check_fields_size();
            DrawPanel.Update();
            lock_buttons();
            Generation_Interface generation_By_Dividon = new Generation_By_Dividon(this, draw);
            Demostr = new Thread(() =>
            {
                generation_By_Dividon.create_Labyrithm();
            });
            Demostr.Start();
        }

        private void Deep_Generation()
        {
            update_screen();
            Check_fields_size();
            DrawPanel.Update();
            lock_buttons();
            Generation_Interface deep_generation = new GenerationAI(this, Draw);
            Demostr = new Thread(() =>
            {
                deep_generation.create_Labyrithm();
            });
            demostr.Start();
        }
        //-----------------------------------------------------------------------------

        //-------------------------------------------------Закриття і показ форми візуалізації
        public void Hide_load_form()
        {
            if (radioButton1.Checked)
            {
                radioButton2.Enabled = true;
                radioButton3.Enabled = true;
                Form_progres.Hide();
            }
        }

        public void Show_load_form()
        {
            if (radioButton1.Checked)
            {
                radioButton2.Enabled = false;
                radioButton3.Enabled = false;
                Form_progres.Show();
            }
        }
        //------------------------------------------------------------------------------------------

        private void button6_Click_2(object sender, EventArgs e)
        {
            Teory teory_Form = new Teory();
            teory_Form.ShowDialog();
        }

     

        //---------------------------------------------------------

    }
}



