using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class PlateListUc : UserControl
    {
        const int _cellWidth = 60;
        const int _cellHeight = 30;
        const int _margin = 1;

        char[] _rowCtrTitles = new char[] 
        { 
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W' , 'X', 'Y', 'Z'
        };

        public PlateListUc()
        {
            InitializeComponent();

            _columnCtrUniformGrid.Columns = Columns;
            _columnCtrCells = _columnCtrUniformGrid.Children;

            _rowCtrUniformGrid.Rows = Rows;
            _rowCtrCells = _rowCtrUniformGrid.Children;

            _dataUniformGrid.Columns = Columns;
            _dataUniformGrid.Rows = Rows;
            _dataCells = _dataUniformGrid.Children;

            
            _calculateHeight();
            _calculateWidth();
            _resetRowCtrCells();
            _resetColumnCtrCells();
            _slider.ValueChanged += _slider_ValueChanged;
        }

        private void _plateListUc_Loaded(object sender, RoutedEventArgs e)
        {
        }


        #region Row&Column Control

        UIElementCollection _rowCtrCells;

        void _resetRowCtrCells()
        {
            int rows = Rows;
            if (_rowCtrCells.Count > rows)
            {
                while (_rowCtrCells.Count > rows)
                {
                    CtrCellUc rowCtrCell =(CtrCellUc)_rowCtrCells[rows];
                    _rowCtrCells.Remove(rowCtrCell);
                }
            }
            else if (_rowCtrCells.Count < rows)
            {
                int i = _rowCtrCells.Count;
                while (i < rows)
                {
                    CtrCellUc rowCtrCell = new CtrCellUc() { Title = _rowCtrTitles[i].ToString(), Num = i + 1 };
                    _rowCtrCells.Add(rowCtrCell);
                    i++;
                }
            }
            else { }
        }

        private void _rowCtrCell_Placing(object sender, EventArgs e)
        {
            
        }

        UIElementCollection _columnCtrCells;

        void _resetColumnCtrCells()
        {
            int columns = Columns;
            if (_columnCtrCells.Count > columns)
            {
                while (_columnCtrCells.Count > columns)
                {
                    CtrCellUc columnCtrCell = (CtrCellUc)_columnCtrCells[columns];
                    _columnCtrCells.Remove(columnCtrCell);
                }
            }
            else if (_columnCtrCells.Count < columns)
            {
                int i = _columnCtrCells.Count;
                while (i < columns)
                {
                    int num = i + 1;
                    CtrCellUc columnCtrCell = new CtrCellUc() { Title=num.ToString(),Num= num };
                    _columnCtrCells.Add(columnCtrCell);
                    i++;
                }
            }
            else { }
        }

        private void _columnCtrCell_Placing(object sender, EventArgs e)
        {

        }

        #endregion


        #region Data


        UIElementCollection _dataCells;

        #endregion


        #region 计算板区域大小

        void _calculateWidth()
        {
            this._zoomGrid.Width = Columns * _cellWidth * this._slider.Value;
        }

        void _calculateHeight()
        {
            this._zoomGrid.Height = Rows * _cellHeight * this._slider.Value;
        }

        #endregion


        #region 依赖项属性

        public int Columns
        {
            get { return (int)GetValue(ColumnsProperty); }
            set { SetValue(ColumnsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Columns.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColumnsProperty =
            DependencyProperty.Register("Columns", typeof(int), typeof(PlateListUc), new FrameworkPropertyMetadata(12,_onColumnsChangedCallback));

        static void _onColumnsChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PlateListUc uc = (PlateListUc)d;
            uc._calculateWidth();
        }

        public int Rows
        {
            get { return (int)GetValue(RowsProperty); }
            set { SetValue(RowsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Rows.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RowsProperty =
            DependencyProperty.Register("Rows", typeof(int), typeof(PlateListUc), new FrameworkPropertyMetadata(8,_onRowsChangedCallback));

        static void _onRowsChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PlateListUc uc = (PlateListUc)d;
            uc._calculateHeight();
        }

        #endregion


        #region 事件

        //public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(PlateListUc));
        ///// <summary>
        ///// Add / Remove ClickEvent handler
        ///// </summary>
        //[Category("Behavior")]
        //public event RoutedEventHandler Click { add { AddHandler(ClickEvent, value); } remove { RemoveHandler(ClickEvent, value); } }

        #endregion


        private void _slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _calculateWidth();
            _calculateHeight();
        }
    }
}