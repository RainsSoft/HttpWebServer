using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpServer.MVC2.Views
{
    public class ViewProvider
    {
        private List<string> _viewLocations = new List<string>
                                                  {
                                                      "{controllerName}/Views",
                                                      "/Views/{ControllerName}",
                                                      "/Views/Shared"
                                                  };
    }
}
