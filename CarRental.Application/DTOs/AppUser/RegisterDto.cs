namespace CarRental.Application.DTOs.AppUser
{
    public record RegisterDto(string Name, string Surname, string Email, string Username, string Password, string ConfirmPasswsord);
}
