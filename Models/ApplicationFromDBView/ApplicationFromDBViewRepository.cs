using System.Collections.Generic;
using System.Linq;

namespace CMWebAPI.Models
{
    public class ApplicationFromDBViewRepository : IApplicationFromDBViewRepository
    {
        private CMContext _context;

        public ApplicationFromDBViewRepository (CMContext dataContext)
        {
            _context = dataContext;
        }

        public IList<ApplicationFromDBView> GetAll()
        {
            return _context.ApplicationFromDBView.ToList();
        }
    }
}
