using System;
using System.Collections.Generic;
using System.Fabric;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ServiceFabric.Services.Communication.AspNetCore;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.ServiceFabric.Data;
using Microsoft.Extensions.Configuration;

namespace jjapisforders
{
    /// <summary>
    /// The FabricRuntime creates an instance of this class for each service type instance. 
    /// </summary>
    internal sealed class jjapisforders : StatelessService
    {
        public jjapisforders(StatelessServiceContext context)
            : base(context)
        { }

        /// <summary>
        /// Optional override to create listeners (like tcp, http) for this service instance.
        /// </summary>
        /// <returns>The collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new ServiceInstanceListener[]
            {                
                new ServiceInstanceListener(serviceContext =>
                    new HttpSysCommunicationListener(serviceContext, "ServiceEndpoint", (url, listener) =>
                    {
                        url += "/jjapisf2";
                        ServiceEventSource.Current.ServiceMessage(serviceContext, $"Starting HttpSys on {url}");

                        return new WebHostBuilder()
                                    //.UseKestrel()
                                    .UseHttpSys()
                                    .ConfigureServices(
                                        services => services
                                            .AddSingleton<StatelessServiceContext>(serviceContext))
                                    .UseContentRoot(Directory.GetCurrentDirectory())
                                    .UseStartup<Startup>()
                                    .UseServiceFabricIntegration(listener, ServiceFabricIntegrationOptions.None)
                                    .UseUrls(url)
                                    .ConfigureAppConfiguration((webHostBuilderContext, configurationbuilder) =>
                                    {
                                        var environment = webHostBuilderContext.HostingEnvironment;
                                        configurationbuilder
                                                .AddJsonFile("appsettings.json", optional: true)
                                                //Adding specific Environment file
                                                 .AddJsonFile($"{environment.EnvironmentName}_appsettings.json", optional: true);
                                        configurationbuilder.AddEnvironmentVariables();
                                    })

                                    .Build();
                    }))
            };
        }
    }
}
