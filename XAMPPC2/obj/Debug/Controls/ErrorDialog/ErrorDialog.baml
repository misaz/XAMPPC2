   M S B A M L   `   `   `  ����B  >XAMPPC2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:XAMPPC2XAMPPC2  5   	       XAMPPC2.ErrorDialog   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:XAMPPC2     5   	     � Title$  Chyba��5   	   $	��300��6   $	��476��6$    � Icon$. (/XAMPPC2;component/Assets/xampp-logo.png��60   .��5	      � ���	#FFFFF9F9 � RowDefinitions 5
      �� 5      $
��auto �6   �� 5      $��* �6   �� 5      $
��auto �6   .+�5      � $��10,10,10,10q�6	    � ColumnDefinitions 5      �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $��* �6   .+�5      � $��48��6
   $��48��6   .+�5      �� -   5        $��
ErrorImage��5   #   $��32��6   $��32��6   6#   5      �� 5       �� CornerRadius$ 5��6   $��1��6   $��48��6-   $��48��69   ._�5      �� -   5        $��ServiceImage��5      $��32��69   $��32��6D   $��5,5,5,5q�6P   ��6    �� *Image+6P   5      �� 5      $��2��5       $
��Wrap=�5!      $
��10,0q�5"      Při spouštění procesu �� 5$      ��6&   �� *ProcessName+6E   GE došlo k neočekaváné chybě a proces byl neočekávaně ukončen.�� 6�   �� 6�   Vrácený kód: �� 5%      ��6   �� *ResponseCode+6=   5'      5(      �� 5*      $��10, 5q�6,   $��1��6;   ��6   �� *Errors+ �� View 5+      �� 5,       �� AllowsColumnReorder .  .P�5-      �� $��
Kód chyby��6    �� Width$ 75��6)    �� DisplayMemberBinding 64   �� 	 �� Path$
	 Code��� 5.      $��Popis chyby��6   $	 300��6+    67   �� $	 Description��� 5/      $�� ��6   $ 50��6    
 �� CellTemplate
 50      �� 51   	   .R�52   
   �� -   5        ���	#FFF7F7F7
 � Tag 52   =   �� *RawProcessOutput+.��53      �� $��12��6   $*��$/XAMPPC2;component/Assets/google.png��6   54      55   
   57      58      5;      � 5=      $��2��6	   $��10, 0, 10,10q�6    5>      �� 5?      $��* �6   �� 5@      $
��auto �6   �� 5A      $
��auto �6   .+�5D      �� -   5        $��Center=�5E      $��Right=�5F      $��75��5G      $��1��5H      $	��3 0q�5I      y�5K      �� *DisplayRestartProcess+.��5L      Restartovat5N      �� 5O      -   5        $��Center=�5P      $��Right=�5Q      $��75��5R      $��2��5S      $	��3 0q�5T      .��5V      OK5X      5Y      5Z      5[      