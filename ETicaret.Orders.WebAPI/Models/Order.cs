using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ETicaret.Orders.WebAPI.Models;

public class Order
{
    [BsonId]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid Id { get; set; }

    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid ProductId { get; set; }

    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public DateTime CreatAt { get; set; }
}
