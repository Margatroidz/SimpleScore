   M S B A M L   `   `   `  ����O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem  5   	    SimpleScore   SimpleScore.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   9System&clr-namespace:System;assembly=mscorlib     5   	   -   5          �Name$  
mainWindow��5   ?    � Title$ SimpleScore��5   	   $	��810��6   $
��1440��6*   ���	#FFC5C5C5 � WindowStartupLocation$ CenterScreen=�6N    � Icon$ 	title.ico��6s    J� CommandBindings 5      )  #System.Windows.Input.CommandBinding  5   
      Command$	 New��6   6)     5   
   -   5        $
 Open��5      6F   .��5
      � -   5         �Name$ mainGrid��5
      $��0,0,0,0q�6   .+�5   
   o� -   5         o�Name$ mainMenu��5      $��30��6!   $	��Top=�6-   
�������$��3��6X   .@�5      m� $
��File��6   $��Center=�6%   $��Center=�6B   $��40��6]   $��40��6i   .@�5      m� -   5         m�Name$ 	NewButton��5      $	��New��6.   	 m� InputGestureText$	 Ctrl+N��6;   m� 5      -   5        $ 
OpenButton��5      $
��Open��6/   $	 Ctrl+O��6V   m� 5      -   5        $ LoadBankButton��5      $��LoadBank��63   5      m� 5      $
��Edit��6   $��Center=�6%   $��Center=�6B   $��40��6]   $��40��6i   .@�5      m� $��Center=�6   
 J� RenderTransformOrigin$	
 0,1&�68   $��Center=�6T   m� 5      $��Center=�6   $	
 0,1&�68   $��Center=�6T   5      m� 5      $��Option��6   $��Center=�6'   $��Center=�6D   $��40��6_   $��55��6k   .@�5      �� -   5        $��AutoPlay��5      6/   �� 5      	� 5      -	   5         	�Name$ singlePlayRadioButton��5      $��0��6=   $��
SinglePlay��6R    	� 	GroupName$ 	PlayStyle��6�   	� 5      -
   5        $ loopPlayRadioButton��5      $��1��6;   $��LoopPlay��6P   $ 	PlayStyle��6�   	� 5      -   5        $ randomPlayRadioButton��5      $��2��6=   $��
RandomPlay��6R   $ 	PlayStyle��6�   	� 5      -   5        $ sequentialPlayRadioButton��5      $��3��6A   $��SequentialPlay��6V   $ 	PlayStyle��6�   �� 5      	� 5      -   5        $! midiDevicePlayerRadioButton��5      $��MidiDevicePlayer��6C   $��0��6^   $ Player��6�   	� 5      -   5        $ wasPlayerRadioButton��5      $��	WasPlayer��6<   $��1��6P   $ Player��6�   5      5       � 5!   
   -   5        $ pianoRollNotation��5!      $��Stretch=�6*   $��Stretch=�6H   $��20,35,0,200q�6d    � RowDefinitions 5"      �� 5#      $��40 �6    �� 5$      $
��100* �6     � ColumnDefinitions 5&      �� 5'      $	��100 �6#   �� 5(      $
��100* �6#   .+�5+      �� -   5         ��Name$ uiKeyboardScrollViewer��5+      $��Stretch=�6;   $��Stretch=�6Y   $��0,0,0,0q�6u   $��1��6�   $��0��6�   $��Hidden=�6�   $��Hidden=�6�   .��5,      � -   5        $ uiKeyboardGrid��5,      $
��Left=�6/   $	��Top=�6J   $��0,0,0,0q�6b   58      59      �� 5;      -   5        $ uiClockScrollViewer��5;      $	��Top=�68   $
��Left=�6P   $��30��6k   $��0��6w   $��1��6�   $��Hidden=�6�   $��Hidden=�6�   .��5<      �� -   5         ��Name$ uiClockLabel��5<      $��30��6.   $
��5000��6:   $	��Top=�6G   $��0q�6_   $��28(�6k   5=      �� 5>      -   5        $ uiTrackScrollViewer��5>      $	��Top=�68   $
��Left=�6P   $��1��6l   $��1��6y   $��Hidden=�6�   $��Hidden=�6�   5?   K   5@      �� 5A   
   -   5        $ scoreNameLabel��5A      $
��Left=�6(   $��Bottom=�6C   $��
50,0,0,150q�6^   $	��475��6r   $��40��6~   $��Segoe UI Black)�6�   $��20(�6�   �� 5B   
   -   5         ��Name$ previousButton��5B      $��Previous��6)   $
��Left=�6<   $��Bottom=�6W   $��	50,0,0,65q�6r   $��75��6�   $��75��6�   6�   �� 5C   
   -   5        $ 
playButton��5C      $
��Play��6%   $
��Left=�64   $��Bottom=�6O   $��
150,0,0,65q�6j   $��75��6~   $��75��6�   6�   �� 5D   
   -   5        $ 
stopButton��5D      $
��Stop��6%   $
��Left=�64   $��Bottom=�6O   $��
250,0,0,65q�6j   $��75��6~   $��75��6�   6�   �� 5E   
   -   5        $ recordButton��5E      $��Record��6'   $
��Left=�68   $��Bottom=�6S   $��
350,0,0,65q�6n   $��75��6�   $��75��6�   6�   �� 5F   
   -   5        $ 
nextButton��5F      $
��Next��6%   $
��Left=�64   $��Bottom=�6O   $��
450,0,0,65q�6j   $��75��6~   $��75��6�   6�   �� 5G   
   -   5         ��Name$ 
maxTimeBar��5G      
�������$
��Left=�68   $��Bottom=�6S   $	��475��6n   $��30��6z   $��	50,0,0,20q�6�   
���   �6�   �� 5H   
   -   5        $ currentTimeBar��5H      ���	#FF151561$
��Left=�6=   $��Bottom=�6X   $��0��6s   $��30��6}   $��	50,0,0,20q�6�   6�   5I      5J      