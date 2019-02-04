using BLL.ViewModels;

namespace BLL.Integration
{
    public class JwtLoginResponseMessage : ResponseMessageBase
    {
        public JwtViewModel Jwt { get; set; }
    }
}
