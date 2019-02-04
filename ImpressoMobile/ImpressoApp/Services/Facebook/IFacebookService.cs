using System;
using System.Threading.Tasks;
using ImpressoApp.Models.Authentication;

namespace ImpressoApp.Services.Facebook
{
    public interface IFacebookService
    {
        Task<FacebookLoginResult> Login();

        void Logout();
    }
}
