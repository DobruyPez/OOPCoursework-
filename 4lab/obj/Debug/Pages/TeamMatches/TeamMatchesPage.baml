   M S B A M L   `   `   `  ����?  ;4lab, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:_4lab.Pages.TeamMatches_4lab.Pages.TeamMatches  5      -    '_4lab.Pages.TeamMatches.TeamMatchesPage   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      7local%clr-namespace:_4lab.Pages.TeamMatches     5        N� Title$  TeamMatchesPage��5	       � 	Resources 5       �� MergedDictionaries �� Source    BooleanToVisibilityConverter�� 5   
    5      �� 5      $) #/Resources/Stules/GeneralStyle.xaml?�6%   �� 5      $&  /Resources/Stules/DarkTheme.xaml?�6%   %   &	        �� 5      6+   5      .��5      ]� ��. .+�5   
   � $	��Top=�6   $��60��6$   ���#995200 � ColumnDefinitions 5      �� 5      $��* �6#   �� 5      $
��Auto �6#   �� 5      $
��Auto �6#   �� 5      $
��Auto �6#   .+�5      �� -   5         ��Name$ 	SearchBox��5   &   $��0��6     SearchLabel#���  5       
�������
���   �$��14(�5"      $��
10,10,5,10q�6$   $��5q�68   5#      �� 5$      -   5         ��Name$ OfferTypeFilter��5$   '   $��1��6   $
��5,10q�5%      $	��150��6%   .@�5'      �� $	��All��6   �� 5(      $��OneToOne��6   �� 5)      $��TeamDeathMatch��6   5*      �� 5+      -   5        $��2��5+        SearchButton#���  6%     OrangeBlackButtonStyle#��[ 5,      $
��5,10q�5-      $
��10,5q�6#   5.      �� 5/      -   5        $��3��5/      $��Create Offer��6%   #��[ 50      $��
5,10,10,10q�51      $
��10,5q�6)   52      53      �� 56   
   
���   �$��10q�6(   .@�58      �� $��Other Offers��6   .��59      �� -   5         ��Name$ OtherMatchesList��59      $��Stretch=�5:      $��5q�6?   ��5;      �� 5<      .R�5=      �� ���#1A1A1A$��5q�6:    �� CornerRadius$ 5��6E   ._�5>   "   �  5?   &   �� 5@   *   $
��Auto �6;   �� 5A   *   $��* �6;   �� 5B   *   $
��Auto �6;   .+�5D   &   �� $��0��6,   $��80��5E   ,   $��80��67   $��10q�6C   ��5D   <   �� *	ImagePath+5E   C   �� 5F   &   $��1��61   $��10q�6A   .+�5G   *   �� 
�������$��16(�5H   G   $
��Bold"�6U   ��5G   4   �� *
TeamName+5H   U   �� 5I   *   
�������$��14(�5J   G   $��0,5,0,0q�6U   ��5I   4   �� *Maps+5J   U   �� 5K   *   
�������$��14(�5L   G   $��0,5,0,0q�6U   ��5K   4   �� *
DateTime+5L   U   5M   '   �� 5N   &   $��2��6-     JoinMatchButton#���  6=   #��[ 5O   -   $��10q�5P   -   $
��10,5q�69   ��5Q   -   �� *DataContext.MakeOfferCommand+	 �� RelativeSource	 �� 
 �� AncestorType$

 PageL���5R   -   �� y�5S   -   �� *	CanJoin+ �� 	Converter# [  5T   #   5U      5V      5X      5Y      �� 5\      $��	My Offers��6   .��5]      �� -   5        $ MyMatchesList��5]      $��Stretch=�5^      $��5q�6?   ��5_      �� 5`      .R�5a      �� ���#1A1A1A$��5q�6:   $ 5��6E   ._�5b   "   �  5c   &   �� 5d   *   $
��Auto �6;   �� 5e   *   $��* �6;   �� 5f   *   $
��Auto �6;   .+�5h   &   �� $��0��6,   $��80��5i   ,   $��80��67   $��10q�6C   ��5h   <   �� *	ImagePath+5i   C   �� 5j   &   $��1��61   $��10q�6A   .+�5k   *   �� 
�������$��16(�5l   G   $
��Bold"�6U   ��5k   4   �� *
TeamName+5l   U   �� 5m   *   
�������$��14(�5n   G   $��0,5,0,0q�6U   ��5m   4   �� *Maps+5n   U   �� 5o   *   
�������$��14(�5p   G   $��0,5,0,0q�6U   ��5o   4   �� *
DateTime+5p   U   5q   '   �� 5r   &   $��2��6-   #���  6=   #��[ 5s   -   $��10q�5t   -   $
��10,5q�69   ��5u   -   �� *DataContext.MakeOfferCommand+	 �� $

 PageL���5v   -   �� y�5w   -   �� *	CanJoin+# [  5x   #   5y      5z      5|      5}      5~      5      5�      