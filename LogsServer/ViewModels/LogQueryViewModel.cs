using LogsServer.Exceptions;
using LogsServer.Logger;
using System.Text.RegularExpressions;

namespace LogsServer.ViewModels
{
    public class LogQueryViewModel
    {
        private const string EventTypeRegex = "StudentSignup|TeacherSignup|SubjectRegistration|SubjectDeletion|SubjectEnrollment|FileUpload|Grading";
        private const string OrderByRegex = "-?timestamp";

        public string eventType { get; set; }
        public string orderBy { get; set; }

        public void PrepareQuery(LogQuery query)
        {
            if (eventType != null)
            {
                if (!Regex.IsMatch(eventType, EventTypeRegex))
                {
                    throw new InvalidEventTypeException();
                }

                query.MatchEventType(eventType);
            }

            if (orderBy != null)
            {
                if (!Regex.IsMatch(orderBy, OrderByRegex))
                {
                    throw new InvalidSorterException();
                }

                query.OrderBy(orderBy);
            }
        }
    }
}