using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using Thriple.Panels;

namespace ThriplePanel3dDemoApp
{
    public partial class BoundWindow
    {
        private ObservableCollection<string> _dataItems;
        private int _newItemCount = 0;
        private Panel3D _panel3D;

        public BoundWindow()
        {
            InitializeComponent();

            this.ResetDataSource();

            this.itemLayoutDirectionEditor.AddHandler(Slider.ValueChangedEvent, (RoutedEventHandler)this.OnApplyItemLayoutDirection);

            this.listBox.AddHandler(Panel3D.ItemsHostLoadedEvent, (RoutedEventHandler)this.OnPanel3DLoaded);

            this.panelConfigSelector.ItemsSource = this.Resources.Keys;
            this.panelConfigSelector.SelectedItem = "Default";
            this.panelConfigSelector.SelectionChanged += panelConfigSelector_SelectionChanged;

            this.chkAutoAdjustOpacity.Click += this.chkAutoAdjustOpacity_Click;
            this.chkAllowTransparency.Click += this.chkAllowTransparency_Click;
        }

        private void chkAutoAdjustOpacity_Click(object sender, RoutedEventArgs e)
        {
            if (_panel3D != null)
            {
                _panel3D.AutoAdjustOpacity = this.chkAutoAdjustOpacity.IsChecked ?? false;
            }
        }

        private void chkAllowTransparency_Click(object sender, RoutedEventArgs e)
        {
            if (_panel3D != null)
            {
                _panel3D.AllowTransparency = this.chkAllowTransparency.IsChecked ?? false;
            }
        }

        private void panelConfigSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.TryFindResource(this.panelConfigSelector.SelectedItem) is ItemsPanelTemplate panelConfig)
            {
                this.listBox.ItemsPanel = panelConfig;
            }
        }

        private void OnPanel3DLoaded(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is Panel3D)
            {
                // Grab a reference to the Panel3D when it loads.
                _panel3D = e.OriginalSource as Panel3D;
                _panel3D.AllowTransparency = this.chkAllowTransparency.IsChecked ?? false;
                _panel3D.AutoAdjustOpacity = this.chkAutoAdjustOpacity.IsChecked ?? false;

                this.itemLayoutDirectionEditor.DataContext = _panel3D.ItemLayoutDirection;
            }
        }

        private void OnItemClick(object sender, RoutedEventArgs e)
        {
            if (this.chkMoveItemToFrontOnClick.IsChecked ?? false)
            {
                // Move the item that was clicked to the front of the Panel3D scene.
                var elem = e.Source as FrameworkElement;
                int childIndex = this.listBox.Items.IndexOf(elem.DataContext);
                int visibleIndex = _panel3D.GetVisibleIndexFromChildIndex(childIndex);
                if (0 < visibleIndex && !_panel3D.IsMovingItems)
                {
                    _panel3D.MoveItems(visibleIndex, true);
                }
            }
            else
            {
                MessageBox.Show($"You clicked on {(e.Source as Button).DataContext}");
            }
        }

        private void OnApplyItemLayoutDirection(object sender, RoutedEventArgs e)
        {
            Vector3D dir = new Vector3D(
                this.itemLayoutDirectionEditorX.Value,
                this.itemLayoutDirectionEditorY.Value,
                this.itemLayoutDirectionEditorZ.Value);

            _panel3D.ItemLayoutDirection = dir;

            this.itemLayoutDirectionValue.Text = $"{dir.X:##.###},  {dir.Y:##.###},  {dir.Z:##.###}";
        }

        private void MoveForwardButtonClicked(object sender, RoutedEventArgs e)
        {
            if (_panel3D != null)
            {
                _panel3D.MoveItems(1, true);
            }
        }

        private void MoveBackButtonClicked(object sender, RoutedEventArgs e)
        {
            if (_panel3D != null)
            {
                _panel3D.MoveItems(1, false);
            }
        }

        private void PageForwardButtonClicked(object sender, RoutedEventArgs e)
        {
            if (_panel3D != null)
            {
                _panel3D.MoveItems(3, true);
            }
        }

        private void PageBackButtonClicked(object sender, RoutedEventArgs e)
        {
            if (_panel3D != null)
            {
                _panel3D.MoveItems(3, false);
            }
        }

        private void AddItemButtonClicked(object sender, RoutedEventArgs e)
        {
            string newValue = $"item {_newItemCount++}";
            _dataItems.Add(newValue);

            this.UpdateSlider();
        }

        private void Append100ItemsButtonClicked(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 100; ++i)
            {
                string newValue = $"item {_newItemCount++}";
                _dataItems.Add(newValue);
            }

            this.UpdateSlider();
        }

        private void InsertItemButtonClicked(object sender, RoutedEventArgs e)
        {
            if (0 < _dataItems.Count)
            {
                string newValue = $"item {_newItemCount++}";
                _dataItems.Insert(1, newValue);
            }

            this.UpdateSlider();
        }

        private void RemoveItemButtonClicked(object sender, RoutedEventArgs e)
        {
            if (0 < _dataItems.Count)
            {
                _dataItems.RemoveAt(0);
            }

            this.UpdateSlider();
        }

        private void ResetDataSourceButtonClicked(object sender, RoutedEventArgs e)
        {
            this.ResetDataSource();
        }

        private void ResetDataSource()
        {
            this.Cursor = Cursors.Wait;

            _newItemCount = 0;

            if (_dataItems == null)
            {
                _dataItems = new ObservableCollection<string>();
                base.DataContext = _dataItems;
            }
            else
            {
                _dataItems.Clear();
            }

            this.UpdateSlider();

            this.Cursor = Cursors.Arrow;
        }

        private void UpdateSlider()
        {
            this.selectedIndexSlider.Maximum = _dataItems.Count - 1;
        }
    }
}