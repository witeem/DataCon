using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DataConCore.Exceptions;

[Serializable]
public class ParamsException : Exception, IParamsException
{
    public ParamsException(string message)
        : base(message)
    {
        HResult = (int)HttpStatusCode.BadRequest;
    }

    public ParamsException(HttpStatusCode statusCode, string message)
    : base(message)
    {
        HResult = (int)statusCode;
    }
}
