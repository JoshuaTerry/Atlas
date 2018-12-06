using ServiceStack.DataAnnotations;

namespace DriveCentric.Model.Enums
{
    [EnumAsInt]
    public enum StoreCategory
    {
        All = 0,
        General = 1,
        Messaging = 2,
        Client = 3,
        Dealer = 4
    }
}