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
        const int _cellWidth = 75;
        const int _cellHeight = 30;

        char[] _rowCtrTitles = new char[] 
        { 
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W' , 'X', 'Y', 'Z'
        };

        public PlateListUc()
        {
            InitializeComponent();

            _rowCtrItemsControl.ItemsSource = _rowCtrCells;
            _columnCtrItemsControl.ItemsSource = _columnCtrCells;
            
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

        ObservableCollection<PlateCtrCell> _rowCtrCells = new ObservableCollection<PlateCtrCell>();
        public ObservableCollection<PlateCtrCell> RowCtrCells
        {
            get { return _rowCtrCells; }
        }

        void _resetRowCtrCells()
        {
            int rows = Rows;
            if (_rowCtrCells.Count > rows)
            {
                while (_rowCtrCells.Count > rows)
                {
                    PlateCtrCell rowCtrCell= _rowCtrCells[rows];
                    rowCtrCell.Placing -= _rowCtrCell_Placing;
                    _rowCtrCells.Remove(rowCtrCell);
                }
            }
            else if (_rowCtrCells.Count < rows)
            {
                int i = _rowCtrCells.Count;
                while (i < rows)
                {
                    PlateCtrCell rowCtrCell = new PlateCtrCell(_rowCtrTitles[i].ToString(), i + 1);
                    rowCtrCell.Placing += _rowCtrCell_Placing;
                    i++;
                }
            }
            else { }
        }

        private void _rowCtrCell_Placing(object sender, EventArgs e)
        {
            
        }

        ObservableCollection<PlateCtrCell> _columnCtrCells = new ObservableCollection<PlateCtrCell>();
        public ObservableCollection<PlateCtrCell> ColumnCtrCells
        {
            get { return _columnCtrCells; }
        }

        void _resetColumnCtrCells()
        {

            int columns = Columns;
            if (_columnCtrCells.Count > columns)
            {
                while (_columnCtrCells.Count > columns)
                {
                    PlateCtrCell columnCtrCell = _columnCtrCells[columns];
                    columnCtrCell.Placing -= _columnCtrCell_Placing;
                    _columnCtrCells.Remove(columnCtrCell);
                }
            }
            else if (_columnCtrCells.Count < columns)
            {
                int i = _columnCtrCells.Count;
                while (i < columns)
                {
                    int num = i + 1;
                    PlateCtrCell columnCtrCell = new PlateCtrCell(num.ToString(), num);
                    columnCtrCell.Placing += _columnCtrCell_Placing;
                    i++;
                }
            }
            else { }
        }

        private void _columnCtrCell_Placing(object sender, EventArgs e)
        {

        }

        #endregion


        #region 计算板区域大小

        void _calculateWidth()
        {
            this._zoomGrid.Width = Columns * _cellWidth * this._slider.Value+30;
        }

        void _calculateHeight()
        {
            this._zoomGrid.Height = Rows * _cellHeight * this._slider.Value+30;
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