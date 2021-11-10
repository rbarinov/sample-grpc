using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;

namespace Sample.Grpc
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // var channel = GrpcChannel.ForAddress(
            //     "https://sample-grpc.test2.gigpartners.io/",
            //     new GrpcChannelOptions
            //     {
            //     }
            // );
            //
            // var client = new Greeter.GreeterClient(channel);
            //
            // while (true)
            // {
            //     var sw = Stopwatch.StartNew();
            //     var res = await client.SayHelloAsync(new HelloRequest {Name = "Roman Barinov"});
            //     sw.Stop();
            //     Console.WriteLine($"{sw.ElapsedMilliseconds} {res.Message}");
            //     await Task.Delay(500);
            // }

            await CreateHostBuilder(args)
                .Build()
                .RunAsync();
        }

        // Additional configuration is required to successfully run gRPC on macOS.
        // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(
                    webBuilder =>
                    {
#if !DEBUG
                        webBuilder.UseKestrel(e => { e.ListenAnyIP(80, c => c.Protocols = HttpProtocols.Http2); });
#endif
                        webBuilder.UseStartup<Startup>();
                    }
                );
    }
}
