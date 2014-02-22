using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.BitWise;

namespace BWOQ
{
    public class Person
    {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public string DocId { get; set; }
        public string Name { get; set; }
        public float Size { get; set; }
        public float Height { get; set; }
        public float Genre { get; set; }
        public bool Active { get; set; }

        public Address Address { get; set; }
        public Contact Contact { get; set; }
    }

    public class Address
    {
        public int Id { get; set; }
        public string Street { get; set; }
        public int Number { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }

    public class Contact
    {
        public int Id { get; set; }
        public long PhoneNumber { get; set; }
        public string Email { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var personList = new List<Person>();

            personList.Add(new Person { Id = 1, CreationDate = new DateTime(2013, 12, 25), DocId = "44.762.881-50", Name = "Carlos de Andrade", Size = 1.8F, Height = 70, Genre = 1, Active = true, Address = new Address() { Street = "Loreto Avenue" }, Contact = new Contact() { PhoneNumber = 551144523328 } });
            personList.Add(new Person { Id = 2, CreationDate = new DateTime(2014, 01, 26), DocId = "19.333.121-X", Name = "Laura Carvalho", Size = 1.73F, Height = 62, Genre = 2, Active = true, Address = new Address() { Street = "Alcapone Street" }, Contact = new Contact() { PhoneNumber = 551144226328 } });
            personList.Add(new Person { Id = 3, CreationDate = new DateTime(2014, 01, 22), DocId = "16.555.132-X", Name = "João Augusto", Size = 1.90F, Height = 86, Genre = 1, Active = true, Address = new Address() { Street = "Sunshine Street" }, Contact = new Contact() { PhoneNumber = 551176272328 } });
            personList.Add(new Person { Id = 4, CreationDate = new DateTime(2014, 01, 10), DocId = "28.264.165-X", Name = "Julia dos Santos", Size = 1.60F, Height = 55, Genre = 2, Active = true, Address = new Address() { Street = "Matarazzo Street" }, Contact = new Contact() { PhoneNumber = 551135294228 } });
            personList.Add(new Person { Id = 5, CreationDate = new DateTime(2014, 01, 10), DocId = "28.262.110-X", Name = "Clara dos Santos", Size = 1.63F, Height = 57, Genre = 2, Active = true, Address = new Address() { Street = "Mazzarope Street" }, Contact = new Contact() { PhoneNumber = 551122562233 } });
           
            var bwq = new BitWiseQuery<Person>(personList.AsQueryable());

            var objList = bwq.Query("13>1:6>2:2").Where("4::28", EnumSerialDataType.JSON);
        }
    }
}
