using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using HttpServer;
using HttpServer.Modules;

namespace HttpServerSample
{
    class CustomHttpModule : IModule
    {
        public event DoModuleProcess OnDoRequestArrvied;
        #region IModule 成员

        public ProcessingResult Process(RequestContext context) {

            //.do前为main或者插件名
            string requestTarget = Path.GetFileNameWithoutExtension(context.Request.Uri.AbsolutePath);
            requestTarget = requestTarget.ToLower();
            string reqType = Path.GetExtension(context.Request.Uri.AbsolutePath).TrimStart('.');
            //如果do请求，必须处理，否则就无返回。 
            if (string.IsNullOrEmpty(reqType) == false
                && reqType == "do") {
                if (OnDoRequestArrvied != null) {
                    return OnDoRequestArrvied(requestTarget, context);
                }
                return ProcessingResult.Abort;
            }
            else {
                return ProcessingResult.Continue;
            }

        }

        #endregion
    }
    public delegate ProcessingResult DoModuleProcess(string requstTarget, RequestContext context);
}
