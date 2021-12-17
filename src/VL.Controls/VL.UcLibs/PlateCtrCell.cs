using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using VL.Mvvm.Ass;

namespace VL.UcLibs
{
    public class PlateCtrCell
    {
        public event EventHandler Placing;
        string _title;
        int _num;

        public PlateCtrCell(string title,int num)
        {
            _title = title;
            _num = num;
        }

        public string Title
        {
            get { return _title; }
        }

        public int Num
        {
            get { return _num; }
        }

        RelayCommand _placeCommand;
        public ICommand PlaceCommand
        {
            get
            {
                if (_placeCommand == null) _placeCommand = new RelayCommand(x => _place());
                return _placeCommand;
            }
        }
        private void _place()
        {
            var temp = Interlocked.CompareExchange(ref Placing, null, null);
            temp.Invoke(this, new EventArgs());
        }


    }
}
