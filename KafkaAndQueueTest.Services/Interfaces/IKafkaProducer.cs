namespace KafkaAndQueueTest.Services.Interfaces
{
    public interface IKafkaProducer
    {
        Task ProduceAsync<TMessage> (string topic, TMessage value);
    }
}