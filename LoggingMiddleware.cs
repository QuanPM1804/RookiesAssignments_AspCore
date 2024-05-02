using System.Text;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DotNetCore_Assignment
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _logFilePath;
        public LoggingMiddleware(RequestDelegate next, string logFilePath)
        {
            _next = next;
            _logFilePath = logFilePath;
            if (!File.Exists(_logFilePath))
            {
                File.Create(_logFilePath).Close();
            }
        }
        public async Task Invoke(HttpContext context)
        {
            string logMessage = $"{DateTime.Now}: " +
                                $"Schema: {context.Request.Scheme}, " +
                                $"Host: {context.Request.Host}, " +
                                $"Path: {context.Request.Path}, " +
                                $"QueryString: {context.Request.QueryString}";
            if (context.Request.ContentLength > 0)
            {
                context.Request.EnableBuffering();
                var requestBodyStream = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true);
                var requestBody = await requestBodyStream.ReadToEndAsync();
                logMessage += $", Request Body: {requestBody}";
                context.Request.Body.Position = 0;
            }
            await File.AppendAllTextAsync(_logFilePath, logMessage + Environment.NewLine);
            await _next(context);
        }
    }

}
