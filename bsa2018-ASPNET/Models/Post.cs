using System;
using System.Collections.Generic;

namespace bsa2018_ASPNET.Models
{
    public class Post
    {
        public int Id { get; set; }
        public DateTime CreateAt { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int UserId { get; set; }
        public int Likes { get; set; }
        public List<Comment> Comments { get; set; }

        public override string ToString()
        {
            return $"{Title}\n{Body}\nWas created - {CreateAt.ToString("d")}\nLikes-{Likes}";
        }
    }
}
