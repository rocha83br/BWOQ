using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.BitWise;
using System.IO;
using Newtonsoft.Json;

namespace BWOQ
{
    [Serializable]
    public class Person
    {
        public Person()
        {
        }

        public decimal Id { get; set; }         // 1
        public string Address { get; set; }     // 2
        public string Telephone { get; set; }   // 4
        public string City { get; set; }        // 8
        public string PostalCode { get; set; }  // 16
        public string Name { get; set; }        // 32
        public decimal Number { get; set; }     // 64
        public string WebSite { get; set; }     // 128
        public string Email { get; set; }       // 256
        public string District { get; set; }    // 512
        public string State { get; set; }       // 1024
        public string CellPhone { get; set; }   // 2048
    }

    class Program
    {
        static void Main(string[] args)
        {
            // FILLING DEMO DATA SOURCE

            var jsonContent = File.OpenText("C:\\Renato\\TestMass.json");
            var jsonListInstance = JsonConvert.DeserializeObject<List<Person>>(jsonContent.ReadToEnd());

            var bwq = new BitWiseQuery<Person>(jsonListInstance.AsQueryable());

            var timer = System.Diagnostics.Stopwatch.StartNew();
            timer.Start();

            var objList = bwq.Query("303", true);

            timer.Stop();
            Console.WriteLine(string.Concat(jsonListInstance.Count, " records. Query time : ", timer.ElapsedMilliseconds.ToString()));
            Console.Read();
        }
    }
}
