namespace CommanderREST.Dtos
{
    public class ToolReadDto : IReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

    }
}