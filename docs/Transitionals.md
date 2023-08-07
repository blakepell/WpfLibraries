# Transitionals

Transitionals is a framework that allows more than one piece of graphical content to share the same space in an applications user interface. It does this by providing a set of controls and an extensible library of animations that allow the user to switch between these pieces of content dynamically at run time.

An application that wishes to use transitions can start by defining a placeholder in the UI where shared content will be hosted. For the initial design any placeholder will do. A static image or even an empty panel is enough to get you started building the surrounding UI. When it's time to actually start hosting content you'll need to decide how that content will be shown and how it will be navigated. After these questions are answered, choose a transition-aware control to put in place of your temporary content.

Currently Transitionals ships with only two controls out of the box: TransitionElement and Slideshow. Other controls, like a Tab control for example, could also be created. We encourage the community to come up with other common navigation and presentation scenarios that can leverage transitions.

For the remainder of this getting started document we'll focus on leveraging TransitionElement. TransitionElement has a Transition property that can be used to specify a single transition that will occur whenever content changes. In the Transitions namespace you'll find several transitions that can be instantiated and set to this property. You can also specify the transition in xaml:

``` xml
<transc:TransitionElement x:Name="TransitionBox">
    <transc:TransitionElement.Transition>
        <transt:RotateTransition Angle="45" />
    </transc:TransitionElement.Transition>
</transc:TransitionElement>
```

Notice in this example that the Angle property is explicitly set to 45°. This is entirely optional and overrides the default angle of 90°.

Once the TransitionElement has been added to the UI and a transition has been specified, you can begin supplying content to be displayed. This is accomplished by simply setting the Content property on TransitionElement. To do that, make sure you've named your TransitionElement so that you can access it from your code-behind. In the example above you can see we've named our TransitionElement TransitionBox.

Now let's assume for a moment that we created two different user controls at the top of our class and stored them for later use:

``` csharp
UserControlA userControlA = new UserControlA();
UserControlB userControlB = new UserControlB();
```

We could add two buttons to our UI and allow the user to switch between the two controls like this:

``` csharp
private void AButton_Click(object sender, RoutedEventArgs e)
{
    TransitionBox.Content = userControlA;
}

private void BButton_Click(object sender, RoutedEventArgs e)
{
    TransitionBox.Content = userControlB;
}
```

If the user clicks the same button twice no transition will occur because the content is not actually changing. On the other hand, if userControlA is displayed and the user clicks 'BButton' content will change and a transition will occur.

Another feature I'd like to cover as part of getting started is what I call transition strategies. In addition to the Transition property, each control that supports transitions also has a TransitionSelector property. This property works similarly to the way ItemTemplateSelector works on an ItemsControl. In other words, each time a transition needs to occur you can use your own custom code to supply the transition to run. You do this by creating your own class that inherits from TransitionSelector. TransitionSelector has one method, SelectTransition, that you must override. Here is the SelectTransition signature:

``` csharp
public virtual Transition SelectTransition(object oldContent, object newContent)
```

As you can see, a transition selector is passed the old content and the new content and is expected to return a Transition to perform. It's worth noting that null (Nothing in VB) can be returned and no transition will occur. Instead, new content will immediately replace old content.

In building samples and applications with the Transitionals framework, one strategy we bumped into quite a bit is the desire to select a random transition each time one should occur. To meet this need we created RandomTransitionSelector. RandomTransitionSelector allows you to supply a list of transitions along with their configuration and each time a transition occurs, one is required randomly selected from the supplied list. RandomTransitionSelector can be used like this:

``` xml
<transc:TransitionElement x:Name="TransitionBox">
    <transc:TransitionElement.TransitionSelector>
        <trans:RandomTransitionSelector>
            <transt:DoorTransition/>
            <transt:DotsTransition/>
            <transt:RotateTransition Angle="45" />
            <transt:RollTransition/>
        </trans:RandomTransitionSelector>
    </transc:TransitionElement.TransitionSelector>
</TransitionElement>
```
 

But what if you want to use every transition available in an assembly, or even multiple assemblies? Just add them using the TransitionAssemblies property like this:

``` xml
<trans:RandomTransitionSelector>
    <trans:RandomTransitionSelector.TransitionAssemblies>
        <refl:AssemblyName Name="Transitionals" />
    </trans:RandomTransitionSelector.TransitionAssemblies>
</trans:RandomTransitionSelector>
```
 
Note here that Name is an assembly name, so standard conventions apply.

Now suppose you want to load up all the transitions available in an assembly but you need to specify some non-default values for one or two transitions. No problem, just list the ones you want to customize along with their settings like so:

``` xml
<trans:RandomTransitionSelector>
    <trans:RandomTransitionSelector.TransitionAssemblies>
        <refl:AssemblyName Name="Transitionals" />
    </trans:RandomTransitionSelector.TransitionAssemblies>
    <transt:RotateTransition Angle="45" />
</trans:RandomTransitionSelector>
```
 
 Whenever RandomTransitionSelector loads a transition from an assembly it checks to see if you've already added that particular transition. If you have, it won't try to add it again. So if you want to add two or more copies of a particular transition with different settings, you can add two or more entries to the list:

 
``` xml
<trans:RandomTransitionSelector>
    <trans:RandomTransitionSelector.TransitionAssemblies>
        <refl:AssemblyName Name="Transitionals" />
    </trans:RandomTransitionSelector.TransitionAssemblies>
    <transt:RotateTransition Angle="45" />
    <transt:RotateTransition Angle="25" />
</trans:RandomTransitionSelector>
```
 
Finally, since each transition can specify its own default duration, what if you want all of them to have the same duration? You can use the TransitionDuration property like this:

``` xml
<trans:RandomTransitionSelector TransitionDuration="0:0:5">
    <trans:RandomTransitionSelector.TransitionAssemblies>
        <refl:AssemblyName Name="Transitionals" />
    </trans:RandomTransitionSelector.TransitionAssemblies>
</trans:RandomTransitionSelector>
```
 
Using the Xaml above, RandomTransitionSelector will attempt to update the Duration property to 5 seconds right before the transition is returned to TransitionElement. Note that in the first release some transition don't allow their durations to be changed. This is a known issue and if the duration can't be changed the default value will be used instead.

That's iThat's it for this first release. If you'd like to check out all the included transitions, run the TransitionTester sample. That sample will even let you load up your own transition assemblies and test them out too. If you'd like to see a working example of using RandomTransitionSelector, check out the ImageViewer sample application. Please note that very large images currently don't transition as expected becase of the time it takes to load the image. We're looking into it.

In the examples in this document the following namespace aliases were used:

``` text
xmlns:trans="clr-namespace:Transitionals;assembly=Transitionals"
xmlns:transc="clr-namespace:Transitionals.Controls;assembly=Transitionals"
xmlns:transt="clr-namespace:Transitionals.Transitions;assembly=Transitionals"
xmlns:refl="clr-namespace:System.Reflection;assembly=mscorlib"
```

### Credits:

Originally created by Jared Bienz for Acropolis and ported to the Transitionals framework on 2/24/2008.

### License:

Microsoft Public License (Ms-PL)