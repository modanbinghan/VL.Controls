using System;
using System.Collections.Generic;
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
    public class PlateList : Control
    {
        static PlateList()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PlateList), new FrameworkPropertyMetadata(typeof(PlateList)));
        }



        #region 依赖项属性

        public int ColumnNum
        {
            get { return (int)GetValue(ColumnNumProperty); }
            set { SetValue(ColumnNumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ColumnNum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColumnNumProperty =
            DependencyProperty.Register("ColumnNum", typeof(int), typeof(PlateList), new FrameworkPropertyMetadata(12));



        public int RowNum
        {
            get { return (int)GetValue(RowNumProperty); }
            set { SetValue(RowNumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RowNum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RowNumProperty =
            DependencyProperty.Register("RowNum", typeof(int), typeof(PlateList), new FrameworkPropertyMetadata(8));

        #endregion


        #region 事件

        public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(PlateList));
        /// <summary>
        /// Add / Remove ClickEvent handler
        /// </summary>
        [Category("Behavior")]
        public event RoutedEventHandler Click { add { AddHandler(ClickEvent, value); } remove { RemoveHandler(ClickEvent, value); } }

        #endregion
    }
}
