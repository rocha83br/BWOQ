Hi Contributors,

The initial idea was to create something similar to Dynamic LINQ
using a different strategy to Reflection.Emit ,
based on class CSharpCodeProvider / CompileAssemblyFromSource ,
executing methods containing LINQ queries generated at runtime
as a contribution to this solution based on logical comparison .

As our notorious colleagues on Microsoft ever thought this idea,
we extend ourselves from it !

BWOQ (BitWise Object Query)
is a suggestion shorter code when performing searches on objects ,
SQL database , XML files , text files , etc.
analogous to the proposal of John Resig with JQuery in 2006 .

Think any attribute of an object can be indexed within a binary table
defining binary values at runtime to each according to his ordinaridade,
We can make comparisons using logical disjunction recursively for
these attributes, both as predicates, such as the lambda search criteria.

This, together with the concept of postfix representation of the operations,
(Reverse Polish Notation Charles Hamblin in 1950).

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
			TokenId
			Password
		List of Address
			Id
			Street
			Number
			District
			City
			State
			ZipCode
			Lat
			Lng
		List of Contact
			Id
			TypeId : Enum of [TelPhone, Email, InstantMessenger, SocialNetwork]
			Name
			Description


I need to populate a visual interface with Id , Name and City attributes only
of active users, we generate the table [1,2,4,8,16,32,64,128], once we have 
the combination bound to represent the 3 columns for logical or [1,8] giving
decimal 9 that will be used in the query expression, eg :


/*

static void Main ( string [ ] args )
{

   var personList = new List<object>();
           
        personList.Add( new { Id = 1 , CreationDate = new DateTime (2013 , 12 , 25 ) , DocId = " 44762881-50 " , Name = " Carlos de Andrade " , Size = 1.8 , Height = 70 Genre = 1 , Address = " Loreto Avenue " , Active = true });
        personList.Add( new { Id = 2 , CreationDate = new DateTime ( 2014 , 01 , 26 ) , DocId = " 19333121 -X" , Name = " Laura Carvalho ," Size = 1.73 , Height = 62 , = 2 Genre , Address = " Alcapone Street " , Active = false});
        personList.Add( new { Id = 3 , CreationDate = new DateTime ( 2014 , 01 , 22 ) , DocId = " 16555132 -X" , Name = " John Augustus " Size = 1.90 , Height = 86 Genre = 1 , Address = " Sunshine Street " , Active = true });
        personList.Add( new { Id = 4 , CreationDate = new DateTime ( 2014 , 01 , 10 ) , DocId = " 28262165 -X" , Name = " Julia Santos " Size = 1.60 , Height = 55 , = Genre 2 , Address = " Matarazzo Street " , Active = true });
        personList.Add( new { Id = 5 , CreationDate = new DateTime ( 2014 , 01 , 10 ) , DocId = " 28262110 -X" , Name = " Light of the Saints," Size = 1.63 , Height = 57 Genre = 2 , Address = " Matarazzo Street " , Active = true });

        var = new bwq BitWiseQuery<object>(personList);

        var = objList bwq.Query("9>2:16").Where("128:1&=");
    }

*/


where the operator & indicates the use of the conjunction predicate ,
and = is equality between value parameter and informed
expression after the token : in the context of the criteria .

It is also observed using token > to navigation
within aggregated objects , then informing the ordinal
household and the combination of attributes also informed
after the token : in the context of the predicate .

Using this practice , combined with the use of dynamic typing
eliminate the need for models for data transport in specific scenarios, 
the famous value objects or DTO (including support for JSON).


/*

Syntax Ref.:
=============

Predicate
---------
 9  = Binary combination representing the selected attributes [Numeric]
 >  = Start Navigation in Tree of aggregate objects [Character]
 2  = Ordinal position of the aggregate [Numeric]
 :  = Token defining action ' get ' [Character]
 16 = Binary combination representing the selected attributes of the aggregate [Numeric]

Criteria
---------
128 = Binary representation of Active [Numeric] column
:   = Token defining the condition ' where the value is ' [Char]
1   = [ Alphanumeric ] Value to search
&   = Token defining the conjunction ' and ' in the predicate [Char]
      ( By default the library considers the disjunction 'OR' )
=   = Define equality as comparison of values and criteria [Char]
      ( By default the library considers the similarity 'like sentence'

*/


My thanks to my wonderful wife, my Puppy
and God, the Creator of all, by understanding and care.


I hope help, enjoy  ;-)


Reggards,
Renato Rocha

=========================================================================

Olá Contribuidores,


A idéia inicial era criar algo similar ao Dynamic LINQ, 
utilizando uma estratégia diferente a Reflection.Emit, 
baseado na classe CSharpCodeProvider / CompileAssemblyFromSource,
executando métodos contendo consultas LINQ geradas em tempo de execução
como aporte a esta solução baseada em comparação lógica.

