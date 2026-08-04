namespace CarRental.Application.Exceptions;

public class CarNotAvailableException : Exception
{
    public CarNotAvailableException()
        : base("Car is not available for rental.")
    {
    }
}
