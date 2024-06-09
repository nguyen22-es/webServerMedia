using Database.Contexts;
using Database.Migrations;
using Microsoft.Extensions.DependencyInjection;
using System;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var configuration = builder.Configuration;
        builder.Services.Startup(configuration);
        // Add services to the container.
   


   

        var app = builder.Build();

        // Configure the HTTP request pipeline.

        //InitializeDatabase(app.Services);
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
          
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();
        app.UseAuthentication();
  

        app.UseMvc();

        app.Run();
    }


}