using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PathFinding
{
    class Algorithm_Wave
    {
        private Form1 form1;
        private Drowing draw_copy;
        private Stack<Drowing.Point> front_wave = new Stack<Drowing.Point>();
        private List<Drowing.Point> future_front = new List<Drowing.Point>();
        private List<min_way> min_Ways = new List<min_way>();

        public struct min_way
        {
            public Drowing.Point point;
            public int step;

            public min_way(int x, int y, int z, int nstep)
            {
                point = new Drowing.Point(x, y, z);
                step = nstep;
            }
            
        }

        public Algorithm_Wave(Form1 form1,Drowing draw_copy)
        {
            this.form1 = form1;
            this.draw_copy = draw_copy;
        }

        private void Side_checker(Drowing.Point work_cell, string side, int mode = 0)
        {      
            int _y = 0, _x = 0, _z = 0;
            int x = work_cell.x;
            int y = work_cell.y;
            switch (side)
            {
                case "U":
                    _y = y - 1;
                    if (_y < 0) return;
                    _x = x;
                    break;
                case "D":
                    _y = y + 1;
                    if (_y >= draw_copy.NRowsY) return;
                    _x = x;
                    break;
                case "L":
                    _x = x - 1;
                    if (_x < 0) return;
                    _y = y;
                    break;
                case "R":
                    _x = x + 1;
                    if (_x >= draw_copy.NColumnX) return;
                    _y = y;
                    break;
                case "LU":
                    _x = x - 1;
                    _y = y - 1;
                    if (_x < 0 || _y < 0 ||draw_copy.Map[Drowing.Absol_Coord(y,_x,draw_copy.NColumnX)]
                        || draw_copy.Map[Drowing.Absol_Coord(_y, x, draw_copy.NColumnX)]) return;
                    break;
                case "LD":
                    _x = x - 1;
                    _y = y + 1;
                    if (_x < 0 || _y >= draw_copy.NRowsY || draw_copy.Map[Drowing.Absol_Coord(y, _x, draw_copy.NColumnX)]
                        || draw_copy.Map[Drowing.Absol_Coord(_y, x, draw_copy.NColumnX)]) return;
                    break;
                case "RU":
                    _x = x + 1;
                    _y = y - 1;
                    if (_x >= draw_copy.NColumnX || _y < 0 || draw_copy.Map[Drowing.Absol_Coord(y, _x, draw_copy.NColumnX)]
                        || draw_copy.Map[Drowing.Absol_Coord(_y, x, draw_copy.NColumnX)]) return;
                    break;
                case "RD":
                    _x = x + 1;
                    _y = y + 1;
                    if (_x >= draw_copy.NColumnX || _y >= draw_copy.NRowsY || draw_copy.Map[Drowing.Absol_Coord(y, _x, draw_copy.NColumnX)]
                        || draw_copy.Map[Drowing.Absol_Coord(_y, x, draw_copy.NColumnX)]) return;
                    break;
            }
            _z = Drowing.Absol_Coord(_y, _x, draw_copy.NColumnX);
            if (draw_copy.Map[_z]) return;
    
            switch (mode)
            {
                case 0:
                    if (draw_copy.Processed_cells[_z] != 0) return;
                    future_front.Add(new Drowing.Point(_x, _y, _z));
                    draw_copy.Processed_cells[_z] = draw_copy.Step;
                    break;
                case 1:
                    if (draw_copy.Processed_cells[_z] <= 0) return;
                    min_Ways.Add(new min_way(_x, _y, _z, draw_copy.Processed_cells[_z]));
                    break;
            }
        }

        public void Path_Finding()
        {

            if(draw_copy.Start_point.z == -1 || draw_copy.Finish_point.z == -1)
            {
                MessageBox.Show("Встановіть старт та фініш", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                form1.lock_buttons();
                return;
            }
            int mode = form1.Flag_radioButtons;
            int find_step = 0;
            for (int i = 0; i < (draw_copy.NRowsY * draw_copy.NColumnX); i++)
            {
                if (draw_copy.Map[i])
                {
                    draw_copy.Processed_cells[i] = -1;
                }
            }

            draw_copy.Step = 1;
            front_wave.Push(draw_copy.Start_point);
            draw_copy.Processed_cells[draw_copy.Start_point.z] = draw_copy.Step;
            form1.textBox1.Text =Convert.ToString(draw_copy.Step);
            form1.visualisation_mode(mode, form1, 1);
            draw_copy.Step++;
            while(front_wave.Count != 0)
            {
                
                future_front = new List<Drowing.Point>(); ;

                while (front_wave.Count != 0)
                {
                    Drowing.Point work_cell = front_wave.Pop();
                    Side_checker(work_cell, "U");
                    Side_checker(work_cell, "D");
                    Side_checker(work_cell, "L");
                    Side_checker(work_cell, "R");
                }

                future_front.Sort((x, y) => y.z.CompareTo(x.z));

                int zz = -999;
                foreach(Drowing.Point t_point in future_front)
                {
                    if(t_point.z != zz)
                    {
                        front_wave.Push(t_point);
                        zz = t_point.z;
                        if (t_point == draw_copy.Finish_point) 
                        {
                            find_step = draw_copy.Step;
                            if (form1.checkBox1.Checked)
                            {
                                front_wave.Clear();
                                break;
                            }
                        }
             
                    }
                }
                form1.textBox1.Text = Convert.ToString(draw_copy.Step);
                form1.visualisation_mode(mode, form1, 1);
                draw_copy.Step++;
            }


            if (find_step != 0)
            {
                Stack<Drowing.Point> way_back = new Stack<Drowing.Point>();
                way_back.Push(draw_copy.Finish_point);

                while (way_back.Peek() != draw_copy.Start_point)
                {
                    Drowing.Point work_cell = way_back.Peek();
                    min_Ways.Clear();

                    Side_checker(work_cell, "U", 1);
                    Side_checker(work_cell, "D", 1);
                    Side_checker(work_cell, "L", 1);
                    Side_checker(work_cell, "R", 1);

                    if (form1.checkBox2.Checked)
                    {
                        Side_checker(work_cell, "LU", 1);
                        Side_checker(work_cell, "LD", 1);
                        Side_checker(work_cell, "RU", 1);
                        Side_checker(work_cell, "RD", 1);
                    }

                    min_Ways.Sort((x, y) => x.step.CompareTo(y.step));
                    way_back.Push(min_Ways[0].point);

                }
                draw_copy.Anim.draw_way(way_back);
                form1.update_panel_without_redraw();
            }
            else
            {
                MessageBox.Show("Шлях не знайдено", "Повідомелння", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            draw_copy.Processed_cells = new int[draw_copy.NColumnX * draw_copy.NRowsY];
            draw_copy.Check_point = new Drowing.Point(-1, -1, -1);
            draw_copy.Step = 1;
            form1.lock_buttons();
            form1.DrawPanel.Enabled = false;
        }
    }
}
