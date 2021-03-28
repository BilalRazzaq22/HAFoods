using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ERP.Common
{
    public class ViewManager
    {
        private Panel _rootPanel;
        private AnimateTransitionCallBackDelegate _animateTransition;
        private ObservableCollection<ViewFolder> _views = new ObservableCollection<ViewFolder>();
        private Stack<FrameworkElement> _currentViewStack = new Stack<FrameworkElement>();

        private Panel RootPanel
        {
            get { return _rootPanel; }
            set { _rootPanel = value; }
        }

        private ObservableCollection<ViewFolder> Views
        {
            get { return _views; }
        }

        private FrameworkElement CurrentView
        {

            get
            {
                if (_currentViewStack.Count == 0)
                {
                    return null;

                }
                else
                {
                    return _currentViewStack.Peek();
                }

            }
            set
            {
                _currentViewStack.Clear();

                if (value != null)
                {
                    _currentViewStack.Push(value);
                }

            }
        }

        private ViewManager(Panel rootPanel, AnimateTransitionCallBackDelegate animateTransition)
        {
            this.RootPanel = rootPanel;
            _animateTransition = animateTransition;
        }

        public static ViewManager Create(Panel rootPanel, AnimateTransitionCallBackDelegate animateTransition)
        {
            if (rootPanel == null)
            {
                throw new ArgumentNullException("rootPanel", "No Panel");
            }

            return new ViewManager(rootPanel, animateTransition);
        }

        public void Transition<T>(ViewTypes viewTypeKey) where T : FrameworkElement, new()
        {
            this.Transition<T>(new NavigateKey(viewTypeKey), null);
        }

        public void Transition<T>(NavigateKey navigateKey, object objDataContext) where T : FrameworkElement, new()
        {
            if (navigateKey == null)
            {
                throw new ArgumentNullException("navigateKey", "");
            }

            //Console.WriteLine("{0} Finding View {1}", DateTime.Now,navigateKey.ViewKey);
            FrameworkElement fwe = GetView(navigateKey);
            bool bolCreated = false;

            //Console.WriteLine("{0} View Not Found", DateTime.Now);
            if (fwe == null)
            {
                //Console.WriteLine("{0} Creating Instance of View {1}", DateTime.Now, navigateKey.ViewKey);
                this.Views.Add(new ViewFolder(navigateKey, new T()));
                bolCreated = true;
                // Console.WriteLine("{0} Finding View {1}", DateTime.Now, navigateKey.ViewKey);
                fwe = this.GetView(navigateKey);


            }


            if (fwe.DataContext is IViewClient)
            {
                ((IViewClient)fwe.DataContext).NavigateKey = navigateKey;
            }



            // Console.WriteLine("{0} Performing Trasition", DateTime.Now);
            //
            //NEVER MOVE THIS LINE OF CODE
            PerformTransition(navigateKey);

            //NEVER MOVE THIS LINE OF CODE
            //
            if (navigateKey.PrimaryKey != null && objDataContext is ILoadable)
            {
                ((ILoadable)objDataContext).LoadRecord(navigateKey.PrimaryKey);

            }
            else if (bolCreated && fwe.DataContext is ILoadable)
            {
                ((ILoadable)fwe.DataContext).LoadRecord(navigateKey.PrimaryKey);

            }
            else if (navigateKey.PrimaryKey != null && fwe is ILoadable)
            {
                ((ILoadable)fwe).LoadRecord(navigateKey.PrimaryKey);
            }
            else if (navigateKey.PrimaryKey != null && fwe.DataContext is ILoadable)
            {
                ((ILoadable)fwe.DataContext).LoadRecord(navigateKey.PrimaryKey);
            }


            if (navigateKey.PrimaryKey != null && fwe.DataContext is ICriteriaView)
            {
                ((ICriteriaView)fwe.DataContext).Load(navigateKey.PrimaryKey);
            }

            if (objDataContext != null)
            {
                fwe.DataContext = objDataContext;

                if (fwe.DataContext is IViewClient)
                {
                    ((IViewClient)fwe.DataContext).NavigateKey = navigateKey;
                }



            }

        }


        //public void OrderTransition<T>(NavigateKey navigateKey) where T : FrameworkElement, new()
        //{
        //    if (navigateKey == null)
        //    {
        //        throw new ArgumentNullException("navigateKey", "");
        //    }

        //    var caseId = 0;
        //    var viewToLoad = OrderViewTypes.OrderInformation;

        //    if (navigateKey.PrimaryKey is EditOrderParamModel)
        //    {
        //        var pk = (navigateKey.PrimaryKey as EditOrderParamModel);
        //        caseId = pk.OrderId;
        //        viewToLoad = pk.LoadView;
        //    }
        //    else
        //    {
        //        caseId = (int)navigateKey.PrimaryKey;
        //    }

        //    FrameworkElement fwe = GetView(navigateKey.ViewKey);
        //    bool boolCreated = false;

        //    EditOrderViewModel model = null;

        //    if (fwe == null)
        //    {
        //        this.Views.Add(new ViewFolder(navigateKey, new T()));
        //        boolCreated = true;
        //        fwe = this.GetView(navigateKey);

        //        if (fwe.DataContext == null && navigateKey.ViewKey == ViewTypes.EditOrder)
        //        {
        //            model = ApplicationManager.Instance.GetCaseModel(caseId);

        //            if (model == null)
        //            {
        //                model = new EditOrderViewModel(/*fwe as ILoadUserControl*/);
        //                ApplicationManager.Instance.CurrentOrders.Add(model);
        //            }

        //            fwe.DataContext = model;
        //        }
        //    }
        //    else
        //    {
        //        fwe.Visibility = Visibility.Visible;
        //    }

        //    if (fwe.DataContext is IViewClient)
        //    {
        //        ((IViewClient)fwe.DataContext).NavigateKey = navigateKey;
        //    }

        //    //
        //    //NEVER MOVE THIS LINE OF CODE
        //    PerformTransition(navigateKey);

        //    //NEVER MOVE THIS LINE OF CODE
        //    //
        //    if (navigateKey.PrimaryKey != null && fwe.DataContext is ILoadable && !boolCreated)
        //    {


        //        model = ApplicationManager.Instance.CurrentOrders.FirstOrDefault(c => c.Id == caseId);

        //        if (model != null)
        //        {
        //        //TODO:
        //            //model.SetCaseView(fwe as ILoadUserControl);
        //            fwe.DataContext = model;

        //            //TODO:
        //            //model.ShowUserControl(OrderViewTypes.CaseInformation.ToString());

        //            return;
        //        }
        //        else
        //        {
        //            model = new EditOrderViewModel(/*fwe as ILoadUserControl*/);

        //            (model as IViewClient).NavigateKey = navigateKey;

        //            ApplicationManager.Instance.CurrentOrders.Add(model);
        //            fwe.DataContext = model;


        //            ((ILoadable)fwe.DataContext).LoadRecord(navigateKey.PrimaryKey);

        //        }


        //    }
        //    else if (boolCreated && fwe.DataContext is ILoadable)
        //    {
        //        ((ILoadable)fwe.DataContext).LoadRecord(navigateKey.PrimaryKey);
        //    }

        //    if (navigateKey.PrimaryKey != null && fwe.DataContext is ICriteriaView)
        //    {
        //        ((ICriteriaView)fwe.DataContext).Load(navigateKey.PrimaryKey);
        //    }

        //    if (model != null)
        //    {
        //        //TODO:
        //        //model.RefreshSubModelBindings();
        //        //model.ShowUserControl(viewToLoad.ToString());
        //    }

        //}

        public void Hide(NavigateKey navigateKey, NavigateKey loadViewOnRemoveKey = null)
        {

            if (navigateKey == null)
            {
                throw new ArgumentNullException("objNavigationKey", "Navigate key is null");
            }

            if (this.ContainsView(navigateKey.ViewKey) && this.RootPanel.Children.Contains(this.GetView(navigateKey.ViewKey)))
            {
                this.GetView(navigateKey.ViewKey).Visibility = Visibility.Collapsed;
            }

            if (navigateKey.ParentNavigateKey != null)
            {
                if (this.ContainsView(navigateKey.ParentNavigateKey))
                {
                    PerformTransition(navigateKey.ParentNavigateKey);

                    var fwe = this.GetView(navigateKey.ParentNavigateKey);

                    if (fwe != null)
                    {
                        if (navigateKey.ParentNavigateKey.PrimaryKey != null && fwe.DataContext is ILoadable)
                        {
                            ((ILoadable)fwe).LoadRecord(navigateKey.ParentNavigateKey.PrimaryKey);
                        }

                        if (navigateKey.ParentNavigateKey.PrimaryKey != null && fwe.DataContext is ICriteriaView)
                        {
                            ((ICriteriaView)fwe.DataContext).Load(navigateKey.ParentNavigateKey.PrimaryKey);
                        }
                    }
                }

            }
        }

        //        public bool Remove(NavigateKey navigateKey, ViewTypes loadViewOnRemoveKey = ViewTypes.None)
        public bool Remove(NavigateKey navigateKey, NavigateKey loadViewOnRemoveKey = null)
        {

            if (navigateKey == null)
            {
                throw new ArgumentNullException("objNavigationKey", "Navigate key is null");
            }

            if (this.ContainsView(navigateKey) && this.RootPanel.Children.Contains(this.GetView(navigateKey)))
            {
                this.RootPanel.Children.Remove(this.GetView(navigateKey));
            }

            bool bolReturn = false;
            ViewFolder objViewFolder = GetViewFolder(navigateKey);

            if (objViewFolder != null)
            {
                bolReturn = this.Views.Remove(objViewFolder);
            }

            //
            Debug.Assert(bolReturn, "View not removed");
            //
            //TODO - developers - you can exit the function here if it fails to remove the requested view. (bolReturn = False)
            //
            //this does help keep memory down in WPF apps by forcing the GC to keep things tidy, I find a little help, helps
            GC.Collect(3, GCCollectionMode.Forced);


            if (loadViewOnRemoveKey != null)
            {
                //NavigateKey objLoadViewNavigationKey = new NavigateKey(loadViewOnRemoveKey);

                if (this.ContainsView(loadViewOnRemoveKey))
                {
                    PerformTransition(loadViewOnRemoveKey);

                    var fwe = this.GetView(loadViewOnRemoveKey);

                    if (fwe != null)
                    {
                        if (loadViewOnRemoveKey.PrimaryKey != null && fwe.DataContext is ILoadable)
                        {
                            ((ILoadable)fwe).LoadRecord(loadViewOnRemoveKey.PrimaryKey);
                        }

                        if (navigateKey.PrimaryKey != null && fwe.DataContext is ICriteriaView)
                        {
                            ((ICriteriaView)fwe.DataContext).Load(loadViewOnRemoveKey.PrimaryKey);
                        }
                    }

                    return bolReturn;
                }

            }


            if (navigateKey.ParentNavigateKey != null)
            {
                if (this.ContainsView(navigateKey.ParentNavigateKey))
                {
                    PerformTransition(navigateKey.ParentNavigateKey);

                    var fwe = this.GetView(navigateKey.ParentNavigateKey);

                    if (fwe != null)
                    {
                        //if (navigateKey.ParentNavigateKey.PrimaryKey != null && fwe.DataContext is ILoadable && navigateKey.ParentNavigateKey.ViewKey != ViewTypes.EditOrder)
                        //{
                        //    ((ILoadable)fwe.DataContext).LoadRecord(navigateKey.ParentNavigateKey.PrimaryKey);
                        //}

                        if (navigateKey.ParentNavigateKey.PrimaryKey != null && fwe.DataContext is ICriteriaView)
                        {
                            ((ICriteriaView)fwe.DataContext).Load(navigateKey.ParentNavigateKey.PrimaryKey);
                        }
                    }
                    return bolReturn;
                }

            }

            return bolReturn;
        }

        public bool RemoveByViewType(NavigateKey navigateKey, ViewTypes viewType = ViewTypes.None)
        {

            if (navigateKey == null)
            {
                throw new ArgumentNullException("objNavigationKey", "Navigate key is null");
            }

            if (this.ContainsView(navigateKey) && this.RootPanel.Children.Contains(this.GetView(navigateKey)))
            {
                this.RootPanel.Children.Remove(this.GetView(navigateKey));
            }

            bool bolReturn = false;
            ViewFolder objViewFolder = GetViewFolder(navigateKey);

            if (objViewFolder != null)
            {
                bolReturn = this.Views.Remove(objViewFolder);
            }

            //
            Debug.Assert(bolReturn, "View not removed");
            //
            //TODO - developers - you can exit the function here if it fails to remove the requested view. (bolReturn = False)
            //
            //this does help keep memory down in WPF apps by forcing the GC to keep things tidy, I find a little help, helps
            GC.Collect(3, GCCollectionMode.Forced);


            if (viewType != ViewTypes.None)
            {
                //NavigateKey objLoadViewNavigationKey = new NavigateKey(loadViewOnRemoveKey);

                if (this.ContainsView(viewType))
                {
                    PerformTransition(new NavigateKey(viewType));
                    return bolReturn;
                }
            }

            if (navigateKey.ParentNavigateKey != null)
            {
                if (this.ContainsView(navigateKey.ParentNavigateKey))
                {
                    PerformTransition(navigateKey.ParentNavigateKey);
                    return bolReturn;
                }

            }

            return bolReturn;
        }

        private void PerformTransition(NavigateKey objNavigateKey)
        {
            FrameworkElement fweNewView = this.GetView(objNavigateKey);
            FrameworkElement objCurrentView = this.CurrentView;

            if (fweNewView == null)
            {
                //if (objNavigateKey.ViewKey == ViewTypes.EditOrder)
                //{
                //    fweNewView = this.GetView(objNavigateKey.ViewKey);
                //}

                if (fweNewView == null)
                    return;
            }

            if (objCurrentView != null && objCurrentView.Equals(fweNewView))
            {
                //Messenger.Default.Send(new BusyIndicatorMessage { IsBusy = false });
                return;
            }

            if (!this.RootPanel.Children.Contains(fweNewView))
            {
                this.RootPanel.Children.Add(fweNewView);
            }

            foreach (FrameworkElement obj in this.RootPanel.Children)
            {
                if (!object.ReferenceEquals(obj, fweNewView) && !object.ReferenceEquals(obj, objCurrentView))
                {
                    if (obj.Visibility == Visibility.Visible)
                    {
                        obj.Visibility = Visibility.Collapsed;
                    }
                }
            }

            fweNewView.Visibility = Visibility.Visible;

            if (this.CurrentView != null)
            {
                objCurrentView.SetValue(Panel.ZIndexProperty, 0);
            }

            fweNewView.SetValue(Panel.ZIndexProperty, 99);
            fweNewView.Visibility = Visibility.Visible;
            fweNewView.Opacity = 1;
            //
            //NEVER REMOVE OR CHANGE THESE LINES OF CODE
            this.RootPanel.InvalidateVisual();
            fweNewView.UpdateLayout();
            //NEVER REMOVE OR CHANGE THESE LINES OF CODE

            //if (_animateTransition != null)
            //{
            //    //_animateTransition(objCurrentView, fweNewView);
            //}
            //else
            //{
            if (objCurrentView != null)
            {
                objCurrentView.Visibility = Visibility.Collapsed;
            }
            //}

            this.CurrentView = fweNewView;

            if (fweNewView is INotifyOnBringIntoView)
            {
                ((INotifyOnBringIntoView)fweNewView).OnBringIntoView();
            }

            if (fweNewView.DataContext is INotifyOnBringIntoView)
            {
                ((INotifyOnBringIntoView)fweNewView.DataContext).OnBringIntoView();
            }

        }

        public bool ContainsView(NavigateKey objNavigateKey)
        {
            return this.Views.SingleOrDefault(f => f.NavigateKey.Equals(objNavigateKey)) != null;
        }

        public bool ContainsView(ViewTypes viewKey)
        {
            return this.Views.SingleOrDefault(f => f.NavigateKey.ViewKey.Equals(viewKey)) != null;
        }

        public ViewTypes GetCurrentViewKey()
        {

            NavigateKey obj = GetNavigateKey(this.CurrentView);

            if (obj != null)
            {
                return obj.ViewKey;

            }
            else
            {
                return ViewTypes.None;
            }

        }

        public NavigateKey GetCurrentNavigateKey()
        {
            return GetNavigateKey(CurrentView);
        }

        public NavigateKey GetNavigateKey(NavigateKey objNavigateKey)
        {

            ViewFolder obj = this.Views.Where(f => f.NavigateKey.Equals(objNavigateKey)).SingleOrDefault();

            if (obj != null)
            {
                return obj.NavigateKey;

            }
            return null;
        }

        public NavigateKey GetNavigateKey(FrameworkElement fwe)
        {

            ViewFolder obj = this.Views.Where(f => object.ReferenceEquals(f.View, fwe)).SingleOrDefault();

            if (obj != null)
            {
                return obj.NavigateKey;

            }
            else
            {
                return null;
            }

        }

        private FrameworkElement GetView(NavigateKey navigateKey)
        {

            ViewFolder obj = this.Views.Where(f => f.NavigateKey.Equals(navigateKey)).SingleOrDefault();

            if (obj != null)
            {
                return obj.View;

            }
            else
            {
                return null;
            }

        }


        private FrameworkElement GetView(ViewTypes viewKey)
        {

            ViewFolder obj = this.Views.Where(f => f.NavigateKey.ViewKey.Equals(viewKey)).SingleOrDefault();

            if (obj != null)
            {
                return obj.View;

            }
            else
            {
                return null;
            }

        }

        private ViewFolder GetViewFolder(NavigateKey objNavigateKey)
        {
            return this.Views.SingleOrDefault(f => f.NavigateKey.Equals(objNavigateKey));
        }

        private ViewFolder GetViewFolder(ViewTypes viewKey)
        {
            return this.Views.SingleOrDefault(f => f.NavigateKey.ViewKey.Equals(viewKey));
        }

    }
}
