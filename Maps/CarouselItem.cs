using System;
namespace Maps
{
    public class CarouselItem
    {

        public string Title { get; set; }
        public Uri Image { get; set; }

        public CarouselItem(string Title, string Image)
        {
            this.Title = Title;
            this.Image = new Uri(Image);
        }
    }
}
