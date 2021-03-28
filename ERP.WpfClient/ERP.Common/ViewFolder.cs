using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ERP.Common
{
    public class ViewFolder
    {
        private NavigateKey _navigateKey;
        //private OrderNavigateKey _caseNavigateKey;
        private FrameworkElement _view;

        private BitmapSource _objViewBitmapSource;

        public NavigateKey NavigateKey
        {
            get { return _navigateKey; }

            internal set { _navigateKey = value; }
        }

        //public OrderNavigateKey CaseNavigateKey
        //{
        //    get { return _caseNavigateKey; }

        //    internal set { _caseNavigateKey = value; }
        //}

        public FrameworkElement View
        {
            get { return _view; }

            internal set { _view = value; }
        }

        public BitmapSource ViewBitmapSource
        {
            get { return _objViewBitmapSource; }

            internal set { _objViewBitmapSource = value; }
        }

        public ViewFolder(NavigateKey navigateKey, FrameworkElement view)
        {
            _navigateKey = navigateKey;
            _view = view;
        }

        //public ViewFolder(OrderNavigateKey caseNavigateKey, FrameworkElement view)
        //{
        //    _caseNavigateKey = caseNavigateKey;
        //    _view = view;
        //}
    }
}
