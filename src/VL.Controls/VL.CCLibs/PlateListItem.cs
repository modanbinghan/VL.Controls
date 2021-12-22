﻿using System;
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

namespace VL.CCLibs
{
    /// <summary>
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:VL.CCLibs"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:VL.CCLibs;assembly=VL.CCLibs"
    ///
    /// 您还需要添加一个从 XAML 文件所在的项目到此项目的项目引用，
    /// 并重新生成以避免编译错误:
    ///
    ///     在解决方案资源管理器中右击目标项目，然后依次单击
    ///     “添加引用”->“项目”->[浏览查找并选择此项目]
    ///
    ///
    /// 步骤 2)
    /// 继续操作并在 XAML 文件中使用控件。
    ///
    ///     <MyNamespace:PlateListItem/>
    ///
    /// </summary>
    public class PlateListItem : ContentControl
    {
        public PlateListItem():base()
        {

        }
        static PlateListItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PlateListItem), new FrameworkPropertyMetadata(typeof(PlateListItem)));
        }

        #region Row&Column DependencyProperty

        public int Row
        {
            get { return (int)GetValue(RowProperty); }
            set { SetValue(RowProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Row.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RowProperty =
            DependencyProperty.Register("Row", typeof(int), typeof(PlateListItem), new FrameworkPropertyMetadata(0,_onRowChangedCallback));


        static void _onRowChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PlateListItem cc = (PlateListItem)d;
            Grid.SetRow(cc, (int)e.NewValue);
        }


        public int Column
        {
            get { return (int)GetValue(ColumnProperty); }
            set { SetValue(ColumnProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Column.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColumnProperty =
            DependencyProperty.Register("Column", typeof(int), typeof(PlateListItem), new FrameworkPropertyMetadata(0,_onColumnChangedCallback));

        static void _onColumnChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PlateListItem cc = (PlateListItem)d;
            Grid.SetColumn(cc, (int)e.NewValue);
        }

        #endregion
    }
}
