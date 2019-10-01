using System;

namespace SimpleRouter
{
    public class SimpleHandler : Attribute
    {
        public string OperationId { get; }
        public string Summary { get; }

        public SimpleHandler(string operationId, string summary)
        {
            OperationId = operationId;
            Summary = summary;
        }
    }
}
