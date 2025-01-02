# WPF Libraries

The project includes a collection legacy WPF libraries that have been converted to target the latest versions of .NET Core.  When possible the demo projects associated with those projects will also be included.

## Libaries

- [MvvmFoundation.Wpf](docs/MvvmFoundation.Wpf.md) : Simple MVVM Library for WPF
- [Thriple](docs/Thriple.md) : 3D Control Library
- [Transitionals](docs/Transitionals.md) : Transition Animations between visual controls.

## Supported Targets

These libraries have been updated to support the latest .NET Core targets.  When possible, legacy target frameworks are also left to allow for the widest range of usage from whatever target you're using, whether it be an old .NET Framework 3.5 app from 2007 or a .NET 7 app from 2023.  In general this is the base set of targets I'm compiling the released Nuget packages for.  This makes it a requirement to have these SDK's or targeting packs installed in order to build (as of the writing of this these are all supported versions of .NET or .NET Framework).

 - .NET 8.0
 - .NET 7.0
 - .NET 6.0
 - .NET Framework 4.8
 - .NET Framework 4.6.2
 - .NET Framework 3.5

## Contributions

Contributions are not expected but are welcome!  At a minimum new projects should build and target .NET Core (they don't need to be updated for new constructs but they can be).  There are a lot of great WPF libraries that have been lost over time (and a lot of projects committed to GitHub that can't run without those libraries).  Many of these libraries predated wide spread adoption of Nuget (or Nuget itself which was initially released in October of 2010).  Although many can still be found today the prospects of them disappearing totally increase as the months and years go by.

## Resources

 - [CodePlex Archive](http://www.codeplexarchive.org/).
