   M S B A M L   `   `   `  ����F  BProxyServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:ProxyServerProxyServer  5   	       ProxyServer.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:ProxyServer     5   	     � Title$  NotS - HTTP Proxyserver��5   	   $��Arial)�6)   $��14(�6<    � WindowStyle$ ThreeDBorderWindow=�6J    � 
ResizeMode$ 	CanResize=�6k    � WindowStartupLocation$ CenterScreen=�6�   .��5	      � $��20q�6    � RowDefinitions 5
   
   �� 5      $
��auto �6   �� 5      $��* �6    � ColumnDefinitions 5   
   �� 5      $��4* �6   �� 5      $��5* �6   �� 5      $��3* �6   .+�5   
   �� $��HTTP Proxyserver��6    �� Padding$ 10q�6,   $��25(�69   $
��Bold"�6G   � 5   
   $��1��6    5      �� 5      $��* �6    �� 5      $
��auto �6    .+�5      �� -   5        $��LogList��5      $��10q�6%   ��61   �� *Log+6M   �� 5      -   5        $��ClearLogButton��5      $��1��6+   $��	Clear log��68   $��10q�6L   $��5q�6X    � Cursor$
 Hand��6d   6r   5      �� 5   
   -   5        $��HttpMessageDetails��5      $��10q�6,   $��1��68   $��1��6E   $��0q�6U   � 5    
   $��2��6   $��0��6   $��2��6,   $��0 0 10 0q�6=    5!      �� 5"      $
��auto �6    �� 5#      $
��auto �6    �� 5$      $
��auto �6    �� 5%      $
��auto �6    �� 5&      $
��auto �6    �� 5'      $
��auto �6    �� 5(      $
��auto �6    �� 5)      $
��auto �6    �� 5*      $
��auto �6    �� 5+      $
��auto �6    �� 5,      $
��auto �6    �� 5-      $
��auto �6    �� 5.      $
��auto �6     51      �� 52      $��3* �6#   �� 53      $��* �6#   .+�56      �� -   5         ��Name$ StartStopButton��56   {   $��2��6   $��0��6)   $��Start/Stop proxy��66   $��20 15q�6Q   $��10q�6a   $
 Hand��6m   6�   �� 58      $��1��6   $��Proxy poort��6!   $��Right=�67   $��Center=�6S   �� 59      $��1��6   $��1��6&   $��10q�63   $��5q�6?   ��6K   �� *	ProxyPort+{�6f   �� *	NotActive+�� 5;      $��2��6   $ ��Cache time out in secondes��6!   $��Right=�6F   $��Center=�6b   �� 5<      $��1��6   $��2��6&   $��10q�63   $��5q�6?   ��6K   �� *ProxyCacheTimeoutSec+�� 5>      $��3��6   $)��#Check op gewijzigde content aan/uit��6!   $��Right=�6O   $��Center=�6k   $��0,0,0,5q�6�   �� 5?      $��1��6   $��3��6'   $��10q�64   $��5q�6@   	 d� 	IsChecked	 6L   �� *CheckChangeContent+�� 5A      $��4��6   $ ��Zet content filter aan/uit��6!   $��Right=�6F   $��Center=�6b   $��0,0,0,5q�6}   �� 5B      $��1��6   $��4��6'   $��10q�64   $��5q�6@   	 6L   �� *ContentFilter+�� 5D      $��5��6   $*��$Zet het wijzigen van headers aan/uit��6!   $��Right=�6P   $��Center=�6l   $��0,0,0,5q�6�   �� 5E      $��1��6   $��5��6'   $��10q�64   $��5q�6@   	 6L   �� *ChangeHeaders+�� 5G      $��6��6   $-��'Zet basic access authentication aan/uit��6!   $��Right=�6S   $��Center=�6o   $��0,0,0,5q�6�   �� 5H      $��1��6   $��6��6'   $��10q�64   $��5q�6@   	 6L   �� *AccessAuthentication+�� 5J      $��7��6   $��
Buffersize��6!   $��Right=�66   $��Center=�6R   $��0,0,0,5q�6m   �� 5K      $��1��6   $��7��6&   $��10q�63   $��5q�6?   ��6K   �� *
BufferSize+�� 5M      $��8��6   $%��Logging request headers aan/uit��6!   $��Right=�6K   $��Center=�6g   $��0,0,0,5q�6�   �� 5N      $��1��6   $��8��6'   $��10q�64   $��5q�6@   	 6L   �� *LogRequestHeaders+�� 5P      $��9��6   $&�� Logging response headers aan/uit��6!   $��Right=�6L   $��Center=�6h   $��0,0,0,5q�6�   �� 5Q      $��1��6   $��9��6'   $��10q�64   $��5q�6@   	 6L   �� *LogResponseHeaders+�� 5S      $��10��6   $ ��Logging content IN aan/uit��6"   $��Right=�6G   $��Center=�6c   $��0,0,0,5q�6~   �� 5T      $��1��6   $��10��6'   $��10q�65   $��5q�6A   	 6M   �� *LogContentIn+�� 5V      $��11��6   $!��Logging content UIT aan/uit��6"   $��Right=�6H   $��Center=�6d   $��0,0,0,5q�6   �� 5W      $��1��6   $��11��6'   $��10q�65   $��5q�6A   	 6M   �� *LogContentOut+�� 5Y      $��12��6   $��Logging client��6"   $��Right=�6;   $��Center=�6W   $��0,0,0,5q�6r   �� 5Z      $��1��6   $��12��6'   $��10q�65   $��5q�6A   	 6M   �� *	LogClient+{�6m   �� *EnableUserShow+5[      5\      5]      