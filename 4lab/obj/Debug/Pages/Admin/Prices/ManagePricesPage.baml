   M S B A M L   `   `   `  ����  4lab/    )_4lab.Pages.Admin.Prices.ManagePricesPage   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5        N� Title$  Manage Prices��5      .��5
      � $��20q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $8��2Управление ценами подписок��6   $��24(�5      $
��Bold"�6"   $��0,0,0,20q�64   �� 5   
   $��1��6   $	��300��6"   .+�5      �� $��Lite:��6   $	��0,5q�6%   �� 5      -   5         ��Name$ LitePriceBox��5      ��6,   �� *	LitePrice+ �� UpdateSourceTrigger$ PropertyChanged=��� 5      $��SemiPro:��6   $	��0,5q�6(   �� 5      -   5        $ SemiProPriceBox��5      ��6/   �� *SemiProPrice+$ PropertyChanged=��� 5      $
��Pro:��6   $	��0,5q�6$   �� 5      -   5        $ ProPriceBox��5      ��6+   �� *
ProPrice+$ PropertyChanged=�5      �� 5   
   -   5        $��2��5      $!��Сохранить цены��6   $
��0,20q�5!      $
��10,5q�6   $
��Left=�6.   5"      5#      