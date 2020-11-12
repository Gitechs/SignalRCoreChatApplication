using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using ChatApplication.Domain.Contracts;
using ChatApplication.Domain.Entities;
using ChatApplication.Models.HubModels;
using ChatApplication.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ChatApplication.Hubs
{
    public class ChatHub : Hub
    {

        private readonly IConnectionManager connectionManager;
        private readonly IChatHelper chatHelper;
        private readonly ILogger<ChatHub> logger;
        private readonly UserManager<User> userManager;

        public ChatHub(IConnectionManager connectionManager, IChatHelper chatHelper, ILogger<ChatHub> logger,
            UserManager<User> userManager)
        {
            this.connectionManager = connectionManager;
            this.chatHelper = chatHelper;
            this.logger = logger;
            this.userManager = userManager;
        }

        public override Task OnConnectedAsync()
        {
            // var userName = Context.User.Identity.Name;
            // connectionManager.AddConnection (userName, Context.ConnectionId);

            return base.OnConnectedAsync();
        }
        public async override Task OnDisconnectedAsync(Exception exception)
        {
            // var userName = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            connectionManager.RemoveConnection(Context.ConnectionId);

            if (exception != null)
                logger.LogError(exception, $"client with username was disconnected at {DateTimeOffset.Now}");
            else
                logger.LogInformation($"client with username was disconnected at {DateTimeOffset.Now}");

            await this.FeedUsersToAllAsync(CancellationToken.None);

            await base.OnDisconnectedAsync(exception);
        }

        public async Task AuthenticateByEmail(LoginByEmailVm Data)
        {

            var result = await chatHelper.AuthenticateByEmailAsync(Data);
            if (result.IsSuccessed)
            {
                var user = await userManager.FindByEmailAsync(Data.Email); //Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
                connectionManager.AddConnection(await userManager.GetUserNameAsync(user), Context.ConnectionId);

                await this.FeedUsersToAllAsync(Context.ConnectionAborted);
            }
            await Clients.Caller.SendAsync("OnAuthenticate", result, Context.ConnectionAborted);
        }

        public async Task AuthenticateByPhoneNumber(LoginbyPhoneNmberVm Data)
        {

            FormatResponse<AuthVm> result = await chatHelper.AuthenticateByPhoneNumberAsync(Data, Context.ConnectionAborted);
            if (result.IsSuccessed)
            {
                User user = await userManager.Users.FirstOrDefaultAsync(user => user.PhoneNumber == Data.PhoneNumber, Context.ConnectionAborted); ; //Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
                connectionManager.AddConnection(await userManager.GetUserNameAsync(user), Context.ConnectionId);
                await this.FeedUsersToAllAsync(Context.ConnectionAborted);
            }
            await Clients.Caller.SendAsync("OnAuthenticate", result, Context.ConnectionAborted);
        }

        [Authorize]
        public async Task GetUserMessages(GetUserConvesationVm model)
        {
            FormatResponse<List<MessageVm>> result = new FormatResponse<List<MessageVm>>();

            var userId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            result.Data = await chatHelper.GetUserConversationWithAsync(userId, model.ContactId, model.PageIndex, Context.ConnectionAborted);
            result.IsSuccessed = true;
            result.Message = "لیست پیام ها";

            await Clients.Caller.SendAsync("OnGetUserMessages", result, Context.ConnectionAborted);

        }

        [Authorize]
        public async Task SendMessageToUser(SendMessageToUserVm model)
        {
            try
            {
                var userId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
                FormatResponse<MessageVm> result = await chatHelper.AddMessageForUserAsync(userId, model, Context.ConnectionAborted);
               
                //get receiver connetion ids;
                HashSet<string> connections = connectionManager.GetConnections(model.ReceiverUserName) ?? new HashSet<string>();
                
                //add current sender connetion id to our list
                connections.Add(Context.ConnectionId);

                //sent back message to all online devise of receiver and sender 
                await Clients.Clients(connections.ToList()).SendAsync("OnSendBackMessageToUser", result, Context.ConnectionAborted);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "thrwo error when user send message");
            }
        }

        /// <summary>
        /// when user logged or logged out, this method sent all users back to him\her.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task FeedUsersToAllAsync(CancellationToken cancellationToken)
        {
            FormatResponse<List<UserVm>> result = new FormatResponse<List<UserVm>>();

            List<UserVm> data = await chatHelper.GetAllUsersAsync(cancellationToken);
            if (data != null || data.Any())
            {
                result.Data = data;
                result.IsSuccessed = true;
                result.Message = "لیست کاربران به همراه تعداد پیام های خوانده نشده";
            }

            await Clients.All.SendAsync("OnUsersFeed", result, cancellationToken);
        }


        //[Authorize]
        //public void TestAuthentication()
        //{
        //    var dd = Context.GetHttpContext().Request.Query;
        //    logger.LogInformation("authenticated");
        //}
    }
}