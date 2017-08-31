using System;
using System.Collections.Generic;
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

namespace sqlQuery
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //LibSQLConnection.AddNewTransaction(12.5, Categories.Kosmetyki.CatGuid, Users.Andrzej.UGuid, "test from app");
            //var a = LibSQLConnection.GetTransactionSum(DateTime.Today);
            //var b = LibSQLConnection.GetDailyLimit();
            //var c = LibSQLConnection.GetAdditionalPool();
            //LibSQLConnection.AddNewTransactionWithDate(28, Categories.Kosmetyki.CatGuid, Users.Andrzej.UGuid, DateTime.Today.AddDays(-8), "28.08");
            //var d = LibSQLConnection.GetLastWeekTransactionsSum();
            //var e = LibSQLConnection.HasSeenMessage(new Guid(Users.Andrzej.UGuid));
            //var f = LibSQLConnection.GetMessageData();
            //var g = LibSQLConnection.GetTransactionsHistory();
            //LibSQLConnection.ModifyAmountToSpend(780.5);
        }
    }
}
