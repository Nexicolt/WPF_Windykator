   M S B A M L   `   `   `  ����  Windykator_PRO    Windykator_PRO.LoginForm   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   >d2http://schemas.microsoft.com/expression/blend/2008    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -   5        $��LoginWindow��5   	     � WindowStyle$
  None=�5   	    � AllowsTransparency . $��0q�5	   	   $��0q�5
   	    � Title$ 	LoginForm��5   	   $	��300��6   $	��400��6(    � 	Resources 5       �� Source�� 5   
   $+ %/Dictionares/LoginFormDictionary.xaml?�6   %    .��5      � -   5        ���#302d2d � RowDefinitions 5   
   �� 5      $��30 �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   ��    TopStackPanel#��[  6   .+�5      �� -   5        $��x��5        FunctionButtons#��[ 65   $
��Left=�6^   5      � 5   
   $��1��6    5      �� 5      $��* �6    �� 5       $��2* �6    .+�5#      �� .+�5$      �� $��/Icons/small_logo.png��6   $��50��67   �� 5%      $��ZALOGUJ SIĘ��6    �� TextAlignment$ Center=�6/   $��0,20,0,0q�6F   ���#aaaaaa$��18(�6m   $
��Bold"�6{   5&      �� 5(      $��1��6   $	��220��6&   $��0,20,0,0q�62   .+�5)      ��  �� Orientation$ 
Horizontal=�6   .+�5*      �� $��Login:��6      InputTextBlock#��[ 6.   �� 5+      -   5        $��
LoginInput��5+   =    	 Input#��[ 6   6=   5,      �� 5-      $ 
Horizontal=�6   $��0,10,0,0q�66   .+�5.      �� $��Hasło:��6    #��[ 6.   G� 5/      -   5        $��PasswordInput��5/   J     PasswordInput#��[ 6#   6J   50      �� 51      -   5        $��
ErrorPanel��51   H   $ 
Horizontal=�6   $��0,10,0,0q�66   $y�	Collapsed=�6Z   .+�52      �� $��Błędne dane logowania��6   $��Center=�6<   $	��220��6`   
���  ��6l   53      54      55      �� 57   
   $��2��6   $ 
Horizontal=�6"   $��Center=�6;   .+�58      �� -   5        $��Zaloguj��58       
 Button#��[ 6@   �� 59      -   5        $��Anuluj��59      #��[ 6;   5:      5;      5<      