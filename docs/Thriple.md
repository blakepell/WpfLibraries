# Thriple

### Nuget Package

**Thriple** - [https://www.nuget.org/packages/Thriple.Core](https://www.nuget.org/packages/Thriple.Core)

### Project Description

Thriple is a library of 3D controls and panels, for use in WPF applications. These reusable components make it easy to incorporate 3D in your user interfaces. Each component in the library is accompanied by sample applications, which allow you to experiment with them.

#### Why Does Thriple Exist?
WPF has powerful support for creating 3D user interfaces. Unfortunately, 3D programming is a separate discipline unto itself. To put it simply, it's complicated.

It's a shame to have so much potential for including 3D goodness in a user interface, but then not leverage it due to the very steep learning curve imposed by the 3D concepts and APIs. Let's face it; most developers are far too busy trying to get their apps working, unit tests passing, business rules updated, etc. Who has time to master the esoteric art of manipulating abstractions of 3D objects?

Over the past couple of years I have spent some time trying to come to terms with WPF 3D. I have learned enough about WPF 3D programming in my free time to be able to put together some reusable components that can help anyone easily add some 3D pizazz to their UIs. You can simply drop these components into your application, and start incorporating 3D effects in your UIs today.

#### Why Should I Add 3D To My UIs?

You certainly do not have to add 3D to your user interfaces just because they are built in WPF. However, you can. If used tastefully, 3D controls can add elegance and a certain "Wow!" factor to an otherwise plain UI. This is especially important for teams that must produce applications with an appealing user experience (UX); such as shrink-wrapped consumer facing applications. Even a more business-oriented application can benefit from using 3D controls, such as ContentControl3D, because they can help to conserve valuable screen real estate.

#### What Does Thriple Contain?
Thriple consists of a class library (DLL) which contains some 3D components, and several sample applications showing those 3D components in action. The Thriple DLL contains two components that you can use in your applications:

**ContentControl3D** - a ContentControl that can rotate in 3D space to show its BackContent to the user. This control offers many properties that allow you to customize the animated rotation. To learn more about ContentControl3D, be sure to read my Rotating WPF Content in 3D Space article on CodeProject.

**Panel3D** - a layout panel that arranges its items along a straight line in 3D space. It exposes a MoveItems method, which can be used to perform an animated movement of the items in the panel. You can read more about Panel3D in my Animating Interactive 2D Elements in a 3D Panel article on CodeProject.

The ContentControl3D_Demo project contains various samples that show how to configure ContentControl3D. A screenshot of the 'Property Explorer' sample is seen below:

Both Panel3D and ContentControl3D are put to use in the WPF Disciples Blog Roll 3D application. Each blogger's information is hosted in a ContentControl3D, with their photo and a button that opens their blog on the front side, and more information about that blogger on the back side. Those controls are, in turn, hosted in a ListBox whose ItemsPanel is a Panel3D. The net result of this UI design is that you have tiles rotating in 3D along the Y axis, and moving in/out of view along the X, Y, and Z axis. A screenshot of that demo application is below:

### Credits

This library was originally written by Josh Smith, author of (Advanced MVVM)[https://joshsmithonwpf.wordpress.com/advanced-mvvm/] (which even being 13 years old now is an excellent resource on implementing the MVVM pattern in WPF).

### License

Microsoft Public License (Ms-PL)