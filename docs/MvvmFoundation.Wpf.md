# MvvmFoundation.Wpf

This library is a .NET Core port of the MvvmFoundation.Wpf library that used to exist on CodePlex and was written by
Josh Smith.  It has been updated to additionally support .NET Core targets for the purpose of allowing old projects that use it
to be updated to .NET Core.

### Nuget Package

**MvvmFoundation** - [https://www.nuget.org/packages/MvvmFoundation](https://www.nuget.org/packages/MvvmFoundation)

### Supported Targets:

 - .NET 7.0
 - .NET 6.0
 - .NET Framework 4.8
 - .NET Framework 4.6.2
 - .NET Framework 3.5

### Project Description

MVVM Foundation is a library of classes that are very useful when building applications based on the Model-View-ViewModel philosophy. The library is small and concentrated on providing only the most indispensable tools needed by most MVVM application developers.

Model-View-ViewModel is a way of creating client applications that leverages core features of the WPF platform, allows for simple unit testing of application functionality, and helps developers and designers work together with less technical difficulties. The classes in the MVVM Foundation are time-tested tools in the toolbox of many WPF developers around the world. Now they all live in one convenient project...MvvmFoundation.Wpf. The source code download also contains a set of unit tests and a demo application, which show how to use the classes. If you want to learn more about MVVM be sure to read Josh Smith's Advanced MVVM book.

**ObservableObject** - This is intended to be the base class for ViewModel types, or any type that must provide property change notifications. It implements INotifyPropertyChanged and, in debug builds, will verify that all property names passed through the PropertyChanged event are valid properties. This class used to be called ViewModelBase.

**RelayCommand** - Provides for small, simple command declarations. The execution logic, and optionally can-execute logic, is injected into its constructor.

**PropertyObserver** - A standardized way to handle the INotifyPropertyChanged.PropertyChanged event of other objects. This class uses weak references and the weak-event pattern to prevent memory leaks.

**Messenger** - The Messenger class is a lightweight way of passing messages between various ViewModel objects who do not need to be aware of each other. This is based on the Mediator implementation created by Marlon Grech and Josh Smith, as seen on Marlon's blog.