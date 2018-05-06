using System;

namespace Shortest.Models.ViewModels
{
    public class LinkViewModel
    {
        public DateTime CreationDate { get; set; }
        public string OriginalUrl { get; set; }
        public string ShortUrl { get; set; }
        public long RedirectsCount { get; set; }
    }
}