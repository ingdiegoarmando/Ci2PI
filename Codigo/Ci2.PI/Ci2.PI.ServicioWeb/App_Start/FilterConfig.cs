using System.Web;
using System.Web.Mvc;

namespace Ci2.PI.ServicioWeb
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
