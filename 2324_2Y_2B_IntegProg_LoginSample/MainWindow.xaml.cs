using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _2324_2Y_2B_IntegProg_LoginSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DataClassDataContext _dbConn = null;
        bool flag = false;

        public MainWindow()
        {
            InitializeComponent();
            _dbConn = new DataClassDataContext(
                Properties.Settings.Default._2324_2B_LoginSampleConnectionString);
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if(txtUser.Text.Length > 0 && txtPass.Text.Length > 0)
            {
                flag = false;
                IQueryable<tblLogin> selectResults = from s in _dbConn.tblLogins
                                    where s.LoginID == txtUser.Text
                                    select s;

                if(selectResults.Count() == 1)
                {
                    //MessageBox.Show("Username exists");
                    foreach(tblLogin s in selectResults) 
                    { 
                        if(s.LoginPassword == txtPass.Text) 
                        {
                            string messageString = $"Login complete.";
                            if (s.LoginDate == null)
                                messageString += $" Welcome {s.LoginName}!";
                            else
                                messageString += $" Welcome back {s.LoginName}! Havent seen you since {s.LoginDate}";

                            MessageBox.Show(messageString);
                            s.LoginDate = DateTime.Now;

                            tblLog tlog = new tblLog();
                            tlog.LoginID = s.LoginID;
                            tlog.LogDate = (DateTime)s.LoginDate;

                            _dbConn.tblLogs.InsertOnSubmit(tlog);
                            flag = true;
                            break;
                        }
                    }
                    _dbConn.SubmitChanges();
                }
            }

            if(flag)
            {
                Window1 w = new Window1();
                w.Show();
                this.Close();
            }

        }
    }
}
