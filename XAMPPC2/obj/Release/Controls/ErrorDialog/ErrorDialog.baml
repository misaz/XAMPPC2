   M S B A M L   `   `   `  ���� B  >XAMPPC2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:XAMPPC2XAMPPC2      XAMPPC2.ErrorDialog   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     'localclr-namespace:XAMPPC2       � Title$  Chyba��$	��300��$	��476�� � Icon$. (/XAMPPC2;component/Assets/xampp-logo.png��.��� ���	#FFFFF9F9 � RowDefinitions �� $
��auto ��� $��* ��� $
��auto �.+�� $��10,10,10,10q� � ColumnDefinitions �� $
��auto ��� $
��auto ��� $��* �.+�� $��48��$��48��.+��� -   $��
ErrorImage��$��32��$��32����  �� CornerRadius$ 5��$��1��$��48��$��48��._��� -   $��ServiceImage��$��32��$��32��$��5,5,5,5q����� *Image+�� $��2��$
��Wrap=�$
��10,0q�Při spouštění procesu �� ���� *ProcessName+GE došlo k neočekaváné chybě a proces byl neočekávaně ukončen.�� �� Vrácený kód: �� ���� *ResponseCode+�� $��10, 5q�$��1������ *Errors+ �� View ��  �� AllowsColumnReorder .  .P��� $��
Kód chyby�� �� Width$ 75�� �� DisplayMemberBinding �� 	 �� Path$
	 Code��� $��Popis chyby��$	 300�� �� $	 Description��� $�� ��$ 50��
 �� CellTemplate
 �� .R��� -   ���	#FFF7F7F7
 � Tag �� *RawProcessOutput+.���� $��12��$*��$/XAMPPC2;component/Assets/google.png��� $��2��$��10, 0, 10,10q� �� $��* ��� $
��auto ��� $
��auto �.+��� -   $��Center=�$��Right=�$��75��$��1��$	��3 0q�y��� *DisplayRestartProcess+.��Restartovat�� -   $��Center=�$��Right=�$��75��$��2��$	��3 0q�.��OK