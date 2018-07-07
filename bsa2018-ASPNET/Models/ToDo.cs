using System;

namespace bsa2018_ASPNET.Models
{
    public class ToDo
    {
        public int Id { get; set; }
        public DateTime CreateAt { get; set; }
        public string Name { get; set; }
        public bool IsComplete { get; set; }
        public int UserId { get; set; }

        public override string ToString()
        {
            return $"{Name} - {IsComplete}\n{CreateAt.ToString("d")}";
        }
    }
}
