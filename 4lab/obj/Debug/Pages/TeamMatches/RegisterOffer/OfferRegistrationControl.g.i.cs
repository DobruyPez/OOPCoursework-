﻿#pragma checksum "..\..\..\..\..\Pages\TeamMatches\RegisterOffer\OfferRegistrationControl.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "049C972A99DD5D36F37554F1652A4183C29ACB7D8619E9AA5F59F739B8FC0376"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

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


namespace _4lab.Pages.TeamMatches.RegisterOffer {
    
    
    /// <summary>
    /// OfferRegistrationControl
    /// </summary>
    public partial class OfferRegistrationControl : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 286 "..\..\..\..\..\Pages\TeamMatches\RegisterOffer\OfferRegistrationControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox OfferTypeComboBox;
        
        #line default
        #line hidden
        
        
        #line 292 "..\..\..\..\..\Pages\TeamMatches\RegisterOffer\OfferRegistrationControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox MapsComboBox;
        
        #line default
        #line hidden
        
        
        #line 303 "..\..\..\..\..\Pages\TeamMatches\RegisterOffer\OfferRegistrationControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker DatePicker;
        
        #line default
        #line hidden
        
        
        #line 306 "..\..\..\..\..\Pages\TeamMatches\RegisterOffer\OfferRegistrationControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox TimePicker;
        
        #line default
        #line hidden
        
        
        #line 333 "..\..\..\..\..\Pages\TeamMatches\RegisterOffer\OfferRegistrationControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button RegisterOfferButton;
        
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
            System.Uri resourceLocater = new System.Uri("/4lab;component/pages/teammatches/registeroffer/offerregistrationcontrol.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\Pages\TeamMatches\RegisterOffer\OfferRegistrationControl.xaml"
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
            this.OfferTypeComboBox = ((System.Windows.Controls.ComboBox)(target));
            
            #line 286 "..\..\..\..\..\Pages\TeamMatches\RegisterOffer\OfferRegistrationControl.xaml"
            this.OfferTypeComboBox.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.OfferTypeComboBox_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            this.MapsComboBox = ((System.Windows.Controls.ListBox)(target));
            return;
            case 3:
            this.DatePicker = ((System.Windows.Controls.DatePicker)(target));
            return;
            case 4:
            this.TimePicker = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 5:
            this.RegisterOfferButton = ((System.Windows.Controls.Button)(target));
            
            #line 333 "..\..\..\..\..\Pages\TeamMatches\RegisterOffer\OfferRegistrationControl.xaml"
            this.RegisterOfferButton.Click += new System.Windows.RoutedEventHandler(this.RegisterOfferButton_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

