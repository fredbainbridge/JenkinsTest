using System.Collections.Generic;

namespace CMWebAPI.Models
{
    public interface IApplicationFromDBViewRepository
    {
        IList<ApplicationFromDBView> GetAll();
    }
}
