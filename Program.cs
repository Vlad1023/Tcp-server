using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
namespace Tcp_server
{
    class Program
    {
        const string VERSION = "HTTP/1.0";
        const string SERVERNAME = "myserv/1.1";
        static void Main(string[] args)
        {
            Console.WriteLine("simple server");
            try
            {
                var port = 19539;
                var localAddr = IPAddress.Parse("127.0.0.1");
                var server = new TcpListener(localAddr, port);
                server.Start();
                while (true)
                {
                    Console.Write("waiting for connection");
                    var client = server.AcceptTcpClient();
                    Console.WriteLine("Accept connection from {0}",client.Client.RemoteEndPoint);
                    NetworkStream stream = client.GetStream();
                    string responceHtmlst = generateResponce();
                    var responceHtml = System.Text.Encoding.ASCII.GetBytes(responceHtmlst);
                    stream.Write(responceHtml, 0, responceHtml.Length);
                    Console.WriteLine("Sent: {0}", responceHtmlst);
                    client.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        static string generateResponce()
        {
            string html = File.ReadAllText(String.Format("{0}/web/index.html", Environment.CurrentDirectory));
            string toReturn = getHeaders(html.Length) + html;
            return toReturn;
        }
        static string getHeaders(int dataLength)
        {
           string toReturn =  (String.Format("{0} 200\r\nServer: {1}\r\nContent-Language: en\r\nContent-Type: text/html; charset=utf-8\r\nAccept-Ranges: bytes\r\nContent-Length: {2}\r\nConnection: close\r\n\n",
           VERSION, SERVERNAME, dataLength));
           return toReturn;
        }
    }
}
