# NOT MAINTAINED
I don't plan on fixing this.
Feel free to fix and create pull request.
If I were to work on this, I would instead create new version that uses [Roslyn](https://github.com/dotnet/roslyn/tree/master/src/Interactive/Features/Interactive/Core).

# mcs-ICodeCompiler

[MCS](http://www.mono-project.com/docs/about-mono/languages/csharp/) (also know as Mono.CSharp) is a C# code compiler coded in C#, I took it's source and used it to implement [ICodeCompiler](https://msdn.microsoft.com/en-us/library/system.codedom.compiler.icodecompiler(v=vs.110).aspx) interface. Out of the box Mono.CSharp.dll already supports limited form of dynamic compilation (Mono.CSharp.Evaluator.Compile), I just went out of the way to modify it to implement ICodeCompiler interface.

### Why
Mono version that Unity uses has an ICodeCompiler implementation that heavily depends on paths and other shenanigans. Thus if your game uses ICodeCompiler implementation provided by Mono it will likely cause exceptions in release build. (Because a release build uses only 2MB Mono runtime, without the required files, whereas Unity editor uses the full ~300MB Mono install)

MCS was recently dual licensed under MIT X11 and GNU GPL. Thus we choose MIT X11 which allows everyone to use it in commercial applications. (see: https://github.com/mono/mono/blob/master/LICENSE)

Part of ongoing effort to perfect the answer for: http://answers.unity3d.com/questions/364580/scripting-works-in-editor-try-it-but-not-build.html

## Mentioned by

More humanly explained: [Compile C# at runtime in Unity3D](http://www.arcturuscollective.com/archives/22) (thank you [exodrifter](https://github.com/exodrifter))

Useful warning http://gamedev.stackexchange.com/a/130584/41980

Pretty much replaces [CS-Script for Unity](https://www.assetstore.unity3d.com/en/#!/content/23510)

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

Latest MCS uses expression trees for dynamic compilation instead of dynamic assembly, thus latest MCS with dynamic compilation won't work with Unity.

Theoretically you don't need to implement ICodeCompiler at all, MCS out of the does expose Mono.CSharp.Evaluator class. You however have less control over the output if you compile your code this way.

## Why am I releasing this here
* I had this laying around.
* I see others doing ugly workarounds. 
* I never properly tested it.

## Future work
You can limit the permissions of assemblies by loading them into your own sandboxed AppDomain. This is what [Space Engineers](https://github.com/KeenSoftwareHouse/SpaceEngineers/) and other games do. It would be nice if this project contained an easy to use classes to do this.

## License
Same as MCS: MIT X11
