using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VismaOvidijusRapalis.Models;

namespace VismaOvidijusRapalis.Repositories
{
    public interface IMeetingsRepository
    {
        void Save(IDictionary<Guid, Meeting> data);
        IDictionary<Guid, Meeting> ReadAll();
    }
}
