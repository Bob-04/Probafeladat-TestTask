namespace Products.Api.Domain.Contracts;

public record ProductCreate
{
    public string Name { get; init; }
    public string Description { get; init; }
    public decimal Price { get; init; }
};