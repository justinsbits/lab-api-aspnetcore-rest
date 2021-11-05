using System.ComponentModel.DataAnnotations;

namespace CommanderREST.Dtos
{
    public class ToolReadDto : IIdDto
    {
        [Required]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

    }
}