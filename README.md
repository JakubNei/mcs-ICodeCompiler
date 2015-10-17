# mcs-ICodeCompiler


Most useful for Unity3D or any other application that needs a proper implementation of ICodeCompiler. Main reason is that the official mono implementation depends heavily on paths and more often than not does not work at all, see for your self: https://github.com/mosa/Mono-Class-Libraries/blob/master/mcs/class/System/Microsoft.CSharp/CSharpCodeCompiler.cs

Thankfully for us mcs was recently dual licensed under MIT X11 as well. Thus it is usable in comercial applications. (see: https://github.com/mono/mono/blob/master/LICENSE)


What I did to make this work:

Download official mono release.
Delete everything that is not needed for mcs, download externals that are needed by mcs.

Find a way to run jay (the parser generator), mostly from looking at the code of it and or the Makefile
Jay parser generator was compiled and then ran using the #_GENERATE_PARSER_FROM_cs-parser.jay.bat
Once jay is used, the cs-parser.jay is transformed into parser file called cs-parser.cs
cs-parser.cs is the core of the mcs.
In order to compile the mcs for dynamic runtime compilation you need to adjust the compilation symbols:
Remove STATIC
Add BOOTSTRAP_BASIC
Change NET_X_X to NET_2_1 (we need NEt 2.1 because we want to use this mcs inside Unity3D as well)

I also changed all internal classes to public, so i can use them in my modified driver (the main class of mcs).
The modified driver is then used to implement ICodeCompiler interface.




Why am I releasing this for free:

I had this laying around for about 3 months.
I see others doing horrific workarounds. 
(Cities: Skylines had a complete 300mb mono in their release just so they can compile C# code during runtime ?)
I never properly tested it to be release and battle worthy.