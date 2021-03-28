using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ERP.Common
{
    public class ViewManagerService
    {
        private const string _STRDEFAULT = "_Default";

        private static Dictionary<string, ViewManager> _viewManagers = new Dictionary<string, ViewManager>();

        private static ViewManagerService _instance;

        private static Dictionary<string, ViewManager> ViewManagers
        {
            get { return _viewManagers; }
        }

        public static ViewManagerService CreateInstance()
        {
            return _instance ?? (_instance = new ViewManagerService());
        }

        public ViewManager Add(Panel objRootPanel, AnimateTransitionCallBackDelegate objAnimateTransition)
        {
            return Add(objRootPanel, objAnimateTransition, _STRDEFAULT);
        }

        public ViewManager Add(Panel objRootPanel, AnimateTransitionCallBackDelegate objAnimateTransition, string strViewManagerKey)
        {

            if (string.IsNullOrEmpty(strViewManagerKey))
            {
                throw new ArgumentNullException("strViewManagerKey", "It is null");
            }

            if (objRootPanel == null)
            {
                throw new ArgumentNullException("objRootPanel", "Root panel is null");
            }


            lock (ViewManagers)
            {
                if (_viewManagers.ContainsKey(strViewManagerKey))
                {
                    throw new ArgumentException("Already exist", "strViewManagerKey");
                }

                ViewManagers.Add(strViewManagerKey, ViewManager.Create(objRootPanel, objAnimateTransition));
                return ViewManagers[strViewManagerKey];
            }
        }

        public ViewManager Select()
        {
            return Select(_STRDEFAULT);
        }

        public ViewManager Select(string strViewManagerKey)
        {
            return ViewManagers.ContainsKey(strViewManagerKey)
                ? ViewManagers[strViewManagerKey]
                : null;
        }
    }
}
