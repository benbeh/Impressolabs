using System;
namespace ImpressoApp.Models.User
{
    public class UserNameWithPhotoModel : BaseResponseModel
    {
        public string UserName { get; set; }
        public string Photo { get; set; }
    }
}
