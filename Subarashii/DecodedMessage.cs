namespace Subarashii.Core
{
    internal class DecodedMessage<T>
    {
        public bool IsResponse { get; set; }
        public string Code { get; set; }
        public bool IsFile { get; set; }
        public string Auth { get; set; }
        public T Payload { get; set; }
    }
}
