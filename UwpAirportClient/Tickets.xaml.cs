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
    public sealed partial class Tickets : Page
    {
        public ObservableCollection<TicketDTO> ticketList = new ObservableCollection<TicketDTO>();
        public Tickets()
        {
            getTickets();
            this.InitializeComponent();
            this.Loaded += Tickets_Loaded;
        }

        private void Tickets_Loaded(object sender, RoutedEventArgs e)
        {
            getTickets();
           // ticketsList.ItemsSource = this.ticketList;
        }

        private async void getTickets()
        {
            var serv = new TicketService();
            (await serv.getAllAsync()).ForEach(o=>ticketList.Add(o));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(ticketList);
        }
    }
}
