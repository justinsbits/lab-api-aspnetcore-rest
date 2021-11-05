using System.ComponentModel.DataAnnotations;

namespace CommanderREST.Dtos
{
    public class ToolUpdateDto : IIdDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(250)]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}