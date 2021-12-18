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
    /// ReHoleUc.xaml 的交互逻辑
    /// </summary>
    public partial class ReHoleUc : UserControl
    {
        public ReHoleUc()
        {
            InitializeComponent();
            this.Loaded += ReHoleUc_Loaded;
        }

        private void ReHoleUc_Loaded(object sender, RoutedEventArgs e)
        {
            _uniformGrid.Columns = _columnUpDown.Value.Value;
            _uniformGrid.Rows = _rowUpDown.Value.Value;
            _reCells();
            _columnUpDown.ValueChanged += _columnUpDown_ValueChanged;
            _rowUpDown.ValueChanged += _rowUpDown_ValueChanged;
        }

        private void _columnUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            _uniformGrid.Columns = _columnUpDown.Value.Value;
            _reCells();
        }

        private void _rowUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            _uniformGrid.Rows = _rowUpDown.Value.Value;
            _reCells();
        }

        void _reCells()
        {
            int aimCellsNum = _uniformGrid.Columns * _uniformGrid.Rows;
            if (aimCellsNum < _uniformGrid.Children.Count)
            {
                while (_uniformGrid.Children.Count > aimCellsNum)
                {
                    _uniformGrid.Children.RemoveAt(0);
                }
            }
            else if (aimCellsNum > _uniformGrid.Children.Count)
            {
                for (int i = _uniformGrid.Children.Count; i < aimCellsNum; i++)
                {
                    Rectangle rec = _createRectange();
                    _uniformGrid.Children.Add(rec);
                }
            }
        }

        Rectangle _createRectange()
        {
            return new Rectangle()
            {
                Stroke = Brushes.Transparent,
                Fill = Brushes.DarkBlue,
                Margin = new Thickness(1d)
            };
        }

        public ushort Columns
        {
            get { return (ushort)GetValue(ColumnsProperty); }
            set { SetValue(ColumnsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Columns.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColumnsProperty =
            DependencyProperty.Register("Columns", typeof(ushort), typeof(ReHoleUc), new FrameworkPropertyMetadata((ushort)2));




        public ushort Rows
        {
            get { return (ushort)GetValue(RowsProperty); }
            set { SetValue(RowsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Rows.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RowsProperty =
            DependencyProperty.Register("Rows", typeof(ushort), typeof(ReHoleUc), new FrameworkPropertyMetadata((ushort)1));



    }
}
