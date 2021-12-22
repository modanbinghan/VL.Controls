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
            this.DataContext = new MainViewModel();
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //var presenter = (ItemsPresenter)this.Template.FindName("PART_Presenter", this);
            //var stackPanel = (StackPanel)this.ItemsPanel.FindName("PART_StackPanel", presenter);
            _listBox.ApplyTemplate();

            //var presenter = (ItemsPresenter)_listBox.Template.FindName("_itemsPresenter", _listBox);
            //var grid =(Grid)_listBox.ItemsPanel.FindName("_grid", presenter);
            //var grid = _listBox.Template.FindName("_grid", _listBox);
        }
    }
}
