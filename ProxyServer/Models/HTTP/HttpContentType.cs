using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxyServer.Models.HTTP
{
    public enum HttpContentType
    {
        text,
        multipart,
        message,
        image,
        audio,
        video,
        application,
        unknown
    }
}
