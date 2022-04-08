using System;

namespace Pictures.data
{
    public class Image
    {
        public int Id { get; set; }
        public int Likes { get; set; }
        public DateTime Date { get; set; }
        public string Title { get; set; }
        public string ImageLocation { get; set; }
    }
}
