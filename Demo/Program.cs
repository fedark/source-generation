using Demo;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MongoExt.Interfaces;
using MongoExt.Impl;

var host = Host
    .CreateDefaultBuilder()
    .ConfigureServices(services =>
    {
        services.AddMemoryCache();
        services.AddSingleton<IMongoDbContext, MongoDbContext>(provider => new MongoDbContext("mongodb://localhost:27017/", "source-generation"));
        services.AddSingleton<IBookRepository, BookRepository>();
    })
    .Build();

var books = host.Services.GetRequiredService<IBookRepository>();

books.Add(new Book { Title = "Onegin" });
await books.CreateAsync(new Book { Title = "Idiot" });

var book = new Book { Title = "Animal Farm" };
books.Add(book);

Console.WriteLine((await books.GetAsync(book.Id!))?.Title);