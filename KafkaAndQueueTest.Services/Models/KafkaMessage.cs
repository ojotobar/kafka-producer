namespace KafkaAndQueueTest.Services.Models
{
    public class KafkaMessage<TMessage>
    {
        public string Key { get; set; } = string.Empty;
        public required TMessage Value { get; set; }
    }
}
