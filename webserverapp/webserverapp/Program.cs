using System;
using System.IO;
using System.Net;

namespace webserverapp
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpListener listener = new HttpListener();

            listener.Prefixes.Add("http://localhost:8080/");

            SimpleListenerExample(listener);
        }

        public static void SimpleListenerExample(HttpListener listen)
        {

            if (!HttpListener.IsSupported)
            {
                Console.WriteLine("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
                return;
            }
            // URI prefixes are required,
            // for example "http://contoso.com:8080/index/".
            if (listen == null || listen.Prefixes.Count == 0)
                throw new ArgumentException("prefixes");

            // Create a listener.
            HttpListener listener = new HttpListener();
            // Add the prefixes.
            foreach (string s in listen.Prefixes)
            {
                listener.Prefixes.Add(s);
            }
            listener.Start();
            Console.WriteLine("Listening...");


            Cookie counterCookie = new Cookie();
            int cookies = 0;
            while (true)
            {
                string extra = "";
                // Note: The GetContext method blocks while waiting for a request.
                HttpListenerContext context = listener.GetContext();
                HttpListenerRequest request = context.Request;
                // Obtain a response object.
                HttpListenerResponse response = context.Response;

                //string[] files = System.IO.Directory.GetFiles(@"Content\index", request.RawUrl);
                // Construct a response.

                if (!context.Request.RawUrl.Contains("favicon.ico"))
                {

                    if (context.Request.RawUrl.Contains(@"/Subfolder/"))
                    {
                        extra = @"\index.html";
                        response.AppendCookie(counterCookie);
                        cookies += response.Cookies.Count;
                    }

                    if (context.Request.RawUrl.Contains("counter"))
                    {
                        string responseString = $"<HTML><BODY> Amount of requests: {cookies} </BODY></HTML>";

                        byte[] buffer1 = System.Text.Encoding.UTF8.GetBytes(responseString);
                        response.ContentLength64 = buffer1.Length;
                        System.IO.Stream output = response.OutputStream;
                        output.Write(buffer1, 0, buffer1.Length);

                        response.AppendCookie(counterCookie);
                        cookies += response.Cookies.Count;

                        output.Close();
                        Console.WriteLine("Request sent" + request.RawUrl);

                        Console.WriteLine("amount of requests: " + cookies);
                    }
                    else
                    {

                        byte[] buffer = File.ReadAllBytes(@"C:\Dataåtkomster\webserver2-grupp5\Content" + context.Request.RawUrl.Replace("%20", " ") + extra);





                        response.AppendCookie(counterCookie);
                        cookies += response.Cookies.Count;

                        response.ContentLength64 = buffer.Length;
                        System.IO.Stream output = response.OutputStream;
                        output.Write(buffer, 0, buffer.Length);
                        output.Close();
                        Console.WriteLine("Request sent" + request.RawUrl);


                        Console.WriteLine("amount of requests: " + cookies);
                    }
                }
                //byte[] buffer = File.ReadAllBytes(@"C:\Dataåtkomster\webserver2-grupp5" + context.Request.RawUrl.Replace("%20", " "));
                // Get a response stream and write the response to it.

                // You must close the output stream. 
            }
            listener.Stop();
        }
    }
}
