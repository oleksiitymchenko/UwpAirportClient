using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UwpAirportClient.Models;
using UwpAirportClient.Services;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UwpAirportClient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Planetypes : Page
    {
        public ObservableCollection<PlaneTypeDTO> planetypesList = new ObservableCollection<PlaneTypeDTO>();

        public double height => Window.Current.Bounds.Height;
        public Thickness marginCreate => new Thickness(Window.Current.Bounds.Width - 210, 10, 10, 10);

        private GenericService<PlaneTypeDTO> service = new GenericService<PlaneTypeDTO>(new System.Net.Http.HttpClient(), Url.Value + "PlaneTypes");

        public Planetypes()
        {
            UpdateList();
            this.InitializeComponent();
            this.Loaded += Planetypes_Loaded;
        }

        private void Planetypes_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateList();
        }

        private async void UpdateList()
        {
            planetypesList.Clear();
            try
            {
                (await service.getAllAsync()).ForEach(o => planetypesList.Add(o));
            }
            catch (Exception) { };
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
                Frame.GoBack();
        }

        private (Button, TextBox, TextBox, TextBox) RenderCreate()
        {
            SingleItem.Children.Clear();

            var model = new TextBox
            {
                Header = "Model",
                Width = 200,
                PlaceholderText = "Model",
                Margin = new Thickness(0, 100, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center
            };

            var places = new TextBox
            {
                Header = "Places ",
                PlaceholderText = "Places",
                Width = 200,
                Margin = new Thickness(0, 10, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center
            };

            places.KeyDown += (object obj, KeyRoutedEventArgs evArgs) =>
            {
                if (evArgs.Key.ToString().Equals("Back"))
                {
                    evArgs.Handled = false;
                    return;
                }
                for (int i = 0; i < 10; i++)
                {
                    if (evArgs.Key.ToString() == string.Format("Number{0}", i))
                    {
                        evArgs.Handled = false;
                        return;
                    }
                }
                evArgs.Handled = true;
            };

            var carrying = new TextBox
            {
                Header = "Carrying",
                PlaceholderText = "Carrying",
                Width = 200,
                Margin = new Thickness(0, 10, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center
            };

            carrying.KeyDown += (object obj, KeyRoutedEventArgs evArgs) =>
            {
                if (evArgs.Key.ToString().Equals("Back"))
                {
                    evArgs.Handled = false;
                    return;
                }
                for (int i = 0; i < 10; i++)
                {
                    if (evArgs.Key.ToString() == string.Format("Number{0}", i))
                    {
                        evArgs.Handled = false;
                        return;
                    }
                }
                evArgs.Handled = true;
            };


            var btnCreate = new Button
            {
                Content = "Add item",
                Width = 200,
                Margin = new Thickness(0, 20, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center
            };
            SingleItem.Children.Add(model);
            SingleItem.Children.Add(places);
            SingleItem.Children.Add(carrying);
            SingleItem.Children.Add(btnCreate);
            return (btnCreate, model, places, carrying);
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            var turple = RenderCreate();
            var btnCreate = turple.Item1;
            var model = turple.Item2;
            var places = turple.Item3;
            var carrying = turple.Item4;

            btnCreate.Click += async (object sen, RoutedEventArgs evArgs) =>
            {
                var planetype = new PlaneTypeDTO()
                { Model = model.Text, Places = int.Parse(places.Text), Carrying = double.Parse(carrying.Text) };
                try
                {
                    await service.CreateAsync(planetype);
                }
                catch (Exception) { }

                planetypesList.Add(planetype);
                UpdateList();
                SingleItem.Children.Clear();
            };
        }

        private (Button, Button, TextBox, TextBox,TextBox, PlaneTypeDTO) RenderDetail(ItemClickEventArgs e)
        {
            var planetype = (PlaneTypeDTO)e.ClickedItem;
            SingleItem.Children.Clear();

            var id = new TextBlock();
            id.Text = "Planetype id:" + planetype.Id;
            id.FontWeight = Windows.UI.Text.FontWeights.Bold;
            id.Margin = new Thickness(0, 100, 10, 10);
            id.Width = 200;

            var model = new TextBox
            {
                Header = "Model",
                Text = planetype.Model,
                Width = 200,
                HorizontalAlignment = HorizontalAlignment.Center
            };


            var places = new TextBox
            {
                Header = "Price ",
                Text = planetype.Places.ToString(),
                Width = 200,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            places.KeyDown += (object obj, KeyRoutedEventArgs evArgs) =>
            {
                if (evArgs.Key.ToString().Equals("Back"))
                {
                    evArgs.Handled = false;
                    return;
                }
                for (int i = 0; i < 10; i++)
                {
                    if (evArgs.Key.ToString() == string.Format("Number{0}", i))
                    {
                        evArgs.Handled = false;
                        return;
                    }
                }
                evArgs.Handled = true;
            };

            var carrying = new TextBox
            {
                Header = "Price ",
                Text = planetype.Carrying.ToString(),
                Width = 200,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            carrying.KeyDown += (object obj, KeyRoutedEventArgs evArgs) =>
            {
                if (evArgs.Key.ToString().Equals("Back"))
                {
                    evArgs.Handled = false;
                    return;
                }
                for (int i = 0; i < 10; i++)
                {
                    if (evArgs.Key.ToString() == string.Format("Number{0}", i))
                    {
                        evArgs.Handled = false;
                        return;
                    }
                }
                evArgs.Handled = true;
            };


            var btnUpdate = new Button
            {
                Content = "Update",
                Width = 100
            };
            var btnDelete = new Button
            {
                Content = "Delete",
                Width = 100
            };

            var buttonsStack = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 20, 0, 0)
            };
            SingleItem.Children.Add(id);
            SingleItem.Children.Add(model);
            SingleItem.Children.Add(places);
            SingleItem.Children.Add(carrying);
            buttonsStack.Children.Add(btnUpdate);
            buttonsStack.Children.Add(btnDelete);
            SingleItem.Children.Add(buttonsStack);

            return (btnUpdate, btnDelete, model, places,carrying, planetype);
        }

        private void itemsList_ItemClick(object sender, ItemClickEventArgs e)
        {
            var turple = RenderDetail(e);
            var btnUpdate = turple.Item1;
            var btnDelete = turple.Item2;
            var model = turple.Item3;
            var places = turple.Item4;
            var carrying = turple.Item5;
            var planetype = turple.Item6;

            btnUpdate.Click += async (object sen, RoutedEventArgs evArgs) =>
            {
                var planetypeCreating = new PlaneTypeDTO()
                { Id = planetype.Id, Model = model.Text, Places = int.Parse(places.Text), Carrying = int.Parse(carrying.Text)};

                int index = planetypesList.ToList().FindIndex(t => t.Id == planetype.Id);
                planetypesList.Insert(index, planetypeCreating);

                try
                {
                    await service.UpdateAsync(planetypeCreating);
                }
                catch (Exception) { }

                UpdateList();
                SingleItem.Children.Clear();
            };

            btnDelete.Click += async (object sen, RoutedEventArgs evArgs) =>
            {
                planetypesList.Remove(planetype);

                try
                {
                    await service.DeleteAsync(planetype);
                }
                catch (Exception) { }
                UpdateList();
                SingleItem.Children.Clear();
            };
        }
    }
}
