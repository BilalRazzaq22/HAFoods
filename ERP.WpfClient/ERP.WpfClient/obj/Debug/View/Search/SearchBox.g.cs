﻿#pragma checksum "..\..\..\..\View\Search\SearchBox.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "86E4CED966EA7F8DBDF1F26459E51A18F5AC3AFD0148AC12FD0EFF7D1616ED4D"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using ERP.WpfClient.View.Search;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace ERP.WpfClient.View.Search {
    
    
    /// <summary>
    /// SearchBox
    /// </summary>
    public partial class SearchBox : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 9 "..\..\..\..\View\Search\SearchBox.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid LayoutRoot;
        
        #line default
        #line hidden
        
        
        #line 11 "..\..\..\..\View\Search\SearchBox.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border _border;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\..\..\View\Search\SearchBox.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image _imgSearch;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\..\..\View\Search\SearchBox.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox _txtSearch;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\..\..\View\Search\SearchBox.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image _imgClear;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/ERP.WpfClient;component/view/search/searchbox.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\View\Search\SearchBox.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 7 "..\..\..\..\View\Search\SearchBox.xaml"
            ((ERP.WpfClient.View.Search.SearchBox)(target)).Loaded += new System.Windows.RoutedEventHandler(this.UserControl_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.LayoutRoot = ((System.Windows.Controls.Grid)(target));
            return;
            case 3:
            this._border = ((System.Windows.Controls.Border)(target));
            return;
            case 4:
            this._imgSearch = ((System.Windows.Controls.Image)(target));
            return;
            case 5:
            this._txtSearch = ((System.Windows.Controls.TextBox)(target));
            
            #line 29 "..\..\..\..\View\Search\SearchBox.xaml"
            this._txtSearch.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this._txtSearch_TextChanged);
            
            #line default
            #line hidden
            
            #line 29 "..\..\..\..\View\Search\SearchBox.xaml"
            this._txtSearch.MouseEnter += new System.Windows.Input.MouseEventHandler(this._txtSearch_MouseEnter);
            
            #line default
            #line hidden
            return;
            case 6:
            this._imgClear = ((System.Windows.Controls.Image)(target));
            
            #line 34 "..\..\..\..\View\Search\SearchBox.xaml"
            this._imgClear.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this._imgClear_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
