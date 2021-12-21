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

namespace ListBoxStyleTester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = new MainViewModel();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            this.Loaded += MainWindow_Loaded1;
        }

        private void MainWindow_Loaded1(object sender, RoutedEventArgs e)
        {
            //var presenter = (ItemsPresenter)this.Template.FindName("PART_Presenter", this);
            //var stackPanel = (StackPanel)this.ItemsPanel.FindName("PART_StackPanel", presenter);
            _listBox.ApplyTemplate();

            var presenter = (ItemsPresenter)_listBox.Template.FindName("_itemsPresenter", _listBox);
            var grid = _listBox.ItemsPanel.FindName("_grid", presenter);
        }

        //void _initialize()
        //{
        //    ItemsPanelTemplate ipt = _listBox.ItemsPanel;

        //    var grid = ipt.FindName("_grid", _listBox);
        //}
    }
}
