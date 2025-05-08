using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace LoanShark.Web.Extensions
{
    public static class ProxyServiceRegistration
    {
        public static void AddAllServiceProxies(this IServiceCollection services)
        {
            var proxyAssembly = typeof(LoanShark.API.Proxies.UserServiceProxy).Assembly;

            var proxyTypes = proxyAssembly
                    .GetTypes()
                    .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("ServiceProxy"));

            var method = typeof(HttpClientFactoryServiceCollectionExtensions)
                .GetMethods()
                .First(m => m.Name == "AddHttpClient" && m.IsGenericMethodDefinition && m.GetGenericArguments().Length == 2);

            foreach (var implementationType in proxyTypes)
            {
                var interfaceType = implementationType.GetInterfaces()
                    .FirstOrDefault(i => i.Name == $"I{implementationType.Name}");

                if (interfaceType != null)
                {
                    var genericMethod = method.MakeGenericMethod(interfaceType, implementationType);
                    genericMethod.Invoke(null, new object[] { services });
                }
            }
        }
    }
}
