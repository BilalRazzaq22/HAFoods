   M S B A M L   `   `   `  ����H  DERP.WpfClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:ERP.WpfClientERP.WpfClient  5   	       ERP.WpfClient.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:ERP.WpfClient     5   	   X TXceed.Wpf.Toolkit, Version=4.0.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44wpfx)http://schemas.xceed.com/wpf/xaml/toolkit  63   b ^MaterialDesignThemes.Wpf, Version=4.0.0.2422, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  6j     � Title$  HAFoods��5   	   $	��780��6   $
��1024��6&    � WindowStartupLocation$ CenterScreen=�63    � WindowState$ 	Maximized=�6X    � Icon$# Resources/Images/mainLogo.ico��6p   .��5	      � 
������� � ColumnDefinitions 5
   
   �� 5      $	��180 �6   �� 5      $��* �6    � RowDefinitions 5   
   �� 5      $��50 �6   �� 5      $��* �6   .+�5   
   � $��0��6   $��2��6   $��0��6-    5      �� 5      $	��100 �6    �� 5      $��* �6    .+�5      ��  �� Orientation$ 
Horizontal=�6   $��Center=�62   $��Center=�6M   .+�5      �� $%��Resources/Images/HighEnd@4x.png��6   $	��120��6A   5      �� 5      $��1��6   
���   �$��0 0 0 0q�69   ._�5      �� $��1��6   .+�5      	�    LeftMenuButton#��[  6"    d� 	IsChecked$
 TrueY�6J   $
��Home��6[   $
��Home��6�   $y�Hidden=�6�   ��6j   �� *MainViewCommand+6�   	� 5      #��[  6"   $��Current Transaction��6J   $��Current Transaction��6�   ��6h   �� *MainViewCommand+6�   	� 5      #��[  6"   $��	Customers��6J   $��Customer��6�   ��6^   �� *MainViewCommand+6�   	� 5      #��[  6"   $��	Suppliers��6J   $��Supplier��6�   ��6^   �� *MainViewCommand+6�   	� 5       #��[  6"   $��Stock��6J   $��Stock��6~   ��6Z   �� *MainViewCommand+6~   	� 5!      #��[  6"   $��Purchase Orders��6J   $��Purchase Order��6�   ��6d   �� *MainViewCommand+6�   	� 5"      #��[  6"   $��Sales Orders��6J   $��Sales Order��6�   $y�	Collapsed=�6�   ��6a   �� *MainViewCommand+6�   	� 5#      #��[  6"   $��	Cash Book��6J   $��	Cash Book��6�   ��6^   �� *MainViewCommand+6�   	� 5$      #��[  6"   $��Cash Book 2��6J   $��Cash Book 2��6�   ��6`   �� *MainViewCommand+6�   	� 5%      #��[  6"   $��All Reports��6J   $��Reports��6�   ��6`   �� *MainViewCommand+6�   	� 5&      #��[  6"   $
��User��6J   $
��User��6}   ��6Y   �� *MainViewCommand+6}   5'      5(      5)      �� 5*   
   $��0��6   $��1��6"   .+�5+      � .+�5.      � $��50��6   $	��Top=�6#   
������� J� Effect 5/      3  -System.Windows.Media.Effects.DropShadowEffect  50      	   
BlurRadius$	 1Q�6+   
   ShadowDepth$
 1Q�6:      Color$ 	#FFACACAC��6J   .+�52      �� $ 
