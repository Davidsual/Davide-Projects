   M S B A M L   `   `   `  ����  Concurrency.Tpl     Concurrency.Tpl.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  
MainWindow��5   	   $	��450��6   $	��525��6)   .��5      � .+�5   
   �� -   5        $;��5Run independent processes in parallel PARALLEL INVOKE��5      $
��Left=�6Q   $��	10,10,0,0q�6l   $	��Top=�6   $	��497��6�   6�   �� 5   
   -   5         ��Name$ lsMyList��5   b   $��Center=�6   $��10,137,10,10q�61   $��Center=�6G   $	��497��6t   $	��273��6�    �� View 5      �� 5	      .P�5
      �� 5      5      �� 5   
   -   5        $=��7Run independent processes in parallel TASK.CONTINUEWITH��5      $
��Left=�6S   $��	10,37,0,0q�6n   $	��Top=�6�   $	��497��6�   6�   �� 5   
   -   5        $@��:Compare performance PLINQ - int.Maxvalue select (item * 2)��5      $
��Left=�6V   $��	10,64,0,0q�6q   $	��Top=�6�   $	��497��6�   6�   5      5      