# mcs-ICodeCompiler

Most useful for Unity3D or any other application that needs a proper implementation of ICodeCompiler. Main reason is that the official mono implementation depends heavily on paths and more often than not does not work at all, see for your self: https://github.com/mosa/Mono-Class-Libraries/blob/master/mcs/class/System/Microsoft.CSharp/CSharpCodeCompiler.cs

Thankfully for us mcs was recently dual licensed under MIT X11 and GNU GPL. Thus we choose MIT X11 which allows use in commercial applications. (see: https://github.com/mono/mono/blob/master/LICENSE)

## Steps I took to make this work

0. Download official mono release.
0. Delete everything that is not needed for mcs, download externals that are needed by mcs.
0. Find a way to run jay (the parser generator), mostly from looking at the code of it and or the Makefiles
0. Jay parser generator was compiled and then ran using the mcs/jay/#_GENERATE_PARSER_FROM_cs-parser.jay.bat
0. Once jay is used, the cs-parser.jay is transformed into parser file called cs-parser.cs
0. cs-parser.cs is the core of the mcs.
0. In order to compile the mcs for dynamic runtime compilation you need to adjust the compilation symbols:
	0. Remove STATIC
	0. Add BOOTSTRAP_BASIC
	0. Change NET_X_X to NET_2_1 (we need older NET because we want to use this mcs inside Unity3D)
0. Change all internal classes to public, they can be used in modified driver (the main class of mcs).
0. Compile mcs.dll with .NET subset for Unity provided by Microsoft Visual Tools for Unity. 
0. The modified driver is then used to implement ICodeCompiler interface.


Note that this way mcs.dll is compiled for dynamic compilation thus it uses System.Reflection.Emit as it parses the code. That means it is only compatible with platfroms where emiting is available on.

## Why am I releasing this here

* I had this laying around for about 3 months.
* I see others doing ugly workarounds. 
(Cities: Skylines had a complete 300mb mono in their release just so they can compile C# code during runtime ?)
* I never properly tested it.