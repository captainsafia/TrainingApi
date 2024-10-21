using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using TrainingApi.Shared;
using TrainingApi;
using Moq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

public class UnitTests
{
    [Fact]
    public void CreateClientReturnsCorrectResponse()
    {
        // Arrange
        var clientToCreate = new Client(4, "Gretchen", "Beslier", "gbeslier0@nba.com", 311, 65, DateTime.Parse("7/22/1984", CultureInfo.InvariantCulture));
        var mockContext = CreateMockDbContext();
        var service = new TrainingService(mockContext.Object);

        // Act
        var result = service.CreateClient(clientToCreate);

        // Assert
        var typedResult = Assert.IsType<Created<Client>>(result.Result);
        Assert.Equal(StatusCodes.Status201Created, typedResult.StatusCode);
        Assert.Equal(clientToCreate, typedResult.Value);
    }

    private static Mock<TrainingDb> CreateMockDbContext()
    {
        var clients = new List<Client>
        {
            new Client(1, "Vonnie", "Mawer", "vmawer0@go.com", 149, 66, DateTime.Parse("4/24/2000", CultureInfo.InvariantCulture)),
            new Client(2, "Langston", "Feldberg", "lfeldberg1@hc360.com", 329, 73, DateTime.Parse("10/20/1982", CultureInfo.InvariantCulture)),
            new Client(3, "Olwen", "Maeer", "omaeer3@purevolume.com", 261, 70, DateTime.Parse("8/22/1993", CultureInfo.InvariantCulture)) 
        }.AsQueryable();
        var trainers = new List<Trainer>
        {
            new Trainer(1, "Inna", "Spedroni", "ispedroni0@studiopress.com", Level.Junior, true),
            new Trainer(2, "Nikoletta", "Orrell", "norrell1@nydailynews.com", Level.Senior, true),
            new Trainer(3, "Briana", "Diprose", "bdiprose0@t.co", Level.Senior, true),
            new Trainer(4, "Zerk", "Riepl", "svanshin5@google.com", Level.Elite, true)
        }.AsQueryable();
        var mockClientSet = CreateMockDbSet<Client>(clients);
        var mockTrainerSet = CreateMockDbSet<Trainer>(trainers);

        var mockContext = new Mock<TrainingDb>();
        mockContext.Setup(m => m.Clients).Returns(mockClientSet.Object);
        mockContext.Setup(m => m.Trainers).Returns(mockTrainerSet.Object);

        return mockContext;
    }

    private static Mock<DbSet<T>> CreateMockDbSet<T>(IQueryable<T> data) where T: class
    {
        var mockSet = new Mock<DbSet<T>>();
        mockSet.As<IAsyncEnumerable<T>>()
            .Setup(m => m.GetAsyncEnumerator(new CancellationToken()))
            .Returns(new TestDbAsyncEnumerator<T>(data.GetEnumerator()));

        mockSet.As<IQueryable<T>>()
            .Setup(m => m.Provider)
            .Returns(new TestDbAsyncQueryProvider<T>(data.Provider));

        mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
        mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
        mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

        return mockSet;
    }
}