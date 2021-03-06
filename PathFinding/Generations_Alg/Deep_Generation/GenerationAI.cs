﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PathFinding
{
    class GenerationAI : Generation,  Generation_Interface
    {
        //-----------------------------------------Variables
        private Stack<Ppoint> chain_cells = new Stack<Ppoint>();
        private Ppoint temp;
        private List<neibours> neibor = new List<neibours>();

        struct neibours
        {
            public neibours(Ppoint pt, char sidet)
            {
                p = pt;
                side = sidet;
            }
            public Ppoint p;
            public char side;
        }

        //------------------------------------------------

        public GenerationAI(Form1 form1, Drowing draw_copy) : base(form1, draw_copy) { }
     

        protected override void init_mas()
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

        //пеервірка на чи можна прорубувати отвір в 4-ох напрямках від поточної точки
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
            neibor.Add(new neibours(new Ppoint(_x,_y,_z), side));
        }
        
        public override void create_Labyrithm()
        {
            init_mas();   
            draw_copy.Step = 2;
            Random rand = new Random();
            temp = new Ppoint(1, 1, Drowing.Absol_Coord(1, 1, draw_copy.NColumnX));
            neibours nt;
            int _y = 0, _x = 0, _z = 0;
            draw_copy.Processed_cells[Drowing.Absol_Coord(1, 1, draw_copy.NColumnX)] = draw_copy.Step;
            chain_cells.Push(temp);
            draw_copy.Check_point = temp;
            form1.visualisation_mode(mode,form1, 1);
            
            while (chain_cells.Count != 0)//початок генерації
            {
                Mode = form1.Flag_radioButtons;
                temp = chain_cells.Peek();
                draw_copy.Check_point = temp;
                neibor.Clear();
                foreach (char tem in Mas_OF_sides)
                {
                    check_neibours(temp.x, temp.y, tem);
                }
                if (neibor.Count == 0)
                {
                    chain_cells.Pop();         
                    form1.visualisation_mode(mode,form1);
                    continue;
                }
                nt = neibor[rand.Next(0, neibor.Count)];//обриаємо випадкову сторону сере доступних
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
                draw_copy.Map[_z] = false;//прорубуємо стінку
                draw_copy.Processed_cells[_z] = draw_copy.Step-1;
                draw_copy.Processed_cells[nt.p.z] = draw_copy.Step;
            
                form1.visualisation_mode(mode,form1, 1);
                chain_cells.Push(nt.p);
                draw_copy.Step++;

            }
            Reset_values();
        }
    }
}
