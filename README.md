           .---.     .---.
          ( -o- )---( -o- )
          ;-...-`   `-...-;
         /                 \
        /                   \
       | /_               _\ |
       \`'.`'"--.....--"'`.'`/
        \  '.   `._.`   .'  /
     _.-''.  `-.,___,.-`  .''-._
    `--._  `'-._______.-'`  _.--`
      /                 \
        /.-'`\   .'.   /`'-.\
       `      '.'   '.' 

BWOQ

The initial idea was to create something similar to Dynamic LINQ using a different strategy to Reflection.Emit , based on class CSharpCodeProvider / CompileAssemblyFromSource , executing methods containing LINQ queries generated at runtime to hold this solution based on logical comparison .

As our notorious colleagues on Microsoft ever thought this idea, we extend ourselves from it !

BWOQ (BitWise Object Query) is a suggestion shorter code when performing Elastic searches on objects, SQL Databases, XML files, Text files, Etc.,
analogous to the proposal of John Resig with JQuery in 2006 in the html DOM and Javascript context.

Think any attribute of an object can be indexed within a binary table defining binary values at runtime to each attribute according to his ordinaridade, 
we can make comparisons using logical disjunction recursively for these attributes, both as predicates, such as the lambda search criteria.

This, together with the concept of postfix representation of the operations, (Reverse Polish Notation Charles Hamblin in 1950).

---

Scenario :

Class Person
    Id
    CreationDate
    DocId
    Name
    Size
    Height
    Genre
    Active
    Credential
        Id
	Logon
        Password
        TokenId
    Address
        Id
        Street
        Number
        District
        City
        State
        ZipCode
        Lat
        Lng
    Contact
        Id
        TypeId : Enum of [TelPhone, Email, InstantMessenger, SocialNetwork]
        Name
        Description

We need to populate a visual interface with Id , Name and City attributes only of active users, we generate the mind table [1,2,4,8,16,32,64,128], 
once we have the combination bound to represent the 3 columns for logical or [1,8] giving decimal 9 that will be used in the query expression, eg :

/*

public static void Main(string[] args) { var personList = new List();

    personList.Add(new { Id = 1, CreationDate = new DateTime(2013, 12, 25), DocId = "44.762.881-50", Name = "Carlos de Andrade", Size = 1.8, Height = 70, Genre = 1, Address = "Loreto Avenue", Active = true });
    personList.Add(new { Id = 2, CreationDate = new DateTime(2014, 01, 26), DocId = "19.333.121-X", Name = "Laura Carvalho", Size = 1.73, Height = 62, Genre = 2, Address = "Alcapone Street", Active = false });
    personList.Add(new { Id = 3, CreationDate = new DateTime(2014, 01, 22), DocId = "16.555.132-X", Name = "Joao Augusto", Size = 1.90, Height = 86, Genre = 1, Address = "Sunshine Street", Active = true });
    personList.Add(new { Id = 4, CreationDate = new DateTime(2014, 01, 10), DocId = "28.262.165-X", Name = "Julia dos Santos", Size = 1.60, Height = 55, Genre = 2, Address = "Matarazzo Street", Active = true });
    personList.Add(new { Id = 5, CreationDate = new DateTime(2014, 01, 10), DocId = "28.262.110-X", Name = "Clara da Silva", Size = 1.63, Height = 57, Genre = 2, Address = "Matarazzo Street", Active = true });

var bwq = new BitWiseQuery<Person>(personList);

var objList = bwq.Query("9>2:16").Where("128::1&=");

}

*/

---

Where the operator & indicates the use of the criteria conjunction, and = is equality between attribute value 
and informed expression after the token :: and between posfix operators in the criteria context.

It is also observed using token > to navigation within aggregated objects, then informing the ordinal household 
and the combination of attributes also informed after the token : in the context of the predicate .

Using this practice, combined with the use of dynamic typing eliminate the need for models for data transport in specific scenarios, 
the famous value objects or DTO (including support for JSON and XML outputs).

---

/*

Syntax Ref.:
Predicate

9  = Binary combination representing the selected attributes 
>  = Start Navigation in Tree of aggregate objects 
2  = Ordinal position of the aggregate 
:  =  Token defining action 'get' the aggregate attributes combination
16 = Binary combination representing the selected attributes of the aggregate
*   = Token defining Count aggregation operation
^   = Token defining Sum aggregation operation
~   = Token defining Average aggregation operation
+   = Token defining Maximux aggregation operation
-   = Token defining Minimum aggregation operation

Criteria

128 = Binary representation of Active column 
::  = Token defining the condition ' where the value is '
1   = Value to search 
&   = Token defining the conjunction ' and ' of the criteria attributes
    ( By default the library considers the disjunction 'OR' ) 
=   = Token defining equality as comparison of values and criteria 
    ( By default the library considers the similarity 'like sentence' )
+   = Token defining more than as comparison of values and criteria 
-   = Token defining less than as comparison of values and criteria 
=+   = Token defining more or equals than as comparison of values and criteria 
=-   = Token defining less or equals than as comparison of values and criteria 

*/

---

My thanks to my wonderful Wife, my Puppy and God, the Creator of all, by understanding and care.

I hope help !
Enjoy ;-)

Reggards, Rocha Renato
