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
    public sealed partial class Pilots : Page
    {
        public ObservableCollection<PilotDTO> pilotsList = new ObservableCollection<PilotDTO>();

        public double height => Window.Current.Bounds.Height;
        public Thickness marginCreate => new Thickness(Window.Current.Bounds.Width - 210, 10, 10, 10);

        private GenericService<PilotDTO> service = new GenericService<PilotDTO>(new System.Net.Http.HttpClient(), Url.Value + "Pilots");

        public Pilots()
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
            pilotsList.Clear();
            try
            {
                (await service.getAllAsync()).ForEach(o => pilotsList.Add(o));
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

            var firstname = new TextBox
            {
                Header = "Firstname",
                Width = 200,
                PlaceholderText = "Firstname",
                Margin = new Thickness(0, 100, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center
            };

            var lastname = new TextBox
            {
                Header = "Lastname ",
                PlaceholderText = "Lastname",
                Width = 200,
                Margin = new Thickness(0, 10, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center
            };

            var expirience = new TextBox
            {
                Header = "Carrying",
                PlaceholderText = "Carrying",
                Width = 200,
                Margin = new Thickness(0, 10, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center
            };

            expirience.KeyDown += (object obj, KeyRoutedEventArgs evArgs) =>
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
            SingleItem.Children.Add(firstname);
            SingleItem.Children.Add(lastname);
            SingleItem.Children.Add(expirience);
            SingleItem.Children.Add(btnCreate);
            return (btnCreate, firstname, lastname, expirience);
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            var turple = RenderCreate();
            var btnCreate = turple.Item1;
            var firstname = turple.Item2;
            var lastname = turple.Item3;
            var expirience = turple.Item4;

            btnCreate.Click += async (object sen, RoutedEventArgs evArgs) =>
            {
                var pilot = new PilotDTO()
                { FirstName = firstname.Text, LastName = lastname.Text, Experience = int.Parse(expirience.Text) };
                try
                {
                    await service.CreateAsync(pilot);
                }
                catch (Exception) { }

                pilotsList.Add(pilot);
                UpdateList();
                SingleItem.Children.Clear();
            };
        }

        private (Button, Button, TextBox, TextBox, TextBox, PilotDTO) RenderDetail(ItemClickEventArgs e)
        {
            var pilot = (PilotDTO)e.ClickedItem;
            SingleItem.Children.Clear();

            var id = new TextBlock();
            id.Text = "Pilot id:" + pilot.Id;
            id.FontWeight = Windows.UI.Text.FontWeights.Bold;
            id.Margin = new Thickness(0, 100, 10, 10);
            id.Width = 200;

            var firstname = new TextBox
            {
                Header = "FirstName",
                Text = pilot.FirstName,
                Width = 200,
                HorizontalAlignment = HorizontalAlignment.Center
            };


            var lastname = new TextBox
            {
                Header = "Lastname",
                Text = pilot.LastName,
                Width = 200,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            var experience = new TextBox
            {
                Header = "Experience",
                Text = pilot.Experience.ToString(),
                Width = 200,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            experience.KeyDown += (object obj, KeyRoutedEventArgs evArgs) =>
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
            SingleItem.Children.Add(firstname);
            SingleItem.Children.Add(lastname);
            SingleItem.Children.Add(experience);
            buttonsStack.Children.Add(btnUpdate);
            buttonsStack.Children.Add(btnDelete);
            SingleItem.Children.Add(buttonsStack);

            return (btnUpdate, btnDelete, firstname, lastname, experience, pilot);
        }

        private void itemsList_ItemClick(object sender, ItemClickEventArgs e)
        {
            var turple = RenderDetail(e);
            var btnUpdate = turple.Item1;
            var btnDelete = turple.Item2;
            var firstname = turple.Item3;
            var lastname = turple.Item4;
            var experience = turple.Item5;
            var pilot = turple.Item6;

            btnUpdate.Click += async (object sen, RoutedEventArgs evArgs) =>
            {
                var pilotCreating = new PilotDTO()
                { Id = pilot.Id, FirstName = firstname.Text, LastName = lastname.Text, Experience = int.Parse(experience.Text) };

                int index = pilotsList.ToList().FindIndex(t => t.Id == pilot.Id);
                pilotsList.Insert(index, pilotCreating);

                try
                {
                    await service.UpdateAsync(pilotCreating);
                }
                catch (Exception) { }

                UpdateList();
                SingleItem.Children.Clear();
            };

            btnDelete.Click += async (object sen, RoutedEventArgs evArgs) =>
            {
                try
                {
                    await service.DeleteAsync(pilot);
                }
                catch (Exception) { }
                pilotsList.Remove(pilot);
                UpdateList();
                SingleItem.Children.Clear();
            };
        }
    }
}
