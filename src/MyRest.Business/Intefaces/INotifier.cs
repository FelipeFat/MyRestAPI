using System.Collections.Generic;
using MyRest.Business.Notificacoes;

namespace MyRest.Business.Intefaces
{
    public interface INotifier
    {
        bool HasNotifications();
        List<Notification> GetNotifications();
        void Handle(Notification notification);
    }
}