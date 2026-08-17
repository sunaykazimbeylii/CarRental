namespace CarRental.Application.Interfaces.Services;

public interface IEmailNotificationService
{
    Task SendRentalCreatedEmailAsync(long userId, long rentalId);
}