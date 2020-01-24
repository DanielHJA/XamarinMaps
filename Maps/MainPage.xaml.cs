using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Maps
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {

        MainPageViewModel viewModel;

        public MainPage()
        {
            InitializeComponent();
            viewModel = new MainPageViewModel(map);
            BindingContext = viewModel;
            viewModel.setMapRegion(new Position(36.9628066, -122.0194722));


        }

        void MapModeButtonClicked(System.Object sender, System.EventArgs e) {
            viewModel.ChangeMapType();
        }

        void TheCarousel_CurrentItemChanged(CarouselView sender, Xamarin.Forms.CurrentItemChangedEventArgs e)
        {
            viewModel.CarouselItemDidChange(sender.Position);
        }

        async void ToolbarItem_Clicked(System.Object sender, System.EventArgs e)
        {
            string Message = "My Detail page";
            DetailsPage page = new DetailsPage(Message);
            await Navigation.PushAsync(page);
        }
    }
}
