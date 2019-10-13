using System;

namespace TheMonolith.ECommerce
{
    public class Customer
    {
        public string Email { get; }
        public int Age { get; }
        public string LastName { get; }
        public string FirstName { get; }
        public Guid Id { get; }
        public Customer(Guid id, string firstName, string lastName, int age, string email)
        {
            this.Id = id;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Age = age;
            this.Email = email;
        }
    }
}
