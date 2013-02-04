#r @"C:\projetos\bsn\Fontes\resources\MongoDB.Driver.dll";
#r @"C:\projetos\bsn\Fontes\resources\MongoDB.Bson.dll";
#r @"C:\projetos\bsn\Fontes\core\bin\Debug\bsn.core.dll";
open bsn.core;

//let teste = MongoDB.Driver.MongoServer.Create("mongodb://localhost/?safe=true").GetDatabase("bsn").GetCollection<Site>("sites");;
//
//let query1 = query { for customer in db.Customers do
//                     select customer }


let resultado = bsn.core.TesteLinqPowershell();
let str = resultado.teste();
str;

//let teste = MongoDB.Driver.MongoServer.Create("mongodb://localhost/?safe=true").GetDatabase("bsn").GetCollection<Site>("sites");;