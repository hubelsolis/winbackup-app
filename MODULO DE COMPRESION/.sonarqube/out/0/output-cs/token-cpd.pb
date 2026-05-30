‰
\C:\Users\DIEGCOP20\Desktop\MODULO DE COMPRESION\RAR\WinFormsAppRAR\WinFormsAppRAR\Program.cs
	namespace 	
WinFormsAppRAR
 
{ 
internal 
static 
class 
Program !
{ 
[ 	
	STAThread	 
] 
static		 
void		 
Main		 
(		 
)		 
{

 	$
ApplicationConfiguration $
.$ %

Initialize% /
(/ 0
)0 1
;1 2
Application 
. 
Run 
( 
new 
Form1  %
(% &
)& '
)' (
;( )
} 	
} 
} ‚T
ZC:\Users\DIEGCOP20\Desktop\MODULO DE COMPRESION\RAR\WinFormsAppRAR\WinFormsAppRAR\Form1.cs
	namespace 	
WinFormsAppRAR
 
{ 
public 

partial 
class 
Form1 
:  
Form! %
{ 
public		 
Form1		 
(		 
)		 
{

 	
InitializeComponent 
(  
)  !
;! "
} 	
private 
void "
btnBuscarEntrada_Click +
(+ ,
object, 2
sender3 9
,9 :
	EventArgs; D
eE F
)F G
{ 	
DialogResult 
opcion 
=  !

MessageBox" ,
., -
Show- 1
(1 2
$str r
,r s
$str %
,% &
MessageBoxButtons !
.! "
YesNoCancel" -
,- .
MessageBoxIcon 
. 
Question '
)' (
;( )
if 
( 
opcion 
== 
DialogResult &
.& '
Yes' *
)* +
{ 
using 
( 
OpenFileDialog %
ofd& )
=* +
new, /
OpenFileDialog0 >
(> ?
)? @
)@ A
{ 
ofd 
. 
Title 
= 
$str  L
;L M
ofd 
. 
Filter 
=  
$str! ?
;? @
if 
( 
ofd 
. 

ShowDialog &
(& '
)' (
==) +
DialogResult, 8
.8 9
OK9 ;
); <
{   
txtRutaEntrada!! &
.!!& '
Text!!' +
=!!, -
ofd!!. 1
.!!1 2
FileName!!2 :
;!!: ;
}"" 
}## 
}$$ 
else%% 
if%% 
(%% 
opcion%% 
==%% 
DialogResult%% +
.%%+ ,
No%%, .
)%%. /
{&& 
using(( 
((( 
FolderBrowserDialog(( *
fbd((+ .
=((/ 0
new((1 4
FolderBrowserDialog((5 H
(((H I
)((I J
)((J K
{)) 
fbd** 
.** 
Description** #
=**$ %
$str**& S
;**S T
fbd++ 
.++ "
UseDescriptionForTitle++ .
=++/ 0
true++1 5
;++5 6
if-- 
(-- 
fbd-- 
.-- 

ShowDialog-- &
(--& '
)--' (
==--) +
DialogResult--, 8
.--8 9
OK--9 ;
)--; <
{.. 
txtRutaEntrada// &
.//& '
Text//' +
=//, -
fbd//. 1
.//1 2
SelectedPath//2 >
;//> ?
}00 
}11 
}22 
}33 	
private44 
void44 
btnComprimir_Click44 '
(44' (
object44( .
sender44/ 5
,445 6
	EventArgs447 @
e44A B
)44B C
{55 	
try66 
{77 
	EstelaRar88 
.88 
	Comprimir88 #
(88# $
txtRutaEntrada88$ 2
.882 3
Text883 7
,887 8
txtRutaSalida889 F
.88F G
Text88G K
)88K L
;88L M
lstResultado99 
.99 
Items99 "
.99" #
Add99# &
(99& '
$str99' D
)99D E
;99E F
}:: 
catch;; 
(;; 
	Exception;; 
ex;; 
);;  
{<< 
lstResultado== 
.== 
Items== "
.==" #
Add==# &
(==& '
$str==' 2
+==3 4
ex==5 7
.==7 8
Message==8 ?
)==? @
;==@ A
}>> 
}?? 	
privateBB 
voidBB !
btnDescomprimir_ClickBB *
(BB* +
objectBB+ 1
senderBB2 8
,BB8 9
	EventArgsBB: C
eBBD E
)BBE F
{CC 	
stringDD 
entradaDD 
=DD 
txtRutaEntradaDD +
.DD+ ,
TextDD, 0
.DD0 1
TrimDD1 5
(DD5 6
)DD6 7
;DD7 8
stringEE 
salidaEE 
=EE 
txtRutaSalidaEE )
.EE) *
TextEE* .
.EE. /
TrimEE/ 3
(EE3 4
)EE4 5
;EE5 6
ifGG 
(GG 
stringGG 
.GG 
IsNullOrEmptyGG $
(GG$ %
entradaGG% ,
)GG, -
||GG. 0
stringGG1 7
.GG7 8
IsNullOrEmptyGG8 E
(GGE F
salidaGGF L
)GGL M
)GGM N
{HH 

MessageBoxII 
.II 
ShowII 
(II  
$strII  k
,IIk l
$strIIm {
,II{ |
MessageBoxButtons	II} é
.
IIé è
OK
IIè ë
,
IIë í
MessageBoxIcon
IIì °
.
II° ¢
Warning
II¢ ©
)
II© ™
;
II™ ´
returnJJ 
;JJ 
}KK 
ifMM 
(MM 
!MM 
SystemMM 
.MM 
IOMM 
.MM 
FileMM 
.MM  
ExistsMM  &
(MM& '
entradaMM' .
)MM. /
)MM/ 0
{NN 

MessageBoxOO 
.OO 
ShowOO 
(OO  
$strOO  G
,OOG H
$strOOI P
,OOP Q
MessageBoxButtonsOOR c
.OOc d
OKOOd f
,OOf g
MessageBoxIconOOh v
.OOv w
ErrorOOw |
)OO| }
;OO} ~
returnPP 
;PP 
}QQ 
trySS 
{TT 
lstResultadoUU 
.UU 
ItemsUU "
.UU" #
AddUU# &
(UU& '
$strUU' C
)UUC D
;UUD E
	EstelaRarVV 
.VV 
DescomprimirVV &
(VV& '
entradaVV' .
,VV. /
salidaVV0 6
)VV6 7
;VV7 8
lstResultadoWW 
.WW 
ItemsWW "
.WW" #
AddWW# &
(WW& '
$strWW' `
)WW` a
;WWa b

MessageBoxXX 
.XX 
ShowXX 
(XX  
$strXX  ;
,XX; <
$strXX= D
,XXD E
MessageBoxButtonsXXF W
.XXW X
OKXXX Z
,XXZ [
MessageBoxIconXX\ j
.XXj k
InformationXXk v
)XXv w
;XXw x
}YY 
catchZZ 
(ZZ 
	ExceptionZZ 
exZZ 
)ZZ  
{[[ 
lstResultado\\ 
.\\ 
Items\\ "
.\\" #
Add\\# &
(\\& '
$"\\' )
$str\\) @
{\\@ A
ex\\A C
.\\C D
Message\\D K
}\\K L
"\\L M
)\\M N
;\\N O

MessageBox]] 
.]] 
Show]] 
(]]  
$"]]  "
$str]]" 4
{]]4 5
ex]]5 7
.]]7 8
Message]]8 ?
}]]? @
"]]@ A
,]]A B
$str]]C J
,]]J K
MessageBoxButtons]]L ]
.]]] ^
OK]]^ `
,]]` a
MessageBoxIcon]]b p
.]]p q
Error]]q v
)]]v w
;]]w x
}^^ 
}__ 	
privatebb 
voidbb !
btnBuscarSalida_Clickbb *
(bb* +
objectbb+ 1
senderbb2 8
,bb8 9
	EventArgsbb: C
ebbD E
)bbE F
{cc 	
stringdd 
rutaEntradadd 
=dd  
txtRutaEntradadd! /
.dd/ 0
Textdd0 4
.dd4 5
Trimdd5 9
(dd9 :
)dd: ;
;dd; <
ifhh 
(hh 
!hh 
stringhh 
.hh 
IsNullOrEmptyhh %
(hh% &
rutaEntradahh& 1
)hh1 2
&&hh3 5
Systemhh6 <
.hh< =
IOhh= ?
.hh? @
Filehh@ D
.hhD E
ExistshhE K
(hhK L
rutaEntradahhL W
)hhW X
&&hhY [
rutaEntradahh\ g
.hhg h
EndsWithhhh p
(hhp q
$strhhq w
,hhw x
StringComparison	hhy â
.
hhâ ä
OrdinalIgnoreCase
hhä õ
)
hhõ ú
)
hhú ù
{ii 
usingjj 
(jj 
FolderBrowserDialogjj *
dialogoCarpetajj+ 9
=jj: ;
newjj< ?
FolderBrowserDialogjj@ S
(jjS T
)jjT U
)jjU V
{kk 
dialogoCarpetall "
.ll" #
Descriptionll# .
=ll/ 0
$strll1 n
;lln o
dialogoCarpetamm "
.mm" #"
UseDescriptionForTitlemm# 9
=mm: ;
truemm< @
;mm@ A
ifoo 
(oo 
dialogoCarpetaoo &
.oo& '

ShowDialogoo' 1
(oo1 2
)oo2 3
==oo4 6
DialogResultoo7 C
.ooC D
OKooD F
)ooF G
{pp 
txtRutaSalidaqq %
.qq% &
Textqq& *
=qq+ ,
dialogoCarpetaqq- ;
.qq; <
SelectedPathqq< H
;qqH I
}rr 
}ss 
}tt 
elseuu 
{vv 
usingxx 
(xx 
SaveFileDialogxx %
dialogoArchivoxx& 4
=xx5 6
newxx7 :
SaveFileDialogxx; I
(xxI J
)xxJ K
)xxK L
{yy 
dialogoArchivozz "
.zz" #
Filterzz# )
=zz* +
$strzz, H
;zzH I
dialogoArchivo{{ "
.{{" #
Title{{# (
={{) *
$str{{+ _
;{{_ `
dialogoArchivo|| "
.||" #
FileName||# +
=||, -
$str||. D
;||D E
if~~ 
(~~ 
dialogoArchivo~~ &
.~~& '

ShowDialog~~' 1
(~~1 2
)~~2 3
==~~4 6
DialogResult~~7 C
.~~C D
OK~~D F
)~~F G
{ 
txtRutaSalida
ÄÄ %
.
ÄÄ% &
Text
ÄÄ& *
=
ÄÄ+ ,
dialogoArchivo
ÄÄ- ;
.
ÄÄ; <
FileName
ÄÄ< D
;
ÄÄD E
}
ÅÅ 
}
ÇÇ 
}
ÉÉ 
}
ÑÑ 	
private
ÖÖ 
void
ÖÖ /
!lstResultado_SelectedIndexChanged
ÖÖ 6
(
ÖÖ6 7
object
ÖÖ7 =
sender
ÖÖ> D
,
ÖÖD E
	EventArgs
ÖÖF O
e
ÖÖP Q
)
ÖÖQ R
{
ÜÜ 	
}
àà 	
}
ââ 
}ää ¨
^C:\Users\DIEGCOP20\Desktop\MODULO DE COMPRESION\RAR\WinFormsAppRAR\WinFormsAppRAR\EstelaRar.cs
	namespace 	
WinFormsAppRAR
 
{ 
public 

class 
	EstelaRar 
{		 
public

 
static

 
void

 
Descomprimir

 '
(

' (
string

( .
rutaRar

/ 6
,

6 7
string

8 >
rutaDestino

? J
)

J K
{ 	
using 
var 
stream 
= 
File #
.# $
OpenRead$ ,
(, -
rutaRar- 4
)4 5
;5 6
using 
var 
reader 
= 
ReaderFactory ,
., -
Open- 1
(1 2
stream2 8
)8 9
;9 :
while 
( 
reader 
. 
MoveToNextEntry )
() *
)* +
)+ ,
{ 
if 
( 
! 
reader 
. 
Entry !
.! "
IsDirectory" -
)- .
reader 
. !
WriteEntryToDirectory 0
(0 1
rutaDestino1 <
,< =
new> A
ExtractionOptionsB S
{ 
ExtractFullPath '
=( )
true* .
,. /
	Overwrite !
=" #
true$ (
} 
) 
; 
} 
} 	
public 
static 
void 
	Comprimir $
($ %
string% +
rutaEntrada, 7
,7 8
string9 ?

rutaSalida@ J
)J K
{ 	
string 
winrar 
= 
$str A
;A B
if 
( 
! 
File 
. 
Exists 
( 
winrar #
)# $
)$ %
{ 
throw 
new 
	Exception #
(# $
$str$ N
)N O
;O P
} 
var   
proceso   
=   
new   
Process   %
(  % &
)  & '
;  ' (
proceso!! 
.!! 
	StartInfo!! 
.!! 
FileName!! &
=!!' (
winrar!!) /
;!!/ 0
proceso"" 
."" 
	StartInfo"" 
."" 
	Arguments"" '
=""( )
$"""* ,
$str"", 0
{""0 1

rutaSalida""1 ;
}""; <
$str""< A
{""A B
rutaEntrada""B M
}""M N
$str""N P
"""P Q
;""Q R
proceso## 
.## 
	StartInfo## 
.## 
WindowStyle## )
=##* +
ProcessWindowStyle##, >
.##> ?
Hidden##? E
;##E F
proceso$$ 
.$$ 
Start$$ 
($$ 
)$$ 
;$$ 
proceso%% 
.%% 
WaitForExit%% 
(%%  
)%%  !
;%%! "
}&& 	
}'' 
}(( 