using CommanderDA.Entities;
using System.Collections.Generic;

namespace CommanderREST.Data
{
    public interface ICommandRepo
    {
        bool SaveChanges();

        IEnumerable<Command> GetAllCommands();
        Command GetCommandById(int id);
        void CreateCommand(Command cmd);
        void UpdateCommand(Command cmd);
        void DeleteCommand(Command cmd);
    }
}
