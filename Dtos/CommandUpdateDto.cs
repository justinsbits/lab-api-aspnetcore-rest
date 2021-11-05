using System.ComponentModel.DataAnnotations;

namespace CommanderREST.Dtos
{
    public class CommandUpdateDto : IIdDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(250)]
        public string HowTo {get; set;}
        
        [Required]
        public string CommandLine {get; set;}
    }
}