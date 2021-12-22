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

namespace PlateListTester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private Border currentBoxSelectedBorder = null;//拖动展示的提示框
        private bool isCanMove = false;//鼠标是否移动
        private Point tempStartPoint;//起始坐标


        /// <summary>
        /// 鼠标按下记录起始点
        /// </summary>
        private void MainCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isCanMove = true;
            tempStartPoint = e.GetPosition(this.mainCanvas);
        }

        /// <summary>
        /// 框选逻辑
        /// </summary>
        private void MainCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (isCanMove)
            {
                Point tempEndPoint = e.GetPosition(this.mainCanvas);
                //绘制跟随鼠标移动的方框
                DrawMultiselectBorder(tempEndPoint, tempStartPoint);
            }
        }

        /// <summary>
        /// 鼠标抬起时清除选框
        /// </summary>
        private void MainCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (currentBoxSelectedBorder != null)
            {
                //获取选框的矩形位置
                Point tempEndPoint = e.GetPosition(this.mainCanvas);
                Rect tempRect = new Rect(tempStartPoint, tempEndPoint);
                //获取子控件
                List<Rectangle> childList = GetChildObjects<Rectangle>(this.mainCanvas);
                foreach (var child in childList)
                {
                    //获取子控件矩形位置
                    Rect childRect = new Rect(Canvas.GetLeft(child), Canvas.GetTop(child), child.Width, child.Height);
                    //若子控件与选框相交则改变样式
                    if (childRect.IntersectsWith(tempRect))
                        child.Opacity = 0.4;
                }
                this.mainCanvas.Children.Remove(currentBoxSelectedBorder);
                currentBoxSelectedBorder = null;
            }

            isCanMove = false;
        }

        /// <summary>
        /// 绘制跟随鼠标移动的方框
        /// </summary>
        private void DrawMultiselectBorder(Point endPoint, Point startPoint)
        {
            if (currentBoxSelectedBorder == null)
            {
                currentBoxSelectedBorder = new Border();
                currentBoxSelectedBorder.Background = new SolidColorBrush(Colors.Orange);
                currentBoxSelectedBorder.Opacity = 0.4;
                currentBoxSelectedBorder.BorderThickness = new Thickness(2);
                currentBoxSelectedBorder.BorderBrush = new SolidColorBrush(Colors.OrangeRed);
                //Canvas.SetZIndex(currentBoxSelectedBorder, 100);
                this.mainCanvas.Children.Add(currentBoxSelectedBorder);
            }
            currentBoxSelectedBorder.Width = Math.Abs(endPoint.X - startPoint.X);
            currentBoxSelectedBorder.Height = Math.Abs(endPoint.Y - startPoint.Y);
            if (endPoint.X - startPoint.X >= 0)
                Canvas.SetLeft(currentBoxSelectedBorder, startPoint.X);
            else
                Canvas.SetLeft(currentBoxSelectedBorder, endPoint.X);
            if (endPoint.Y - startPoint.Y >= 0)
                Canvas.SetTop(currentBoxSelectedBorder, startPoint.Y);
            else
                Canvas.SetTop(currentBoxSelectedBorder, endPoint.Y);
        }

        /// <summary>
        /// 获得所有子控件
        /// </summary>
        public static List<T> GetChildObjects<T>(System.Windows.DependencyObject obj) where T : System.Windows.FrameworkElement
        {
            System.Windows.DependencyObject child = null;
            List<T> childList = new List<T>();
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);
                if (child is T)
                {
                    childList.Add((T)child);
                }
                childList.AddRange(GetChildObjects<T>(child));
            }
            return childList;
        }

        /// <summary>
        /// 恢复原来状态
        /// </summary>
        private void ReSet_Click(object sender, RoutedEventArgs e)
        {
            List<Rectangle> childList = GetChildObjects<Rectangle>(this.mainCanvas);
            foreach (var child in childList)
            {
                if (child.Opacity != 1)
                    child.Opacity = 1;
            }
        }
    }
}
