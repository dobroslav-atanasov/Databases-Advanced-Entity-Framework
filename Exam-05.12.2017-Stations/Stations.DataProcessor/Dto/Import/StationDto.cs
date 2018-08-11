namespace Stations.DataProcessor.Dto.Import
{
    using System.ComponentModel.DataAnnotations;

    public class StationDto
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Town { get; set; }
    }
}