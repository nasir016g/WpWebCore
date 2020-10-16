using System.Collections.Generic;

namespace Wp.Service.Navigation
{
    public interface INavigationService
    {
        IList<NavigationModel> SubLevel(int parentId);
        IList<NavigationModel> TopLevel();
    }
}