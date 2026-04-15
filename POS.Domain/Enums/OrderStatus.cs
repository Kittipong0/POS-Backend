namespace POS.Domain.Enums;

public enum OrderStatus
{
    Pending,
    Preparing,
    Cooking,
    ReadyToServe,
    Served,
    Completed,
    Cancelled
}
