﻿

/*===================================================================================
* 
*   Copyright (c) Userware/OpenSilver.net
*      
*   This file is part of the OpenSilver Runtime (https://opensilver.net), which is
*   licensed under the MIT license: https://opensource.org/licenses/MIT
*   
*   As stated in the MIT license, "the above copyright notice and this permission
*   notice shall be included in all copies or substantial portions of the Software."
*  
\*====================================================================================*/


using System;

#if MIGRATION
namespace System.Windows.Controls
#else
namespace Windows.UI.Xaml.Controls
#endif
{
#if WORKINPROGRESS
    public partial class TreeView : ItemsControl
    {
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(TreeView), null);

        public object SelectedItem
        {
            get { return (object)this.GetValue(TreeView.SelectedItemProperty); }
        }

#if MIGRATION
        public event RoutedPropertyChangedEventHandler<object> SelectedItemChanged;
#else
#endif
    }
#endif
}
