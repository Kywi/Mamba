using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Windows.Forms;

namespace PathFinding
{
    class Drowing
    {
        //-----------------------------------------Variables
        private int cellSize;
        private bool[] map;
        Form1 form1;
        [JsonIgnore]
        private Animations anim;
        private int nColumnX;
        private int nRowsY;
        private int[] processed_cells;
        private int step=1;
        private int[] closed_cell;

        public Ppoint Start_point { get; set; } = new Ppoint(-1, -1, -1);
        public Ppoint Finish_point { get; set; } = new Ppoint(-1, -1, -1);
        [JsonIgnore]
        public Ppoint Check_point { get; set; } = new Ppoint(-1, -1, -1);
        public int NColumnX { get => nColumnX; set => nColumnX = value; }
        public int NRowsY { get => nRowsY; set => nRowsY = value; }
        public int CellSize { get => cellSize; set => cellSize = value; }
        public bool[] Map { get => map; set => map = value; }
        [JsonIgnore]
        public Form1 Form1 { get => form1; set => form1 = value; }
        internal Animations Anim { get => anim; set => anim = value; }
        public int[] Processed_cells { get => processed_cells; set => processed_cells = value; }
        [JsonIgnore]
        public int Step { get => step; set => step = value; }
        public int[] Closed_cell { get => closed_cell; set => closed_cell = value; }

        //----------------------------------------------------


        public Drowing(Form1 form1)
        {
            this.Form1 = form1;
        }

        public Drowing()
        {

        }

        public static int Absol_Coord(int rowY, int columnX, int numberOfColums)//Метод для отримання абсолютнї координати в масиві маючи тільки Х і У
        {
            return rowY * numberOfColums + columnX;
        }

        public static void Reference_to_XY(ref int x, ref int y,int z,int numberOfColums)//Метод для отримання абсолютного Х і У маючи тільки z 
        {
            y = z / numberOfColums;
            x = z - y * numberOfColums;
        }

        private void map_new_labir()//формирование лабиринта с бордюром(без отрисовки на панели только в масиве)
        {
            Map = new bool[NColumnX * NRowsY];
            Processed_cells = new int[NColumnX * NRowsY];
            Closed_cell = new int[NColumnX * NRowsY];

            for (int i = 0; i < NRowsY; i++)
            {
                Map[Absol_Coord(i, 0, NColumnX)] = true;
                Map[Absol_Coord(i, NColumnX - 1, NColumnX)] = true;
            }

            for (int i = 1; i < NColumnX; i++)
            {
                Map[Absol_Coord(0, i, NColumnX)] = true;
                Map[Absol_Coord(NRowsY - 1, i, NColumnX)] = true;
            }
        }

        public void cell_clicker(int x, int y, MouseButtons c_button, int mode)//Змінює значення в натиснутій комірці на робочій області
        {
            if ((x >= (NColumnX * CellSize)) || (y >= (NRowsY * CellSize)) || (y < 0))
            {
                return;
            }

            int x_cell_cliceked, y_cell_cliced;
            x_cell_cliceked = x / CellSize;
            y_cell_cliced = y / CellSize;
            int abs_coord = Drowing.Absol_Coord(y_cell_cliced, x_cell_cliceked, NColumnX);
            switch (c_button)
            {
                case MouseButtons.Left:
                    if (Start_point.z == abs_coord)
                    {
                        Start_point = new Ppoint(-1, -1, -1);
                    }

                    if (Finish_point.z == abs_coord)
                    {
                        Finish_point = new Ppoint(-1, -1, -1);
                    }

                    if (mode == 2)
                    {
                        Map[abs_coord] = true;
                        Form1.X = x_cell_cliceked;
                        Form1.Y = y_cell_cliced;
                    }
                    else
                    {
                        Map[abs_coord] = !Map[abs_coord];
                    }

                    break;
                case MouseButtons.Right:
                    Map[abs_coord] = false;
                    Start_point = new Ppoint(x_cell_cliceked, y_cell_cliced, abs_coord);
                    break;
                case MouseButtons.Middle:
                    Map[abs_coord] = false;
                    Finish_point = new Ppoint(x_cell_cliceked, y_cell_cliced, abs_coord);
                    break;
            }
        }

        public void init_drowing(int mode)//Встановлення початкових значень для лабіринту
        {
            switch (mode)
            {
                case 1:
                    CellSize = Convert.ToInt32(Form1.size_cell.Value);
                    NColumnX = Form1.DrawPanel.Width / CellSize;
                    NRowsY = Form1.DrawPanel.Height / CellSize;

                    Form1.cRows_Y.Value = NRowsY;
                    Form1.cColunm_X.Value = NColumnX;
                    break;
                case 2:
                    NColumnX = Convert.ToInt32(Form1.cColunm_X.Value);
                    NRowsY = Convert.ToInt32(Form1.cRows_Y.Value);
                    CellSize = Math.Min(Form1.DrawPanel.Width / NColumnX, Form1.DrawPanel.Height / NRowsY);

                    Form1.size_cell.Value = CellSize;
                    break;
            }
            Start_point  = new Ppoint(-1, -1, -1);
            Finish_point = new Ppoint(-1, -1, -1);
            Form1.DrawPanel.Image = new Bitmap(Form1.DrawPanel.Width, Form1.DrawPanel.Height);
            Anim = new Animations(Form1, this);
            map_new_labir();
            Anim.reDraw_Screen();
        }

        //Виводить крок на якому знайдено шлях
        public void Show_Step(int Find_Step)
        {
            MessageBox.Show("Виконано за " + Find_Step.ToString() + " кроків", "Результат виконання", MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

    }
}
