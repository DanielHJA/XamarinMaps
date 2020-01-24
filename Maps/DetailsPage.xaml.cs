using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Maps
{
    public partial class DetailsPage : ContentPage
    {

        public string Message { get; set; }

        public DetailsPage(string Message)
        {
            InitializeComponent();
            this.Message = Message;
            System.Console.WriteLine(this.Message);
        }
    }
}
