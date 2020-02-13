﻿using System;
using MassTransit;
using MassTransit.Azure.ServiceBus.Core;
using Microsoft.Azure.ServiceBus.Primitives;
using ProductCatalog.Api.Infrastructure.Settings;

namespace ProductCatalog.Api.Infrastructure.BusConfigurator
{
    public static class BusConfigurator
    {
        public static IBusControl ConfigureBus(ServiceBusSettings settings, Action<IServiceBusBusFactoryConfigurator, IServiceBusHost> registrationAction = null)
        {

            return Bus.Factory.CreateUsingAzureServiceBus(sbc =>
            {
                var host = sbc.Host(settings.ServiceBusConnectionString, h =>
                {
                    h.TokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider(settings.ServiceBusKeyName,
                        settings.ServiceBusSharedAccessKey,
                        TimeSpan.FromDays(1),
                        TokenScope.Namespace);
                });

                sbc.UseServiceBusMessageScheduler();
                registrationAction?.Invoke(sbc, host);
            });
        }
    }
}
