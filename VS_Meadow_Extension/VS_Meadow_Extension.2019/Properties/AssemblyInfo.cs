﻿using Meadow;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: Microsoft.VisualStudio.Shell.ProvideCodeBase(CodeBase = @"$PackageFolder$\Meadow.2019.dll")]
[assembly: Microsoft.VisualStudio.Shell.ProvideCodeBase(CodeBase = @"$PackageFolder$\Meadow.CLI.Core.dll")]
[assembly: Microsoft.VisualStudio.Shell.ProvideCodeBase(CodeBase = @"$PackageFolder$\Mono.Cecil.dll")]
[assembly: Microsoft.VisualStudio.Shell.ProvideCodeBase(CodeBase = @"$PackageFolder$\Mono.Cecil.Rocks.dll")]
[assembly: Microsoft.VisualStudio.Shell.ProvideCodeBase(CodeBase = @"$PackageFolder$\System.IO.Ports.dll")]

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("VS_Meadow_Extension.2019")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("VS_Meadow_Extension.2019")]
[assembly: AssemblyCopyright("Copyright © 2020")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("022b76f5-0995-412e-88e7-699c2559d0bf")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion(Globals.AssemblyVersion)]
[assembly: AssemblyFileVersion(Globals.AssemblyVersion)]
