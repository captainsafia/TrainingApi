using System.Net;
using System.Net.Http.Json;
using TrainingApi.Shared;
using TrainingApi;

namespace TrainingApi.Tests;

public class IntegrationTests
{
    [Fact]
    public async Task GET_Client_ReturnsClient()
    {
        // Arrange
        var app = new ApiApplication();

        // Act
        var client = app.CreateClient();
        var response = await client.GetAsync("/clients/1");
        var responseBody = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}