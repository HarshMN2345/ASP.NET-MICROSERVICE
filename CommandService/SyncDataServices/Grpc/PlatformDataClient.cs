using AutoMapper;
using CommandService.Models;
using Grpc.Net.Client;
using Grpc.Core;
using PlatformService;
using System;
using System.Collections.Generic;

namespace CommandService.SyncDataServices.Grpc
{
    public class PlatformDataClient : IPlatformDataClient
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public PlatformDataClient(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
        }

        public IEnumerable<Platform>? ReturnAllPlatforms()
        {
            var grpcPlatformUrl = _configuration["GrpcPlatform"];
            if (string.IsNullOrEmpty(grpcPlatformUrl))
            {
                Console.WriteLine("GRPC Platform URL is not configured or is empty.");
                return null;
            }

            Console.WriteLine($"--> Calling GRPC Service {grpcPlatformUrl}");

            try
            {
                var channel = GrpcChannel.ForAddress(grpcPlatformUrl, new GrpcChannelOptions
                {
                    Credentials = ChannelCredentials.Insecure
                });

                var client = new GrpcPlatform.GrpcPlatformClient(channel);
                var request = new GetAllRequest();
                var reply = client.GetAllPlatforms(request);

                return _mapper.Map<IEnumerable<Platform>>(reply.Platform);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not call GRPC Server: {ex.Message}");
                return null;
            }
        }
    }
};

