using MessageService.Models;

namespace MessageService.Abstraction
{
    public interface IMessageRepository
    {
        IEnumerable<Message> GetMessageForUser(Guid userId);
        void SendMessage(Message message);
    }
}
