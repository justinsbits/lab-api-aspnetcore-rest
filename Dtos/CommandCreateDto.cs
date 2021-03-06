using System.ComponentModel.DataAnnotations;

namespace CommanderREST.Dtos
{
    public class CommandCreateDto
    {
        [Required]
        [MaxLength(250)]
        public string HowTo {get; set;}
        
        [Required]
        public string CommandLine {get; set;}

        [Required]
        public int ToolId { get; set; }
    }
}