Como nossos notórios colegas da Microsoft já pensaram nesta idéia, 
vamos nos estender a partir dela !

BWOQ (BitWise Object Query) ou em pt-BR (Consulta a objetos bit a bit),
é uma sugestão a códigos mais curtos quando realizando buscas em objetos,
base de dados SQL, arquivos XML, arquivos texto, etc, 
análoga a proposta de John Resig com o JQuery em 2006.

Cenário :

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
			TokenId
			Password
		List of Address
			Id
			Street
			Number
			District
			City
			State
			ZipCode
			Lat
			Lng
		List of Contact
			Id
			TypeId : Enum of [TelPhone, Email, InstantMessenger, SocialNetwork]
			Name
			Description

Eu preciso preencher uma interface visual com os atributos Id, Name e City apenas
dos usuários ativos, geramos a tabela [1,2,4,8,16, 32, 64, 128], consequentemente temos a 
combinação ligada para representar os 3 atributos por ou lógico [1,8] gerando 
o decimal 9 que será utilizado na expressão de consulta, exemplo :

Pense que qualquer atributo de um objeto pode ser indexado dentro de uma tabela binária, 
definindo valores binários em tempo de execução para cada um deles conforme sua ordinaridade, 
podemos realizar comparações utilizando disjunção lógica de forma recursiva para obter 
estes atributos, tanto como predicados, quanto como critérios nas pesquisas lambda.

Isto, unido ao conceito de representação pós-fixa das operações,
(Notação polonesa inversa de  Charles Hamblin em 1950).


/*

static void Main(string[] args)
{
	var personList = new List<object>();
           
        personList.Add(new { Id = 1, CreationDate = new DateTime(2013, 12, 25), DocId = "44.762.881-50", Name = "Carlos de Andrade", Size = 1.8, Height = 70, Genre = 1, Address = "Loreto Avenue", Active = true });
        personList.Add(new { Id = 2, CreationDate = new DateTime(2014, 01, 26), DocId = "19.333.121-X", Name = "Laura Carvalho", Size = 1.73, Height = 62, Genre = 2, Address = "Alcapone Street", Active = false });
        personList.Add(new { Id = 3, CreationDate = new DateTime(2014, 01, 22), DocId = "16.555.132-X", Name = "João Augusto", Size = 1.90, Height = 86, Genre = 1, Address = "Sunshine Street", Active = true });
        personList.Add(new { Id = 4, CreationDate = new DateTime(2014, 01, 10), DocId = "28.262.165-X", Name = "Julia dos Santos", Size = 1.60, Height = 55, Genre = 2, Address = "Matarazzo Street", Active = true });
        personList.Add(new { Id = 5, CreationDate = new DateTime(2014, 01, 10), DocId = "28.262.110-X", Name = "Clara dos Santos", Size = 1.63, Height = 57, Genre = 2, Address = "Matarazzo Street", Active = true });

        var bwq = new BitWiseQuery<object>(personList);

        var objList = bwq.Query("9>2:16").Where("128:1&=");
}

*/


onde o operador & indica o uso de conjunção dos predicados,
e = representa igualdade entre valor e parâmetro informados 
na expressão após o token  :   no contexto dos critérios.

Também é possível observar o uso do token > para navegação 
dentro de objetos agregados, informando em seguida o ordinal 
do agregado e a combinação de atributos também informada 
após o token  :   no contexto do predicado.

Com o uso desta prática, aliado a utilização de tipagem dinâmica 
estinguimos de vez a necessidade de modelos para transporte de dados em 
cenários específicos, os famosos value objects ou DTO (incluindo suporte a JSON)


/*

Ref. Sintaxe:
=============

Predicado
---------
9  = Combinação binária representando os atributos elegidos [Numérico]
>  = Início da navegação na árvore dos objetos agregados [Caracter] 
2  = Posição ordinal do agregado [Numérico]
:  = Token definindo a ação 'obtenha' [Caracter]
16 = Combinação binária representando os atributos elegidos do agregado [Numérico]

Critérios
---------
128 = Representação binária da coluna Active [Numérico]
:   = Token definindo a condição 'onde o valor for' [Caracter]
1   = Valor a procurar [Alfa-numérico]
&   = Token definindo a utilização da conjunção 'E' no predicado [Caracter]
      (Por padrão a biblioteca considera a disjunção 'Ou')
=   = Token definido a igualdade como comparação dos valores e critérios [Caracter]
      (Por padrão a biblioteca considera a semelhança 'sentença like'

*/


Agradeço a minha maravilhosa esposa, Filhote 
e Deus criador de todos, pela compreensão e cuidado.


Espero ajudar, aproveitem  ;-)


Recomendações,
Renato Rocha
