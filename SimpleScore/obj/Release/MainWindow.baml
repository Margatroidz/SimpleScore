   M S B A M L   `   `   `  ���� O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem   SimpleScore   SimpleScore.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     9System&clr-namespace:System;assembly=mscorlib     -     �Name$  
mainWindow�� � Title$ SimpleScore��$	��810��$
��1440�����	#FFC5C5C5 � WindowStartupLocation$ CenterScreen=� � Icon$ 	title.ico�� J� CommandBindings )  #System.Windows.Input.CommandBinding     Command$	 New��  -   $
 Open��.��� -    �Name$ mainGrid��$��0,0,0,0q�.+�o� -    o�Name$ mainMenu��$��30��$	��Top=�
�������$��3��.@�m� $
��File��$��Center=�$��Center=�$��40��$��40��.@�m� -    m�Name$ 	NewButton��$	��New��	 m� InputGestureText$	 Ctrl+N��m� -   $ 
OpenButton��$
��Open��$	 Ctrl+O��m� -   $ LoadBankButton��$��LoadBank��m� $
��Edit��$��Center=�$��Center=�$��40��$��40��.@�m� $��Center=�
 J� RenderTransformOrigin$	
 0,1&�$��Center=�m� $��Center=�$	
 0,1&�$��Center=�m� $��Option��$��Center=�$��Center=�$��40��$��55��.@��� -   $��AutoPlay���� 	� -	    	�Name$ singlePlayRadioButton��$��0��$��
SinglePlay�� 	� 	GroupName$ 	PlayStyle��	� -
   $ loopPlayRadioButton��$��1��$��LoopPlay��$ 	PlayStyle��	� -   $ randomPlayRadioButton��$��2��$��
RandomPlay��$ 	PlayStyle��	� -   $ sequentialPlayRadioButton��$��3��$��SequentialPlay��$ 	PlayStyle���� 	� -   $! midiDevicePlayerRadioButton��$��MidiDevicePlayer��$��0��$ Player��	� -   $ wasPlayerRadioButton��$��	WasPlayer��$��1��$ Player��� -   $ pianoRollNotation��$��Stretch=�$��Stretch=�$��20,35,0,200q� � RowDefinitions �� $��40 ��� $
��100* � � ColumnDefinitions �� $	��100 ��� $
��100* �.+��� -    ��Name$ uiPianoScrollViewer��$��Stretch=�$��Stretch=�$��0,0,0,0q�$��1��$��0��$��Hidden=�$��Hidden=�.��� $
��Left=�$	��Top=�$��0,0,0,0q�$
��1040��$	��100��.+��� -    ��Name$ uiPianoImage��$
��Left=�$	��Top=�$��0,0,0,0q�$
��1040��$	��100��$��PianoNotation.png��� -   $ uiPianoTagGrid��$
��Left=�$	��Top=�$��0,0,0,0q�$
��1040��$	��100��.+��� $��C8��$
��Left=�$��20��$��10,0,0,0q�$	��Top=�$��0q�$��16(��� $��C7��$
��Left=�$��20��$��
10,120,0,0q�$	��Top=�$��0q�$��16(��� $��C6��$
��Left=�$��20��$��
10,280,0,0q�$	��Top=�$��0q�$��16(��� $��C5��$
��Left=�$��20��$��
10,420,0,0q�$	��Top=�$��0q�$��16(��� $��C4��$
��Left=�$��20��$��
10,560,0,0q�$	��Top=�$��0q�$��16(��� $��C3��$
��Left=�$��20��$��
10,700,0,0q�$	��Top=�$��0q�$��16(��� $��C2��$
��Left=�$��20��$��
10,840,0,0q�$	��Top=�$��0q�$��16(��� $��C1��$
��Left=�$��20��$��
10,980,0,0q�$	��Top=�$��0q�$��16(��� -   $ uiClockScrollViewer��$	��Top=�$
��Left=�$��30��$��0��$��1��$��Hidden=�$��Hidden=�.���� -    ��Name$ uiClockLabel��$��30��$
��5000��$	��Top=�$��0q�$��28(��� -   $ uiTrackScrollViewer��$	��Top=�$
��Left=�$��Hidden=�$��Hidden=�$��1��$��1���� -   $ scoreNameLabel��$
��Left=�$��Bottom=�$��
50,0,0,150q�$	��475��$��40��$��Segoe UI Black)�$��20(��� -    ��Name$ previousButton��$��Previous��$
��Left=�$��Bottom=�$��	50,0,0,65q�$��75��$��75���� -   $ 
playButton��$
��Play��$
��Left=�$��Bottom=�$��
150,0,0,65q�$��75��$��75���� -   $ 
stopButton��$
��Stop��$
��Left=�$��Bottom=�$��
250,0,0,65q�$��75��$��75���� -   $ recordButton��$��Record��$
��Left=�$��Bottom=�$��
350,0,0,65q�$��75��$��75���� -   $ 
nextButton��$
��Next��$
��Left=�$��Bottom=�$��
450,0,0,65q�$��75��$��75���� -    ��Name$ 
maxTimeBar��
�������$
��Left=�$��Bottom=�$	��475��$��30��$��	50,0,0,20q�
���   ��� -   $ currentTimeBar�����	#FF151561$
��Left=�$��Bottom=�$��0��$��30��$��	50,0,0,20q