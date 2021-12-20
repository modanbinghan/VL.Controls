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
        char[] _rowCtrTitles = new char[] 
        { 
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W' , 'X', 'Y', 'Z'
        };

        public PlateListUc()
        {
            InitializeComponent();
            this.Loaded += _plateListUc_Loaded;
        }


        private void _plateListUc_Loaded(object sender, RoutedEventArgs e)
        {
            _initialize();
            _scrollViewer.SizeChanged += _scrollViewer_SizeChanged;
            _slider.ValueChanged += _slider_ValueChanged;
            _resetSizeBtn.Click += _resetSizeBtn_Click;
            _readyDrag();
        }

        #region 初始化

        void _initialize()
        {
            _columnCtrCells = _columnCtrUniformGrid.Children;
            _rowCtrCells = _rowCtrUniformGrid.Children;
            _gridRowDefs = _dataUniformGrid.RowDefinitions;
            _gridColumnDefs = _dataUniformGrid.ColumnDefinitions;
            _cells = _dataUniformGrid.Children;
            _onMeshChanged();
        }

        #endregion


        #region 容器ScorllViewer的尺码改变时，重置Slider缩放条的大小-》重新计算_zoomGrid的大小

        private void _scrollViewer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _zoomSize();
        }

        private void _resetSizeBtn_Click(object sender, RoutedEventArgs e)
        {
            _slider.Value = 1;
        }

        #endregion


        #region 鼠标拖动选择

        void _readyDrag()
        {
            this._dataUniformGrid.AddHandler(UIElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(this.OnMouseLeftButtonDown), false);
        }


        Point _startPoint;
        //CellUc _startCell;
        //CellUc _endStartCell;
        private Border _currentBoxSelectedBorder = null;//拖动展示的提示框



        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.StartDrag(e.GetPosition(this._dataUniformGrid));

            //if (this.DragBegun != null)
            //{
            //    this.DragBegun(this, e);
            //}
        }

        internal void StartDrag(Point positionInElementCoordinates)
        {
            this._startPoint = positionInElementCoordinates;

            this._dataUniformGrid.CaptureMouse();

            this._dataUniformGrid.MouseMove += this.OnMouseMove;
            this._dataUniformGrid.LostMouseCapture += this.OnLostMouseCapture;
            this._dataUniformGrid.AddHandler(UIElement.MouseLeftButtonUpEvent, new MouseButtonEventHandler(this.OnMouseLeftButtonUp), false /* handledEventsToo */);
        }


        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            this.HandleDrag(e.GetPosition(this._dataUniformGrid));

            //if (this.Dragging != null)
            //{
            //    this.Dragging(this, e);
            //}
        }

        internal void HandleDrag(Point newPositionInElementCoordinates)
        {
            Point endPoint = newPositionInElementCoordinates;
            if (_currentBoxSelectedBorder == null)
            {
                _currentBoxSelectedBorder = new Border();
                _currentBoxSelectedBorder.Background = new SolidColorBrush(Colors.Orange);
                _currentBoxSelectedBorder.HorizontalAlignment = HorizontalAlignment.Left;
                _currentBoxSelectedBorder.VerticalAlignment = VerticalAlignment.Top;
                _currentBoxSelectedBorder.Opacity = 0.4;
                _currentBoxSelectedBorder.BorderThickness = new Thickness(1);
                _currentBoxSelectedBorder.BorderBrush = new SolidColorBrush(Colors.OrangeRed);
                Grid.SetColumn(_currentBoxSelectedBorder, 0);
                Grid.SetRow(_currentBoxSelectedBorder, 0);
                Grid.SetColumnSpan(_currentBoxSelectedBorder, _dataUniformGrid.ColumnDefinitions.Count);
                Grid.SetRowSpan(_currentBoxSelectedBorder, _dataUniformGrid.RowDefinitions.Count);
                this._dataUniformGrid.Children.Add(_currentBoxSelectedBorder);
            }
            _currentBoxSelectedBorder.Width = Math.Abs(endPoint.X - _startPoint.X);
            _currentBoxSelectedBorder.Height = Math.Abs(endPoint.Y - _startPoint.Y);
            Thickness margin = new Thickness();
            if (endPoint.X - _startPoint.X >= 0)
                margin.Left = _startPoint.X;
            else
                margin.Left = endPoint.X;
            if (endPoint.Y - _startPoint.Y >= 0)
                margin.Top = _startPoint.Y;
            else
                margin.Top = endPoint.Y;
            _currentBoxSelectedBorder.Margin = margin;
        }

        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this._dataUniformGrid.ReleaseMouseCapture();
        }

        private void OnLostMouseCapture(object sender, MouseEventArgs e)
        {
            this.EndDrag();

            //if (this.DragFinished != null)
            //{
            //    this.DragFinished(this, e);
            //}
        }

        internal void EndDrag()
        {
            this._dataUniformGrid.MouseMove -= this.OnMouseMove;
            this._dataUniformGrid.LostMouseCapture -= this.OnLostMouseCapture;
            this._dataUniformGrid.RemoveHandler(UIElement.MouseLeftButtonUpEvent, new MouseButtonEventHandler(this.OnMouseLeftButtonUp));
            if (_currentBoxSelectedBorder != null&& _dataUniformGrid.Children.Contains(_currentBoxSelectedBorder))
            {
                _dataUniformGrid.Children.Remove(_currentBoxSelectedBorder);
                _currentBoxSelectedBorder = null;
            }
        }

        #endregion


        #region DargColor



        public Color DargColor
        {
            get { return (Color)GetValue(DargColorProperty); }
            set { SetValue(DargColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DargColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DargColorProperty =
            DependencyProperty.Register("DargColor", typeof(Color), typeof(PlateListUc), new FrameworkPropertyMetadata(Colors.LightBlue));



        #endregion


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

        #region Grid RowDefinitionCollection/ColumnDefinitionCollection

        RowDefinitionCollection _gridRowDefs;
        ColumnDefinitionCollection _gridColumnDefs;

        void _removeRowDefinitions(int newRows)
        {
            int i = newRows;
            while (_gridRowDefs.Count > newRows)
            {
                _gridRowDefs.RemoveAt(i);
            }
        }

        void _addRowDefinitions(int newRows)
        {
            for (int i = _gridRowDefs.Count; i < newRows; i++)
            {
                _gridRowDefs.Add(new RowDefinition() { Height = new GridLength(1d, GridUnitType.Star) });
            }
        }

        void _removeColumnDefinitions(int newColumns)
        {
            int i = newColumns;
            while (_gridColumnDefs.Count > newColumns)
            {
                _gridColumnDefs.RemoveAt(i);
            }
        }

        void _addColumnDefinitions(int newColumns)
        {
            for (int i = _gridColumnDefs.Count; i < newColumns; i++)
            {
                _gridColumnDefs.Add(new ColumnDefinition() { Width = new GridLength(1d, GridUnitType.Star) });
            }
        }

        #endregion


        #region Cell添加、移除

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

        #endregion


        #region 单元格创建、释放

        CellUc _createCellUc(int row,int column)
        {
            CellUc cell = new CellUc(row, column);
            Grid.SetRow(cell, cell.Row - 1);
            Grid.SetColumn(cell, cell.Column - 1);
            //cell.AddHandler(UIElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(this._onCellMouseLeftButtonDown), true);
            //cell.AddHandler(UIElement.MouseMoveEvent, new MouseEventHandler(this._onCellMouseMove), false);
            return cell;
        }

        void _disposeCellUc(CellUc cell)
        {
            //    cell.RemoveHandler(UIElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(this._onCellMouseLeftButtonDown));
            //    cell.RemoveHandler(UIElement.MouseMoveEvent, new MouseEventHandler(this._onCellMouseMove));
        }


        //private void _onCellMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    if (_startCell == null)
        //    {
        //        _startCell = (CellUc)sender;
        //        _startCell.Background = new SolidColorBrush(DargColor);
        //    }
        //}

        //private void _onCellMouseMove(object sender, MouseEventArgs e)
        //{
        //    if (_startCell != null)
        //    {
        //        _endStartCell = (CellUc)sender;
        //        _endStartCell.Background = new SolidColorBrush(DargColor);
        //    }
        //}

        #endregion


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
                //_dataUniformGrid.Rows = mesh.Rows;
            }
            if (mesh.Columns != _columnCtrUniformGrid.Columns)
            {
                _columnCtrUniformGrid.Columns = mesh.Columns;
                //_dataUniformGrid.Columns = mesh.Columns;
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
            _removeRowDefinitions(newRows);
        }

        void _addRows(int currentColumns,int oldRows, int newRows)
        {
            _addCtrCellRows(newRows);
            _addRowDefinitions(newRows);
            _addCellRows(currentColumns,oldRows,newRows);
        }

        void _removeColumns(int newColumns)
        {
            _removeCtrCellColumns(newColumns);
            _removeCellColumns(newColumns);
            _removeColumnDefinitions(newColumns);
        }

        void _addColumns(int currentRows,int oldColumns, int newColumns)
        {
            _addCtrCellColumns(newColumns);
            _addColumnDefinitions(newColumns);
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


        #region 数据获取、数据模板、模板选择器



        public CellProvider Provider
        {
            get { return (CellProvider)GetValue(ProviderProperty); }
            set { SetValue(ProviderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Provider.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProviderProperty =
            DependencyProperty.Register("Provider", typeof(CellProvider), typeof(PlateListUc), new PropertyMetadata(new CellProvider(null)));




        public DataTemplate CellTemplate
        {
            get { return (DataTemplate)GetValue(CellTemplateProperty); }
            set { SetValue(CellTemplateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CellTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CellTemplateProperty =
            DependencyProperty.Register("CellTemplate", typeof(DataTemplate), typeof(PlateListUc), new PropertyMetadata(null));





        public DataTemplateSelector CellTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(CellTemplateSelectorProperty); }
            set { SetValue(CellTemplateSelectorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CellTemplateSelector.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CellTemplateSelectorProperty =
            DependencyProperty.Register("CellTemplateSelector", typeof(DataTemplateSelector), typeof(PlateListUc), new PropertyMetadata(null));




        #endregion
    }
}