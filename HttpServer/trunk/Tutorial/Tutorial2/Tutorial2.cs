using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using HttpServer;
using HttpListener=HttpServer.HttpListener;

namespace Tutorial.Tutorial2
{
    class Tutorial2 : Tutorial
    {
        private HttpListener _listener;
        private X509Certificate2 _cert;

        #region Tutorial Members

        public void StartTutorial()
        {
            Console.WriteLine("Welcome to tutorial number 2, which will demonstrate how to setup HttpListener for secure requests.");
            Console.WriteLine();
            Console.WriteLine("You will need to create a certificate yourself. A good guide for OpenSSL can be found here:");
            Console.WriteLine("http://www.towersoft.com/sdk/doku.php?id=ice:setting_up_an_ice_server_to_use_ssl");
            Console.WriteLine();
            Console.WriteLine("Browse to https://localhost/hello when you have installed your certificate.");


            _cert = new X509Certificate2("../../certInProjectFolder.p12", "yourCertPassword");
            _listener = new HttpListener(IPAddress.Any, 443, _cert);
            _listener.RequestHandler += OnSecureRequest;
            _listener.Start(5);
        }

        private void OnSecureRequest(HttpClientContext client, HttpRequest request)
        {
            // Here we create a response object, instead of using the client directly.
            // we can use methods like Redirect etc with it,
            // and we dont need to keep track of any headers etc.
            HttpResponse response = new HttpResponse(client, request);

            byte[] body = Encoding.UTF8.GetBytes("Hello secure you!");
            response.Body.Write(body, 0, body.Length);
            response.Send();
        }

        public void EndTutorial()
        {
            _listener.Stop();
        }

        #endregion
    }
}