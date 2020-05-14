using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SLParkApi.Models;

namespace SLParkApi.Repository.IRepository
{
    public interface ITrailRepository
    {
        ICollection<Trail> GetTrails();
        Trail GetTrail(int nationalParkId);

        bool TrailExists(string name);
        bool TrailExists(int id);
        bool CreateTrail(Trail nationalPark);

        bool UpdateTrail(Trail nationalPark);

        bool DeleteTrail(Trail nationalPark);

        bool Save();
    }
}
