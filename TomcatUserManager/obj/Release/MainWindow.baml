   M S B A M L   `   `   `  ���� L  HTomcatUserManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:TomcatUserManagerTomcatUserManager  "    TomcatUserManager.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     1localclr-namespace:TomcatUserManager     -     � Title$)  #Správce uživatelů serveru Tomcat��$	��350��$	��525�� � Icon$ Assets/group.png��.���  � RowDefinitions �� $��* ��� $
��auto �.+��� -    ��Name$ 	viewItems������ *TomcatItems+ �� View �� .P��� $�� �� �� Width$ 20�� �� CellTemplate �� .R��� $��20��$��20��$��	-6 0 -6 0q�����  �� Path$ 	Thumbnail��� $��Jméno��$	 120�� �� DisplayMemberBinding �� $
 Name��� $��Heslo��$	 120�� �� $ Password��� $
��Role��$	 200�� �� $
 Role�]� $��1��$��5q�.+��� -   $��0,0,5,0q�$
��10,0q�.��Nový uživatel�� -   $��0,0,5,0q�$
��10,0q�.��
Nová role�� -   $
��10,0q�{��� *EnableDelete+.��	Odstranit�� -   $
��10,0q�$��right=�.��	Uložit