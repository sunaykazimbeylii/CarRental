namespace CarRental.Application.Exceptions;

public class PastRentalDateException : Exception
{
    public PastRentalDateException()
        : base("Rental start date cannot be in the past.")
    {
    }
}