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
        public CtrCellUc()
        {
            InitializeComponent();
        }




        public int Num
        {
            get { return (int)GetValue(NumProperty); }
            set { SetValue(NumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Num.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NumProperty =
            DependencyProperty.Register("Num", typeof(int), typeof(CtrCellUc), new PropertyMetadata(0));





        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(CtrCellUc), new PropertyMetadata(null));



    }
}
