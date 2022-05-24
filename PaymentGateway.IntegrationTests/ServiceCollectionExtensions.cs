namespace PaymentGateway.IntegrationTests;

public static class ServiceCollectionExtensions
{
    public static void RemoveService(this IServiceCollection services, Type type)
    {
        var descriptor = services.SingleOrDefault(descriptor => descriptor.ServiceType == type);

        if (descriptor != null)
        {
            services.Remove(descriptor);
        }
    }
}
