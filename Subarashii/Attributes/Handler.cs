using System;
using Subarashii.Core.Exceptions;

namespace Subarashii.Core
{
    public class Handler : Attribute
    {
        public string OperationId { get; set; }

        public Handler(string operationId)
        {
            if (!Constants.codeRegex.IsMatch(operationId))
            {
                throw new InvalidCodeException();
            }

            OperationId = operationId;
        }
    }
}
