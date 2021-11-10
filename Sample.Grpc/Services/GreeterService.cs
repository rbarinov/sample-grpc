using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace Sample.Grpc
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;

        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(
                new HelloReply
                {
                    Message = "Hello " + request.Name
                }
            );
        }

        public override async Task<HelloReply> SayHelloCs(
            IAsyncStreamReader<HelloRequest> requestStream,
            ServerCallContext context
        )
        {
            var ss = "";

            await foreach (var req in requestStream.ReadAllAsync())
            {
                ss += Environment.NewLine;
                ss += req.Name;
            }

            return new HelloReply
            {
                Message = $"hi there : {ss}"
            };
        }

        public override async Task SayHelloSs(
            HelloRequest request,
            IServerStreamWriter<HelloReply> responseStream,
            ServerCallContext context
        )
        {
            foreach (var i in Enumerable.Range(1, 10))
            {
                await responseStream.WriteAsync(
                    new HelloReply
                    {
                        Message = $"hello {request.Name} {i}"
                    }
                );

                await Task.Delay(500);
            }
        }

        public override async Task SayHelloBs(
            IAsyncStreamReader<HelloRequest> requestStream,
            IServerStreamWriter<HelloReply> responseStream,
            ServerCallContext context
        )
        {
            var ss = "";

            await foreach (var req in requestStream.ReadAllAsync())
            {
                ss += Environment.NewLine;
                ss += req.Name;

                var res = new HelloReply
                {
                    Message = $"hi there : {ss}"
                };

                await responseStream.WriteAsync(res);
            }
        }
    }
}
