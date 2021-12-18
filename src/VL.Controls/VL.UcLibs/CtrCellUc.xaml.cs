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

namespace VL.UcLibs
{
    /// <summary>
    /// CtrCellUc.xaml 的交互逻辑
    /// </summary>
    public partial class CtrCellUc : UserControl
    {
        public CtrCellUc(string title,int num)
        {
            InitializeComponent();
            _num = num;
            _title = title;
            _btn.Content = _title;
        }


        private int _num;
        public int Num
        {
            get { return _num; }
        }



        string _title;
        public string Title
        {
            get { return _title; }
        }



    }
}
