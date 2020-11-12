using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ChatApplication.Models.HubModels;
using ChatApplication.Utilities;

namespace ChatApplication.Domain.Contracts {
    public interface IChatHelper {
        Task<List<MessageVm>> GetUserConversationWithAsync(string userId, string contactId, int pageIndex, CancellationToken cancellationToken);
        Task<FormatResponse<AuthVm>> AuthenticateByEmailAsync (LoginByEmailVm model);
        Task<FormatResponse<AuthVm>> AuthenticateByPhoneNumberAsync(LoginbyPhoneNmberVm model, CancellationToken cancellationToken);
        Task<List<UserVm>> GetAllUsersAsync(CancellationToken cancellationToken);
        Task<FormatResponse<MessageVm>> AddMessageForUserAsync(string senderId, SendMessageToUserVm message, CancellationToken cancellationToken);
    }
}