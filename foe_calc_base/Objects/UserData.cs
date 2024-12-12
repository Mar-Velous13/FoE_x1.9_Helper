using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace foe_calc_base.Objects
{
    /* Object holding values of user preferences */
    public class UserData
    {
        int positions, shortForm;
        string lastGB, prefix;
        char[] positionValues;
        bool showGuide = false;

        //constructor
        public UserData(string pre, int shortF, int pos, string gb, int guide)
        {
            this.Positions = pos;//e.g. 11111 - each digit corresponds to certain position (P5,P4,P3,P2,P1)
            this.DisplayShort = shortF;
            this.Prex = pre;
            this.Last_GB = gb;
            this.DisplayGuide = guide;
        }

        /* Getters and Setters */
        public int Positions
        {
            get { return positions; }
            set
            {
                positions = value;
                positionValues = positions.ToString().ToCharArray();
            }
        }

        public char GetPosition(int pos)
        {
            return positionValues[pos];
        }


        public void SetSinglePosition(int pos)
        {
            positionValues[pos] = positionValues[pos].Equals('1') ? '2' : '1';
            positions = Int32.Parse(new string(positionValues));
        }

        public int DisplayShort
        {
            get { return shortForm; }
            set { shortForm = value; }
        }

        public string Last_GB
        {
            get { return lastGB; }
            set { lastGB = value; }
        }

        public string Prex
        {
            get { return prefix; }
            set { prefix = value; }
        }

        public int DisplayGuide
        {
            get { return showGuide ? 1 : 0; }
            set { showGuide = value == 1 ? true : false; }
        }
    }
}
