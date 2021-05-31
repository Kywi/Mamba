using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu;
using Emgu.CV.Structure;
using System.Drawing;

namespace PathFinding
{
    class Recognition_pict
    {
        //----------------------------------------------------Variables
        private Form1 form1;
        private Drowing draw_copy;
        //-------------------------------------------------------------

        public Recognition_pict(Form1 form1, Drowing draw_copy)
        {
            this.form1 = form1;
            this.draw_copy = draw_copy;
        }

        public void recogn_Picture(string fileName)
        {
            Image<Bgr, Byte> img = null; 
            img = new Image<Bgr, byte>(fileName);      
            //  img._SmoothGaussian(7);
            Image<Gray, Byte> gr_img = img.Convert<Gray, Byte>();//ковнвертування в сірий колір
            gr_img = gr_img.ThresholdBinary(new Gray(150), new Gray(255));
            int w_b, h_b;
            gr_img = gr_img.Resize(form1.DrawPanel.Width, form1.DrawPanel.Height, Emgu.CV.CvEnum.Inter.Linear, true);
            int yr = gr_img.Height / 10,
                xc = gr_img.Width / 10;// підгонка розмірів робочої області

            gr_img = gr_img.Resize(xc * 10, yr * 10, Emgu.CV.CvEnum.Inter.Linear);//маштабуємо розмір картинки
            form1.size_cell.Value = 10;
            form1.cColunm_X.Value = xc;
            form1.cRows_Y.Value = yr;
            form1.DrawPanel.Update();
            w_b = gr_img.Width / (int)form1.cColunm_X.Value;
            h_b = gr_img.Height / (int)form1.cRows_Y.Value;
         
            for (int i = 0; i < form1.cRows_Y.Value; i++)//розпізнавання лабіринту на картинці
            {
                int ki = (i == 0) ? 0 : 1;
                for (int j = 0; j < form1.cColunm_X.Value; j++)
                {
                    int kj = (j == 0) ? 0 : 1;
                    gr_img.ROI = new Rectangle(j * w_b + kj, i * h_b + ki, (int)w_b, (int)h_b);
                    double avr_brit = gr_img.GetAverage().MCvScalar.V0;//отримання середньої якрості області
                    form1.Draw.Map[Drowing.Absol_Coord(i, j, form1.Draw.NColumnX)] = (avr_brit < 240);
                }
            }
            form1.update_screen();        
        }
    }
}
