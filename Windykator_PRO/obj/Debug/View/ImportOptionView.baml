   M S B A M L   `   `   `  ����I  EWindykator_PRO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:Windykator_PRO.ViewWindykator_PRO.View  5      *    $Windykator_PRO.View.ImportOptionView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      3local!clr-namespace:Windykator_PRO.View     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5        � 	Resources  5
       �� Source�� 5   
   $8 2/Dictionares/Workspace/ImportOptionDictionary.xaml?�6   %    .��5      �    Tab_MainGrid#��[  6    � ColumnDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��10 �6   �� 5      $��* �6   �� 5      $��30 �6   �� 5      $
��Auto �6   �� 5      $��10 �6   �� 5      $��* �6   �� 5      $��10 �6   �� 5      $
��Auto �6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��10 �6   �� 5      $
��Auto �6   �� 5      $��10 �6   �� 5      $
��Auto �6   �� 5       $��10 �6   �� 5!      $
��Auto �6   �� 5"      $��10 �6   �� 5#      $
��Auto �6   �� 5$      $��10 �6   �� 5%      $	��260 �6   �� 5&      $��10 �6   �� 5'      $
��Auto �6   �� 5(      $��10 �6   .+�5+   
   ��   StandardTextBlock#��[ 6   $��Scieżka do pliku��6?   �� 5,   
   $��2��5-      $��5��5.        TextBoxDark#��[ 5/      �� 50   
   $��8��51      $
��OPEN��52        FindFileButton#��[ 53      �� 55   
   $��2��56      #��[ 57      $��Format danych:��58      �� 5:   
   $��2��5;      $��2��5<        ComboBoxDark#��[ 5=      .@�5>      �� $
��JSON��6     ComboBoxItemDark#��[ 6*   �� 5?      $	��XML��6   #��[ 6)   �� 5@      $	��CSV��6   #��[ 6)   5A      �� 5B   
   $��4��5C      #��[ 5D      $��Znak rozdzielający:��5E      � 5G   
   $��4��6   $��2��6    5H      �� 5I      $
��Auto �6#   �� 5J      $��20 �6#   �� 5K      $
��Auto �6#   �� 5L      $��20 �6#   �� 5M      $
��Auto �6#   �� 5N      $��20 �6#   .+�5Q      	� $��; (średnik)��6     StandardRadioButton#��[ 60   	� 5R      $��2��5S      $��
. (kropka)��5T      #��[ 5U      	� 5V      $��4��5W      $��, (przecinek)��5X      #��[ 5Y      5Z      �� 5\   
   $��6��5]      #��[ 5^      $��Ogranicznik tekstu:��5_      � 5a   
   $��6��6   $��2��6    5b      �� 5c      $
��Auto �6#   �� 5d      $��20 �6#   �� 5e      $
��Auto �6#   �� 5f      $��20 �6#   �� 5g      $
��Auto �6#   �� 5h      $��20 �6#   .+�5k      	� $��' (apostrof)��6   #��[ 61   	� 5l      $��2��5m      $��'' (cudzysłów)��5n      #��[ 5o      	� 5p      $��4��5q      $�� (brak)��5r      #��[ 5s      5t      �� 5v   
   $��8��5w      #��[ 5x      $��Ignoruj błędy:��5y      � 5{   
   $��8��6   $��2��6    5|      �� 5}      $
��Auto �6#   �� 5~      $��20 �6#   �� 5      $
��Auto �6#   �� 5�      $��20 �6#   .+�5�      	� $	��Tak��6   #��[ 6(   	� 5�      $��2��5�      $	��Nie��5�      #��[ 5�      5�      &   System.Windows.Controls.DataGrid  5�   
   -   5        $��ClientsDataGrid��5�      $��10��5�      $��9��5�      $	��250��5�         ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$ * 5�         RowHeaderWidth$ 0Q�5�        DataGirdDark#��[ 5�        5�      =  7System.Windows.Controls.Primitives.DataGridColumnHeader �� BasedOn �� 
TargetType �� Property	 �� Value��%�   '        7 �� 5�      8   "  6Q   .�5�      ��  � ��	 �#51515163   �� 5�       � ��	 �#aaaaaa63   �� 5�       � ��$	 	10,5,10,5q�60   �� 5�       � ��$	 Center=�6C   5�      5�      
   Columns
 5�      0  *System.Windows.Controls.DataGridTextColumn  5�      ,  &System.Windows.Controls.DataGridColumn   Width$ 40 6%      Header$	 Lp.��60     5�      $	 200 6%   $ Nazwa��61     5�      $	 110 6%   $ Miasto��61     5�      $	 110 6%   $ Ulica��61     5�      $	 110 6%   $ Należność��61     5�      $	 110 6%   $ Waluta��61     5�      $	 110 6%   $ Czy zagraniczny��61   5�      �� 5�   
   $��12��5�      $��2��5�      $��Weryfikuj plik��5�        ImportClinetsButton#��[ 5�      �� 5�   
   $��12��5�      $��3��5�      $��2��5�      $��Importuj klientów��5�      #��[ 5�      5�      5�      