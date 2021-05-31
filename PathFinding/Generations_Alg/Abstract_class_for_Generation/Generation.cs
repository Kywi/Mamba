using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFinding
{
    abstract class Generation
    {
        protected Form1 form1;
        protected Drowing draw_copy;
        protected List<char> Mas_OF_sides;
        protected int mode;

        public int Mode { get => mode; set => mode = value; }

        public Generation(Form1 form1, Drowing draw_copy)
        {
            this.draw_copy = draw_copy;
            this.form1 = form1;

            Mas_OF_sides = new List<char>();
            Mas_OF_sides.Add('U');
            Mas_OF_sides.Add('D');
            Mas_OF_sides.Add('L');
            Mas_OF_sides.Add('R');
        }

        //початкове заповення лабіринту(Деякі алгоритми потребоють початкових значень)
        abstract protected void init_mas();

        //створення лабіринту
        abstract public void create_Labyrithm();

        //Повернення значень до початковго значення
        protected void Reset_values()
        {
            draw_copy.Processed_cells = new int[draw_copy.NColumnX * draw_copy.NRowsY];
            draw_copy.Check_point = new Ppoint(-1, -1, -1);
            draw_copy.Step = 1;

            form1.update_screen();
            form1.lock_buttons();
            form1.DrawPanel.Enabled = true;
        }
    }
}
