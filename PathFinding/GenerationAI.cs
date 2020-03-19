using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PathFinding
{
    class GenerationAI : Generation_Interface
    {
        private Form1 form1;
        private Drowing draw_copy;
        private Stack<Drowing.Point> chain_cells = new Stack<Drowing.Point>();
        private Drowing.Point temp;
        private List<neibours> neibor = new List<neibours>();

        struct neibours
        {
            public neibours(Drowing.Point pt, char sidet)
            {
                p = pt;
                side = sidet;
            }

            public Drowing.Point p;
            public char side;
        }

        public GenerationAI(Form1 form1, Drowing draw_copy)
        {
            this.draw_copy = draw_copy;
            this.form1 = form1;
        }

        public void init_mas()
        {
            draw_copy.init_drowing(2);
            for (int i = 0; i < draw_copy.NColumnX; i++)
            {
                for (int j = 0; j < draw_copy.NRowsY; j++)
                {
                    if (((i % 2) == 0) || ((j % 2) == 0)) draw_copy.Map[Drowing.Absol_Coord(j, i, draw_copy.NColumnX)] = true;
                }
            }
        }

        private void check_neibours(int x, int y, char side)
        {
            int _y=0, _x=0, _z=0;
            switch (side)
            {
                case 'U':
                    _y = y - 2;
                    if (_y < 0) return;
                    _x = x;
                    break;
                case 'D':
                    _y = y + 2;
                    if (_y >= draw_copy.NRowsY) return;
                    _x = x;
                    break;
                case 'L':
                    _x = x - 2;
                    if (_x < 0) return;
                    _y = y;
                    break;
                case 'R':
                    _x = x + 2;
                    if (_x >= draw_copy.NColumnX) return;
                    _y = y;
                    break;
            }
            _z = Drowing.Absol_Coord(_y, _x, draw_copy.NColumnX);
            if (draw_copy.Map[_z]) return;
            if (draw_copy.Processed_cells[_z] != 0) return;
            neibor.Add(new neibours(new Drowing.Point(_x,_y,_z), side));
        }
        
        public void create_Labyrithm()
        {
            int mode = form1.Flag_radioButtons;
            draw_copy.Step = 2;
            Random rand = new Random();
            temp = new Drowing.Point(1, 1, Drowing.Absol_Coord(1, 1, draw_copy.NColumnX));
            neibours nt;
            int _y = 0, _x = 0, _z = 0;
            draw_copy.Processed_cells[Drowing.Absol_Coord(1, 1, draw_copy.NColumnX)] = draw_copy.Step;
            chain_cells.Push(temp);
            draw_copy.Check_point = temp;
            form1.visualisation_mode(mode,form1, 1);
            
            while (chain_cells.Count != 0)
            {
                temp = chain_cells.Peek();
                draw_copy.Check_point = temp;
                neibor.Clear();
                check_neibours(temp.x, temp.y, 'U');
                check_neibours(temp.x, temp.y, 'D');
                check_neibours(temp.x, temp.y, 'L');
                check_neibours(temp.x, temp.y, 'R');
                if (neibor.Count == 0)
                {
                    chain_cells.Pop();
                 
                    form1.visualisation_mode(mode,form1);
                    continue;
                }
                nt = neibor[rand.Next(0, neibor.Count)];
                switch (nt.side)
                {
                    case 'U':
                        _y = temp.y - 1;
                        _x = temp.x;
                        break;
                    case 'D':
                        _y = temp.y + 1;
                        _x = temp.x;
                        break;
                    case 'L':
                        _y = temp.y;
                        _x = temp.x-1;
                        break;
                    case 'R':
                        _y = temp.y;
                        _x = temp.x+1;
                        break;
                }
                _z = Drowing.Absol_Coord(_y, _x, draw_copy.NColumnX);
                draw_copy.Map[_z] = false;
                draw_copy.Processed_cells[_z] = draw_copy.Step-1;
                draw_copy.Processed_cells[nt.p.z] = draw_copy.Step;
            
                form1.visualisation_mode(mode,form1, 1);
                chain_cells.Push(nt.p);
                draw_copy.Step++;

            }
            draw_copy.Processed_cells = new int[draw_copy.NColumnX * draw_copy.NRowsY];
            draw_copy.Check_point = new Drowing.Point(-1, -1, -1);
            draw_copy.Step = 1;
            
            form1.update_screen();
            form1.lock_buttons();
            form1.DrawPanel.Enabled = true;

        }
    }
}
