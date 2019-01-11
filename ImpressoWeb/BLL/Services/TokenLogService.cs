using System;
using BLL.AutoMapper;
using BLL.Interfaces;
using BLL.ViewModels;
using DAL.Interfaces;
using Core.Entities;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;

namespace BLL.Services
{
    public class TokenLogService : Service<TokenLog, TokenLogViewModel>, ITokenLogService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<AppUser> _userManager;

        public TokenLogService(IUnitOfWork unitOfWork, IConfiguration configuration, UserManager<AppUser> userManager) : base(unitOfWork, unitOfWork.TokenLogs)
        {
            _configuration = configuration;
            _userManager = userManager;
        }

        public int GetTotalTokensCount(string userId)
        {
            var list = Database.TokenLogs.GetAll().Where(record => record.ReceiverId == userId).Select(record => record.Count);

            var sum = 0;
            foreach(var item in list)
            {
                sum += item;
            }

            return sum;
        }

        public void AddTokensForSingingIn(string receiverId)
        {
            var numberOfTokens = Convert.ToInt32(_configuration["Data:AmountOfTokenForRegistration"]);
            Database.TokenLogs.Add(new TokenLog()
            {
                ReceiverId = receiverId,
                SenderId = _userManager.FindByNameAsync(_configuration["Data:AdminUser:Name"]).Result.Id,
                Count = numberOfTokens,
                Message = "singing up"
            });
            Database.Notifications.Add(new Notification()
            {
                Description = "You received " + numberOfTokens + " tokens for singing up",
                AppUserId = receiverId
            });
            Database.Save();
            _userManager.FindByIdAsync(receiverId).Result.Tokens = numberOfTokens;

        }
    }
}
