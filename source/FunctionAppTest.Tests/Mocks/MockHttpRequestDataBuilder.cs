using System.Text.Json;
using Azure.Core.Serialization;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;

namespace FunctionAppTest.Tests.Mocks;

public class MockHttpRequestDataBuilder
{
    private readonly IServiceCollection requestServiceCollection;
    private IInvocationFeatures invocationFeatures;
    private IServiceProvider requestContextInstanceServices;
    private FunctionContext functionContext;

    private string rawJsonBody;

    public MockHttpRequestDataBuilder()
    {
        this.requestServiceCollection = new ServiceCollection();
        this.requestServiceCollection.AddOptions();
    }

    public MockHttpRequestDataBuilder WithDefaultJsonSerializer()
    {
        this.requestServiceCollection
            .Configure<WorkerOptions>(workerOptions =>
            {
                workerOptions.Serializer =
                    new JsonObjectSerializer(
                        new JsonSerializerOptions
                        {
                            AllowTrailingCommas = true,
                        });
            });

        return this;
    }

    public MockHttpRequestDataBuilder WithCustomJsonSerializerSettings(
        Func<JsonObjectSerializer> jsonObjectSerializerOptions)
    {
        this.requestServiceCollection.Configure<WorkerOptions>(
            workerOptions => workerOptions.Serializer = jsonObjectSerializerOptions());
        return this;
    }

    public MockHttpRequestDataBuilder WithRequestContextInstanceServices(
        IServiceProvider requestContextInstanceServices)
    {
        this.requestContextInstanceServices = requestContextInstanceServices;
        return this;
    }

    public MockHttpRequestDataBuilder WithInvocationFeatures(
        IInvocationFeatures invocationFeatures)
    {
        this.invocationFeatures = invocationFeatures;
        return this;
    }

    public MockHttpRequestDataBuilder WithFakeFunctionContext()
    {
        this.requestContextInstanceServices ??= this.requestServiceCollection.BuildServiceProvider();

        this.functionContext = 
            new FakeFunctionContext(this.invocationFeatures)
            {
                InstanceServices = this.requestContextInstanceServices ?? this.requestServiceCollection.BuildServiceProvider(),
            };

        return this;
    }

    public MockHttpRequestDataBuilder WithRawJsonBody(string rawJsonBody)
    {
        this.rawJsonBody = rawJsonBody;
        return this;
    }

    public MockHttpRequestData Build()
        => new(this.functionContext, this.rawJsonBody);
}

public class FakeFunctionContext(IInvocationFeatures features, IDictionary<object, object> items = null) : FunctionContext
{
    public override string InvocationId { get; }

    public override string FunctionId { get; }

    public override TraceContext TraceContext { get; }

    public override BindingContext BindingContext { get; }

    public override RetryContext RetryContext { get; }

    public override IServiceProvider InstanceServices { get; set; }

    public override FunctionDefinition FunctionDefinition { get; }

    public override IDictionary<object, object> Items { get; set; } = items;

    public override IInvocationFeatures Features { get; } = features;
}