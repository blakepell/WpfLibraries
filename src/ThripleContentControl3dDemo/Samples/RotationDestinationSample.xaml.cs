﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using Thriple.Controls;

namespace ThripleContentControl3dDemoApp.Samples
{
	public partial class RotationDestinationSample : ISample
	{
		public RotationDestinationSample()
		{
			InitializeComponent();

			base.DataContext = new ThingCollectionViewModel(new List<ThingViewModel>
			{
				new("Sea Creature", "Fish1.jpg"),
				new("Pimp Masta G", "Grant.jpg"),
				new("WPF Geek", "JoshSmith.jpg"),
				new(".NET", "DotNetNew.png"),
				new("Das Spaceship", "Spaceship1.jpg"),
				new("Hawai'i View", "HawaiianBayAtSunrise.jpg")
			});
		}

		public string Description =>
            "This sample shows how passing a RotationDestination value as a parameter to the RotateCommand " +
            "can allow you to specify which side of the ContentControl3D should be in view.  In this example, " +
            "a ContentControl3D is injected into its associated ViewModel object, so that the RotateCommand " +
            "has a target element to execute against.";

        void ContentControl3D_Loaded(object sender, RoutedEventArgs e)
		{
			// Inject the ContentControl3D into the ViewModel object, so that it
			// knows which control the RotateCommand should target.
			ContentControl3D cc3D = sender as ContentControl3D;
			ThingViewModel thing = cc3D.DataContext as ThingViewModel;
			if (thing != null)
            {
                thing.CommandTarget = cc3D;
            }
        }
	}

	public class ThingCollectionViewModel
	{
		bool _showDetailedView;
		readonly ReadOnlyCollection<ThingViewModel> _things;

		public ThingCollectionViewModel(List<ThingViewModel> things)
		{
			_things = new ReadOnlyCollection<ThingViewModel>(things);
		}

		public bool ShowDetailedView
		{
			get => _showDetailedView;
            set
			{
				if (value == _showDetailedView)
                {
                    return;
                }

                _showDetailedView = value;

				foreach (ThingViewModel thing in _things)
                {
                    thing.VerifyCorrectSideIsInView(_showDetailedView);
                }
            }
		}

		public ReadOnlyCollection<ThingViewModel> Things => _things;
    }

	public class ThingViewModel
	{
		public ThingViewModel(string name, string imageFileName)
		{
			this.Name = name;
			this.ImageUri = new Uri("Images/" + imageFileName, UriKind.Relative);
		}

		public ContentControl3D CommandTarget { get; set; }
		public Uri ImageUri { get; private set; }
		public string Name { get; private set; }

		internal void VerifyCorrectSideIsInView(bool showDetailedView)
		{
			if (this.CommandTarget == null)
            {
                return;
            }

            RotationDestination destination =
				showDetailedView ?
				RotationDestination.BackSide :
				RotationDestination.FrontSide;

			if (ContentControl3D.RotateCommand.CanExecute(destination, this.CommandTarget))
            {
                ContentControl3D.RotateCommand.Execute(destination, this.CommandTarget);
            }
        }
	}
}