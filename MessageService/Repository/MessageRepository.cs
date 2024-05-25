using MessageService.Abstraction;
using MessageService.Context;
using MessageService.Models;

namespace MessageService.Repository
{
    public class MessageRepository(MessageContext context) : IMessageRepository
    {
        public IEnumerable<Message> GetMessageForUser(Guid userId)
        {
            var unreadMessages =  context.Messages.Where(x => x.RecipientId == userId && !x.IsRead).ToList();
            unreadMessages.ForEach(x => x.IsRead = true);
            context.SaveChanges();
            return unreadMessages;
        }

        public void SendMessage(Message message)
        {
            message.Id = Guid.NewGuid();
            message.SentAt = DateTime.UtcNow;
            context.Messages.Add(message);
            context.SaveChanges();
        }
    }
}
