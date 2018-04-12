using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using HttpServer.BodyDecoders;
using HttpServer.Headers;
using Xunit;

namespace HttpServerTest.Decoders
{

    public class MultipartTest
    {
        [Fact]
        public void FormTest()
        {
            FileStream stream = new FileStream("C:\\temp\\bodymime.mime", FileMode.Open);
            MultiPartDecoder  decoder = new MultiPartDecoder();
            var header = new ContentTypeHeader("multipart/form-data");
            header.Parameters.Add("boundary", "----WebKitFormBoundaryQsuJaNmu3FVqrYwp");
            var data = decoder.Decode(stream, header, Encoding.Default);
            if (data.Files.Count > 0)
            {
                
            }

        }
    }
}
