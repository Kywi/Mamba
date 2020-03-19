using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFinding
{
    public partial class Labiritnm : Component
    {
        public Labiritnm()
        {
            InitializeComponent();
        }

        public Labiritnm(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
    }
}
