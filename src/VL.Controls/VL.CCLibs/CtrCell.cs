using System;
using System.Collections.Generic;
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
    ///     <MyNamespace:CtrCell/>
    ///
    /// </summary>
    [TemplatePart(Name = "PART_Btn", Type = typeof(ButtonBase))]
    public class CtrCell : Control
    {
        static CtrCell()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CtrCell), new FrameworkPropertyMetadata(typeof(CtrCell)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ButtonBase btn = GetTemplateChild("PART_Btn") as ButtonBase;
            btn.Content = _title;
            btn.Click += Btn_Click;
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            _onClickedChanged();
        }

        public CtrCell(string title, int num)
        {
            _num = num;
            _title = title;
        }

        private int _num;
        public int Num
        {
            get { return _num; }
        }

        string _title;
        public string Title
        {
            get { return _title; }
        }


        public static readonly RoutedEvent MulPlaceEvent = 
            EventManager.RegisterRoutedEvent("MulPlace", RoutingStrategy.Bubble,typeof(MulPlaceRoutedEventHander), typeof(CtrCell));

        public event MulPlaceRoutedEventHander MulPlace
        {
            add { AddHandler(MulPlaceEvent, value); }
            remove { RemoveHandler(MulPlaceEvent, value); }
        }

        private void _onClickedChanged()
        {
            MulPlaceRoutedEventArgs args = new MulPlaceRoutedEventArgs(_num,CtrCell.MulPlaceEvent,this);
            RaiseEvent(args);
        }
    }
}
