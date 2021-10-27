using CommanderDA;
using CommanderDA.Entities;
using CommanderREST.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CommanderREST.Data
{
    public class ToolRepo : IToolRepo
    {
        private readonly AppDbContext _context;

        public ToolRepo(AppDbContext context)
        {
            _context = context;
        }

        public void CreateTool(Tool cmd)
        {
            if(cmd == null)
            {
                throw new ArgumentNullException(nameof(cmd));
            }

            _context.Tools.Add(cmd);
        }

        public void DeleteTool(Tool cmd)
        {
            if(cmd == null)
            {
                throw new ArgumentNullException(nameof(cmd));
            }
            _context.Tools.Remove(cmd);
        }

        public IEnumerable<Tool> GetAllTools()
        {
            return _context.Tools.ToList();
        }

        public Tool GetToolById(int id)
        {
            return _context.Tools.FirstOrDefault(p => p.Id == id);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void UpdateTool(Tool cmd)
        {
            //This does nothing
        }
    }
}