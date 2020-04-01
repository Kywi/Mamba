using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PathFinding
{
    class Algorithm_Wave : PathFind
    {
        //-----------------------------------------Variables
     
        private Stack<Ppoint> front_wave = new Stack<Ppoint>();
        private List<Ppoint> future_front = new List<Ppoint>();
        private List<min_way> min_Ways = new List<min_way>();
       
        public struct min_way
        {
            public Ppoint point;
            public int step;

            public min_way(int x, int y, int z, int nstep)
            {
                point = new Ppoint(x, y, z);
                step = nstep;
            }
            
        }
        //---------------------------------------------

        public Algorithm_Wave(Form1 form1, Drowing draw_copy) : base(form1, draw_copy) { }
      

        private void Side_checker(Ppoint work_cell, string side, int mode = 0)
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
                    future_front.Add(new Ppoint(_x, _y, _z));
                    draw_copy.Processed_cells[_z] = draw_copy.Step;
                    break;
                case 1:
                    if (draw_copy.Processed_cells[_z] <= 0) return;
                    min_Ways.Add(new min_way(_x, _y, _z, draw_copy.Processed_cells[_z]));
                    break;
            }
        }

        public override void Path_Finding()
        {

            if(draw_copy.Start_point.z == -1 || draw_copy.Finish_point.z == -1)
            {
                form1.Hide_load_form();
                MessageBox.Show("Встановіть старт та фініш", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                form1.lock_buttons();
                return;
            }
            int find_step = 0;
            form1.Show_load_form();
            First_Step();

            while(front_wave.Count != 0)
            {
                Mode = form1.Flag_radioButtons;

                future_front = new List<Ppoint>(); ;

                while (front_wave.Count != 0)
                {
                    Ppoint work_cell = front_wave.Pop();
                    for (int i = 0; i < 4; i++)
                    {
                        Side_checker(work_cell, Mas_OF_sides[i]);
                    }
                }

                future_front.Sort((x, y) => y.z.CompareTo(x.z));

                int zz = -999;
                foreach(Ppoint t_point in future_front)
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
                Stack<Ppoint> way_back = new Stack<Ppoint>();
                way_back.Push(draw_copy.Finish_point);

                while (way_back.Peek() != draw_copy.Start_point)
                {
                    Ppoint work_cell = way_back.Peek();
                    min_Ways.Clear();

                    for (int i = 0; i < 4; i++)
                    {
                        Side_checker(work_cell, Mas_OF_sides[i], 1);
                    }

                    if (form1.checkBox2.Checked)
                    {
                        for (int i = 4; i < 8; i++)
                        {
                            Side_checker(work_cell, Mas_OF_sides[i], 1);
                        }
                    }

                    min_Ways.Sort((x, y) => x.step.CompareTo(y.step));
                    way_back.Push(min_Ways[0].point);

                }
                draw_copy.Anim.draw_way(way_back);
                form1.update_panel_without_redraw();
                form1.Hide_load_form();
                draw_copy.Show_Step(find_step);
            }
            else
            {
                form1.Hide_load_form();
                MessageBox.Show("Шлях не знайдено", "Повідомелння", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            Reset_Values();   
        }

        protected override void First_Step()
        {
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
            form1.textBox1.Text = Convert.ToString(draw_copy.Step);
            form1.visualisation_mode(mode, form1, 1);
            draw_copy.Step++;
        }
    }
}
