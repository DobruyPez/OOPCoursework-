   M S B A M L   `   `   `  ����  4lab)    #_4lab.Pages.Admin.Ads.ManageAdsPage   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5        N� Title$  Manage Advertisements��5       N� 
Background �#1A1A1A � 	Resources 5	         TextBlockStyle �� 
TargetType �� Property �� Value3  -System.Windows.Media.Effects.DropShadowEffect   Color   ShadowDepth   
BlurRadius	   Opacity  HeaderTextBlockStyle
 �� BasedOn  CustomButtonTemplate �� 
TargetType ��Name �� CornerRadius � Triggers :� RoutedEvent �� 
TargetName �� TargetProperty	 �� To g� Duration	 W� To g� AutoReverse �� CenterX �� CenterY  ButtonStyle��%�  &	        &	 �    7   &	 2    &	 u    7  �� 5   
   " ��6'   .�5      ��  � �� �#FFFFFF6+   �� 5       � ��$ Segoe UI)�6+   �� 5       �@��TextAlignment$
 Left=�6.   �� 5       � ��$
 Wrap=�6-   �� 5       � ��$ 0,2,0,2q�6'   �� 5       � ��$ 10(�6)   �� 5       �@J�Effect 5        5      $ #000000��6'   $ 1Q�67   $ 3Q�6G   $		 0.3Q�6V   5      5      �� 5   
   8
   " ��6W   .�5      ��  � ��$ 20(�6)   �� 5       � ��$
 Bold"�6+   �� 5       � ��$ 0q�6'   5      �� 5   
   " ��67   .R�5       �� $ ButtonBorder��6   $ 10��5!      ���#2A2A2A
���   �$��1q�5$      ._�5%      �� $��Center=�6#   $��Center=�6@   #��z��6[    5&      :� 5'      $ 
MouseEnter��6#   .[�5(      �� .��5)      �� .��5*   "   �� $ ButtonBorder��61   $1 +(Border.Background).(SolidColorBrush.Color)�5+   1   $ #4A4A4AX�5,   1   $ 0:0:0.3D�6>   W� 5-   "   $ ButtonBorder��62   $ Opacity�5.   2   $	 0.9X�5/   2   $ 0:0:0.3D�6;   50      51      52      :� 53      $ 
MouseLeave��6#   .[�54      �� .��55      �� .��56   "   �� $ ButtonBorder��61   $1 +(Border.Background).(SolidColorBrush.Color)�57   1   $ #2A2A2AX�58   1   $ 0:0:0.3D�6>   W� 59   "   $ ButtonBorder��62   $ Opacity�5:   2   $ 1X�5;   2   $ 0:0:0.3D�69   5<      5=      5>      :� 5?      $ Button.Click��6#   .[�5@      �� .��5A      �� .��5B   "   W� $ ButtonBorder��62   $/ )(RenderTransform).(ScaleTransform.ScaleX)�5C   2   $
 0.95X�5D   2   $ 0:0:0.1D�6<    . 6O   W� 5E   "   $ ButtonBorder��62   $/ )(RenderTransform).(ScaleTransform.ScaleY)�5F   2   $
 0.95X�5G   2   $ 0:0:0.1D�6<    . 6O   5H      5I      5J      z�5L      �� 5M      $	 0.5Q�6%   $	 0.5Q�63   5O      5P      �� 5R   
   " ��6$   .�5S      ��  � ��8   6)   �� 5T       � �� �#FFFFFF6+   �� 5U       � ��$ Segoe UI)�6+   �� 5V       � ��$ 14(�6)   �� 5W       � ��$
 10,5q�6(   �� 5X       � ��$ 5q�6'   �� 5Y       �@�Cursor$
 Hand��6'   5Z      5[      .��5\      � $��20q�6    � RowDefinitions 5]   
   �� 5^      $
��Auto �6   �� 5_      $
��Auto �6   �� 5`      $
��Auto �6   �� 5a      $
��Auto �6   .+�5d   
   �� $��0��6   #��[ 5e      $+��%Управление рекламой��5f      $��0,0,0,20q�5g      �� 5i   
   $��1��6   $��0,0,0,10q�6"   .+�5j      �� #��[  6   $��Изображение:��6@   � 5k       � ColumnDefinitions 5l      �� 5m      $��* �6'   �� 5n      $
��Auto �6'   .+�5p      �� -   5         ��Name$ ImageUrlTextBox��5p      $��0��5q      $��0,5,5,5q�5r      $��5q�5s      ���#444444���#FFFFFF���#FF8C00$��1q�5w       �� 
IsReadOnly .  J� 	AllowDrop . 5{      �� 5|      -   5        $��1��5|      #��[ 5}      $��Выбрать файл��5~      $
��10,5q�5�      $	��0,5q�5�      5�      5�      �� 5�   
   $��2��6   $��0,0,0,10q�6"   .+�5�      �� #��[  6   $��Ссылка:��6@   �� 5�      -   5        $ LinkTextBox��5�      $	��0,5q�5�      $��5q�5�      ���#444444���#FFFFFF���#FF8C00$��1q�5�      5�      �� 5�   
   -   5        $��3��5�      #��[ 5�      $��Сохранить��5�      $
��Left=�5�      $��20,10q�5�      5�      5�      