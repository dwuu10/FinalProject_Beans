using System.ComponentModel.DataAnnotations;

namespace Final_Project.Models
{
    public class City
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int BlueBeanPrice { get; set; }
        public int RedBeanPrice { get; set; }
        public int YellowBeanPrice { get; set; }
        public int GreenBeanPrice { get; set; }

        /*
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
        public string? Genre { get; set; }
        public decimal Price { get; set; }
        */
    }
}