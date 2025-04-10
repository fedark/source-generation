using System;
using Demo;

var collection = new BookMongoCollection();
collection.GetAllAsync();
var title = collection.GetTitle(new() { Id = "1", Title = "2" });

Console.WriteLine(title);
