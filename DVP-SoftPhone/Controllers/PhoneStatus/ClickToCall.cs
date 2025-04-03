using DuoSoftware.DuoSoftPhone.Controllers.Common;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json; // Use System.Text.Json for modern JSON handling
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Controllers.PhoneStatus
{

    internal class ClickToCall
    {

        private HttpListener _listener;
        private readonly int _port;
        private Phone phone;
        private const string ExpectedToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwiZnJvbSI6IkxPTEMteGdlbiIsInRvIjoiZmFjZXRvbmUiLCJhY3Rpb24iOiJtYWtlQ2FsbCIsImlhdCI6MTUxNjIzOTAyMn0.tRu2YLDhOSDErs5QOfPXIQdPTptww-unV_gVHRdQtLo";


        public ClickToCall(int port)
        {

            Console.WriteLine("Application started...============================================================================================================");
            _port = port;
            _listener = new HttpListener();

        }
        public void Start(Phone phone)
        {
            _listener.Prefixes.Add($"http://*:{_port}/makeCall/"); // Listen on the /subscriber context.
            _listener.Start();
            Console.WriteLine($"Server listening on port {_port}/makeCall/");
            this.phone = phone;
            Task.Run(ListenForRequests);
        }

        public void Stop()
        {
            _listener.Stop();
            _listener.Close();
            Console.WriteLine("Server stopped.");
        }

        private async Task ListenForRequests()
        {
            while (_listener.IsListening)
            {
                try
                {
                    HttpListenerContext context = await _listener.GetContextAsync();
                    await ProcessRequest(context);
                }
                catch (HttpListenerException ex) when (ex.ErrorCode == 995)
                {
                    Console.WriteLine("Listener aborted, shutting down");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing request: {ex}");
                }
            }
        }

        private async Task ProcessRequest(HttpListenerContext context)
        {
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;

            string authorizationHeader = request.Headers["Authorization"];
            if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
            {
                response.StatusCode = 401; // Unauthorized.
                await SendJsonError(response, "Authorization header is missing or invalid.");
                return;
            }

            string token = authorizationHeader.Substring("Bearer ".Length).Trim();
            if (token != ExpectedToken)
            {
                response.StatusCode = 401; // Unauthorized.
                await SendJsonError(response, "Invalid token.");
                return;
            }

            if (request.HttpMethod == "POST")
            {
                Console.WriteLine("POST---");

                try
                {
                    using (var reader = new StreamReader(request.InputStream, request.ContentEncoding))
                    {
                        string json = await reader.ReadToEndAsync();
                        // Deserialize the JSON.
                        try
                        {
                            var data = JsonSerializer.Deserialize<CallTo>(json); // Replace MyData with your class.
                            if (data != null)
                            {
                                // Process the data.
                                Console.WriteLine($"Received data: CallTo-Number={data.number}");

                                var responseObject = new { Message = "Dialing Call To : " + data.number, ReceivedData = data };

                                if (this.phone.OprationMode != OperationMode.Outbound)
                                {
                                    response.StatusCode = 400; // Bad request.
                                    await SendJsonError(response, "Make Sure that the agent is on Outbound Mode!");
                                }
                                else
                                {
                                    if (data.number.Length == 10)
                                    {

                                        this.phone.MakeCall(data.number);

                                        string responseJson = JsonSerializer.Serialize(responseObject);

                                        byte[] buffer = Encoding.UTF8.GetBytes(responseJson);

                                        response.ContentLength64 = buffer.Length;
                                        response.ContentType = "application/json"; // Set the Content-Type to JSON.
                                        await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
                                    }
                                    else
                                    {
                                        response.StatusCode = 400; // Bad request.
                                        await SendJsonError(response, "Invalid Mobile Number!");
                                    }

                                }

                            }
                            else
                            {
                                response.StatusCode = 400; // Bad request.
                                await SendJsonError(response, "Bad Request!");
                            }

                        }
                        catch (JsonException ex)
                        {
                            Console.WriteLine($"JSON deserialization error: {ex}");
                            response.StatusCode = 400; // Bad request.
                            await SendJsonError(response, "JSON deserialization error");
                        }

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading request body: {ex}");
                    response.StatusCode = 500; // Internal server error.
                    await SendJsonError(response, "Error reading request body");
                }

            }
            else
            {

                var responseObject = new { Message = "Not Supported!."};

                string responseJson = JsonSerializer.Serialize(responseObject);

                byte[] buffer = Encoding.UTF8.GetBytes(responseJson);

                response.ContentLength64 = buffer.Length;
                response.ContentType = "application/json"; // Set the Content-Type to JSON.
                await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);

            }

        }
        public class CallTo
        {
            public string number { get; set; }

        }
        private async Task SendJsonResponse(HttpListenerResponse response, string json)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(json);
            response.ContentLength64 = buffer.Length;
            response.ContentType = "application/json";
            await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
            response.OutputStream.Close();
        }
        private async Task SendJsonError(HttpListenerResponse response, string errorMessage)
        {
            var errorResponse = new { Error = errorMessage };
            string errorJson = JsonSerializer.Serialize(errorResponse);
            await SendJsonResponse(response, errorJson);
        }
    }
}
