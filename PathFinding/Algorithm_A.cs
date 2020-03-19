using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PathFinding
{
    class Algorithm_A
    {
        private Form1 form1;
        private Drowing draw_copy;
        private List<struct_open_cell> open_Cells = new List<struct_open_cell>();
        
        struct struct_open_cell
        {
            public Drowing.Point point;
            public int G, H, F;
            public Drowing.Point parent_cell;

            public struct_open_cell(int x, int y, int z, int g, int h, int f, Drowing.Point parent)
            {
                point = new Drowing.Point(x, y, z);
                G = g;
                H = h;
                F = f;
                parent_cell = parent;
            }
        }


        public Algorithm_A(Form1 form1, Drowing draw_copy)
        {
            this.draw_copy = draw_copy;
            this.form1 = form1;
        }

        private void Side_checker(struct_open_cell work_cell, string side)
        {
            int _y = 0, _x = 0, _z = 0, _h = 0, _f = 0;
            int x = work_cell.point.x;
            int y = work_cell.point.y;
            int g = work_cell.G;
            switch (side)
            {
                case "U":
                    _y = y - 1;
                    if (_y < 0) return;
                    _x = x;
                    g += 10;
                    break;
                case "D":
                    _y = y + 1;
                    if (_y >= draw_copy.NRowsY) return;
                    _x = x;
                    g += 10;
                    break;
                case "L":
                    _x = x - 1;
                    if (_x < 0) return;
                    _y = y;
                    g += 10;
                    break;
                case "R":
                    _x = x + 1;
                    if (_x >= draw_copy.NColumnX) return;
                    _y = y;
                    g += 10;
                    break;
                case "LU":
                    _x = x - 1;
                    _y = y - 1;
                    g += 14;
                    if (_x < 0 || _y < 0 || draw_copy.Map[Drowing.Absol_Coord(y, _x, draw_copy.NColumnX)]
                        || draw_copy.Map[Drowing.Absol_Coord(_y, x, draw_copy.NColumnX)]) return;
                    break;
                case "LD":
                    _x = x - 1;
                    _y = y + 1;
                    g += 14;
                    if (_x < 0 || _y >= draw_copy.NRowsY || draw_copy.Map[Drowing.Absol_Coord(y, _x, draw_copy.NColumnX)]
                        || draw_copy.Map[Drowing.Absol_Coord(_y, x, draw_copy.NColumnX)]) return;
                    break;
                case "RU":
                    _x = x + 1;
                    _y = y - 1;
                    g += 14;
                    if (_x >= draw_copy.NColumnX || _y < 0 || draw_copy.Map[Drowing.Absol_Coord(y, _x, draw_copy.NColumnX)]
                        || draw_copy.Map[Drowing.Absol_Coord(_y, x, draw_copy.NColumnX)]) return;
                    break;
                case "RD":
                    _x = x + 1;
                    _y = y + 1;
                    g += 14;
                    if (_x >= draw_copy.NColumnX || _y >= draw_copy.NRowsY || draw_copy.Map[Drowing.Absol_Coord(y, _x, draw_copy.NColumnX)]
                        || draw_copy.Map[Drowing.Absol_Coord(_y, x, draw_copy.NColumnX)]) return;
                    break;
            }
            _z = Drowing.Absol_Coord(_y, _x, draw_copy.NColumnX);
            if (draw_copy.Map[_z])
            {
                return;
            }

            if (draw_copy.Closed_cell[_z] != 0)
            {
                return;
            }

            _h = hevristic_funct(_x, _y, draw_copy.Finish_point.x, draw_copy.Finish_point.y);
            _f = g + _h;
            int index_result = open_Cells.FindIndex(cell => cell.point.z.Equals(_z));
            if (index_result == -1)
            {
                open_Cells.Add(new struct_open_cell(_x, _y, _z, 0, _h, _h, work_cell.point));
                draw_copy.Processed_cells[_z] = draw_copy.Step;
            }
            else
            {
                if (open_Cells[index_result].G > g)
                {
                    struct_open_cell temp_add = open_Cells[index_result];
                    temp_add.G = g;
                    temp_add.F = temp_add.H + g;
                    temp_add.parent_cell = work_cell.point;
                    open_Cells[index_result] = temp_add;
                }
            }
        }

        public void Path_Finding()
        {

            if (draw_copy.Start_point.z == -1 || draw_copy.Finish_point.z == -1)
            {
                MessageBox.Show("Встановіть старт та фініш", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                form1.lock_buttons();
                return;
            }
            int mode = form1.Flag_radioButtons;
            int find_step = 0;
            draw_copy.Step = 1;
            draw_copy.Processed_cells[draw_copy.Start_point.z] = draw_copy.Step;
            form1.textBox1.Text = Convert.ToString(draw_copy.Step);
            form1.visualisation_mode(mode, form1, 1);
            draw_copy.Step++;

            for (int i = 0; i < (draw_copy.NRowsY * draw_copy.NColumnX); i++)
            {
                if (draw_copy.Map[i])
                {
                    draw_copy.Processed_cells[i] = -1;
                    draw_copy.Closed_cell[i] = -1;
                }
            }
            int _h = hevristic_funct(draw_copy.Start_point.x, draw_copy.Start_point.y, draw_copy.Finish_point.x, draw_copy.Finish_point.y);

            open_Cells.Add(new struct_open_cell(draw_copy.Start_point.x, draw_copy.Start_point.y,
                draw_copy.Start_point.z, 0, _h, _h, draw_copy.Start_point));

            while(open_Cells.Count != 0)
            {
                open_Cells.Sort((x, y) => x.F.CompareTo(y.F));
                struct_open_cell work_cell = open_Cells[0];
                open_Cells.RemoveAt(0);
                draw_copy.Closed_cell[work_cell.point.z] = work_cell.parent_cell.z;
                draw_copy.Check_point = work_cell.point;

                Side_checker(work_cell, "U");
                Side_checker(work_cell, "D");
                Side_checker(work_cell, "L");
                Side_checker(work_cell, "R");

                if (form1.checkBox2.Checked)
                {
                    Side_checker(work_cell, "LU");
                    Side_checker(work_cell, "LD");
                    Side_checker(work_cell, "RU");
                    Side_checker(work_cell, "RD");
                }
                if ((draw_copy.Closed_cell[draw_copy.Finish_point.z] != 0) && (draw_copy.Step != 2))
                {
                    find_step = draw_copy.Step;
                    if (form1.checkBox1.Checked)
                    {
                        form1.visualisation_mode(mode, form1, 1);
                        open_Cells.Clear();
                        break;
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
                int previous_element = draw_copy.Closed_cell[draw_copy.Finish_point.z];

                while (way_back.Peek() != draw_copy.Start_point)
                {
                    int x = 0, y = 0;
                    Drowing.Reference_to_XY(ref x, ref y, previous_element,draw_copy.NColumnX);
                    way_back.Push(new Drowing.Point(x,y,previous_element));
                    previous_element = draw_copy.Closed_cell[previous_element];
                }
                draw_copy.Anim.draw_way(way_back);
                form1.update_panel_without_redraw();
            }
            else
            {
                MessageBox.Show("Шлях не знайдено", "Повідомелння", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            draw_copy.Processed_cells = new int[draw_copy.NColumnX * draw_copy.NRowsY];
            draw_copy.Closed_cell = new int[draw_copy.NColumnX * draw_copy.NRowsY];
            draw_copy.Check_point = new Drowing.Point(-1, -1, -1);
            draw_copy.Step = 1;
            form1.lock_buttons();
            form1.DrawPanel.Enabled = false;

        }

        private int hevristic_funct(int x1, int y1, int x2, int y2)
        {
            int hevr_value = 0;
            hevr_value = (Math.Abs(y2 - y1) + Math.Abs(x2 - x1)) * 10;
            return hevr_value;
        }
    }
}
