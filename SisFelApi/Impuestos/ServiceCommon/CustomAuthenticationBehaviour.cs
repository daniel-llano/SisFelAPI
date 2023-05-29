
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

public class CustomAuthenticationBehaviour : IEndpointBehavior
{
    readonly string _authToken;
    public CustomAuthenticationBehaviour(string authToken)
    {
        _authToken = authToken;
    }
    public void Validate(ServiceEndpoint endpoint)
    { }
    public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
    { }
    public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
    { }
    public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
    {
        clientRuntime.ClientMessageInspectors.Add(new CustomMessageInspector(_authToken));
    }
}