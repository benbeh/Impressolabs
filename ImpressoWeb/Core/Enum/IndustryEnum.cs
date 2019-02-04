using System.ComponentModel;

namespace Core.Enum
{
    public enum IndustryEnum
    {
        None = 0,
        [Description("Graphic Design")]
        GraphicDesign = 1,
        [Description("User Interface")]
        UserInterface = 2,
        [Description("Animation")]
        Animation = 3,
        [Description("Architecture and Planning")]
        ArchitectureAndPlanning = 4,
        [Description("Arts and Crafts")]
        ArtsAndCrafts = 5,
        [Description("Automotive")]
        Automotive = 6,
        [Description("Broadcast Media")]
        BroadcastMedia = 7,
        [Description("Computer and Network Security")]
        ComputerAndNetworkSecurity = 8,
        [Description("Computer Games")]
        ComputerGames = 9,
        [Description("Computer Networking")]
        ComputerNetworking = 10,
        [Description("Computer Hardware")]
        ComputerHardware = 11,
        [Description("Computer Software")]
        ComputerSoftware = 12,
        [Description("Construction")]
        Construction = 13
    }
}