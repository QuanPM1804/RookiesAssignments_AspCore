using DotNetCore_Assignment;

class Program
{
    static async Task Main(string[] args)
    {
        var host = new WebHostBuilder()
            .UseKestrel()
            .Configure(app =>
            {
                app.UseMiddleware<LoggingMiddleware>("log.txt");

                app.Run(async (context) =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            })
            .Build();

        await host.RunAsync();
    }
}