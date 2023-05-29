
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Xml;

public class CustomMessageInspector : IClientMessageInspector
{
    readonly string _authToken;

    public CustomMessageInspector(string authToken)
    {
        _authToken = authToken;
    }

    public object BeforeSendRequest(ref Message request, IClientChannel channel)
    {
        var reqMsgProperty = new HttpRequestMessageProperty();
        reqMsgProperty.Headers.Add("apikey", _authToken);
        request.Properties[HttpRequestMessageProperty.Name] = reqMsgProperty;
        using (var buffer = request.CreateBufferedCopy(int.MaxValue))
        {
            var document = GetDocument(buffer.CreateMessage());
            Console.WriteLine(document.OuterXml);
            request = buffer.CreateMessage();
            return null;
        }
    }

    private XmlDocument GetDocument(Message request)
    {
        XmlDocument document = new XmlDocument();
        using (MemoryStream memoryStream = new MemoryStream())
        {
            // write request to memory stream
            XmlWriter writer = XmlWriter.Create(memoryStream);
            request.WriteMessage(writer);
            writer.Flush();
            memoryStream.Position = 0;
            // load memory stream into a document
            document.Load(memoryStream);
        }
        return document;
    }
    public void AfterReceiveReply(ref Message reply, object correlationState)
    { }
}