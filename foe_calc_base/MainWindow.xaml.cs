using foe_calc_base.Database;
using foe_calc_base.Forms;
using foe_calc_base.Model;
using foe_calc_base.Objects;
using foe_calc_base.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace foe_calc_base
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //objects
        DBManager db;
        UserData ud;
        GB temp_gb;
        GB_VM gb_vm;
        GBLevel_VM gbl_vm;
        Guide guide;

        //variables
        int ListItemSelected = 0;
        bool initConcluded = false;
        string[] outputString = new string[] { "", "", " P5", " P4", " P3", " P2", " P1" };
        string tempString;

        /// <summary>
        /// CONSTRUCTOR Initialize objects (including database), load and set gui elements
        /// </summary>
        public MainWindow()
        {
            db = new DBManager();
            ud = db.GetUserData();
            InitializeComponent();

            PrefixBox.Text = ud.Prex;
            outputString[0] = ud.Prex;
            LoadList();
            ListItemSelected = LastSelectedGB();//set lists' selected item (ud.lastGB)
            LoadTable();
            SetCheckBoxes();
            this.initConcluded = true;//used to prevent triggering events before everything has been loaded correctly
            UpdateOutputString();
            DisplayGuide(false);
        }


        /*return index of lastGB, used to assign value to gbList.selectedIndex*/
        int LastSelectedGB()
        {
            foreach (GB gb in gb_vm.GBS)
            {
                if (ud.Last_GB.Equals(gb.ShortName)) return gb_vm.GBS.IndexOf(gb);
            }
            return -1;
        }


        /* Upon application start look for user settings regarding the checkbox states and checks/unchecks them*/
        void SetCheckBoxes()
        {
            CheckShort.IsChecked = ud.DisplayShort == 1;
            outputString[1] = ud.DisplayShort == 1 ? FindLastGB(ud.Last_GB).ShortName : FindLastGB(ud.Last_GB).Name;
            CheckP5.IsChecked = ud.GetPosition(0).Equals('1'); outputString[2] = ud.GetPosition(0).Equals('1') ? " P5()" : "";
            CheckP4.IsChecked = ud.GetPosition(1).Equals('1'); outputString[3] = ud.GetPosition(1).Equals('1') ? " P4()" : "";
            CheckP3.IsChecked = ud.GetPosition(2).Equals('1'); outputString[4] = ud.GetPosition(2).Equals('1') ? " P3()" : "";
            CheckP2.IsChecked = ud.GetPosition(3).Equals('1'); outputString[5] = ud.GetPosition(3).Equals('1') ? " P2()" : "";
            CheckP1.IsChecked = ud.GetPosition(4).Equals('1'); outputString[6] = ud.GetPosition(4).Equals('1') ? " P1()" : "";
        }

        void UpdateOutputString()
        {/* Updates output string on gui, seen in field at left side panel below prefix */
            OutputBox.Text = string.Format("{0} {1}{2}{3}{4}{5}{6}",
                outputString[0],
                outputString[1],
                outputString[2],
                outputString[3],
                outputString[4],
                outputString[5],
                outputString[6]);
        }

        GB FindLastGB(string lastGB)
        {/* Used to access various fields on last used GB (age, shortname, etc)*/
            foreach (GB gb in gb_vm.GBS)
            {
                if (gb.ShortName.Equals(lastGB)) return gb;
            }
            return null;
        }

        void LoadList()//Load list of great buildings (side panel)
        {
            gb_vm = new GB_VM();
            gb_vm.LoadGreatBuildings(db);

            ListItemSelected = LastSelectedGB();
            gb_list.ItemsSource = gb_vm.GBS;
            gb_list.Items.Refresh();
        }

        void LoadTable()//load leveling data on great building (table)
        {
            gbl_vm = new GBLevel_VM();
            gbl_vm.LoadLevels(db, FindLastGB(ud.Last_GB).Age);

            tableData.ItemsSource = gbl_vm.GBLevels;
            tableData.Items.Refresh();
        }


        /* Hide or display guide for user*/
        void DisplayGuide(bool requested)
        {
            guide = new Guide();
            if (ud.DisplayGuide == 1)
            {
                ud.DisplayGuide = 0;
                db.WriteUserData(5, ud);
                guide.showGuide();
            }
            else if (requested == true) guide.showGuide();
        }

        /* ========================================== CLICK and etc. EVENTS =====================================*/

        private void List_Loaded(object sender, RoutedEventArgs e)
        {//convert from ud.last_gb to int to assign correct selected building
            gb_list.SelectedIndex = LastSelectedGB();
        }

        private void ListSelectionChanged(object sender, RoutedEventArgs e)
        {/* when list items are changed */

            if (!initConcluded) return;// restrict unneccessary DB writing on component init
            temp_gb = (GB)gb_list.SelectedItem;
            gb_img.Source = new BitmapImage(new Uri("/foe_calc_base;component/Resources/images/" + temp_gb.Image, UriKind.Relative));
            ud.Last_GB = temp_gb.ShortName;
            db.WriteUserData(3, ud);
            outputString[1] = (CheckShort.IsChecked == true) ? ud.Last_GB : FindLastGB(ud.Last_GB).Name;


            //GB has changed so we need to load correct leveling data for it
            gbl_vm.ReloadLevels(db, temp_gb.Age);
            tableData.Items.Refresh();

            this.UpdateOutputString();
        }

        private void Prefix_TextChanged(object sender, TextChangedEventArgs e)
        {/* Save new users username/prefix, won't have to type it every time the application is opened */
            if (!initConcluded) { outputString[0] = ud.Prex; return; }// restrict unneccessary DB writing on component init
            ud.Prex = PrefixBox.Text;
            db.WriteUserData(0, ud);//update database
            outputString[0] = ud.Prex;
            this.UpdateOutputString();//change output string
        }


        /* Updating info when user clicks on any of the checkboxes*/
        private void Check_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.CheckBox cb = (System.Windows.Controls.CheckBox)sender;
            switch (cb.Name)
            {
                case "CheckP1":
                    ud.SetSinglePosition(4);
                    db.WriteUserData(4, ud);
                    outputString[6] = ud.GetPosition(4).Equals('1') ? " P1()" : "";
                    break;
                case "CheckP2":
                    ud.SetSinglePosition(3);
                    db.WriteUserData(4, ud);
                    outputString[5] = ud.GetPosition(3).Equals('1') ? " P2()" : "";
                    break;
                case "CheckP3":
                    ud.SetSinglePosition(2);
                    db.WriteUserData(4, ud);
                    outputString[4] = ud.GetPosition(2).Equals('1') ? " P3()" : "";
                    break;
                case "CheckP4":
                    ud.SetSinglePosition(1);
                    db.WriteUserData(4, ud);
                    outputString[3] = ud.GetPosition(1).Equals('1') ? " P4()" : "";
                    break;
                case "CheckP5":
                    ud.SetSinglePosition(0);
                    db.WriteUserData(4, ud);
                    outputString[2] = ud.GetPosition(0).Equals('1') ? " P5()" : "";
                    break;

                case "CheckShort":
                    ud.DisplayShort = CheckShort.IsChecked == false ? 0 : 1;
                    db.WriteUserData(1, ud);
                    foreach (GB gb in gb_vm.GBS)
                    {
                        if (gb.ShortName.Equals(ud.Last_GB))
                        {
                            outputString[1] = ud.DisplayShort == 1 ? gb.ShortName : gb.Name;
                            break;
                        }
                    }
                    break;
            }
            UpdateOutputString();
        }


        private void TableData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {//Event triggered when user CLICKS ON ROW a.k.a. string generation
            GBLevel gbl = (GBLevel)tableData.SelectedItem;
            if (gbl == null) return;//was triggered when changing great building from the list, caused error

            //generate string to copy to clipboard
            tempString = string.Format("{0} {1}{2}{3}{4}{5}{6}", outputString[0], outputString[1],
                CheckP5.IsChecked == true ? " P5(" + gbl.Pos5 + ")" : "",
                CheckP4.IsChecked == true ? " P4(" + gbl.Pos4 + ")" : "",
                CheckP3.IsChecked == true ? " P3(" + gbl.Pos3 + ")" : "",
                CheckP2.IsChecked == true ? " P2(" + gbl.Pos2 + ")" : "",
                CheckP1.IsChecked == true ? " P1(" + gbl.Pos1 + ")" : "");

            System.Windows.Forms.Clipboard.SetText(tempString);
            db.Update_GB_Level(ud.Last_GB, gbl.Level);

            //show notification
            new Notification("String has been generated for level " + gbl.Level + "! Now use Ctrl+V");
        }

        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            string message = "DISCLAIMER. The materials used in this application are purely for informational purposes. I do not claim ownership of the media and data used, and no copyright infringement is intended. All rights go to their respective owners."
                + Environment.NewLine + Environment.NewLine + "This application was made by Mar. Contact me at:" + Environment.NewLine + "> Mar13 on Noarsil & Odhrorvar" + Environment.NewLine + "> E-mail: vict100@mail.com";
            string title = "Application Info!";
            MessageBoxButton buttons = MessageBoxButton.OK;
            System.Windows.MessageBox.Show(message, title, buttons, MessageBoxImage.Information);
        }

        private void QuestionButton_Click(object sender, RoutedEventArgs e)
        {
            DisplayGuide(true);
        }


    }
}
