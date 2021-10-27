using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CommanderREST.Dtos
{
    public class ToolCreateDto
    {
        [Required]
        [MaxLength(250)]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}