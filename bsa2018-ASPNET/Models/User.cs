using System;
using System.Collections.Generic;

namespace bsa2018_ASPNET.Models
{
    public class User
    {
        public int Id { get; set; }
        public DateTime CreateAt { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Email { get; set; }
        public List<Post> Posts { get; set; }
        public List<ToDo> ToDos { get; set; }
        public List<Comment> Comments { get; set; }
        public Address Address { get; set; }

        public override string ToString()
        {
            return $"{Name} - {Email}";
        }
    }
}
