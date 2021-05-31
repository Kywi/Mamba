using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFinding
{
    abstract class PathFind : PathFind_Int
    {
        protected Form1 form1;
        protected Drowing draw_copy;
        protected List<string> Mas_OF_sides;
        protected int mode;

        public int Mode { get => mode; set => mode = value; }

        public PathFind(Form1 form1, Drowing draw_copy)
        {
            this.draw_copy = draw_copy;
            this.form1 = form1;

            Mas_OF_sides = new List<string>();
            Mas_OF_sides.Add("U");
            Mas_OF_sides.Add("D");
            Mas_OF_sides.Add("L");
            Mas_OF_sides.Add("R");
            Mas_OF_sides.Add("LU");
            Mas_OF_sides.Add("LD");
            Mas_OF_sides.Add("RU");
            Mas_OF_sides.Add("RD");
        }

        //Перевірка сторін(чи можна йти в ту чи іншу сторону)
        public abstract void Path_Finding();

        //Перший шаг в якому накидуєтся початкові значення
        protected abstract void First_Step();

        //Вертає значення в початковий стан пілся роботи з ними
        protected void Reset_Values()
        {
            draw_copy.Processed_cells = new int[draw_copy.NColumnX * draw_copy.NRowsY];
            draw_copy.Closed_cell = new int[draw_copy.NColumnX * draw_copy.NRowsY];
            draw_copy.Check_point = new Ppoint(-1, -1, -1);
            draw_copy.Step = 1;
            form1.lock_buttons();
            form1.DrawPanel.Enabled = false;
        }
    }
}
