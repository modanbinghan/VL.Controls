using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using VL.CCLibs;
using VL.Mvvm.Ass;

namespace PlateListTester
{
    public class MainViewModel:ObservableObject
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
            _contentVms.Add(new ContentViewModel("1,3") { Row = 1, Column = 3 });
            _contentVms.Add(new ContentViewModel("2,2") { Row = 2, Column = 2 });
        }

        private PlateMesh _plateMesh = new PlateMesh() { Rows = 8, Columns = 12 };

        public PlateMesh PlateMesh
        {
            get { return _plateMesh; }
            set 
            { 
                _plateMesh = value;
                OnPropertyChanged("PlateMesh");
            }
        }



    }
}
