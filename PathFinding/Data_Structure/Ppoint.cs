using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFinding
{
    class Ppoint//використовуєтсья для збережання точок
    {
        public int x, y, z;

        public Ppoint(int tx, int ty, int tz)
        {
            x = tx;
            y = ty;
            z = tz;
        }

        public Algorithm_Wave.min_way min_way
        {
            get => default(Algorithm_Wave.min_way);
            set
            {
            }
        }

        public static bool operator ==(Ppoint c1, Ppoint c2)
        {
            return c1.z == c2.z;
        }

        public static bool operator !=(Ppoint c1, Ppoint c2)
        {
            return c1.z != c2.z;
        }
    }
}
