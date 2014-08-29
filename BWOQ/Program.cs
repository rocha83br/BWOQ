using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.BitWise;

namespace BWOQ
{
    [Serializable]
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

    [Serializable]
    public class Address
    {
        public int Id { get; set; }
        public string Street { get; set; }
        public int Number { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }

    [Serializable]
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

            personList.Add(new Person { Id = 1, CreationDate = new DateTime(2013, 12, 25), DocId = "44.762.881-50", Name = "José de Souza", Size = 1.8F, Height = 70, Genre = 1, Active = true, Address = new Address() { Street = "Loreto Avenue", Number = 123 }, Contact = new Contact() { PhoneNumber = 551144523328 } });
            personList.Add(new Person { Id = 2, CreationDate = new DateTime(2014, 01, 26), DocId = "19.333.121-X", Name = "Laura Carvalho", Size = 1.73F, Height = 62, Genre = 2, Active = true, Address = new Address() { Street = "Saint Louis Street", Number = 444 }, Contact = new Contact() { PhoneNumber = 551144226328 } });
            personList.Add(new Person { Id = 3, CreationDate = new DateTime(2014, 01, 22), DocId = "16.555.132-X", Name = "João Augusto", Size = 1.90F, Height = 86, Genre = 1, Active = true, Address = new Address() { Street = "Alcapone Street", Number = 562 }, Contact = new Contact() { PhoneNumber = 551176272328 } });
            personList.Add(new Person { Id = 4, CreationDate = new DateTime(2014, 01, 10), DocId = "28.264.165-X", Name = "Julia dos Santos", Size = 1.60F, Height = 55, Genre = 2, Active = true, Address = new Address() { Street = "Matarazzo Street", Number = 88 }, Contact = new Contact() { PhoneNumber = 551135294228 } });
            personList.Add(new Person { Id = 5, CreationDate = new DateTime(2014, 03, 14), DocId = "32.333.110-X", Name = "Clara dos Santos", Size = 1.63F, Height = 57, Genre = 2, Active = true, Address = new Address() { Street = "Magno Dias Street", Number = 140 }, Contact = new Contact() { PhoneNumber = 551122562233 } });
            personList.Add(new Person { Id = 6, CreationDate = new DateTime(2014, 03, 18), DocId = "24.556.210-X", Name = "Carlos de Andrade", Size = 1.71F, Height = 57, Genre = 1, Active = false, Address = new Address() { Street = "Robert Mozart Street", Number = 228 }, Contact = new Contact() { PhoneNumber = 551133182130 } });
            personList.Add(new Person { Id = 7, CreationDate = new DateTime(2014, 04, 08), DocId = "55.789.330-X", Name = "Mauro Alcantara", Size = 1.59F, Height = 57, Genre = 1, Active = false, Address = new Address() { Street = "Saint Louis Street", Number = 220 }, Contact = new Contact() { PhoneNumber = 551122572280 } });
            personList.Add(new Person { Id = 8, CreationDate = new DateTime(2014, 05, 12), DocId = "48.222.442-X", Name = "Luis Roberto Pasquin", Size = 1.68F, Height = 57, Genre = 1, Active = true, Address = new Address() { Street = "Canada Street", Number = 700 }, Contact = new Contact() { PhoneNumber = 551125321120 } });
            personList.Add(new Person { Id = 9, CreationDate = new DateTime(2014, 06, 07), DocId = "18.445.333-X", Name = "Ana Maria da Silva", Size = 1.74F, Height = 57, Genre = 2, Active = true, Address = new Address() { Street = "Saint Louis Street", Number = 45 }, Contact = new Contact() { PhoneNumber = 551132512344 } });

            var bwq = new BitWiseQuery<Person>(personList.AsQueryable());

            var objList = bwq.Query("13").Where("8::a", EnumSerialDataType.CSV);
        }
    }
}
