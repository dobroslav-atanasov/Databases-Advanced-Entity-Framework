namespace Stations.Models
{
    using System.ComponentModel.DataAnnotations;

    public class TrainSeat
    {
        [Key]
        public int Id { get; set; }

        public int TrainId { get; set; }
        public Train Train { get; set; }

        public int SeatingClassId { get; set; }
        public SeatingClass SeatingClass { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }
    }
}