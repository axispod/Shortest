using System;

namespace DataAccess.Models
{
    public class Link
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string Url { get; set; }
        public long Redirects { get; set; }
        public DateTime CreationDate { get; set; }
    }
}