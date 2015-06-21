using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Markup;
using System.Resources;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Gu.Wpf.Validation")]
[assembly: AssemblyDescription("Attached properties for WPF-textbox")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Johan Larsson")]
[assembly: AssemblyProduct("Gu.Wpf.Validation")]
[assembly: AssemblyCopyright("Copyright ©  2015")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("c69c03d5-338d-41f5-9634-21ed5cc62262")]

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
[assembly: AssemblyVersion("1.0.3.0")]
[assembly: AssemblyFileVersion("1.0.3.0")]
[assembly: InternalsVisibleTo("Gu.Wpf.Validation.Tests", AllInternalsVisible = true)]
[assembly: InternalsVisibleTo("Gu.Wpf.Validation.Demo", AllInternalsVisible = true)]
[assembly: XmlnsDefinition("http://gu.se/Validation", "Gu.Wpf.Validation")]
[assembly: XmlnsDefinition("http://gu.se/Validation", "Gu.Wpf.Validation.Internals")]
[assembly: XmlnsDefinition("http://gu.se/Validation", "Gu.Wpf.Validation.Rules")]
[assembly: XmlnsDefinition("http://gu.se/Validation", "Gu.Wpf.Validation.StringConverters")]
[assembly: XmlnsPrefix("http://gu.se/Validation", "validation")]
[assembly: NeutralResourcesLanguageAttribute("en")]
