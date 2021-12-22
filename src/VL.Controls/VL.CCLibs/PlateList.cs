using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VL.CCLibs
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:CCLibs"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:CCLibs;assembly=CCLibs"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:CustomControl1/>
    ///
    /// </summary>

    //[StyleTypedProperty(Property = nameof(ItemContainerStyle), StyleTargetType = typeof(RibbonTabItem))]
    [StyleTypedProperty(Property = nameof(ItemContainerStyle), StyleTargetType = typeof(PlateListItem))]
    [TemplatePart(Name = "PART_Slider", Type = typeof(RangeBase))]
    [TemplatePart(Name = "PART_ResetSizeBtn", Type = typeof(ButtonBase))]
    [TemplatePart(Name = "PART_ScrollViewer", Type = typeof(ScrollViewer))]
    [TemplatePart(Name = "PART_ZoomGrid", Type = typeof(Grid))]
    [TemplatePart(Name = "PART_ChooseAllBtn", Type = typeof(RangeBase))]
    [TemplatePart(Name = "PART_RowCtrUniformGrid", Type = typeof(UniformGrid))]
    [TemplatePart(Name = "PART_ColumnCtrUniformGrid", Type = typeof(UniformGrid))]
    [TemplatePart(Name = "PART_CellUniformGrid", Type = typeof(Grid))]
    [TemplatePart(Name = "PART_ItemsPresenter", Type = typeof(ItemsPresenter))]
    [TemplatePart(Name = "PART_ContentGrid", Type = typeof(Grid))]
    public class PlateList : ItemsControl
    {
        RangeBase _slider;
        ScrollViewer _scrollViewer;
        Grid _zoomGrid;
        Grid _contentGrid;

        UniformGrid _columnCtrUniformGrid;
        UniformGrid _rowCtrUniformGrid;
        Grid _dataUniformGrid;

        public PlateList():base()
        {

        }

        static PlateList()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PlateList), new FrameworkPropertyMetadata(typeof(PlateList)));
        }



        #region Override


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _slider=(RangeBase)this.Template.FindName("PART_Slider", this);
            ButtonBase resetBtn= (ButtonBase)this.Template.FindName("PART_ResetSizeBtn", this);
            _scrollViewer=(ScrollViewer)this.Template.FindName("PART_ScrollViewer", this);
            _zoomGrid=(Grid)this.Template.FindName("PART_ZoomGrid", this);


            ButtonBase chooseAllBtn = (ButtonBase)this.Template.FindName("PART_ChooseAllBtn", this);
            _columnCtrUniformGrid = (UniformGrid)this.Template.FindName("PART_ColumnCtrUniformGrid", this);
            _rowCtrUniformGrid = (UniformGrid)this.Template.FindName("PART_RowCtrUniformGrid", this);
            _dataUniformGrid = (Grid)this.Template.FindName("PART_CellUniformGrid", this);

            //行列控制
            _columnCtrCells = _columnCtrUniformGrid.Children;
            _rowCtrCells = _rowCtrUniformGrid.Children;

            //背景单元格
            _gridRowDefs = _dataUniformGrid.RowDefinitions;
            _gridColumnDefs = _dataUniformGrid.ColumnDefinitions;
            _cells = _dataUniformGrid.Children;

            //滚动、范围条、尺码重置
            resetBtn.Click += _resetBtn_Click;
            chooseAllBtn.Click += _chooseAllBtn_Click;


            //内容
            var presenter = (ItemsPresenter)this.Template.FindName("PART_ItemsPresenter", this);
            presenter.ApplyTemplate();
            _contentGrid = (Grid)this.ItemsPanel.FindName("PART_ContentGrid", presenter);
            _contentGridRowDefs = _contentGrid.RowDefinitions;
            _contentGridColumnDefs = _contentGrid.ColumnDefinitions;

        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            this.Loaded += PlateList_Loaded;
        }

        private void PlateList_Loaded(object sender, RoutedEventArgs e)
        {
            //初始化布局
            _applyCurrentMesh();
            _scrollViewer.SizeChanged += _scrollViewer_SizeChanged;
            _slider.ValueChanged += _slider_ValueChanged;
            _readyDrag();
        }


        /// <inheritdoc />
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new PlateListItem();
        }

        /// <inheritdoc />
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is PlateListItem;
        }

        #endregion


        #region 鼠标拖动选择

        /// <summary>
        /// 
        /// </summary>
        /// <param name="positionInGirdCoordinates"></param>
        /// <returns></returns>
        Tuple<int, int> _getPositionInGrid(Point positionInGirdCoordinates)
        {
            double rowHeight = _dataUniformGrid.RowDefinitions[0].ActualHeight;
            double columnWidth = _dataUniformGrid.ColumnDefinitions[0].ActualWidth;
            int gridRow = (int)Math.Floor(positionInGirdCoordinates.Y / rowHeight);
            int gridColumn = (int)Math.Floor(positionInGirdCoordinates.X / columnWidth);
            if (gridRow < 0)
            {
                gridRow = 0;
            }
            if (gridRow > _dataUniformGrid.RowDefinitions.Count - 1)
            {
                gridRow = _dataUniformGrid.RowDefinitions.Count - 1;
            }
            if (gridColumn < 0)
            {
                gridColumn = 0;
            }
            if (gridColumn > _dataUniformGrid.ColumnDefinitions.Count - 1)
            {
                gridColumn = _dataUniformGrid.ColumnDefinitions.Count - 1;
            }

            return new Tuple<int, int>(gridRow, gridColumn);
        }

        void _readyDrag()
        {
            this._dataUniformGrid.AddHandler(UIElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(this.OnMouseLeftButtonDown), false);
        }


        Tuple<int, int> _startGridPosition;

        Tuple<int, int> _endGridPosition;
        private Border _currentBoxSelectedBorder = null;//拖动展示的提示框



        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var positionInGirdCoordinates = e.GetPosition(this._dataUniformGrid);
            Tuple<int, int> startGridPosition = _getPositionInGrid(positionInGirdCoordinates);
            this.StartDrag(startGridPosition);

            //if (this.DragBegun != null)
            //{
            //    this.DragBegun(this, e);
            //}
        }

        internal void StartDrag(Tuple<int, int> startGridPosition)
        {
            this._startGridPosition = startGridPosition;

            this._endGridPosition = startGridPosition;
            this._dataUniformGrid.CaptureMouse();
            this._dataUniformGrid.MouseMove += this.OnMouseMove;
            this._dataUniformGrid.LostMouseCapture += this.OnLostMouseCapture;
            this._dataUniformGrid.AddHandler(UIElement.MouseLeftButtonUpEvent, new MouseButtonEventHandler(this.OnMouseLeftButtonUp), false /* handledEventsToo */);
        }


        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            var positionInGirdCoordinates = e.GetPosition(this._dataUniformGrid);
            Tuple<int, int> endGridPosition = _getPositionInGrid(positionInGirdCoordinates);
            this.HandleDrag(endGridPosition);

            //if (this.Dragging != null)
            //{
            //    this.Dragging(this, e);
            //}
        }



        internal void HandleDrag(Tuple<int, int> endGridPosition)
        {
            this._endGridPosition = endGridPosition;
            if (_currentBoxSelectedBorder == null)
            {
                _currentBoxSelectedBorder = new Border();
                _currentBoxSelectedBorder.Background = new SolidColorBrush(DragColor);
                _currentBoxSelectedBorder.HorizontalAlignment = HorizontalAlignment.Stretch;
                _currentBoxSelectedBorder.VerticalAlignment = VerticalAlignment.Stretch;
                _currentBoxSelectedBorder.Opacity = 0.4;
                _currentBoxSelectedBorder.BorderThickness = new Thickness(1);
                _currentBoxSelectedBorder.BorderBrush = new SolidColorBrush(Colors.WhiteSmoke);
                this._dataUniformGrid.Children.Add(_currentBoxSelectedBorder);
            }

            //计算结束位置点
            int gridRow;
            int gridRowSpan;

            int rowMod = PlaceMesh.Rows;
            int sumRow = _dataUniformGrid.RowDefinitions.Count;
            int rowInterval = endGridPosition.Item1 - _startGridPosition.Item1;

            //计算Row
            int iniRowSpan = Math.Abs(rowInterval) + 1;
            int rowRemainder = iniRowSpan % rowMod;
            int rowMultiple = iniRowSpan / rowMod;
            if (rowRemainder != 0)
            {
                gridRowSpan = (rowMultiple + 1) * rowMod;

                if (rowInterval >= 0)
                {
                    gridRow = _startGridPosition.Item1;
                    if (gridRow + gridRowSpan > sumRow)
                    {
                        gridRowSpan = gridRowSpan - rowMod;
                    }
                }
                else
                {
                    if (gridRowSpan - 1 > _startGridPosition.Item1)
                    {
                        gridRowSpan = gridRowSpan - rowMod;
                    }
                    gridRow = _startGridPosition.Item1 + 1 - gridRowSpan;
                }
            }
            else
            {
                gridRowSpan = rowMultiple * rowMod;
                if (rowInterval >= 0)
                {
                    gridRow = _startGridPosition.Item1;
                }
                else
                {
                    gridRow = _startGridPosition.Item1 + 1 - gridRowSpan;
                }
            }

            //计算列
            int gridColumn;
            int gridColumnSpan;

            int columnMod = PlaceMesh.IsWithBlank ? PlaceMesh.Columns + 1 : PlaceMesh.Columns;
            int sumColumn = _dataUniformGrid.ColumnDefinitions.Count;
            int columnInterval = endGridPosition.Item2 - _startGridPosition.Item2;

            //计算Column
            int iniColumnSpan = Math.Abs(columnInterval) + 1;
            int columnRemainder = iniColumnSpan % columnMod;
            int columnMultiple = iniColumnSpan / columnMod;
            if (columnRemainder != 0)
            {
                gridColumnSpan = (columnMultiple + 1) * columnMod;

                if (columnInterval >= 0)
                {
                    gridColumn = _startGridPosition.Item2;
                    if (gridColumn + gridColumnSpan > sumColumn)
                    {
                        gridColumnSpan = gridColumnSpan - columnMod;
                    }
                }
                else
                {
                    if (gridColumnSpan - 1 > _startGridPosition.Item2)
                    {
                        gridColumnSpan = gridColumnSpan - columnMod;
                    }
                    gridColumn = _startGridPosition.Item2 + 1 - gridColumnSpan;
                }
            }
            else
            {
                gridColumnSpan = columnMultiple * columnMod;
                if (columnInterval >= 0)
                {
                    gridColumn = _startGridPosition.Item2;
                }
                else
                {
                    gridColumn = _startGridPosition.Item2 + 1 - gridColumnSpan;
                }
            }
            if (gridRowSpan == 0 || gridColumnSpan == 0)
            {
                _currentBoxSelectedBorder.Visibility = Visibility.Collapsed;
            }
            else
            {
                _currentBoxSelectedBorder.Visibility = Visibility.Visible;
                Grid.SetRow(_currentBoxSelectedBorder, gridRow);
                Grid.SetRowSpan(_currentBoxSelectedBorder, gridRowSpan);
                Grid.SetColumn(_currentBoxSelectedBorder, gridColumn);
                Grid.SetColumnSpan(_currentBoxSelectedBorder, gridColumnSpan);
            }
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
            if (_currentBoxSelectedBorder != null && _dataUniformGrid.Children.Contains(_currentBoxSelectedBorder))
            {
                _dataUniformGrid.Children.Remove(_currentBoxSelectedBorder);
                _currentBoxSelectedBorder = null;
            }
        }

        #endregion


        #region 拖动相关依赖项属性：PlaceMesh、DragColor

        public Color DragColor
        {
            get { return (Color)GetValue(DragColorProperty); }
            set { SetValue(DragColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DargColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DragColorProperty =
            DependencyProperty.Register("DragColor", typeof(Color), typeof(PlateList), new FrameworkPropertyMetadata(Colors.LightBlue));



        public PlaceMesh PlaceMesh
        {
            get { return (PlaceMesh)GetValue(PlaceMeshProperty); }
            set { SetValue(PlaceMeshProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PlaceMesh.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlaceMeshProperty =
            DependencyProperty.Register("PlaceMesh", typeof(PlaceMesh), typeof(PlateList), new PropertyMetadata(new PlaceMesh() { Columns = 2, Rows = 3, IsWithBlank = true }));



        #endregion


        #region 全选控制

        private void _chooseAllBtn_Click(object sender, RoutedEventArgs e)
        {
        }

        #endregion


        #region Row Control

        char[] _rowCtrTitles = new char[]
        {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W' , 'X', 'Y', 'Z'
        };

        UIElementCollection _rowCtrCells;

        void _removeCtrCellRows(int newRows)
        {
            while (_rowCtrCells.Count > newRows)
            {
                var rowCtrCell = (CtrCell)_rowCtrCells[newRows];
                _rowCtrCells.Remove(rowCtrCell);
            }
        }

        void _addCtrCellRows(int newRows)
        {
            int i = _rowCtrCells.Count;
            while (i < newRows)
            {
                var rowCtrCell = new CtrCell(_rowCtrTitles[i].ToString(), i + 1);
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
                var columnCtrCell = (CtrCell)_columnCtrCells[newColumns];
                _columnCtrCells.Remove(columnCtrCell);
            }
        }

        void _addCtrCellColumns(int newColumns)
        {
            int i = _columnCtrCells.Count;
            while (i < newColumns)
            {
                int num = i + 1;
                var columnCtrCell = new CtrCell(num.ToString(), num);
                _columnCtrCells.Add(columnCtrCell);
                i++;
            }
        }

        private void _columnCtrCell_Placing(object sender, EventArgs e)
        {

        }

        #endregion


        #region Area-Cell

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
                var cell = (Cell)_cells[i];
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

        void _addCellRows(int currentColumns, int oldRows, int newRows)
        {
            for (int i = oldRows + 1; i <= newRows; i++)
            {
                for (int j = 1; j <= currentColumns; j++)
                {
                    var cell = _createCellUc(i, j);
                    _cells.Add(cell);
                }
            }
        }

        void _removeCellColumns(int newColumns)
        {
            int i = 0;
            while (i < _cells.Count)
            {
                var cell = (Cell)_cells[i];
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

        void _addCellColumns(int currentRows, int oldColumns, int newColumns)
        {
            for (int i = 1; i <= currentRows; i++)
            {
                for (int j = oldColumns + 1; j <= newColumns; j++)
                {
                    var cell = _createCellUc(i, j);
                    _cells.Insert((i - 1) * newColumns + j - 1, cell);
                }
            }
        }

        #endregion


        #region 单元格创建、释放

        Cell _createCellUc(int row, int column)
        {
            Cell cell = new Cell(row, column);
            Grid.SetRow(cell, cell.Row - 1);
            Grid.SetColumn(cell, cell.Column - 1);
            //cell.AddHandler(UIElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(this._onCellMouseLeftButtonDown), true);
            //cell.AddHandler(UIElement.MouseMoveEvent, new MouseEventHandler(this._onCellMouseMove), false);
            return cell;
        }

        void _disposeCellUc(Cell cell)
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


        #region Zoom


        private void _scrollViewer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _zoomSize();
        }

        private void _resetBtn_Click(object sender, RoutedEventArgs e)
        {
            _slider.Value = 1;
        }

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


        #region CententGrid

        RowDefinitionCollection _contentGridRowDefs;
        ColumnDefinitionCollection _contentGridColumnDefs;

        void _removeContentRowDefinitions(int newRows)
        {
            int i = newRows;
            while (_contentGridRowDefs.Count > newRows)
            {
                _contentGridRowDefs.RemoveAt(i);
            }
        }

        void _addContentRowDefinitions(int newRows)
        {
            for (int i = _contentGridRowDefs.Count; i < newRows; i++)
            {
                _contentGridRowDefs.Add(new RowDefinition() { Height = new GridLength(1d, GridUnitType.Star) });
            }
        }

        void _removeContentColumnDefinitions(int newColumns)
        {
            int i = newColumns;
            while (_contentGridColumnDefs.Count > newColumns)
            {
                _contentGridColumnDefs.RemoveAt(i);
            }
        }

        void _addContentColumnDefinitions(int newColumns)
        {
            for (int i = _contentGridColumnDefs.Count; i < newColumns; i++)
            {
                _contentGridColumnDefs.Add(new ColumnDefinition() { Width = new GridLength(1d, GridUnitType.Star) });
            }
        }


        #endregion


        #region 依赖项属性

        /// <summary>
        /// 重置按钮的图标
        /// </summary>
        public ImageSource ResetSource
        {
            get { return (ImageSource)GetValue(ResetSourceProperty); }
            set { SetValue(ResetSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RefreshSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ResetSourceProperty =
            DependencyProperty.Register("ResetSource", typeof(ImageSource), typeof(PlateList), new FrameworkPropertyMetadata(_createDefaultImage()));

        public static BitmapImage _createDefaultImage()
        {
            return new BitmapImage(new Uri("reset.png", UriKind.Relative));
        }


        public PlateMesh Mesh
        {
            get { return (PlateMesh)GetValue(MeshProperty); }
            set { SetValue(MeshProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Mesh.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MeshProperty =
            DependencyProperty.Register("Mesh", typeof(PlateMesh), typeof(PlateList), new FrameworkPropertyMetadata(new PlateMesh(8, 12), _onSizeChangedCallback));



        static void _onSizeChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PlateList cc = (PlateList)d;
            cc._applyCurrentMesh();
        }


        #endregion


        #region MeshChanged

        void _applyCurrentMesh()
        {
            if (_contentGrid == null) return;
            PlateMesh mesh = Mesh;
            int currentRows = _rowCtrCells.Count;
            int currentColumns = _columnCtrCells.Count;

            if (currentRows > mesh.Rows)
            {
                _removeRows(mesh.Rows);
            }
            if (currentColumns > mesh.Columns)
            {
                _removeColumns(mesh.Columns);
            }
            if (currentRows < mesh.Rows)
            {
                _addRows(currentColumns, currentRows, mesh.Rows);
            }
            if (currentColumns < mesh.Columns)
            {
                _addColumns(mesh.Rows, currentColumns, mesh.Columns);
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

            _removeContentRowDefinitions(newRows);
        }

        void _addRows(int currentColumns, int oldRows, int newRows)
        {
            _addCtrCellRows(newRows);

            _addRowDefinitions(newRows);
            _addCellRows(currentColumns, oldRows, newRows);

            _addContentRowDefinitions(newRows);
        }

        void _removeColumns(int newColumns)
        {
            _removeCtrCellColumns(newColumns);

            _removeCellColumns(newColumns);
            _removeColumnDefinitions(newColumns);

            _removeContentColumnDefinitions(newColumns);
        }

        void _addColumns(int currentRows, int oldColumns, int newColumns)
        {
            _addCtrCellColumns(newColumns);

            _addColumnDefinitions(newColumns);
            _addCellColumns(currentRows, oldColumns, newColumns);

            _addContentColumnDefinitions(newColumns);
        }

        #endregion

    }
}
