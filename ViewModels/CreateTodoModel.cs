using Flunt.Notifications;
using Flunt.Validations;
using System.Diagnostics.Contracts;

namespace MiniToDo.ViewModels
{
    public class CreateTodoModel : Notifiable<Notification>
    {
        public string Title { get; set; }

        public Todo MapTo()
        {
            var contract = new Contract<Notification>()
                .Requires()
                .IsNotNull(Title, "Informe o titulo da tarefa")
                .IsGreaterThan(Title, 5, "O titulo deve conter mais que 5 caracteres");

            AddNotifications(contract);

            return new Todo(Guid.NewGuid(), Title, false);
        }
       
    }
}
