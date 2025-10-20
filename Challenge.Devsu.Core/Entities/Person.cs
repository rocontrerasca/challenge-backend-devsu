using Challenge.Devsu.Core.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Challenge.Devsu.Core.Entities
{

    public abstract class Person
    {
        [Column("full_name")]
        public string FullName { get; set; } = string.Empty;
        [Column("gender")]
        public string Gender { get; set; } = string.Empty;
        [Column("age")]
        public int Age { get; set; }
        [Column("identification_number")]
        public string IdentificationNumber { get; set; } = string.Empty;
        [Column("address")]
        public string Address { get; set; } = string.Empty;
        [Column("phone_number")]
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
