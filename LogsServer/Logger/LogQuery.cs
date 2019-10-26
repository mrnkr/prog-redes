using Gestion.Common;
using Gestion.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LogsServer.Logger
{
    public class LogQuery
    {
        private IEnumerable<LogEntry> Logs { get; set; }

        public LogQuery(IEnumerable<LogEntry> dataSource)
        {
            Logs = dataSource;
        }

        public LogQuery MatchEventType(string e)
        {
            var eventType = (EventType)Enum.Parse(typeof(EventType), e);
            Logs = Logs.Where(log => log.EventType == eventType);
            return this;
        }

        public LogQuery OrderBy(string sorter)
        {
            var sortOrder = ExtractSortOrder(sorter);
            Logs = ApplySort(sortOrder);
            return this;
        }

        private SortOrder ExtractSortOrder(string sorter)
        {
            return sorter.StartsWith("-") ?
                SortOrder.Descending :
                SortOrder.Ascending;
        }

        private IEnumerable<LogEntry> ApplySort(SortOrder sortOrder)
        {
            switch (sortOrder)
            {
                case SortOrder.Ascending:
                    return Logs.OrderBy(l => l.Timestamp);
                case SortOrder.Descending:
                    return Logs.OrderByDescending(l => l.Timestamp);
                default:
                    return Logs;
            }
        }

        public IEnumerable<LogEntry> RunQuery()
        {
            return Logs;
        }
    }
}