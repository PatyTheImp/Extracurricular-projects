   M S B A M L   `   `   `  ����E  AWPFHotelSA, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:WPFHotelSA
WPFHotelSA  5   	       WPFHotelSA.Consultas   5   	   \ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   W SFontAwesome5.Net, Version=2.1.0.0, Culture=neutral, PublicKeyToken=9cfaf01297a008f8/fa5%http://schemas.fontawesome.com/icons/  5   	   $localclr-namespace:WPFHotelSA  5   	     � Title$  .: Consultas :.��5	   	   $	��450��6!   $	��800��6.    � WindowStartupLocation$ CenterScreen=�6:   ���#038C73 � 
ResizeMode$ CanMinimize=�5
   	   $��Microsoft JhengHei UI)�6"   .��5      �  � RowDefinitions 5   
   �� 5      $��50 �6   �� 5      $��70 �6   �� 5      $��* �6   .+�5   
   �� $��0��6    �� Orientation$ 
Horizontal=�6"   $��Center=�6;   $	��750��6X   .+�5      �� -   5        $��tituloBorder��5      ���#033A40$��0,0,0,1q�6?   $	��680��5      $��Center=�6   ._�5      �� -   5        $��	lblTitulo��5      $��	Consultas��6)   $��16(�6=   ���#033A40$
��Bold"�6`   5      �� 5      -   5        $��btnMenu��5      $��60��6$   $��40��6/   $
��Left=�6;    � ToolTip$ Voltar ao Menu Principal��6V   ���#033A40$��	10,10,0,0q�5   @      ButtonRound#��[  6S   .��5        FontAwesome5.ImageAwesome     Icon$ 
Solid_Bars=�6#   $��20��65   $��20��6A      
Foreground �#B6D9366M   5      5      �� 5    
   $��1��6   $��25,0,0,0q�6#   .+�5!      �� $ 
Horizontal=�6   $��0,0,0,5q�62   .+�5"      �� $2��,Empregados que deram entrada em um hotel em ��6   (  "System.Windows.Controls.DatePicker  5#      -   5        $��dpData��5#      ��6+   Z� �� 5$      -   5        $��btnConsulta1��5$      $��25��6-   $��30��69   #��[  6D   $	��Top=�6i   ���#033A40$ 	Consultar��5%   &   .��5&        $ Solid_Search=�6'   $��15��6;    �#B6D9366G   5'      5(      �� 5*      $ 
Horizontal=�6   .+�5+      �� $.��(Empregados cujo nome começa pela letra ��6   �� 5,      -   5        $��cmbLetra��5,   &   $��40��6   $��0,0,5,0q�6M   ��66   Z� 6M   �� 5-      -   5        $��btnConsulta2��5-      $��25��6-   $��30��69   #��[  6D   $	��Top=�6i   ���#033A40$ 	Consultar��5.   &   .��5/        $ Solid_Search=�6'   $��15��6;    �#B6D9366G   50      51      52      &   System.Windows.Controls.DataGrid  55   
   -   5        $��dgTabela��55   ,   $��2��6   $��10q�6    $��Center=�6<   $��Center=�6^      CanUserAddRows$ False��56      	   
IsReadOnly$
	 True��6*   
   HorizontalGridLinesBrush
 6<   Z�    VerticalGridLinesBrush 6`   Z�    AlternatingRowBackground 57      �� 58       �� Color$ 	#FF038C73��6"    �� Opacity$
 0.25Q�64   5<      5=      5>      