using System;
using System.IO;
using System.Net;

namespace DailyHaiku.WP8.Twitter
{
    public class HaikuHttpClient
    {
        private HttpWebRequest httpRequest;

        public void GetResponse(string request, AsyncCallback methodToCall)
        {
            Uri requestUri = new Uri(request);
            httpRequest = (HttpWebRequest)WebRequest.Create(requestUri);

            const string bearerToken = "Bearer CreateYourOwn;)";
            httpRequest.Headers["Authorization"] = bearerToken;

            httpRequest.BeginGetResponse(req =>
            {
                var httpWebRequest = (HttpWebRequest)req.AsyncState;
                try
                {
                    using (var webResponse = httpWebRequest.EndGetResponse(req))
                    {
                        using (var reader = new StreamReader(webResponse.GetResponseStream()))
                        {
                            string response = reader.ReadToEnd();
                            methodToCall(new HaikuHttpClientResult { Response = response });
                        }
                    }
                }
                catch (Exception e)
                {
                    methodToCall(new HaikuHttpClientResult() { Error = e.Message });
                }

            }, httpRequest);
        }

    }
}
