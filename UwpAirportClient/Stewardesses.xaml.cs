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
    public sealed partial class Stewardesses : Page
    {
        public ObservableCollection<StewardessDTO> stewardessesList = new ObservableCollection<StewardessDTO>();

        public double height => Window.Current.Bounds.Height;
        public Thickness marginCreate => new Thickness(Window.Current.Bounds.Width - 210, 10, 10, 10);

        private GenericService<StewardessDTO> service = new GenericService<StewardessDTO>(new System.Net.Http.HttpClient(), Url.Value + "Stewardesses");

        public Stewardesses()
        {
            UpdateList();
            this.InitializeComponent();
            this.Loaded += Stewardesses_Load;
        }

        private void Stewardesses_Load(object sender, RoutedEventArgs e)
        {
            UpdateList();
        }

        private async void UpdateList()
        {
            stewardessesList.Clear();
            try
            {
                (await service.getAllAsync()).ForEach(o => stewardessesList.Add(o));
            }
            catch (Exception) { };
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
                Frame.GoBack();
        }

        private (Button, TextBox, TextBox, CalendarDatePicker) RenderCreate()
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

            var birthday = new CalendarDatePicker
            {
                Header = "Birthday date",
                PlaceholderText = "Birthday",
                Width = 200,
                Margin = new Thickness(0, 10, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center
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
            SingleItem.Children.Add(birthday);
            SingleItem.Children.Add(btnCreate);
            return (btnCreate, firstname, lastname, birthday);
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            var turple = RenderCreate();
            var btnCreate = turple.Item1;
            var firstname = turple.Item2;
            var lastname = turple.Item3;
            var birthday = turple.Item4;

            btnCreate.Click += async (object sen, RoutedEventArgs evArgs) =>
            {
                var pilot = new StewardessDTO()
                { FirstName = firstname.Text, LastName = lastname.Text, DateOfBirth = birthday.Date.Value.Date };
                try
                {
                    await service.CreateAsync(pilot);
                }
                catch (Exception) { }

                stewardessesList.Add(pilot);
                UpdateList();
                SingleItem.Children.Clear();
            };
        }

        private (Button, Button, TextBox, TextBox, CalendarDatePicker, StewardessDTO) RenderDetail(ItemClickEventArgs e)
        {
            var stewardess = (StewardessDTO)e.ClickedItem;
            SingleItem.Children.Clear();

            var id = new TextBlock();
            id.Text = "Pilot id:" + stewardess.Id;
            id.FontWeight = Windows.UI.Text.FontWeights.Bold;
            id.Margin = new Thickness(0, 100, 10, 10);
            id.Width = 200;

            var firstname = new TextBox
            {
                Header = "FirstName",
                Text = stewardess.FirstName,
                Width = 200,
                HorizontalAlignment = HorizontalAlignment.Center
            };


            var lastname = new TextBox
            {
                Header = "Lastname",
                Text = stewardess.LastName,
                Width = 200,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            var birthday = new CalendarDatePicker
            {
                Header = "Experience",
                Date = stewardess.DateOfBirth,
                Width = 200,
                HorizontalAlignment = HorizontalAlignment.Center
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
            SingleItem.Children.Add(birthday);
            buttonsStack.Children.Add(btnUpdate);
            buttonsStack.Children.Add(btnDelete);
            SingleItem.Children.Add(buttonsStack);

            return (btnUpdate, btnDelete, firstname, lastname, birthday, stewardess);
        }

        private void itemsList_ItemClick(object sender, ItemClickEventArgs e)
        {
            var turple = RenderDetail(e);
            var btnUpdate = turple.Item1;
            var btnDelete = turple.Item2;
            var firstname = turple.Item3;
            var lastname = turple.Item4;
            var birthday = turple.Item5;
            var stewardess = turple.Item6;

            btnUpdate.Click += async (object sen, RoutedEventArgs evArgs) =>
            {
                var stewardessCreating = new StewardessDTO()
                { Id = stewardess.Id, FirstName = firstname.Text, LastName = lastname.Text, DateOfBirth = birthday.Date.Value.Date};

                int index = stewardessesList.ToList().FindIndex(t => t.Id == stewardess.Id);
                stewardessesList.Insert(index, stewardessCreating);

                try
                {
                    await service.UpdateAsync(stewardessCreating);
                }
                catch (Exception) { }

                UpdateList();
                SingleItem.Children.Clear();
            };

            btnDelete.Click += async (object sen, RoutedEventArgs evArgs) =>
            {
                try
                {
                    await service.DeleteAsync(stewardess);
                }
                catch (Exception) { }
                stewardessesList.Remove(stewardess);
                UpdateList();
                SingleItem.Children.Clear();
            };
        }
    }
}
