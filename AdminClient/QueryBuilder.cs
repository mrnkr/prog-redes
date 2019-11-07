namespace Gestion.Admin.Cli
{
    internal class QueryBuilder
    {
        private string EventType { get; set; }
        private string SortOrder { get; set; }

        public QueryBuilder WithEventType(string e)
        {
            EventType = e;
            return this;
        }

        public QueryBuilder Sort()
        {
            SortOrder = "timestamp";
            return this;

        }

        public QueryBuilder SortDescending()
        {
            SortOrder = "-timestamp";
            return this;
        }

        public string Build()
        {
            var ret = "";

            if (EventType != null)
            {
                ret += $"eventType={EventType}";
            }

            if (SortOrder != null)
            {
                ret += $"{(ret.Length > 0 ? "&" : "")}orderBy={SortOrder}";
            }

            return ret.Length > 0 ? $"?{ret}" : "";
        }
    }
}
