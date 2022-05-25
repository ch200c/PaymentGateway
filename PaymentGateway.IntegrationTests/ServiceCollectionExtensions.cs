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

    public static void AddMockDateTimeProvider(this IServiceCollection services, DateTime dateTime)
    {
        var dateTimeProviderMock = new Mock<IDateTimeProvider>();

        dateTimeProviderMock
            .Setup(dateTimeProvider => dateTimeProvider.GetDateTime())
            .Returns(dateTime);

        services.RemoveService(typeof(IDateTimeProvider));
        services.AddTransient(_ => dateTimeProviderMock.Object);
    }
}
