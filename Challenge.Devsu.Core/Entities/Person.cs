using Challenge.Devsu.Core.Enums;

namespace Challenge.Devsu.Core.Entities
{

    public abstract class Person
    {
        public string FullName { get; set; } = string.Empty;
        public PersonGender Gender { get; set; }
        public int Age { get; set; }
        public string IdentificationNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
