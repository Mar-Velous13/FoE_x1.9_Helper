using foe_calc_base.Database;
using foe_calc_base.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace foe_calc_base.ViewModel
{
    public class GB_VM : INotifyPropertyChanged
    {
        private ObservableCollection<GB> _gbs { get; set; }
        public ObservableCollection<GB> GBS
        {
            get { return _gbs; }
            set { _gbs = value; RaisePropertyChanged(nameof(GBS)); }
        }

        public void LoadGreatBuildings(DBManager db)
        {
            //ObservableCollection<GBLevel> gb_lvls = new ObservableCollection<GBLevel>();
            GBS = new ObservableCollection<GB>(db.ReadGBs());
            //Console.WriteLine(string.Format("[GBS-VM-Loaded] size:{0}", GBS.Count));
            //Console.WriteLine(string.Format("[GBS-VM-Loaded] size:{0}", GBS.First().Name));

        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }


    }
}
