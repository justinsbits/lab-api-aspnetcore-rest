using System.ComponentModel.DataAnnotations;

namespace CommanderREST.Dtos
{
    public class CommandReadDto : IIdDto
    {
        [Required]
        public int Id { get; set; }

        public string HowTo {get; set;}

        public string CommandLine {get; set;}

    }
}