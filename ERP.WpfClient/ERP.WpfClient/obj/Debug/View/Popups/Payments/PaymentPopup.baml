   M S B A M L   `   `   `  ����  ERP.WpfClient5    /ERP.WpfClient.View.Popups.Payments.PaymentPopup   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      .��5      �    CommonWhiteBackgroundColor#��[  6     � RowDefinitions  5	   
   �� 5
      $��* �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $
��auto �6    � ColumnDefinitions 5   
   �� 5      �� 5      .+�5   
   �� $��20(�6    �� TextAlignment$ Center=�6"   $��Center=�69   $
��auto��6T   $
��auto��6b   $��Total Amount��6o   $��0��6�   $��0��6�   �� 5   
   $��20(�6A   $ Center=�6O   $��Center=�6f   $
��auto��6�   $
��auto��6�   $��1��6�   $��0��6�   ��6   �� *
GrandTotal+ �� StringFormat$ N2��6�   �� 5   
   $��20(�6   $ Center=�6"   $��Center=�69   $
��auto��6T   $
��auto��6b   $��Amount Paid��6o   $��0��6�   $��1��6�   �� 5   
   $��20(�6A   $ Center=�6O   $��Center=�6f   $
��auto��6�   $
��auto��6�   $��1��6�   $��1��6�   ��6   �� *
AmountPaid+$ N2��6�   �� 5   
   $��20(�6   $ Center=�6"   $��Center=�69   $
��auto��6T   $
��auto��6b   $��Remaining Amount��6o   $��0��6�   $��2��6�   �� 5   
   $��20(�6F   $ Center=�6T   $��Center=�6k   $
��auto��6�   $
��auto��6�   $��1��6�   $��2��6�   ��6   �� *RemainingAmount+$ N2��6�   � 5   
   $��3��6   $��0��6   $��2��6,   #��[  6@   $��Center=�6y    5      �� 5      $��* �6#   .+�5!      �� $��0��6     AcceptButton#��[ 6%   $��	New Order��6K   $��Right=�6_   $��	New Order��6{   ��6�   �� *CurrentOrderCommand+5#      5$      5%      