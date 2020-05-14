using Microsoft.EntityFrameworkCore;
using SLParkApi.Data;
using SLParkApi.Models;
using SLParkApi.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SLParkApi.Repository
{
    public class TrailRepository : ITrailRepository
    {
        private readonly ApplicationDbContext _context;

        public TrailRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool CreateTrail(Trail Trail)
        {
            _context.Add(Trail);
            return Save();
        }

        public bool DeleteTrail(Trail Trail)
        {
            _context.Remove(Trail);
            return Save();
        }

        public ICollection<Trail> GetTrails()
        {
            return _context.Trails.OrderBy(a => a.Name).ToList();
        }

        public Trail GetTrail(int TrailId)
        {
            return _context.Trails.FirstOrDefault(p => p.Id == TrailId);
        }

        public bool TrailExists(string name)
        {
            bool value = _context.Trails.Any(a => a.Name.ToLower().Trim() == name.ToLower().Trim());
            return value;
        }

        public bool TrailExists(int id)
        {
            return _context.Trails.Any(a => a.Id == id);
        }

        public bool Save()
        {
            return _context.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateTrail(Trail Trail)
        {
            _context.Trails.Update(Trail);
            return Save();
        }

        public ICollection<Trail> GetTrailsInTrail(int parkId)
        {
            return _context.Trails.Include(c => c.NationalPark).Where(n => n.NationalParkId == parkId).ToList();
        }

      
    }
}
