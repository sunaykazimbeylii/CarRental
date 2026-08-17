using CarRental.Application.Interfaces.Services;

namespace CarRental.Application.Services;

public class EmailNotificationService : IEmailNotificationService
{
    public async Task SendRentalCreatedEmailAsync(long userId, long rentalId)
    {
      
        await Task.Delay(3000);

        Console.WriteLine(
            $"Email notification göndərildi. UserId: {userId}, RentalId: {rentalId}");
    }
}