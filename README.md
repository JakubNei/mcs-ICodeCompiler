# mcs-ICodeCompiler

Fully C# mannaged ICodeCompiler implementation that uses modified MCS (Mono C# compiler).

Mono version that Unity uses has an ICodeCompiler iplementation that depends heavily on paths and likely will work only on Linux systems with Mono installed. (see for your self: https://github.com/mosa/Mono-Class-Libraries/blob/master/mcs/class/System/Microsoft.CSharp/CSharpCodeCompiler.cs). Thus if your game uses ICodeCompiler provided by Mono it will likely cause exceptions in release build. (Because a release build uses only 2MB Mono runtime, whereas Unity editor uses full ~300MB Mono install)

MCS was recently dual licensed under MIT X11 and GNU GPL. Thus we choose MIT X11 which allows everyone to use it in commercial applications. (see: https://github.com/mono/mono/blob/master/LICENSE)

Part of ongoing effort to perfect the answer for: http://answers.unity3d.com/questions/364580/scripting-works-in-editor-try-it-but-not-build.html

More humanly explained: [Compile C# at runtime in Unity3D](http://www.arcturuscollective.com/archives/22) (thank you [exodrifter](https://github.com/exodrifter))

## Steps I took to make this work

0. Download official Mono release.
0. Delete everything that is not needed for MCS, download externals that are needed by MCS.
0. Find a way to run jay (the parser generator), mostly from looking at the code of it and or the Makefiles
0. Jay parser generator was compiled and then ran using the mcs/jay/#_GENERATE_PARSER_FROM_cs-parser.jay.bat
0. Once jay is used, the cs-parser.jay is transformed into parser file called cs-parser.cs
0. cs-parser.cs is the core of the mcs.
0. In order to compile the mcs for dynamic runtime compilation you need to adjust the compilation symbols:
	0. Remove STATIC
	0. Add BOOTSTRAP_BASIC
	0. Change NET_X_X to NET_2_1 (we need older NET because we want to use this mcs inside Unity3D)
0. Change all internal classes to public, so they can be used in modified driver (driver = main class of mcs).
0. Compile mcs.dll with .NET subset for Unity provided by Microsoft Visual Tools for Unity. 
0. The modified driver is then used to implement ICodeCompiler interface.

This way mcs.dll is compiled for dynamic compilation thus it uses System.Reflection.Emit as it parses the code. That means it is only compatible with platforms where emiting is available on.

Dynamic compilation means your code is compiled into System.Reflection.Emit.AssemblyBuilder.


## Why am I releasing this here

* I had this laying around for about 3 months.
* I see others doing ugly workarounds. 
(Cities: Skylines had a complete ~300mb mono in their release just so they can compile C# code during runtime ?)
* I never properly tested it.


## Default behavior
Tries to be as close as possible to the official .NET ICodeCompiler implementation.
 
## Performance considerations
All of the classes needed are instantiated and used once per each compilation. Each compilation results in one more loaded assembly that you can not unload (you can only unload AppDomain, but not Assembly).

## License
Same as MCS: MIT X11
