using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.BitWise;

namespace BWOQ
{
    class Program
    {
        static void Main(string[] args)
        {
            var personList = new List<dynamic>();
            
            personList.Add(new { Id = 1, CreationDate = new DateTime(2013, 12, 25), DocId = "44.762.881-50", Name = "Carlos de Andrade", Size = 1.8, Height = 70, Genre = 1, Address = "Loreto Avenue", Active = true });
            personList.Add(new { Id = 2, CreationDate = new DateTime(2014, 01, 26), DocId = "19.333.121-X", Name = "Laura Carvalho", Size = 1.73, Height = 62, Genre = 2, Address = "Alcapone Street", Active = false });
            personList.Add(new { Id = 3, CreationDate = new DateTime(2014, 01, 22), DocId = "16.555.132-X", Name = "João Augusto", Size = 1.90, Height = 86, Genre = 1, Address = "Sunshine Street", Active = true });
            personList.Add(new { Id = 4, CreationDate = new DateTime(2014, 01, 10), DocId = "28.262.165-X", Name = "Julia dos Santos", Size = 1.60, Height = 55, Genre = 2, Address = "Matarazzo Street", Active = true });
            personList.Add(new { Id = 5, CreationDate = new DateTime(2014, 01, 10), DocId = "28.262.110-X", Name = "Clara dos Santos", Size = 1.63, Height = 57, Genre = 2, Address = "Matarazzo Street", Active = true });
           
            var bwq = new BitWiseQuery<dynamic>(personList.AsQueryable());

            var objList = bwq.Query("9>1:2").Where("128:1&=");
        }
    }
}
