cd %CD%

REM To produce a debugging parser, use the version that says "-cvt"
REM JAY_FLAGS=-c
REM JAY_FLAGS=-cvt

jay.exe -cvt ..\mcs\cs-parser.jay < skeleton.cs > tmp-out.cs

move tmp-out.cs ../mcs/cs-parser.cs