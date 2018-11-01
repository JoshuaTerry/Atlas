using System;

namespace DriveCentric.Model.Enums
{
    [Flags]
    public enum ActionType
    {
        Reminder = 0,
        SendEmail = 1,
        PhoneCall = 2,
        SendSnailMail = 3,
        SendTextMessage = 4,
        SendVideoMessage = 6,
        TouchPoint = 7,
        CreateTask = 30,
        Complete = 100,
        ToggleRead = 101,
        ViewXML = 102,
        ServiceLeadPartsOrderStarted = 103,
        ServiceLeadPartsOrderCancelled = 104,
        ServiceLeadPartsOrderCompleted = 105,
        ServiceLeadDelete = 106,
        Undefined = 10000000
    }
}
