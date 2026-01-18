using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ETicaret.Orders.WebAPI.Dtos;
public sealed record OrderDto
{
    [BsonId]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid Id { get; set; }

    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = default!;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public DateTime CreatAt { get; set; }
}
