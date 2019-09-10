using System;

namespace Twits2.Models
{
    public class Twit
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
    }
}