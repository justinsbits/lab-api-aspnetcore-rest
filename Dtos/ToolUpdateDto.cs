using System.ComponentModel.DataAnnotations;

namespace CommanderREST.Dtos
{
    public class ToolUpdateDto
    {
        [Required]
        [MaxLength(250)]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}