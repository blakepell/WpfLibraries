using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Markup;

// In SDK-style projects such as this one, several assembly attributes that were historically
// defined in this file are now automatically added during build and populated with
// values defined in project properties. For details of which attributes are included
// and how to customise this process see: https://aka.ms/assembly-info-properties
[assembly: XmlnsDefinition("http://thriple.codeplex.com/", "Thriple.Controls")]
[assembly: XmlnsDefinition("http://thriple.codeplex.com/", "Thriple.Panels")]
[assembly: XmlnsPrefix("http://thriple.codeplex.com/", "thriple")]

// Setting ComVisible to false makes the types in this assembly not visible to COM
// components.  If you need to access a type in this assembly from COM, set the ComVisible
// attribute to true on that type.

[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM.

[assembly: Guid("85673513-378d-4997-a415-c1f08f48fe90")]


[assembly: ThemeInfo(
    ResourceDictionaryLocation.None, //where theme specific resource dictionaries are located
    //(used if a resource is not found in the page, 
    // or application resource dictionaries)
    ResourceDictionaryLocation.SourceAssembly //where the generic resource dictionary is located
    //(used if a resource is not found in the page, 
    // app, or any theme specific resource dictionaries)
)]