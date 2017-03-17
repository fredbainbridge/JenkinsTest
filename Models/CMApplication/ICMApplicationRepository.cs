using System.Collections.Generic;

namespace CMWebAPI.Models
{
    public interface ICMApplicationRepository
    {
        IList<CMApplication> GetAll();
        IList<CMApplication> GetByName(string id);
        IList <CMApplication> GetAppOnDP(string id, string appName);
    }
}
