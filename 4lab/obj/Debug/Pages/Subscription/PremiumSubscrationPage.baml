   M S B A M L   `   `   `  ����?  ;4lab, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:_4lab_4lab  5      "    _4lab.PremiumSubscrationPage   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      %localclr-namespace:_4lab     5        N� Title$  PremiumSubscrationPage��5	       N� 
Background �#1A1A1A � 	Resources 5         TextBlockStyle �� 
TargetType �� Property �� Value3  -System.Windows.Media.Effects.DropShadowEffect   Color   ShadowDepth   
BlurRadius	   Opacity  HeaderTextBlockStyle
 �� BasedOn  PriceTextBlockStyle  StackPanelStyle  BorderStyle �� 
StartPoint �� EndPoint � Color � Offset  CustomButtonTemplate �� 
TargetType ��Name �� CornerRadius � Triggers :� RoutedEvent �� 
TargetName �� TargetProperty	 �� To g� Duration	 W� To g� AutoReverse �� CenterX �� CenterY  ButtonStyle��%�  &	        &	 z    7   &	 �    7   &	 d    &	 �    &	 �    &	 ?
    7  �� 5   
   " ��6'   .�5      ��  � �� �#FFFFFF6+   �� 5       � ��$ Segoe UI)�6+   �� 5       �@��TextAlignment$ Center=�6.   �� 5       � ��$
 Wrap=�6-   �� 5       � ��$ 0,5,0,0q�6'   �� 5       �@J�Effect 5        5      $ #000000��6'   $ 2Q�67   $ 4Q�6G   $		 0.3Q�6V   5      5      �� 5   
   8
   " ��6W   .�5      ��  � ��$ 24(�6)   �� 5       � ��$
 Bold"�6+   5      �� 5!   
   8
   " ��6V   .�5"      ��  � ��$ 18(�6)   �� 5#       � ��$ SemiBold"�6+   5$      �� 5'   
   " ��6(   .�5(      ��  � ��$ Center=�64   �� 5)       � ��$ Center=�62   �� 5*       � ��$ 20q�6'   5+      �� 5.   
   " ��6$   .�5/      ��  � ��$ 20q�6'   �� 50       �@��CornerRadius$ 15��6-   �� 51       � �� 52      �� 53      $	 0,0&�6*   $	 1,1&�6;   .��54      � $ #995200��6'   $ 0Q�67   � 55      $ #995200��6'   $ 1Q�67   56      58      �� 59       � ��
 �   �6,   �� 5:       � ��$ 1q�60   �� 5;       �@J�Effect 5<        5=      $ #000000��6'   $ 5Q�67   $ 10Q�6G   $		 0.5Q�6W   5?      5@      �� 5C   
   " ��67   .R�5D      �� $ ButtonBorder��6   $ 10��5E      ���#2A2A2A
���   �$��1q�5H      ._�5I      �� $��Center=�6#   $��Center=�6@   #��z��6[    5J      :� 5K      $ 
MouseEnter��6#   .[�5L      �� .��5M      �� .��5N   "   �� $ ButtonBorder��61   $1 +(Border.Background).(SolidColorBrush.Color)�5O   0   $ #4A4A4AX�5P   0   $ 0:0:0.3D�6=   W� 5Q   "   $ ButtonBorder��62   $ Opacity�5R   1   $	 0.9X�5S   1   $ 0:0:0.3D�6:   5T      5U      5V      :� 5W      $ 
MouseLeave��6#   .[�5X      �� .��5Y      �� .��5Z   "   �� $ ButtonBorder��61   $1 +(Border.Background).(SolidColorBrush.Color)�5[   0   $ #2A2A2AX�5\   0   $ 0:0:0.3D�6=   W� 5]   "   $ ButtonBorder��62   $ Opacity�5^   1   $ 1X�5_   1   $ 0:0:0.3D�68   5`      5a      5b      :� 5c      $ Button.Click��6#   .[�5d      �� .��5e      �� .��5f   "   W� $ ButtonBorder��62   $/ )(RenderTransform).(ScaleTransform.ScaleX)�5g   1   $
 0.95X�5h   1   $ 0:0:0.1D�6;    . 6N   W� 5i   "   $ ButtonBorder��62   $/ )(RenderTransform).(ScaleTransform.ScaleY)�5j   1   $
 0.95X�5k   1   $ 0:0:0.1D�6;    . 6N   5l      5m      5n      z�5p      �� 5q      $	 0.5Q�6%   $	 0.5Q�63   5s      5t      �� 5w   
   " ��6$   .�5x      ��  � ��8   6)   �� 5y       � �� �#FFFFFF6+   �� 5z       � ��$ Segoe UI)�6+   �� 5{       � ��$ 14(�6)   �� 5|       � ��$
 15,8q�6(   �� 5}       � ��$ 0,20,0,0q�6'   �� 5~       �@�Cursor$
 Hand��6'   5      5�      .��5�      � -   5         �Name$ MainGrid��5�       � ColumnDefinitions 5�   
   �� 5�      $��* �6   �� 5�      $��* �6   �� 5�      $��* �6   .+�5�   
   �� $��0��6   #��[ 6!   ._�5�      �� #��[ 6   .+�5�      ��   LitePlan#���  6   #��[ 6>   �� 5�        	LitePrice#���  6   #��[ 6?   �� 5�       	 LiteFeatures#��� 	 6   #��[  6B   �� 5�       
 	LiteUsers#��� 
 6   #��[  6?   �� 5�        LiteSupport#���  6   #��[  6A   �� 5�      -   5         ��Name$ ChooseLiteButton��5�        
ChooseLite#���  5�      #��[ 5�      5�      5�      5�      �� 5�   
   $��1��6   #��[ 6!   ._�5�      �� #��[ 6   .+�5�      ��   StandardPlan#���  6   #��[ 6B   �� 5�        StandardPrice#���  6   #��[ 6C   �� 5�        StandardFeatures#���  6   #��[  6F   �� 5�        StandardUsers#���  6   #��[  6C   �� 5�        StandardSupport#���  6   #��[  6E   �� 5�      -   5        $ ChooseStandardButton��5�        ChooseStandard#���  5�      #��[ 5�      5�      5�      5�      �� 5�   
   $��2��6   #��[ 6!   ._�5�      �� #��[ 6   .+�5�      ��   ProPlan#���  6   #��[ 6=   �� 5�        ProPrice#���  6   #��[ 6>   �� 5�        ProFeatures#���  6   #��[  6A   �� 5�        ProUsers#���  6   #��[  6>   �� 5�        
ProSupport#���  6   #��[  6@   �� 5�      -   5        $ ChooseProButton��5�        	ChoosePro#���  5�      #��[ 5�      5�      5�      5�      5�      5�      