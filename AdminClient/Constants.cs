using System.Collections.Generic;

namespace Gestion.Admin.Cli
{
    internal static class Constants
    {
        public const string EMAIL_REGEX = @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$";

        public static List<string> EventTypes = new List<string>()
            {
                "StudentSignup",
                "TeacherSignup",
                "SubjectRegistration",
                "SubjectDeletion",
                "SubjectEnrollment",
                "FileUpload",
                "Grading"
            };
    }
}
