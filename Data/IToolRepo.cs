using CommanderDA.Entities;
using System.Collections.Generic;

namespace CommanderREST.Data
{
    public interface IToolRepo
    {
        bool SaveChanges();

        IEnumerable<Tool> GetAllTools();
        Tool GetToolById(int id);
        void CreateTool(Tool cmd);
        void UpdateTool(Tool cmd);
        void DeleteTool(Tool cmd);
    }
}
