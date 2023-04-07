namespace WEB.Models;
public class EmailViewModel
    {
        public List<string> ToList { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
        public bool IsHtml { get; set; }
    }