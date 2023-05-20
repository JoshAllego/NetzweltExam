namespace Exam.Models
{
    public class LogInResponseModel
    {
        public string userName { get; set; }
        public string displayName { get; set; }

        public List<string> roles { get; set; }
    }
}
