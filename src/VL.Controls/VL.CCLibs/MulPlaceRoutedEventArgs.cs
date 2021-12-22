using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace VL.CCLibs
{
    public class MulPlaceRoutedEventArgs: RoutedEventArgs
    {
        int _position;
        public MulPlaceRoutedEventArgs(int position)
            :this(position,null,null)
        {
        }


        public MulPlaceRoutedEventArgs(int position, RoutedEvent routedEvent)
            :this(position,routedEvent,null)
        {
        }


        public MulPlaceRoutedEventArgs(int position, RoutedEvent routedEvent, object source)
            :base(routedEvent,source)
        {
            _position = position;
        }
    }
}
