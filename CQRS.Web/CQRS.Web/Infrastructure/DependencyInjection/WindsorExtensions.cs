using System;
using System.Reflection;
using Castle.MicroKernel;

namespace CQRS.Web.Infrastructure.DependencyInjection
{
    public static class WindsorExtensions
    {
        public static void InjectProperties(this IKernel kernel, object target)
        {
            var type = target.GetType();
            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!property.CanWrite || !kernel.HasComponent(property.PropertyType)) continue;

                var value = kernel.Resolve(property.PropertyType);
                try
                {
                    property.SetValue(target, value, null);
                }
                catch (Exception ex)
                {
                    var message = string.Format("Error setting property {0} on type {1}, See inner exception for more information.", property.Name, type.FullName);
                    throw new ArgumentException(message, ex);
                }
            }
        }
    }
}