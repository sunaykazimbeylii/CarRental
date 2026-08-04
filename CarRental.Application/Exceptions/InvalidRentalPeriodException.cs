namespace CarRental.Application.Exceptions;

public class InvalidRentalPeriodException : Exception
{
    public InvalidRentalPeriodException()
        : base("End date must be greater than start date.")
    {
    }
}