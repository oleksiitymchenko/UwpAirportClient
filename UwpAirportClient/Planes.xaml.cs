using System;
using System.Collections.ObjectModel;
using System.Linq;
using UwpAirportClient.Models;
using UwpAirportClient.Services;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UwpAirportClient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Planes : Page
    {
            public ObservableCollection<PlaneDTO> planesList = new ObservableCollection<PlaneDTO>();

            public double height => Window.Current.Bounds.Height-50;
            public Thickness marginCreate => new Thickness(Window.Current.Bounds.Width - 210, 10, 10, 10);

            private GenericService<PlaneDTO> service = new GenericService<PlaneDTO>(new System.Net.Http.HttpClient(), Url.Value + "Planes");
        private GenericService<PlaneTypeDTO> serviceType = new GenericService<PlaneTypeDTO>(new System.Net.Http.HttpClient(), Url.Value + "PlaneTypes");
        private ObservableCollection<PlaneTypeDTO> planetypesList = new ObservableCollection<PlaneTypeDTO>();
            public Planes()
            {
                UpdateList();
                this.InitializeComponent();
                this.Loaded += Planes_Load;
            }

            private void Planes_Load(object sender, RoutedEventArgs e)
            {
                UpdateList();
            GetPlaneTypes();
            }
            
            private async void GetPlaneTypes()
            {
                planetypesList.Clear();
                try
                {
                    (await serviceType.getAllAsync()).ForEach(o => planetypesList.Add(o));
                }
                catch (Exception) { };
            }

            private async void UpdateList()
            {
                planesList.Clear();
                try
                {
                    (await service.getAllAsync()).ForEach(o => planesList.Add(o));
                }
                catch (Exception) { };
            }

            private void Back_Click(object sender, RoutedEventArgs e)
            {
                if (Frame.CanGoBack)
                    Frame.GoBack();
            }

            private (Button, TextBox, TextBox, CalendarDatePicker, ComboBox) RenderCreate()
            {
                SingleItem.Children.Clear();

                var Name = new TextBox
                {
                    Header = "Name",
                    Width = 200,
                    PlaceholderText = "Name",
                    Margin = new Thickness(0, 100, 0, 0),
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                var timespan = new TextBox
                {
                    Header = "Timespan ",
                    PlaceholderText = "Timespan",
                    Width = 200,
                    Margin = new Thickness(0, 10, 0, 0),
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                var created = new CalendarDatePicker
                {
                    Header = "Created date",
                    PlaceholderText = "Created",
                    Width = 200,
                    Margin = new Thickness(0, 10, 0, 0),
                    HorizontalAlignment = HorizontalAlignment.Center
                };
            
            var types = new ComboBox
            {
                Header = "Plane types",
                PlaceholderText = "types",
                Width = 200,
                Margin = new Thickness(0, 10, 0, 0),
                ItemsSource=planetypesList
            };

                var btnCreate = new Button
                {
                    Content = "Add item",
                    Width = 200,
                    Margin = new Thickness(0, 20, 0, 0),
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                SingleItem.Children.Add(Name);
                SingleItem.Children.Add(timespan);
                SingleItem.Children.Add(created);
            SingleItem.Children.Add(types);
                SingleItem.Children.Add(btnCreate);
                return (btnCreate, Name, timespan, created, types);
            }

            private void Create_Click(object sender, RoutedEventArgs e)
            {
                var turple = RenderCreate();
                var btnCreate = turple.Item1;
                var name = turple.Item2;
                var timespan = turple.Item3;
                var createdat = turple.Item4;
            var types = turple.Item5;

                btnCreate.Click += async (object sen, RoutedEventArgs evArgs) =>
                {
                    var plane = new PlaneDTO()
                    { Name = name.Text, LifeTime = timespan.Text, Created = createdat.Date.Value.Date.ToString(), TypePlaneId=((PlaneTypeDTO)types.SelectedItem).Id };
                    try
                    {
                        await service.CreateAsync(plane);
                    }
                    catch (Exception) { }

                    planesList.Add(plane);
                    UpdateList();
                    SingleItem.Children.Clear();
                };
            }

            private (Button, Button, TextBox, TextBox, CalendarDatePicker, ComboBox,PlaneDTO) RenderDetail(ItemClickEventArgs e)
            {
                var plane = (PlaneDTO)e.ClickedItem;
                SingleItem.Children.Clear();

                var id = new TextBlock();
                id.Text = "Plane id:" + plane.Id;
                id.FontWeight = Windows.UI.Text.FontWeights.Bold;
                id.Margin = new Thickness(0, 100, 10, 10);
                id.Width = 200;

                var name = new TextBox
                {
                    Header = "Name",
                    Text = plane.Name,
                    Width = 200,
                    HorizontalAlignment = HorizontalAlignment.Center
                };


                var timespan = new TextBox
                {
                    Header = "Timespan",
                    Text = plane.LifeTime,
                    Width = 200,
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                var created = new CalendarDatePicker
                {
                    Header = "Created",
                    Date = DateTime.Parse(plane.Created),
                    Width = 200,
                    HorizontalAlignment = HorizontalAlignment.Center
                };

            var types = new ComboBox
            {
                Header = "Plane types",
                PlaceholderText = "types",
                Width = 200,
                Margin = new Thickness(0, 10, 0, 0),
                ItemsSource = planetypesList,
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
                SingleItem.Children.Add(name);
                SingleItem.Children.Add(timespan);
                SingleItem.Children.Add(created);
            SingleItem.Children.Add(types);
                buttonsStack.Children.Add(btnUpdate);
                buttonsStack.Children.Add(btnDelete);
                SingleItem.Children.Add(buttonsStack);

                return (btnUpdate, btnDelete, name, timespan, created, types, plane);
            }

            private void itemsList_ItemClick(object sender, ItemClickEventArgs e)
            {
                var turple = RenderDetail(e);
                var btnUpdate = turple.Item1;
                var btnDelete = turple.Item2;
                var Name = turple.Item3;
                var lifetime = turple.Item4;
                var created = turple.Item5;
                var type = turple.Item6;
            var plane = turple.Item7;

                btnUpdate.Click += async (object sen, RoutedEventArgs evArgs) =>
                {
                    var stewardessCreating = new PlaneDTO()
                    { Id = plane.Id, Name = Name.Text, LifeTime = lifetime.Text, Created = created.Date.Value.Date.ToString(),TypePlaneId=((PlaneTypeDTO)type.SelectedItem).Id };

                    int index = planesList.ToList().FindIndex(t => t.Id == plane.Id);
                    planesList.Insert(index, stewardessCreating);

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
                        await service.DeleteAsync(plane);
                    }
                    catch (Exception) { }
                    planesList.Remove(plane);
                    UpdateList();
                    SingleItem.Children.Clear();
                };
            }
        }
    }