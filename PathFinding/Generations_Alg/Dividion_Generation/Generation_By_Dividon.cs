using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PathFinding
{
    class Generation_By_Dividon : Generation, Generation_Interface
    {
        //----------------------------------------------------Variables

        private Stack<rect_point> Rect_Points = new Stack<rect_point>();
        private rect_point temp;

        struct hole
        {
            public int count_holes;
            public char side;

            public hole(int ch, char sd)
            {
                count_holes = ch;
                side = sd;
            }
        }

        struct rect_point //РОзміщуються крайні точки області(крайні точки прямокутника) для ділення
        {
            public Ppoint P1;
            public Ppoint P2;

            public rect_point(int p1x, int p1y, int p2x, int p2y)
            {
                P1 = new Ppoint(p1x, p1y, 0);
                P2 = new Ppoint(p2x, p2y, 0);
            }
        }

        //-------------------------------------------------------------

        public Generation_By_Dividon(Form1 form1, Drowing draw_copy) : base(form1, draw_copy) { }
  

        protected override void init_mas()
        {
            draw_copy.init_drowing(2);//повна очистка робочої області
        }

        public override void create_Labyrithm()
        {
            init_mas();
            Random rx = new Random();
            Random ry = new Random();
            Rect_Points.Push(new rect_point(0, 0, draw_copy.NColumnX - 1, draw_copy.NRowsY - 1));
            bool flag_reverse=true;

            while (Rect_Points.Count != 0)
            {
                Mode = form1.Flag_radioButtons;
                temp = Rect_Points.Pop();
                int X_Numb_of_dividion, Y_Numb_of_dividion;
                X_Numb_of_dividion = (temp.P2.x - temp.P1.x) / 2 - 1;
                Y_Numb_of_dividion = (temp.P2.y - temp.P1.y) / 2 - 1;
                if ((X_Numb_of_dividion == 0) || (Y_Numb_of_dividion == 0)) continue;
                int x0, y0; // претин ліній ділення
                int rt=0;//випадкове зачення 
                int rk = ry.Next(Y_Numb_of_dividion - 1) + 1;
                if ((X_Numb_of_dividion / Y_Numb_of_dividion) >= 2)
                {
                    rt = rx.Next(Convert.ToInt32(Math.Round(X_Numb_of_dividion * 0.3)), Convert.ToInt32(Math.Round(X_Numb_of_dividion * 0.7)));
                }
                else  rt = rx.Next(X_Numb_of_dividion - 1) + 1;
                x0 = temp.P1.x + rt * 2;
                y0 = temp.P1.y + rk * 2;
                //області які отримуємо пілся ділення
                Rect_Points.Push(new rect_point(temp.P1.x, temp.P1.y, x0, y0));
                Rect_Points.Push(new rect_point(x0, temp.P1.y, temp.P2.x, y0));
                Rect_Points.Push(new rect_point(x0, y0, temp.P2.x, temp.P2.y));
                Rect_Points.Push(new rect_point(temp.P1.x, y0, x0, temp.P2.y));

                for (int i = temp.P1.x; i < temp.P2.x; i++)
                {
                    draw_copy.Map[Drowing.Absol_Coord(y0, i, draw_copy.NColumnX)] = true;
                }

                for (int j = temp.P1.y; j < temp.P2.y; j++)
                {
                    draw_copy.Map[Drowing.Absol_Coord(j, x0, draw_copy.NColumnX)] = true;
                }
               
                form1.visualisation_mode(mode, form1, 1);

                //отвори в цих областях
                List<hole> h = new List<hole>();
                h.Add(new hole((y0 - temp.P1.y) / 2, Mas_OF_sides[0]));
                h.Add(new hole((temp.P2.y - y0) / 2, Mas_OF_sides[1]));
                h.Add(new hole((x0 - temp.P1.x) / 2, Mas_OF_sides[2]));
                h.Add(new hole((temp.P2.x - x0) / 2, Mas_OF_sides[3]));
                
                //покращення рандомізації
                if (flag_reverse)
                {
                    h.Reverse();
                }
                
                //прорубування отворів
                h.Sort((x, y) => y.count_holes.CompareTo(x.count_holes));
                for(int i=0; i < h.Count-1; i++)
                {
                    rt = rx.Next(1,h[i].count_holes);
                    int _y=0, _x=0;
                    switch (h[i].side)
                    {
                        case 'U':
                            _y = y0 - (rt * 2 - 1);
                            _x = x0;
                            break;
                        case 'D':
                            _y = y0 + rt * 2 - 1;
                            _x = x0;
                            break;
                        case 'L':
                            _x = x0 -( rt * 2 - 1);
                            _y = y0;
                            break;
                        case 'R':
                            _x = x0 + rt * 2 - 1;
                            _y = y0;
                            break;
                    }
                    draw_copy.Map[Drowing.Absol_Coord(_y, _x, draw_copy.NColumnX)] = false;
                }             
                form1.visualisation_mode(mode, form1, 1);
                flag_reverse = !flag_reverse;
            }
            Reset_values();
        }
    }
}
