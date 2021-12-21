using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ListBoxStyleTester
{
    public class MainViewModel
    {
        ObservableCollection<ContentViewModel> _contentVms = new ObservableCollection<ContentViewModel>();
        public ObservableCollection<ContentViewModel> ContentVms
        {
            get { return _contentVms; }
        }

        public MainViewModel()
        {
            _contentVms.Add(new ContentViewModel("0,1") { Row = 0, Column = 1 });
            _contentVms.Add(new ContentViewModel("0,2") { Row = 0, Column = 2 });
            _contentVms.Add(new ContentViewModel("0,3") { Row = 0, Column = 3 });
            _contentVms.Add(new ContentViewModel("2,2") { Row = 2, Column = 2 });
        }
    }
}
