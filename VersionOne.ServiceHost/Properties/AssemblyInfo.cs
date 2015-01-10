using System.Reflection;

[assembly: AssemblyTitle("VersionOne Service Host Executable")]

#if !DEBUG
[assembly: AssemblyDelaySign(false)]
[assembly: AssemblyKeyFile("..\\..\\..\\..\\Common\\SigningKey\\VersionOne.snk")]
[assembly: AssemblyKeyName("")]
#endif
