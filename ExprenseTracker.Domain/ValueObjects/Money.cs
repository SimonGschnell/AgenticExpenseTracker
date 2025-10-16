namespace ExprenseTracker.Domain.ValueObjects;

public record struct Money()
{
    public decimal Amount { get; init; }
    public Currency Currency { get; init; }
};