namespace ChainOfResponsibility
{
    public interface IHandler<TMessage>
    {
        void SetNextHandler(IHandler<TMessage> nextHandler);

        void HandleMessage(TMessage message);
    }
}