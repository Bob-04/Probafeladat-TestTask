namespace Products.Api.Domain.Contracts;

public record ProductUpdate
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public decimal Price { get; init; }
}