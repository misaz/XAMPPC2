   M S B A M L   `   `   `  ����L  HTomcatUserManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:TomcatUserManagerTomcatUserManager  5   	   "    TomcatUserManager.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1localclr-namespace:TomcatUserManager     5   	   -   5          � Title$)  #Správce uživatelů serveru Tomcat��5	   	   $	��350��62   $	��525��6?    � Icon$ Assets/group.png��6K   .��5
      �  � RowDefinitions 5      �� 5      $��* �6   �� 5      $
��auto �6   .+�5      �� -   5         ��Name$ 	viewItems��5      ��5      �� *TomcatItems+ �� View 5      �� 5      .P�5      �� $�� ��6    �� Width$ 20��6     �� CellTemplate 5      �� 5   	   .R�5   
   �� $��20��5   
   $��20��5   
   $��	-6 0 -6 0q�5   
   ��5    
   ��  �� Path$ 	Thumbnail�5!   
   5#      �� 5$      $��Jméno��6A   $	 120��6P    �� DisplayMemberBinding 6   �� $
 Name�6P   �� 5%      $��Heslo��6E   $	 120��6T    6   �� $ Password�6T   �� 5&      $
��Role��6A   $	 200��6O    6   �� $
 Role�6O   5'      5)      ]� 5*      $��1��6   $��5q�6   .+�5+      �� -   5        $��0,0,5,0q�5+      $
��10,0q�6   .��6B   Nový uživatel6Q   �� 5,      -   5        $��0,0,5,0q�5,      $
��10,0q�6   .��6B   
Nová role6M   �� 5-      -   5        $
��10,0q�5-      {�60   �� *EnableDelete+.��6S   	Odstranit6^   �� 5.      -   5        $
��10,0q�5.      $��right=�6   .��6J   	Uložit6R   5/      50      51      