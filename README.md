# mcs-ICodeCompiler

[Mono MCS](http://www.mono-project.com/docs/about-mono/languages/csharp/) is a C# console application used to compile C#, I took it's source and used it to implement [ICodeCompiler](https://msdn.microsoft.com/en-us/library/system.codedom.compiler.icodecompiler(v=vs.110).aspx) interface.

### Why
Mono version that Unity uses has an ICodeCompiler iplementation that depends heavily on paths and likely will work only on Linux systems with Mono installed. (see for your self: https://www.google.cz/search?q=mono+CSharpCodeCompiler.cs). Thus if your game uses ICodeCompiler implementation provided by Mono it will likely cause exceptions in release build. (Because a release build uses only 2MB Mono runtime, whereas Unity editor uses the full ~300MB Mono install)

MCS was recently dual licensed under MIT X11 and GNU GPL. Thus we choose MIT X11 which allows everyone to use it in commercial applications. (see: https://github.com/mono/mono/blob/master/LICENSE)

Part of ongoing effort to perfect the answer for: http://answers.unity3d.com/questions/364580/scripting-works-in-editor-try-it-but-not-build.html

## Mentioned by

More humanly explained: [Compile C# at runtime in Unity3D](http://www.arcturuscollective.com/archives/22) (thank you [exodrifter](https://github.com/exodrifter))

http://gamedev.stackexchange.com/a/130584/41980 (useful warning here)

[CS-Script for Unity](https://www.assetstore.unity3d.com/en/#!/content/23510)

## Steps I took to make this work

1. Download official Mono release.
1. Delete everything that is not needed for MCS, download externals that are needed by MCS.
1. Find a way to run jay (the parser generator), mostly from looking at the code of it and or the Makefiles
1. Jay parser generator was compiled and then ran using the mcs/jay/#_GENERATE_PARSER_FROM_cs-parser.jay.bat
1. Once jay is used, the cs-parser.jay is transformed into parser file called cs-parser.cs
1. cs-parser.cs is the core of the mcs.
1. In order to compile the mcs for dynamic runtime compilation you need to adjust the compilation symbols:
	1. Remove STATIC
	1. Add BOOTSTRAP_BASIC
	1. Change NET_X_X to NET_2_1 (we need older NET because we want to use this mcs inside Unity3D)
1. Change all internal classes to public, so they can be used outside of the dll, in modified driver (driver = main class of mcs).
1. Compile mcs.dll with .NET subset for Unity provided by Microsoft Visual Tools for Unity.
1. Take the original driver and modify it to implement ICodeCompiler interface.

This way mcs.dll is compiled for dynamic compilation thus it uses System.Reflection.Emit as it parses the code. That means it is only compatible with platforms where emiting is available on.

Dynamic compilation means your code is compiled into System.Reflection.Emit.AssemblyBuilder.

If compilation takes long time everytime you start your game, you could technically save compiled dynamic assembly into dll file with [System.Reflection.Emit.AssemblyBuilder.Save(string filePath)](https://msdn.microsoft.com/en-us/library/8zwdfdeh(v=vs.110).aspx) and on the next run load it with [System.Reflection.Assembly.LoadFrom(string assemblyFile)](https://msdn.microsoft.com/en-us/library/1009fa28(v=vs.110).aspx).

## Why am I releasing this here
* I had this laying around for about 3 months.
* I see others doing ugly workarounds. 
(I think Cities: Skylines had a complete ~300mb Mono in their release just so they can compile C# code during runtime ?)
* I never properly tested it.

## Future work
You can limit the permissions of assemblies by loading them into your own sandboxed AppDomain. This is what [Space Engineers](https://github.com/KeenSoftwareHouse/SpaceEngineers/) and other games do. It would be nice if this project contained an easy to use classes to do this.

## Default behavior
Tries to be as close as possible to the official .NET ICodeCompiler implementation.
 
## Performance considerations
All of the classes needed are instantiated and used once per each compilation. Each compilation results in one more loaded assembly that you can not unload (you can only unload AppDomain, but not Assembly).

## License
Same as MCS: MIT X11
