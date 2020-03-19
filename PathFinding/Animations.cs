using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace PathFinding
{
    class Animations
    {
        private Form1 form1;
        private Graphics elemnt_draw;
        private Drowing draw_copy;
        private Bitmap picture;
        private Pen p;

        public Animations()
        {

        }

        public Animations(Form1 form1, Drowing draw_copy)
        {
            this.form1 = form1;
            this.draw_copy = draw_copy;
            this.picture = (Bitmap)form1.DrawPanel.Image;
            elemnt_draw = Graphics.FromImage(this.picture);

        }

        private void draw_Cells(int x, int y, int type_of_Paint, int type_of_element = 0)
        {
            int Cell_X, Cell_Y;
            Cell_X = x * draw_copy.CellSize;
            Cell_Y = y * draw_copy.CellSize;
            Rectangle rec = new Rectangle(Cell_X, Cell_Y, draw_copy.CellSize-1, draw_copy.CellSize-1);
            SolidBrush br = new SolidBrush(Color.Blue);
            switch (type_of_Paint)
            {
                case 1:
                    br = new SolidBrush(Color.FromArgb(119, 119, 119));
                    break;
                case 2:
                    br = new SolidBrush(Color.Blue);
                    break;
                case 3:
                    br = new SolidBrush(Color.FromArgb(34, 244, 34));
                    break;
                case 4:
                    br = new SolidBrush(Color.Red);
                    break;
                case 5:
                    br = new SolidBrush(Color.Purple);
                    break;
                case 6:
                    br = new SolidBrush(Color.FromArgb(0, 128, 255));
                    break;
            }
            elemnt_draw.FillRectangle(br, rec);
            p = (type_of_element == 2) ?  new Pen(Color.Orange, 1) :  new Pen(Color.Black, 1);

            
            elemnt_draw.DrawRectangle(p, rec);
            if (type_of_element == 1)
            {
                br = new SolidBrush(Color.Orange);
                rec.Height /= 2;
                rec.Width /= 2;
                rec.X += rec.Height/2;
                rec.Y += rec.Width/2;
                elemnt_draw.FillEllipse(br, rec);
            }
        }

        public void chek_colour(int x, int y)
        {
            int z, circuit = 0;
            z = Drowing.Absol_Coord(y, x, draw_copy.NColumnX);
            if (z == draw_copy.Start_point.z)
            {
                circuit = (draw_copy.Closed_cell[z] != 0) ?  2 : 0;
                draw_Cells(x, y, 3, circuit);
                return;
            }
            if (z == draw_copy.Finish_point.z)
            {
                circuit = (draw_copy.Closed_cell[z] != 0) ? 2 : 0;
                draw_Cells(x, y, 4, circuit);
                return;
            }
            if (z == draw_copy.Check_point.z)
            {
                draw_Cells(x, y, 5, 1);
                return;
            }
            if (draw_copy.Map[z])
            {
                draw_Cells(x, y, 1);
                return;
            }      
            if (draw_copy.Processed_cells[z] == draw_copy.Step)
            {
                draw_Cells(x, y, 6);
                return;
            }
            if (draw_copy.Processed_cells[z] != 0)
            {
                circuit = (draw_copy.Closed_cell[z] != 0) ? 2 : 0;
                draw_Cells(x, y, 5, circuit);
                return;
            }

            draw_Cells(x, y, 2);
        }

        public void draw_way(Stack<Drowing.Point> points)
        {
            SolidBrush yellow_br = new SolidBrush(Color.Yellow);
            Pen yellow_pen = new Pen(yellow_br);
            Drowing.Point previous_point = points.Peek();

            while (points.Count != 0)
            {
                Drowing.Point temp = points.Pop();
                int x = temp.x * draw_copy.CellSize + draw_copy.CellSize/4;
                int y = temp.y * draw_copy.CellSize + draw_copy.CellSize / 4;
                Rectangle rect = new Rectangle(x, y, draw_copy.CellSize / 2, draw_copy.CellSize / 2);

                elemnt_draw.FillEllipse(yellow_br,rect);

                elemnt_draw.DrawLine(yellow_pen, previous_point.x * draw_copy.CellSize + draw_copy.CellSize / 2,
                    previous_point.y * draw_copy.CellSize + draw_copy.CellSize / 2,
                    temp.x * draw_copy.CellSize + draw_copy.CellSize / 2,
                    temp.y * draw_copy.CellSize + draw_copy.CellSize / 2);
                previous_point = temp;
            }

        }

        public void reDraw_Screen()
        {
            for (int i = 0; i < draw_copy.NColumnX; i++)
            {
                for (int j = 0; j < draw_copy.NRowsY; j++)
                {
                    chek_colour(i, j);
                }
            }
            form1.DrawPanel.Image = picture;
        }

        public void reDraw_cell(int x, int y, int type_paint)
        {
            draw_Cells(x, y, type_paint);
            form1.DrawPanel.Image = picture;
        }

        public void update_drow_panel()
        {
            form1.DrawPanel.Image = picture;
        }
    }
}
