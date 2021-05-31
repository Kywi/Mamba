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
        //-----------------------------------------Variables
        private Form1 form1;
        private Graphics elemnt_draw;
        private Drowing draw_copy;
        private Bitmap picture;
        private Pen p;

        //------------------------------------------------

        public Animations()//empty constructor
        {

        }

        public Animations(Form1 form1, Drowing draw_copy)
        {
            this.form1 = form1;
            this.draw_copy = draw_copy;
            this.picture = (Bitmap)form1.DrawPanel.Image;
            elemnt_draw = Graphics.FromImage(this.picture);
        }

        //Метод дял типу кольору комірки
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
                    br = new SolidBrush(Color.FromArgb(119, 119, 119));//Колір для стінки
                    break;
                case 2:
                    br = new SolidBrush(Color.Blue);//Колір для свободної комірки
                    break;
                case 3:
                    br = new SolidBrush(Color.FromArgb(34, 244, 34));
                    break;
                case 4:
                    br = new SolidBrush(Color.Red); //Колір дял фінішу
                    break;
                case 5:
                    br = new SolidBrush(Color.Purple);//Колрі для опрацьованих комірок
                    break;
                case 6:
                    br = new SolidBrush(Color.FromArgb(0, 128, 255));//Колір комірок які опрацьовуються
                    break;
            }
            elemnt_draw.FillRectangle(br, rec);
            p = (type_of_element == 2) ?  new Pen(Color.Orange, 1) :  new Pen(Color.Black, 1);//Колір рамок комірок (оранжевий для тих які у масиві закритих)     
            elemnt_draw.DrawRectangle(p, rec);

            if (type_of_element == 1)//Комірка від якої відбувається аналіз має в собі круг оранжевого кольору
            {
                br = new SolidBrush(Color.Orange);
                rec.Height /= 2;
                rec.Width /= 2;
                rec.X += rec.Height/2;
                rec.Y += rec.Width/2;
                elemnt_draw.FillEllipse(br, rec);
            }
        }

        //Визначення кольору який потрібен дял данної комірки за наявносто тільки координат данної комірки
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
            if (z == draw_copy.Check_point.z)//Якщо комірка є аналізуючою
            {
                draw_Cells(x, y, 5, 1);
                return;
            }
            if (draw_copy.Map[z])//Якщо комірка є стіною
            {
                draw_Cells(x, y, 1);
                return;
            }      
            if (draw_copy.Processed_cells[z] == draw_copy.Step)//Якщо данну комріку ми перевіряємо
            {
                draw_Cells(x, y, 6);
                return;
            }
            if (draw_copy.Processed_cells[z] != 0)//Якщо комірка є обробленою
            {
                circuit = (draw_copy.Closed_cell[z] != 0) ? 2 : 0;
                draw_Cells(x, y, 5, circuit);
                return;
            }
            draw_Cells(x, y, 2);
        }

        //Метод який молює шлях назад отримуючи масив з точок типу Ppoint 
        public void draw_way(Stack<Ppoint> points)
        {
            SolidBrush yellow_br = new SolidBrush(Color.Yellow);
            Pen yellow_pen = new Pen(yellow_br);
            Ppoint previous_point = points.Peek();

            while (points.Count != 0)
            {
                Ppoint temp = points.Pop();
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

        public Bitmap getBlack_WhiteImage()
        {
            Bitmap black_whiteIMG = new Bitmap(form1.DrawPanel.Width, form1.DrawPanel.Height);
            Graphics backWhite = Graphics.FromImage(black_whiteIMG);
            for (int x = 0; x < draw_copy.NColumnX; x++)
            {
                for (int y = 0; y < draw_copy.NRowsY; y++)
                {
                    int z;
                    z = Drowing.Absol_Coord(y, x, draw_copy.NColumnX);
                    int Cell_X, Cell_Y;
                    Cell_X = x * draw_copy.CellSize;
                    Cell_Y = y * draw_copy.CellSize;
                    backWhite.FillRectangle(draw_copy.Map[z] ? new SolidBrush(Color.Black) : new SolidBrush(Color.White),
                        new Rectangle(Cell_X, Cell_Y, draw_copy.CellSize, draw_copy.CellSize));
                }
            }

            return black_whiteIMG;
        }

        //Перемальовує картинку повністю
        public void reDraw_Screen()
        {
            for (int i = 0; i < draw_copy.NColumnX; i++)
            {
                for (int j = 0; j < draw_copy.NRowsY; j++)
                {
                    chek_colour(i, j);
                }
            }
            update_drow_panel();
        }

        //Перемальовує 1 комірку
        public void reDraw_cell(int x, int y, int type_paint)
        {
            draw_Cells(x, y, type_paint);
            update_drow_panel();
        }

        //Переприсвоює картинку робочій області
        public void update_drow_panel()
        {
            form1.DrawPanel.Image = picture;
        }
    }
}
