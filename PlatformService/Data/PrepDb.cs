using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data;

public static class PrepDb
{
   public static void PrepPopulation(IApplicationBuilder app, bool isProd)
    {
        using (var serviceScope = app.ApplicationServices.CreateScope()){
#pragma warning disable CS8604 // Possible null reference argument.
            SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(),isProd);
#pragma warning restore CS8604 // Possible null reference argument.

        }
        
   }
   private static void SeedData(AppDbContext context,bool isProd){

    if(isProd){
        Console.WriteLine("We are about to apply Migrations");
        try{
          context.Database.Migrate();
        }catch(Exception ex){
            Console.WriteLine("Unable to aplly migartions");
        }
        
    }
    if(!context.Platforms.Any()){
        Console.WriteLine("--> Seeding Data...");
        context.Platforms.AddRange(
            new Platform(){Name="Dot Net",Publisher="Microsoft",Cost="Free"},
            new Platform(){Name="SQL Server Express",Publisher="Microsoft",Cost="Free"},
            new Platform(){Name="Kubernetes",Publisher="Cloud Native Computing Foundation",Cost="Free"}
        );
        context.SaveChanges();

    }else{
        Console.WriteLine("WE alREADY pOSSESS THE DATA :)");
    }
         
   }
}
