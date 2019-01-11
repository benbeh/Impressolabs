using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.ViewModels.API
{
    public class NotificationViewModel
    {
        public int Id { get; set; }
        public string AppUserId { get; set; }
        public string Description { get; set; }
        public DateTime DateOfPost { get; set; }
        public bool IsNewest { get; set; }
    }
}
