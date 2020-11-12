using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChatApplication.Domain.Attributes;
using ChatApplication.Domain.Contracts;
using ChatApplication.Domain.Entities;
using ChatApplication.Infrastructures.Stores;
using ChatApplication.Models.HubModels;
using ChatApplication.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ChatApplication.Hubs
{
    [ScopeDependecy]
    public class ChatHelper : IChatHelper
    {
        private readonly IConnectionManager connectionManager;
        private readonly ILogger<ChatHelper> logger;
        private readonly ChatDbContext dbContext;
        private readonly UserManager<User> userManager;
        private readonly JwtSetting jwtSetting;

        IHubContext<ChatHub> HubContext { get; }

        public ChatHelper(
            IHubContext<ChatHub> hubContext,
            IConnectionManager connectionManager,
            ILogger<ChatHelper> logger,
            ChatDbContext dbContext,
            UserManager<User> userManager, IOptionsSnapshot<AppSetting> options)
        {
            this.userManager = userManager;
            HubContext = hubContext;
            this.connectionManager = connectionManager;
            this.logger = logger;
            this.dbContext = dbContext;
            jwtSetting = options.Value.JwtSetting;
        }

        public async Task<List<UserVm>> GetAllUsersAsync(CancellationToken cancellationToken)
        {
            var users = await userManager
                .Users
                .AsNoTracking()
                .Select(user => new UserVm
                {
                    UserName = user.UserName,
                    UserId = user.Id,
                    UnreadMessageCount = user.Messages.Where(c => !c.IsReed && c.ReceiverId == user.Id).Count()

                })
                .ToListAsync(cancellationToken);

            foreach (var user in users)
            {
                user.IsOnline = connectionManager.userMap.Any(c => c.Key == user.UserName) && connectionManager.userMap[user.UserName].Any();
            }
            return users;
        }

        public Task<List<MessageVm>> GetUserConversationWithAsync(string userId, string contactId, int pageIndex, CancellationToken cancellationToken)
        {
            int skipItem = (pageIndex - 1) * 20;
            return dbContext
                .Messages
                .Where(msg =>
                               (msg.SenderId == userId && msg.ReceiverId == contactId) ||
                               (msg.SenderId == contactId && msg.ReceiverId == userId))
                .OrderBy(msg => msg.SendingTime)
                .Skip(skipItem)
                .Take(20)
                .AsNoTracking()
                .Select(msg => new MessageVm
                {
                    Id = msg.Id,
                    ReceiverId = msg.ReceiverId,
                    SenderId = msg.SenderId,
                    SendingTime = msg.SendingTime,
                    Text = msg.Text
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<FormatResponse<AuthVm>> AuthenticateByEmailAsync(LoginByEmailVm model)
        {
            var user = await userManager.FindByEmailAsync(model.Email); //dbContext.Users.SingleOrDefault (user => user.Email == model.Email);
            var response = new FormatResponse<AuthVm>();

            if (user == null)
            {
                response.IsSuccessed = false;
                response.Message = "کاربر یافت نشد";
                response.AddError("کاربر یافت نشد");

                return response;
            }

            PasswordVerificationResult result = userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                response.IsSuccessed = false;
                response.Message = "Invalid login attempt.";
                response.AddError("نام کاربری یا رمز عبور صحیح نیست");
                return response;
            }
            //var jwtGenerator = new JwtGenerator(jwtSetting);

            //var token = jwtGenerator.GenerateAsync(user);

            //logger.LogInformation($"authenticate user {user.UserName} at {DateTimeOffset.Now}");
            var token = await this.GenerateTokenAsync(user);
            response.IsSuccessed = true;
            response.Message = "احراز هویت با موفقیت انجام شد";
            response.Data = new AuthVm
            {
                Token = token,
                UserId = user.Id
            };
            return response;
        }

        public async Task<FormatResponse<AuthVm>> AuthenticateByPhoneNumberAsync(LoginbyPhoneNmberVm model, CancellationToken cancellationToken)
        {
            var user = await userManager.Users.FirstOrDefaultAsync(user => user.PhoneNumber == model.PhoneNumber, cancellationToken);//(model.Email); //dbContext.Users.SingleOrDefault (user => user.Email == model.Email);
            var response = new FormatResponse<AuthVm>();

            if (user == null)
            {
                response.IsSuccessed = false;
                response.Message = "کاربر یافت نشد";
                response.AddError("کاربر یافت نشد");

                return response;
            }

            //PasswordVerificationResult result = userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);
            //if (result == PasswordVerificationResult.Failed)
            //{
            //    response.IsSuccessed = false;
            //    response.Message = "Invalid login attempt.";
            //    response.AddError("نام کاربری یا رمز عبور صحیح نیست");
            //    return response;
            //}
            //var jwtGenerator = new JwtGenerator(jwtSetting);

            //var token = jwtGenerator.GenerateAsync(user);

            //logger.LogInformation($"authenticate user {user.UserName} at {DateTimeOffset.Now}");
            var token = await this.GenerateTokenAsync(user);
            response.IsSuccessed = true;
            response.Message = "احراز هویت با موفقیت انجام شد";
            response.Data = new AuthVm
            {
                Token = token,
                UserId = user.Id
            };
            //response.Data.Token = token;
            //response.Data.UserId = user.Id;

            return response;
        }

        public async Task<FormatResponse<MessageVm>> AddMessageForUserAsync(string senderId, SendMessageToUserVm message, CancellationToken cancellationToken)
        {
            try
            {
                var msg = new Message()
                {
                    IsReed = connectionManager.userMap.Any(u => u.Key == message.ReceiverUserName),
                    ReceiverId = message.ReceiverId,
                    SenderId = senderId,
                    Text = message.Text,
                };
                await dbContext.Messages.AddAsync(msg, cancellationToken);
                await dbContext.SaveChangesAsync(cancellationToken);
                return new FormatResponse<MessageVm>
                {
                    IsSuccessed = true,
                    Data = new MessageVm
                    {
                        Id = msg.Id,
                        ReceiverId = msg.ReceiverId,
                        SenderId = msg.SenderId,
                        SendingTime = msg.SendingTime,
                        Text = msg.Text,
                    },
                    Message = "با موفقیت ارسال شد"
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private Task<string> GenerateTokenAsync(User user)
        {
            var jwtGenerator = new JwtGenerator(jwtSetting);

            var token = jwtGenerator.GenerateAsync(user);

            logger.LogInformation($"authenticate user {user.UserName} at {DateTimeOffset.Now}");
            return Task.FromResult(token);
        }
    }
}