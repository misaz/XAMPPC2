   M S B A M L   `   `   `  ���� B  >XAMPPC2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:XAMPPC2XAMPPC2      XAMPPC2.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     'localclr-namespace:XAMPPC2     -     �Name$	  wnd�� � Title$ XAMPP Control Panel��$	��356��$	��525�� � 
ResizeMode$ CanMinimize=� � Icon$ Assets/xampp-logo.png��.��� $��
10,10,10,0q� � RowDefinitions �� $	��120 ��� $��* �.+�� $��0�� � ColumnDefinitions �� $��0 ��� $��9* ��� $��0 �.+�� $��1�� �� $��* ��� $��* ��� $��* ��� $��* ��� $��* �.+�   XAMPPC2.ServiceControl  -     Name$ apache��$��0��   Source$ apache��   	Thumbnail$ Assets/apache.png��  -   $ mysql��$��1��$ mysql��$ Assets/mariadb.png��  -   $ 	filezilla��$��2��$ 	filezilla��$ Assets/filezilla.png��  -   $ tomcat��$��3��$ tomcat��$ Assets/tomcat.png��  -   $ mercury��$��4��$ mercury��$ Assets/mercury.png��� $��1��$��0,15,0,0q� �� $
��auto ��� $��* � �� $
��277* ��� $
��240* �.+��� $��10,0,0,3q�$��16(�Užitečné nástroje�� $��10,0,0,3q�$��16(�$��1��Informace o verzi�� $��1��$��	10,0,10,0q�.+�   XAMPPC2.ToolHyperlink  	   Program$	 vhm.exe��
   Label$
 VirtualHost Manager��  $	 netstat.exe��$
 Netstat��  $	 services.exe��$
 Služby systému Windows��  $	 tomcatusers.exe��$!
 Správce uživatelů Tomcat��� $��1��$��1��$��	10,0,10,0q� �� $��* ��� $
��auto � �� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto �.+��� $��0��$��0��$��0,0,5,0q�XAMPP Control Panel�� $��0��$��1��Apache�� $��0��$��2��PHP�� $��0��$��3��	MariaDB�� $��0��$��4��
phpMyAdmin�� $��0��$��5��	FileZilla�� $��0��$��6��Tomcat�� $��0��$��7��	Mercury�� -   $��XamppCPVersion��$��1��$��0��	Unknown�� -   $��ApacheVersion��$��1��$��1��	Unknown�� -	   $��
PhpVersion��$��1��$��2��	Unknown�� -
   $��MariaDbVersion��$��1��$��3��	Unknown�� -   $��PhpMyAdminVersion��$��1��$��4��	Unknown�� -   $��FileZillaVersion��$��1��$��5��	Unknown�� -   $��TomcatVersion��$��1��$��6��	Unknown�� -   $��MercuryVersion��$��1��$��7��	Unknown