using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace PlateListTester
{
    public class ContentViewModel
    {
        public ContentViewModel(string content)
        {
            _content = content;
        }

        private int _column;
        public int Column
        {
            get { return _column; }
            set
            {
                _column = value;
            }
        }


        private int _row;
        public int Row
        {
            get { return _row; }
            set
            {
                _row = value;
            }
        }




        //public int Row
        //{
        //    get { return (int)GetValue(RowProperty); }
        //    set { SetValue(RowProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for Row.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty RowProperty =
        //    DependencyProperty.Register("Row", typeof(int), typeof(ContentViewModel), new PropertyMetadata(0));



        //public int Column
        //{
        //    get { return (int)GetValue(ColumnProperty); }
        //    set { SetValue(ColumnProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for Column.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty ColumnProperty =
        //    DependencyProperty.Register("Column", typeof(int), typeof(ContentViewModel), new PropertyMetadata(0));



        private string _content;

        public string Content
        {
            get { return _content; }
        }

    }
}
