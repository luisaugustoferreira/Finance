using Empreendedores.MVVM.Enuns;
using LiteDB;

namespace Empreendedores.MVVM.Models
{
    public class Transaction
    {
        [BsonId]
        public int Id { get; set; }
        public TransactionType Type { get; set; }
        public string Name { get; set; }
        public DateTimeOffset Data { get; set; }
        public double Value { get; set; }
    }
}
