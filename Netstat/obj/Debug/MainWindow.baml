   M S B A M L   `   `   `  ����B  >Netstat, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:NetstatNetstat  5   	       Netstat.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:Netstat     5   	   -   5          � Title$%  Netstat - TCP Listening sockets��5
   	   $	��530��61   $	��381��6>   .��5      � .+�5      �� ��6   �� *NetstatReponse+ � 	Resources 5       �� 
TargetType �� Property �� Value �� Triggers �� Binding �� Value��%o  '��       �� 5      " ��6   .�5      ��  � ��
 �   �64    5      �� 5       5      �� *IsOld+ True5      .a�5   	   ��  � �� �	#FFFF8C8C6:   5   	   �� 5       5      �� *IsNew+ True5      .a�5   	   ��  � ��
 �����66   �� 5   	    � �� �	#FF00A8086:   5   	   5      5       �� View 5      �� 5       .P�5!      �� $��Address��6   	 �� Width$		 100��6U   
 �� DisplayMemberBinding
 6'   ��  �� Path$ Address�6U   �� 5"      $
��Port��6   $	 55��6O   
 6$   �� $
 Port�6O   �� 5#      $	��Pid��6   $	 55��6M   
 6#   �� $	 Pid�6M   �� 5$      $
��Name��6   $		 120��6V   
 6$   �� $ ProcessName�6V   5%      5'      5(      5)      