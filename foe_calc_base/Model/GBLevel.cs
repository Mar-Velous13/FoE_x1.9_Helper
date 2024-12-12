using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace foe_calc_base.Model
{
    public class GBLevel
    {
        private int lvl, total;
        private int[] positions = new int[5];

        public GBLevel(int level, int fp_total, int[] fp_positions)
        {
            this.Level = level;
            this.Total = fp_total;
            this.Positions = fp_positions;
        }

        /* ======================== GETTERS & SETTERS =================*/
        public int Level
        {
            get { return lvl; }
            set { lvl = value; }
        }

        public int Total
        {
            get { return total; }
            set
            {
                total = value;
            }
        }

        public int[] Positions
        {
            get { return positions; }
            set {  positions = value;}
        }

        public int Pos1 { get { return positions[0]; } }
        public int Pos2 { get { return positions[1]; } }
        public int Pos3 { get { return positions[2]; } }
        public int Pos4 { get { return positions[3]; } }
        public int Pos5 { get { return positions[4]; } }

        public void SetPosition(int id, int value)
        {
            positions[id] = value;
        }



    }
}
