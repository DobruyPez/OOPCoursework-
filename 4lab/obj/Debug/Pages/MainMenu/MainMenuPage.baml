   M S B A M L   `   `   `  ����  4lab    _4lab.MainMenuPage   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5        N� 
Background  �#1A1A1A � 	Resources 5         TextBlockStyle �� 
TargetType �� Property �� Value3  -System.Windows.Media.Effects.DropShadowEffect   Color   ShadowDepth   
BlurRadius   Opacity  IconTextBlockStyle	 �� BasedOn  StackPanelStyle  CustomButtonTemplate
 �� 
TargetType ��Name �� CornerRadius � Triggers :� RoutedEvent �� 
TargetName �� TargetProperty	 �� To g� Duration	 W� To g� AutoReverse �� CenterX �� CenterY  ButtonStyle��%w	  &	        &	 w    7   &	 �    &	     &	 �    7  �� 5
   
   " ��6'   .�5      ��  � ��
 �   �6+   �� 5       � ��$ Segoe UI)�6+   �� 5       �@��TextAlignment$ Center=�6.   �� 5       � ��$
 Wrap=�6-   �� 5       � ��$ 0,10,0,0q�6'   �� 5       �@J�Effect 5        5      $ #000000��6'   $ 2Q�67   $ 4Q�6G   $	 0.3Q�6V   5      5      �� 5   
   8	   " ��6U   .�5      ��  � ��$ 48(�6)   �� 5       � ��$ 0q�6'   5      �� 5   
   " ��6(   .�5      ��  � ��$ Center=�64   �� 5        � ��$ Center=�62   �� 5!       � ��$ 20q�6'   5"      �� 5$   
   "
 ��67   .R�5%      �� $ ButtonBorder��6   $ 10��5&      ���#995200
���   �$��1q�5)      ._�5*      �� $��Center=�6#   $��Center=�6@   #��z��6[    5+      :� 5,      $ 
MouseEnter��6#   .[�5-      �� .��5.      �� .��5/   "   �� $ ButtonBorder��61   $1 +(Border.Background).(SolidColorBrush.Color)�50   (   $ #4A4A4AX�51   (   $ 0:0:0.3D�65   W� 52   "   $ ButtonBorder��62   $ Opacity�53   )   $	 0.9X�54   )   $ 0:0:0.3D�62   55      56      57      :� 58      $ 
MouseLeave��6#   .[�59      �� .��5:      �� .��5;   "   �� $ ButtonBorder��61   $1 +(Border.Background).(SolidColorBrush.Color)�5<   (   $ #995200X�5=   (   $ 0:0:0.3D�65   W� 5?   "   $ ButtonBorder��62   $ Opacity�5@   )   $ 1X�5A   )   $ 0:0:0.3D�60   5B      5C      5D      :� 5E      $ Button.Click��6#   .[�5F      �� .��5G      �� .��5H   "   W� $ ButtonBorder��62   $/ )(RenderTransform).(ScaleTransform.ScaleX)�5I   )   $
 0.95X�5J   )   $ 0:0:0.1D�63    . 6F   W� 5K   "   $ ButtonBorder��62   $/ )(RenderTransform).(ScaleTransform.ScaleY)�5L   )   $
 0.95X�5M   )   $ 0:0:0.1D�63    . 6F   5N      5O      5P      z�5R      �� 5S      $	 0.5Q�6%   $	 0.5Q�63   5U      5V      �� 5Y   
   " ��6$   .�5Z      ��  � ��8   6)   �� 5[       � �� �#FFFFFF6+   �� 5\       � ��$ Segoe UI)�6+   �� 5]       � ��$ 14(�6)   �� 5^       � ��$ 10q�6(   �� 5_       � ��$ 20q�6'   �� 5`       � ��$	 180��6&   �� 5a       � ��$	 180��6'   �� 5b       �@�Cursor$
 Hand��6'   5c      5d      .��5e      ]� ��. .+�5f   
   � $	��Top=�6    � RowDefinitions 5g      �� 5h      $��* �6    �� 5i      $
��Auto �6    �� 5j      $��* �6    .+�5l      �� $��1��6    �� Orientation$ 
Horizontal=�6&   $��Center=�6?   $
��0,20q�6\   .+�5m      �� -   5        #��[ 5m      .��5n      �� #��[ 6!   .+�5o      �� $
��👥��6$   #��[ 6.   �� 5p        CreateTeamButton#���  6$   #��[  6N   5q      5r      �� 5s      -   5        #��[ 5s      .��5t      �� #��[ 6!   .+�5u      �� $
��🔍��6$   #��[ 6.   �� 5v        FindPraccsButton#���  6$   #��[  6N   5w      5x      �� 5y      -   5        #��[ 5y      .��5z      �� #��[ 6!   .+�5{      �� $
��💰��6$   #��[ 6.   �� 5|        SubscriptionButton#���  6$   #��[  6P   5}      5~      5      5�      5�      5�      