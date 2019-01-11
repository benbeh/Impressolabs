using BLL.ViewModels;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Interfaces
{
    public interface ITokenLogService : IService<TokenLog, TokenLogViewModel>
    {
        int GetTotalTokensCount(string userId);

        void AddTokensForSingingIn(string userId);
    }
}
