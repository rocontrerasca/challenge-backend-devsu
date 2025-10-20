using Challenge.Devsu.Core.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Challenge.Devsu.Core.Entities
{

    public class Move
    {
        [Column("move_id")]
        public Guid MoveId { get; set; }
        [Column("transaction_date")]
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
        [Column("move_type")]
        public MoveType MoveType { get; set; }
        [Column("amount")]
        public decimal Amount { get; set; }
        [Column("balance")]
        public decimal Balance { get; set; }
        [Column("account_ref_id")]
        public Guid AccountRefId { get; set; }
        [Column("success")]
        public bool Success { get; set; }
        public Account Account { get; set; } = default!;
    }
}
