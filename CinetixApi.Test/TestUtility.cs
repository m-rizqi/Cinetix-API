using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Cinetix_Api.Test
{
    static class TestUtility
    {
        public static HttpStatusCode GetHttpStatusCode<T>(ActionResult<T> actionResult)
        {
            try
            {
                return (HttpStatusCode)actionResult
                    .GetType()
                    .GetProperty("StatusCode")
                    .GetValue(actionResult, null);
            }
            catch
            {
                return HttpStatusCode.InternalServerError;
            }
        }
    }
}
