using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc.Testing;

namespace minimalApiStarter.Tests;

public class EndpointsTests
{
    private WebApplicationFactory<Program> _application;

    public EndpointsTests()
    {
        // Fixture
        _application = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            // ... Configure test services
        });
    }

    [Fact]
    public async Task WheatherForecastTestResponse()
    {
        // Arrange

        // Act
        var client = _application.CreateClient();
        var result = await client.GetFromJsonAsync<List<WeatherForecast>>("/weatherforecast");

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().HaveCount(5, "I know it should be 5");
        }
    }

    [Fact]
    public async Task TestParamEndpointTest()
    {
        // Arrange

        // Act
        var client = _application.CreateClient();
        var result = await client.GetStringAsync("/test/?param=123");

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().Contain("123");
        }
    }

    [Theory]
    [MemberData(nameof(LoginUserData))]
    public async Task AuthEndpointTest(LoginQuery data, bool shouldFail = true)
    {
        // Arrange

        // Act
        var client = _application.CreateClient();
        var response = await client.PostAsJsonAsync("/security/getToken",
            data);

        var result = await response.Content.ReadAsStringAsync();
        // Assert
        using (new AssertionScope())
        {
            if (shouldFail)
            {
                response.IsSuccessStatusCode.Should().BeFalse();
                response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
                result.Should().NotBeNull().And.NotBeEmpty().And.Contain("Incorrect");
            }
            else
            {
                response.IsSuccessStatusCode.Should().BeTrue();
                result.Should().NotBeNull().And.NotBeEmpty();
            }
        }
    }

    public static IEnumerable<object[]> LoginUserData =>
        new List<object[]>
        {
            new object[] { new LoginQuery("admin@test.com", "Pass"), false },
            new object[] { new LoginQuery("test", "test") },
            new object[] { new LoginQuery("test@test", "") }
        };
}