Horizontal=�6!   $��Right=�6:   .+�53      �� -   5         ��Name$ txtLoginUser��53   $   $��Center=�6:   $��16(�6U   $��Center=�6c   $��10 0 5 0q�6�   '  !MaterialDesignThemes.Wpf.PopupBox  54         PlacementMode$ BottomAndAlignRightEdges=�62      	StaysOpen .  $��10 0 5 0q�6m      PopupContent. 55      �� $��1q�6%   
���   �._�56   "   �� $	��150��6-   
�������.+�5;   &   �� -   5         ��Name$ logout��5;   -   $��Logout��6=   6N   5<   #   5=      5>      5?      �� 5@      $��Center=�6    $��Center=�6=   $��22(�6X   $��H.A Enterprises��6f   6   5A      5E      5F      � 5G   
   $��1��6   $��1��6   .+�5H      � -   5         �Name$ MainControlsHost��5H      
�������.+�5I      � -   5        $ PopupContainer��5I       K� ZIndex$	 198��6/   $y�Visible=�6B   .+�5J      �� -   5         ��Name$ Overlay��5J       $y�Visible=�61   ���	#88000000 J� Opacity$ 0.495Q�6X   �� 5K      
�������$��0q�60    �� CornerRadius$ 3��6D   $��Center=�6U   $��Center=�6r   ._�5L      � $	��400��6    5M      �� 5N   "   $
��Auto �60   �� 5O   "   $
��Auto �60   .+�5Q      �� -   5         ��Name$ PopupTitleContainer��5Q   B   $ 3��6%   $��60��66   ._�5R   "   � .+�5S   &   �� -   5        $ 
PopupTitle��5S   E   $	��Top=�60    �� TextAlignment$ Center=�6[   $��Center=�6r   $��20(�6�   $
��Left=�5T      $��Popup Title��64   $��WordEllipsis=�6G   $
��Auto��6c   $��NoWrap=�5U      $��10,0,0,0q�6.    �� Padding$ 5,0,0,0q�6@   $
��Bold"�6R   �� 5X   &   -   5        $ btnClose��5X   k   $��Right=�6-   $��0,0,15,0q�6I    � Cursor$
 Hand��6[   $��30��6   $��40��6�   ��5Y   *   �� 5Z   .    �� 
TargetType" ��6>   .R�5[   2   � .+�5^   6   �� $��Center=�6G   $��Center=�6d   5_   3   5`   /   5b   '   5h   #   5j      � 5l      -	   5        $ PopupControlHost��5l   B   $��1��6#   $��5,25,5,3q�60   .+�5n   "   �� Hellow64   5o      5q      5r      5s      � 5u      -
   5        $ PopupContainer2��5u      $y�	Collapsed=�61   $	 199��6H   .+�5v      �� -   5        $ Overlay2��5v       $y�Visible=�62   ���	#88000000$ 0.495Q�6Y   �� 5w      $��0q�6   $ 0��62   $��Center=�6C   $��Center=�6`   ._�5x      � $	��400��6    5y      �� 5z   "   $
��Auto �60   �� 5{   "   $
��Auto �60   .+�5}      �� -   5        $ PopupTitleContainer2��5}   M   $ 0��6%   $��60��66   $��0q�6B   ._�5~   "   � .+�5   &   �� -   5        $ PopupTitle2��5   E   $	��Top=�60   $ Center=�6\   $��Center=�6s   $��20(�6�   $
��Left=�5�      $��Popup Title��64   $��WordEllipsis=�6G   $
��Auto��6c   $��NoWrap=�5�      $��10,0,0,0q�6.   $ 5,0,0,0q�6@   $
��Bold"�6R   �� 5�   &   -   5        $ 	btnClose2��5�   j   $��Right=�6-   $��0,0,15,0q�6I   $
 Hand��6[   $��30��6   $��40��6�   ��5�   *   �� 5�   .   " ��6>   .R�5�   2   � .+�5�   6   �� $��Center=�6G   $��Center=�6d   5�   3   5�   /   5�   '   5�   #   5�      � 5�      -   5        $ PopupControlHost2��5�   B   $��1��6#   $��5,25,5,3q�60   .+�5�   "   �� Hellow64   5�      5�      5�      5�      � 5�      -   5        $ MsgPopupContainer��5�      $	��400��62   $y�	Collapsed=�6A   $	 201��6X   .+�5�      �� -   5        $ 
MsgOverlay��5�       $y�Visible=�64   ���	#88000000$ 0.495Q�6[   �� 5�      $��0q�6   $ 0��62   $��Center=�6C   $��Center=�6`   ._�5�      �  5�      �� 5�   "   $
��Auto �60   �� 5�   "   $
��Auto �60   .+�5�      �� -   5        $ MsgPopupTitleContainer��5�   M   $ 0��6%   $��60��66   $��0q�6B   ._�5�   "   �� $ 
Horizontal=�6-   .+�5�   &   �� -   5        $ MsgPopupTitle��5�   E   $	��Top=�60   $ Center=�6^   $��Center=�6u   $��20(�6�   $��Stretch=�5�      $��0q�66   $��	helvetica)�6B   $��Popup Title��6Y   $��WordEllipsis=�6l   $	��400��6�   $��NoWrap=�5�      $ 5,0,0,0q�6.   �� 5�   &   -   5        $ MsgBtnClose��5�   J   $��Right=�6-   $��60��6`   $��0,0,0,0q�6k   $��60��6}   $��0q�6�   5�   '   5�   #   5�      � 5�      -   5        $ MsgPopupControlHost��5�   B   $��1��6#   $��5,25,5,3q�60   .+�5�   "   �� Hellow64   5�      5�      5�      5�      5�      %  Xceed.Wpf.Toolkit.BusyIndicator  5�      -   5        $��BusyBar��5�   !      IsBusy 60   �� *ApplicationManager.IsBusy+5�      5�      5�      