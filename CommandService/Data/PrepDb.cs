using CommandService.Models;
using CommandService.SyncDataServices.Grpc;

namespace CommandService.Data;
  public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var grpcClient = serviceScope.ServiceProvider.GetService<IPlatformDataClient>();

                var platforms = grpcClient.ReturnAllPlatforms();

                SeedData((ICommandRepo)serviceScope.ServiceProvider.GetService<ICommandRepo>(), platforms);
            }
        }

    private static void SeedData(ICommandRepo repo, IEnumerable<Platform> platforms)
        {
            Console.WriteLine("Seeding new platforms...");

            foreach (var plat in platforms)
            {
                if(!repo.ExternalPlatformExists(plat.ExternalId))
                {
                    repo.CreatePlatform(plat);
                }
                repo.SaveChanges();
            }
        }
    }