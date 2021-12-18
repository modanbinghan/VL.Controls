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
            this.Loaded += _plateListUc_Loaded;
        }

        void _initialize()
        {
            _columnCtrCells = _columnCtrUniformGrid.Children;
            _rowCtrCells = _rowCtrUniformGrid.Children;
            _cells = _dataUniformGrid.Children;
            _onMeshChanged();
        }

        private void _plateListUc_Loaded(object sender, RoutedEventArgs e)
        {
            _initialize();
            _scrollViewer.SizeChanged += _scrollViewer_SizeChanged;
            _slider.ValueChanged += _slider_ValueChanged;
            _resetSizeBtn.Click += _resetSizeBtn_Click;
        }

        private void _resetSizeBtn_Click(object sender, RoutedEventArgs e)
        {
            _slider.Value = 1;
        }

        private void _scrollViewer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _zoomSize();
        }


        #region Row Control

        UIElementCollection _rowCtrCells;

        void _removeCtrCellRows(int newRows)
        {
            while (_rowCtrCells.Count > newRows)
            {
                CtrCellUc rowCtrCell = (CtrCellUc)_rowCtrCells[newRows];
                _rowCtrCells.Remove(rowCtrCell);
            }
        }

        void _addCtrCellRows(int newRows)
        {
            int i = _rowCtrCells.Count;
            while (i < newRows)
            {
                CtrCellUc rowCtrCell = new CtrCellUc(_rowCtrTitles[i].ToString(), i + 1);
                _rowCtrCells.Add(rowCtrCell);
                i++;
            }
        }

        private void _rowCtrCell_Placing(object sender, EventArgs e)
        {
            
        }


        #endregion


        #region Column Control

        UIElementCollection _columnCtrCells;


        void _removeCtrCellColumns(int newColumns)
        {
            while (_columnCtrCells.Count > newColumns)
            {
                CtrCellUc columnCtrCell = (CtrCellUc)_columnCtrCells[newColumns];
                _columnCtrCells.Remove(columnCtrCell);
            }
        }

        void _addCtrCellColumns(int newColumns)
        {
            int i = _columnCtrCells.Count;
            while (i < newColumns)
            {
                int num = i + 1;
                CtrCellUc columnCtrCell = new CtrCellUc(num.ToString(), num);
                _columnCtrCells.Add(columnCtrCell);
                i++;
            }
        }

        private void _columnCtrCell_Placing(object sender, EventArgs e)
        {

        }

        #endregion


        #region Data

        UIElementCollection _cells;

        void _removeCellRows(int newRows)
        {
            int i = 0;
            while (i < _cells.Count)
            {
                CellUc cell = (CellUc)_cells[i];
                if (cell.Row > newRows)
                {
                    _disposeCellUc(cell);
                    _cells.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
        }

        void _addCellRows(int currentColumns,int oldRows,int newRows)
        {
            for (int i = oldRows+1; i <= newRows; i++)
            {
                for (int j = 1; j <= currentColumns; j++)
                {
                    CellUc cell = _createCellUc(i, j);
                    _cells.Add(cell);
                }
            }
        }

        void _removeCellColumns(int newColumns)
        {
            int i = 0;
            while (i < _cells.Count)
            {
                CellUc cell = (CellUc)_cells[i];
                if (cell.Column > newColumns)
                {
                    _disposeCellUc(cell);
                    _cells.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
        }

        void _addCellColumns(int currentRows,int oldColumns,int newColumns)
        {
            for (int i = 1; i <= currentRows; i++)
            {
                for (int j = oldColumns+1; j <= newColumns; j++)
                {

                    CellUc cell = _createCellUc(i, j);
                    _cells.Insert((i - 1) * newColumns + j-1, cell);
                }
            }
        }


        CellUc _createCellUc(int row,int column)
        {
            CellUc cell = new CellUc(row, column);
            return cell;
        }

        void _disposeCellUc(CellUc cell)
        {

        }

        #endregion


        #region 依赖项属性

        public PlateMesh Mesh
        {
            get { return (PlateMesh)GetValue(MeshProperty); }
            set { SetValue(MeshProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Mesh.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MeshProperty =
            DependencyProperty.Register("Mesh", typeof(PlateMesh), typeof(PlateListUc), new FrameworkPropertyMetadata(new PlateMesh(8, 12), _onSizeChangedCallback));



        static void _onSizeChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PlateListUc uc = (PlateListUc)d;
            uc._onMeshChanged();
        }

        void _onMeshChanged()
        {
            PlateMesh mesh = Mesh;
            int currentRows = _rowCtrCells.Count;
            int currentColumns = _columnCtrCells.Count;
            if (mesh.Rows != _rowCtrUniformGrid.Rows)
            {
                _rowCtrUniformGrid.Rows = mesh.Rows;
                _dataUniformGrid.Rows = mesh.Rows;
            }
            if (mesh.Columns != _columnCtrUniformGrid.Columns)
            {
                _columnCtrUniformGrid.Columns = mesh.Columns;
                _dataUniformGrid.Columns = mesh.Columns;
            }
            if(currentRows > mesh.Rows)
            {
                _removeRows(mesh.Rows);
            }
            if(currentColumns > mesh.Columns)
            {
                _removeColumns(mesh.Columns);
            }
            if (currentRows < mesh.Rows)
            {
                _addRows(currentColumns,currentRows,mesh.Rows);
            }
            if (currentColumns < mesh.Columns)
            {
                _addColumns(mesh.Rows,currentColumns,mesh.Columns);
            }
            if ((currentRows != mesh.Rows) || (currentColumns != mesh.Columns))
            {
                _zoomSize();
            }
        }

        /// <summary>
        /// 移除行，包括控制头和
        /// </summary>
        /// <param name="newRows"></param>
        void _removeRows(int newRows)
        {
            _removeCtrCellRows(newRows);
            _removeCellRows(newRows);
        }

        void _addRows(int currentColumns,int oldRows, int newRows)
        {
            _addCtrCellRows(newRows);
            _addCellRows(currentColumns,oldRows,newRows);
        }

        void _removeColumns(int newColumns)
        {
            _removeCtrCellColumns(newColumns);
            _removeCellColumns(newColumns);
        }

        void _addColumns(int currentRows,int oldColumns, int newColumns)
        {
            _addCtrCellColumns(newColumns);
            _addCellColumns(currentRows, oldColumns,newColumns);
        }

        #endregion


        #region Zoom

        private void _slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _zoomSize();
        }

        void _zoomSize()
        {

            this._zoomGrid.Width = _scrollViewer.ActualWidth * this._slider.Value;
            this._zoomGrid.Height = _scrollViewer.ActualHeight * this._slider.Value;
            this._scrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
            this._scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
            this._scrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            this._scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
        }

        #endregion
    }
}