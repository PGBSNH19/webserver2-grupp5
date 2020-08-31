using System;
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

            while (true)
            {              
                // Note: The GetContext method blocks while waiting for a request.
                HttpListenerContext context = listener.GetContext();
                HttpListenerRequest request = context.Request;
                // Obtain a response object.
                HttpListenerResponse response = context.Response;
               
                 string[] files = System.IO.Directory.GetFiles(@"\Users\erifr\Documents\.NET utvecklare\Molnkurs\webserver2-grupp5\Content", request.RawUrl);
                // Construct a response.
                string responseString = "<HTML><BODY> Hello world!</BODY></HTML>";
                
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                // Get a response stream and write the response to it.
                response.ContentLength64 = buffer.Length;
                System.IO.Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                // You must close the output stream. 

               
                Console.WriteLine("Request sent" + request.RawUrl);
                output.Close();                
            }
            listener.Stop();
        }
    }
}
