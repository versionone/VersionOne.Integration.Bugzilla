using System.Reflection;

[assembly: AssemblyTitle("VersionOne ServiceHost Core Services")]

#if !DEBUG
[assembly: AssemblyDelaySign(false)]
[assembly: AssemblyKeyFile("..\\..\\..\\Common\\SigningKey\\VersionOne.snk")]
[assembly: AssemblyKeyName("")]
#endif
