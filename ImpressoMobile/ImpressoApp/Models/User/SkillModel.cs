using System;
using PropertyChanged;
namespace ImpressoApp.Models.User
{
    [AddINotifyPropertyChangedInterfaceAttribute]
    public class SkillModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsTop { get; set; }
    }
}
