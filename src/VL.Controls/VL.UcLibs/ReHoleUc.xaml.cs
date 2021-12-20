using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            //this.IsEnabledChanged += ReHoleUc_IsEnabledChanged;
            this.Loaded += ReHoleUc_Loaded;
        }

        private void ReHoleUc_Loaded(object sender, RoutedEventArgs e)
        {
            //_uniformGrid.Columns = _columnUpDown.Value.Value;
            //_uniformGrid.Rows = _rowUpDown.Value.Value;
            _reCells();
            //_columnUpDown.ValueChanged += _columnUpDown_ValueChanged;
            //_rowUpDown.ValueChanged += _rowUpDown_ValueChanged;
        }

        //private void _columnUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        //{
        //    _uniformGrid.Columns = _columnUpDown.Value.Value;
        //    _reCells();
        //}

        //private void _rowUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        //{
        //    _uniformGrid.Rows = _rowUpDown.Value.Value;
        //    _reCells();
        //}

        void _reCells()
        {
            int aimCellsNum = Columns * Rows;
            if (aimCellsNum < _cells.Count)
            {
                while (_cells.Count > aimCellsNum)
                {
                    _cells.RemoveAt(0);
                }
            }
            else if (aimCellsNum > _cells.Count)
            {
                for (int i = _cells.Count; i < aimCellsNum; i++)
                {
                    _cells.Add(new object());
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

        #region 单元格、单元格刷

        ObservableCollection<Object> _cells = new ObservableCollection<object>();
        public ObservableCollection<Object> Cells
        {
            get { return _cells; }
        }


        public Brush CellBrush
        {
            get { return (Brush)GetValue(CellBrushProperty); }
            set { SetValue(CellBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CellBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CellBrushProperty =
            DependencyProperty.Register("CellBrush", typeof(Brush), typeof(ReHoleUc), new FrameworkPropertyMetadata(new SolidColorBrush(Colors.LightBlue)));

        #endregion


        #region Rows、Columns

        public int Columns
        {
            get { return (int)GetValue(ColumnsProperty); }
            set { SetValue(ColumnsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Columns.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColumnsProperty =
            DependencyProperty.Register("Columns", typeof(int), typeof(ReHoleUc), new FrameworkPropertyMetadata(2,FrameworkPropertyMetadataOptions.None,_onColumnsChangedCallback, _coerceGreaterOne));


        static void _onColumnsChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ReHoleUc uc = (ReHoleUc)d;
            uc._reCells();
        }

        public int Rows
        {
            get { return (int)GetValue(RowsProperty); }
            set { SetValue(RowsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Rows.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RowsProperty =
            DependencyProperty.Register("Rows", typeof(int), typeof(ReHoleUc), new FrameworkPropertyMetadata(1, FrameworkPropertyMetadataOptions.None, _onRowsChangedCallback,_coerceGreaterOne));

        static void _onRowsChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ReHoleUc uc = (ReHoleUc)d;
            uc._reCells();
        }


        /// <summary>
        /// 强制设置的行、列值大于等于1
        /// </summary>
        /// <param name="d"></param>
        /// <param name="baseValue"></param>
        /// <returns></returns>
        static object _coerceGreaterOne(DependencyObject d, object baseValue)
        {
            int value = (int)baseValue;
            if (value <= 0)
            {
                value = 1;
            }
            return value;
        }

        #endregion

    }
}
