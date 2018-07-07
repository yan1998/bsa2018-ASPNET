namespace bsa2018_ASPNET.Models
{
    public class Address
    {
        public int Id { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Zip { get; set; }
        public int UserId { get; set; }

        public override string ToString()
        {
            return $"{Country} - {City} - {Street} - {Zip}";
        }
    }
}
