using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UwpAirportClient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void TicketClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Tickets));
        }

        private void PlanetypesClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Planetypes));
        }

        private void StewardessesClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Stewardesses));
        }

        private void PilotsClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Pilots));
        }

        private void CrewsClick(object sender, RoutedEventArgs e)
        {
            Ticketbtn.Content = "Hello UWP!";
        }

        private void FlightsClick(object sender, RoutedEventArgs e)
        {
            Ticketbtn.Content = "Hello UWP!";
        }

        private void PlanesClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Planes));
        }

        private void DeparturesClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Departures));
        }
    }
}
