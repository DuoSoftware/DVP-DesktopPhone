   M S B A M L   `   `   `  ���� F  Bdvp_timmers, Version=2.2.0.1, Culture=neutral, PublicKeyToken=null>.clr-namespace:dvp_timmers;assembly=dvp_timmersdvp_timmers   FacetoneDesktopPhone"   DVP_DesktopPhone.PhoneWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     E
dvpTimmers.clr-namespace:dvp_timmers;assembly=dvp_timmers     -     � Title$  Facetone��$	��450��$	��246�� � 
ResizeMode$ NoResize=� � WindowStartupLocation$ CenterScreen=� � AllowsTransparency .  � WindowStyle$
 None=� � Icon$! Resources/facetone-logo.ico����Z�  � 	Resources    TextBoxBaseControlTemplate �� 
TargetType ��Name	 �� CornerRadius
 ��Name �� Triggers P� Property P� Value �� Value �� 
TargetName �� Property �� 
TargetType  	TabButton ��Name��%	  &	        &	     �� " ��.R��� $ Bd��#��z�����	#FFA5A5A5#��z��$	 10��._��� $
 PART_ContentHost�� P�  � {� .  . ��� # � ��$ Bd�� � ���� # � �� � ��P�  � ��$
 Auto��. ���  � ��$	 100��P�  � ��$
 Auto��. ���  � ��$ 20���� " ��.���  � �� �#5e5b5b��  �@��TextAlignment$ Center=���  � �� �� " ��.R��� $	 5�����#ccc9c9���#5e5b5b$��1q�._��� $ contentPresenter��#��z��#��z��#��z��#��z��#��z��.���� $	 70�� �� Padding$ 0,9,0,4q�$��5q�����  �� EndPoint$ 0.5,1&� �� 
StartPoint$ 0.5,0&�.���  � Color$ 	#FFA5A5A5�� � Offset$ 0Q�� $ 	#FFABABAB��$ 1Q�� $ 	#FF3C3C3C��$ 0.236Q�� $ 	#FFACACAC��$ 0.675Q����� $ 0.5,1&�$ 0.5,0&�.��� $ 
WhiteSmoke��$ 0Q�� $ 
WhiteSmoke��$ 0.314Q�� $ 
WhiteSmoke��$
 0.81Q�._�� .+�o� $	��Top=�$��Right=�$��0,9,16,0q�$��16��$��49����Z� .@�m� $��20��$��16�� � Cursor$
 Hand��$��Center=�$
��Left=�$��0,0,1,0q� � ToolTip$ Setting�� m� Icon �� $��Resources/setting.png��.@�m� -    m�Name$ settingMenuItem��$��Setting��$��20��$
 Hand��$��Center=�$
��Left=�$��0,0,1,0q�$ Setting��$y�Hidden=�m� -   $ volumeMenuItem��$��Volume��$��20��$
 Hand��$��Center=�$
��Left=�$��0,0,1,0q�$ Volume��$y�Visible=�m� -   $ autoanswerMenu��$��Auto Answer��$��20��$
 Hand��$��Center=�$
��Left=�$��0,0,1,0q�$ Auto Answer�� m� IsCheckable . m� $
��Mode��$��20��$
 Hand��$��Center=�$
��Left=�$��0,0,1,0q�$
 Mode��.@�m� -   $��Inbound��$��Inbound�� . m� -   $��Outbound��$��Outbound�� . m� -   $��About��$��20��$
 Hand��$��Center=�$
��Left=�$��0,0,1,0q�$ About��m� -   $ phone_minimize��$��20��$��16��$
 Hand��$��0q�$ Phone Minimize��$��Center=�$��Center=� �� $'��!Resources/window-minimize-512.png��m� -	   $��20��$��16��$
 Hand��$��0q�$
 Exit��$��Center=�$��Center=�$y�Hidden=� �� $��Resources/exit.png���� -
   $ BorderNumpad��$��Center=�$��Bottom=�$	��200��$	��200��$��0,0,0,32q�._�� .+�� -    �Name$ GrdCallButton��$y�Visible=�$��2,3,0,0q� � RowDefinitions �� $��50 ��� $��35 ��� $��35 ��� $��35 ��� $��35 � � ColumnDefinitions �� $��66 ��� $��66 ��� $��66 �.+��� -     ��Name$  buttonAnswer��#��[ $
 Hand��$��0��$��0��$��64��$��34��$��0,0,0,0q�$��Center=�$��Center=����	#FF91DC5A��Z� .���� $��Resources/answer-call.png���� -   $  buttonDialPad��#��[ $
 Hand��$��0��$��1��$��64��$��34��$��0,0,0,0q�$��Center=�$��Center=�$y�Hidden=�$��^����Z� �� -   $  buttonReject��#��[ $
 Hand��$��0��$��2��$��64��$��34��$��0,0,0,0q�$��Center=�$��Center=����	#FFD80027��Z� .���� $��Resources/end-call.png��� -   $ 
GrdDailpad��$y�Visible=�$��2,0,0,0q� �� $��50 ��� $��35 ��� $��35 ��� $��35 ��� $��35 � �� $��66 ��� $��66 ��� $��66 �.+��� -   #��[ $
 Hand��$��1��$��0��$��64��$��33��$��0,0,0,0q�$��1��$��Center=�$��Center=�
! � Tag$! 1�����	#FF817B7B�� -   #��[ $
 Hand��$��1��$��1��$��64��$��33��$��0,0,0,0q�$��2��$��Center=�$��Center=�$! 2�����	#FF817B7B�� -   #��[ $
 Hand��$��1��$��2��$��64��$��33��$��0,0,0,0q�$��3��$��Center=�$��Center=�$! 3�����	#FF817B7B�� -   #��[ $
 Hand��$��2��$��0��$��64��$��33��$��0,0,0,0q�$��4��$��Center=�$��Center=�$! 4�����	#FF817B7B�� -   #��[ $
 Hand��$��2��$��1��$��64��$��33��$��0,0,0,0q�$��5��$��Center=�$��Center=�$! 5�����	#FF817B7B�� -   #��[ $
 Hand��$��2��$��2��$��64��$��33��$��0,0,0,0q�$��6��$��Center=�$��Center=�$! 6�����	#FF817B7B�� -   #��[ $
 Hand��$��3��$��0��$��64��$��33��$��0,0,0,0q�$��7��$��Center=�$��Center=�$! 7�����	#FF817B7B�� -   #��[ $
 Hand��$��3��$��1��$��64��$��33��$��0,0,0,0q�$��8��$��Center=�$��Center=�$! 8�����	#FF817B7B�� -   #��[ $
 Hand��$��3��$��2��$��64��$��33��$��0,0,0,0q�$��9��$��Center=�$��Center=�$! 9�����	#FF817B7B�� -   #��[ $
 Hand��$��4��$��0��$��64��$��33��$��0,0,0,0q�$��0��$��Center=�$��Center=�$! 0�����	#FF817B7B�� -   #��[ $
 Hand��$��4��$��1��$��64��$��33��$��0,0,0,0q�$��*��$��Center=�$��Center=�$! 10�����	#FF817B7B�� -   #��[ $
 Hand��$��4��$��2��$��64��$��33��$��0,0,0,0q�$��#��$��Center=�$��Center=�$! 11�����	#FF817B7B�� -   $ 	GrdVolume��" K� ZIndex$
" 5000��$y�Hidden=����	#FF797979���	#FF404040$��2q�$��0,4,0,0q�$	 10��._�� $��0q� �� $��30 ��� $��30 ��� $��30 ��� $��30 ��� $��30 ��� $��60 � �� $��53 ��� $	��125 ��� $��16 �.+��� $��0��$��0��$��Speaker��$
��Left=�$��Center=��� -   # ��Name$# ComboBoxSpeakers��$��0��$��2��$��1��$��20��$	��130��$��Center=�$��Center=�$��0q��� $��1��$��0��$��
Microphone��$
��Left=�$��Center=��� -   $# ComboBoxMicrophones��$��1��$��2��$��1��$��20��$	��130��$��Center=�$��Center=�$��0q��� $��2��$��0��$��Speaker��$
��Left=�$��Center=��� -   $ ��Name$$ TrackBarSpeaker��$��2��$��1��$��30��$	��116��$��Center=�$��0,10,0,0q�$��Center=�% � Maximum$	% 255Q�& �� TickFrequency$& 10Q�' � SmallChange$' 5Q�( � LargeChange$( 10Q��� -    ) ��Name$) chkboxMuteSpeaker��$��2��$��3��$��15��$��16��$��0q�$ Mute Speaker��$��Center=����	#FF817B7B$
 Hand��$��Center=��� $��3��$��0��$��
Microphone��$
��Left=�$��Center=��� -!   $$ TrackBarMicrophone��$��3��$��1��$��30��$	��116��$��Center=�$��Center=�$��0,10,0,0q�$	% 255Q�$& 10Q�$' 5Q�$( 10Q��� -"   $) CheckBoxMute��$��3��$��3��$��15��$��16��$��0q�$ Mute microphone��$��Center=����	#FF817B7B$
 Hand��$��Center=��� -#   $) ButtonTestAudio��$��4��$��2��$��1��$
��Auto��$��16��$��7,0,0,0q�$��
Test Audio��$��Center=����	#FF817B7B$
 Hand��$
��Left=��� -$   $  btnSave��$��5��$��3��$��0��$��30��$��0,0,0,19q�$
��Save��$��Center=�$��Center=����	#FF817B7B$
 Hand��$	��185��� -%   $ GrdCallFunctions��$y�Hidden=�$��0,0,0,0q� �� $��50 ��� $��35 ��� $��35 ��� $��35 ��� $��35 � �� $
��Auto ��� $��* �.+��� -&   $  
buttonHold��#��[ $
 Hand��$��1��$��0��$��94��$��31��$��2,2,2,2q�$��Center=�$��Center=�$
��Hold�����	#FF817B7B�� -'   $  buttontransferCall��#��[ $
 Hand��$��1��$��1��$��94��$��31��$��Center=�$��Center=�$��2,2,2,2q�$��Transfer�����	#FF817B7B�� -(   $  	buttonEtl��#��[ $
 Hand��$��2��$��0��$��94��$��31��$��Center=�$��Center=�$��2,2,2,2q�$	��ETL���� -)   $  buttonConference��#��[ $
 Hand��$��2��$��1��$��94��$��31��$��Center=�$��Center=�$��2,2,2,2q�$��
Conference�����	#FF817B7B�� -*   $  buttontransferIvr��#��[ $
 Hand��$��3��$��0��$��94��$��31��$��Center=�$��Center=�$��2,2,2,2q�$	��IVR�����	#FF817B7B�� -+   $  buttonswapCall��#��[ $
 Hand��$��3��$��1��$��94��$��31��$��Center=�$��Center=�$��2,2,2,2q�$
��Swap�����	#FF817B7B�� -,   $  
buttonMute��#��[ $
 Hand��$��4��$��0��$��94��$��31��$��Center=�$��Center=�$��2,2,2,2q�$
��Mute�����	#FF817B7B�� --   $  buttonDilapadshow��#��[ $
 Hand��$��4��$��1��$��94��$��31��$��Center=�$��Center=�$��2,2,2,2q�$��DailPad�����	#FF817B7B� -.   $ 	waitPanel��$y�Hidden=� �� $��50 ��� $��60 � �� $��60 ��� $��* �.+��� $��1��$��2��$��Center=�$��Center=�.+��� $��Please Wait...��$��20(�$��Center=�$��Center=�$��40��� -/   $��prgTest��$	��250��$��13��$��Center=�$��Center=�* � IsIndeterminate* . $
% 1000Q�$
 Wait�����	#FF69C9AA���	#FF5DC370���	#FFACF1B9�� -0   $ BorderDisplay��$	��163��$	��Top=����	#FFA5A5A5$��1q�$	 20��$	��205��$��Center=�$��0,33,0,0q����	#EDFFFFFF._�� .+��� -1   + ��Name$+ textBlockRegStatus��$
��Left=�$��24��$��8,8,0,0q�$
��Wrap=�$��Offline��$	��Top=�$��82��$��16(�$��Times New Roman)��� -2   $+ textBlockIdentifier��$��Right=�$��16��$��0,8,8,0q�$
��Wrap=�$��
Identifier��$	��Top=�, �� TextAlignment$, Right=�$��16(�$��Times New Roman)��� -3   $+ textBlockCallStateInfo��$��8,50,8,0q�$
��Wrap=�$��No connection��$, Center=�$��18.667(�$��Times New Roman)�$��Center=�$	��Top=��� -4   - ��Name$- textBlockDialingNumber��#��[  $��41��. �� TextWrapping$
. Wrap=�/ �� TextAlignment$/ Center=�$��7,0,7,18q�$��30(�$��Times New Roman)�$ Number�����	#FFE7E9F1$��2q�$	��189��$��Bottom=�$
��Bold"��� -5   0 ��Name$0 picMic��$"��Resources/Mutemicrophone.ico��$y�Hidden=�$��18��$��18��$	��Top=�$��24,8,0,0q�$
" 7000���� -6   $0 picSpek��$��Resources/muteSpek.ico��$y�Hidden=�$��18��$��18��$	��Top=�$��0,8,30,0q�$
" 7001���� $
��Left=�$��21��$��28,5,0,0q�$	��Top=�$��24��$9��3pack://siteoforigin:,,,/Resources/facetone-logo.ico���� $��16��$
��Wrap=�$��Facetone��$��92,6,0,0q�$	��Top=�$
��Left=�1 �� 
LineHeight$1 14��$, Center=�$
��Bold"