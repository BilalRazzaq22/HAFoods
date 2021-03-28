using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Common
{
    public class NavigateKey : IEquatable<NavigateKey>
    {
        private NavigateKey _parentNavigateKey;
        private object _primaryKey;
        private ViewTypes _viewTypeKey;
        private bool _singleInstanceView;



        public ViewTypes ViewKey
        {
            get { return _viewTypeKey; }
        }

        public object PrimaryKey
        {
            get { return _primaryKey; }
        }

        public NavigateKey ParentNavigateKey
        {
            get { return _parentNavigateKey; }
        }

        public bool SingleInstanceView
        {
            get { return _singleInstanceView; }
        }


        public NavigateKey(ViewTypes viewTypes)
        {
            _viewTypeKey = viewTypes;
        }

        public NavigateKey(ViewTypes viewTypes, object primarykey)
        {
            _viewTypeKey = viewTypes;
            _primaryKey = primarykey;
        }

        public NavigateKey(ViewTypes viewTypes, object primarykey, NavigateKey parentNavigateKey)
        {
            _viewTypeKey = viewTypes;
            _primaryKey = primarykey;
            _parentNavigateKey = parentNavigateKey;
        }

        public NavigateKey(ViewTypes viewTypes, object primarykey, bool singleInstanceView, NavigateKey parentNavigateKey)
        {
            _viewTypeKey = viewTypes;
            _primaryKey = primarykey;
            _parentNavigateKey = parentNavigateKey;
            _singleInstanceView = singleInstanceView;
        }

        public bool Equals(NavigateKey other)
        {
            if (ViewKey != other.ViewKey)
                return false;

            if (SingleInstanceView || other.SingleInstanceView)
                return true;

            if (PrimaryKey == null && other.PrimaryKey == null)
                return true;

            if (this.PrimaryKey == null && other.PrimaryKey != null)
                return false;

            if (this.PrimaryKey != null && other.PrimaryKey == null)
                return false;

            return string.Compare(PrimaryKey.ToString(), other.PrimaryKey.ToString()) == 0;
        }

        public Action ExecuteAction { get; set; }
    }
}
