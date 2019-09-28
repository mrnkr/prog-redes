using System;

namespace SimpleRouter
{
    public class SimpleHandler : Attribute
    {
        public string OperationId { get; set; }

        public SimpleHandler(string operationId)
        {
            OperationId = operationId;
        }
    }
}
