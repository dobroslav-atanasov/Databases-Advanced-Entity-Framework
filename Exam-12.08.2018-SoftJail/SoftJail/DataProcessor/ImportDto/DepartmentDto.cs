namespace SoftJail.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;

    public class DepartmentDto
    {
        [StringLength(25, MinimumLength = 3)]
        public string Name { get; set; }

        public CellDto[] Cells { get; set; }
    }
}