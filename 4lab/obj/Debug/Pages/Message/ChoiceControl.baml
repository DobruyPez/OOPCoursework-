   M S B A M L   `   `   `  ����  4lab    _4lab.ChoiceControl   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5        � 	Resources  5         TextBlockStyle �� 
TargetType �� Property �� Value  CustomButtonTemplate �� 
TargetType ��Name �� CornerRadius � Triggers :� RoutedEvent	 �� 
TargetName
 �� TargetProperty	 �� To g� Duration  ButtonStyle��%^  &	        &	     &	 �    7  �� 5
   
   " ��6'   .�5      ��  � �� �#FFFFFF6+   �� 5       � ��$ Segoe UI)�6+   �� 5       � ��$
 Wrap=�6-   �� 5       � ��$ 5q�6'   �� 5       � ��$ 12(�6)   �� 5       � ��$ Center=�62   5      �� 5   
   " ��67   .R�5      �� $ ButtonBorder��6   $ 10��5      ���#FF8C00���#FFFFFF$��1q�5      ._�5      �� $��Center=�6#   $��Center=�6@   #��z��5   #    5      :� 5      $ 
MouseEnter��6#   .[�5      �� .��5      �� .��5    "   �� $	 ButtonBorder��61   $1
 +(Border.Background).(SolidColorBrush.Color)�5!   0   $ #FFA040X�5"   0   $ 0:0:0.2D�6=   5#      5$      5%      :� 5&      $ 
MouseLeave��6#   .[�5'      �� .��5(      �� .��5)   "   �� $	 ButtonBorder��61   $1
 +(Border.Background).(SolidColorBrush.Color)�5*   0   $ #FF8C00X�5+   0   $ 0:0:0.2D�6=   5,      5-      5.      :� 5/      $ Button.Click��6#   .[�50      �� .��51      �� .��52   "   �� $	 ButtonBorder��61   $1
 +(Border.Background).(SolidColorBrush.Color)�53   0   $ #E67300X�54   0   $ 0:0:0.1D�6=   55      56      57      59      5:      �� 5=   
   " ��6$   .�5>      ��  � ��8   6)   �� 5?       � �� �#FFFFFF6+   �� 5@       � ��$ Segoe UI)�6+   �� 5A       � ��$ 14(�6)   �� 5B       � ��$
 10,5q�6(   �� 5C       � ��$ 5q�6'   �� 5D       �@�Cursor$
 Hand��6'   �� 5E       � ��$ 80��6)   �� 5F       � ��$ 30��6*   5G      5H      .��5J      � .+�5K   
   �� ���#FF8C00$��1q�6'   $ 5��6;    �� Padding$ 5q�6L   $��5q�6X   ���#333333._�5M      �� .+�5N      �� -   5         ��Name$ 
SenderName��5N      #��[  60   $
��Bold"�5O      ���#FF8C006-   �� 5P      -   5        $ MessageContent��5P      #��[  64   $��0,5,0,5q�5Q      �� 5R       �� Orientation$ 
Horizontal=�6   $��Right=�66   .+�5S      �� -   5         ��Name$ AcceptButton��5S      $	��✓��63   #��[ 5T   8   �� 5U      -   5        $ DeclineButton��5U      $	��✕��64   $��5,0,0,0q�6@   #��[ 5V   9   5W      5X      5Y      5Z      5[      