using System.Text.Json.Serialization;

namespace AZ_DemoRedis.Models
{
    public class Person
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public int Age { get; set; }
    }
}
