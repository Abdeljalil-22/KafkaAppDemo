namespace KafkaAppDemo.Models
{
    public class KafkaMessage
    {
        public string? Key { get; set; }
        public string? Value { get; set; }
        public string? Topic { get; set; }
    }
}