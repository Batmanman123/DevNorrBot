using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevNorrBot
{
    public class DependencyInjection
    {

        private ServiceProvider _serviceProvider;

        public void AddDependencies()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddUserSecrets<Program>();
                })
                .Build();

            var configuration = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json");

            _serviceProvider = new ServiceCollection()
                .AddSingleton<IConfiguration>(configuration.Build())
                .BuildServiceProvider();
            
            var configurations = host.Services.GetRequiredService<IConfiguration>();
            Constants.DiscordToken = configurations.GetSection("DiscordToken").Value;
            Constants.Gpt3Token = configurations.GetSection("Apikey").Value;
        }

        public T GetService<T>()
        {
            return _serviceProvider.GetService<T>();
        }
    }
}

