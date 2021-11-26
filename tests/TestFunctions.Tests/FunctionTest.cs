using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Xunit;
using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;
using AWS.Lambda.Pipeline.APIGatewayProxy.Builders;
using TestFunctions;
using TestFunctions.Commands.CreateComponent;
using TestFunctions.Commands.UpdateComponent;
using System.Text.Json;
using FluentAssertions;
using TestFunctions.Models;

namespace TestFunctions.Tests
{
    public class FunctionTest
    {
        [Fact]
        public async Task WarmFunction_WhenLambdaWarmerTriggered_Should_ReturnNoContent()
        {
            // Arrange
            var function = new Function();
            var context = new TestLambdaContext();
            var request = new ApiGatewayProxyRequestBuilder()
                .WithBody(string.Empty)
                .Build();

            // Act
            var response = await function.CreateComponent(request, context);

            // Assert
            response.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task GivenValidGetComponentsQuery_WhenGetComponentsCalled_Should_ReturnListOfComponents()
        {
            // Arrange
            IList<Component> expected = new List<Component>
            {
                new Component { Id = 1, Name = "test.name.1", Description = "test.description.1" },
                new Component { Id = 2, Name = "test.name.2", Description = "test.description.2" },
                new Component { Id = 3, Name = "test.name.3", Description = "test.description.3" },
            };

            var function = new Function();
            var context = new TestLambdaContext();
            var ids = new List<int> { 1, 2, 3 };

            // Act
            var response = await function.GetComponents(ids, context);

            // Assert
            Assert.Equal(expected.Count, response.Count);
            response.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GivenValidCreateComponentCommand_WhenCreateComponentCalled_Should_ReturnOk()
        {
            // Arrange
            var function = new Function();
            var context = new TestLambdaContext();
            var request = new ApiGatewayProxyRequestBuilder()
                .WithBody(new CreateComponentCommand { Name = "test.name", Description = "test.description" })
                .Build();
            request.HttpMethod = "POST";

            // Act
            var response = await function.CreateComponent(request, context);

            var createdComponent = JsonSerializer.Deserialize<Component>(response.Body);

            // Assert
            response.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task GivenNotValidCreateComponentCommand_WhenCreateComponentCalled_Should_ReturnBadRequest()
        {
            // Arrange
            var function = new Function();
            var context = new TestLambdaContext();
            var request = new ApiGatewayProxyRequestBuilder()
                .WithBody(new CreateComponentCommand { Name = string.Empty, Description = "test.description" })
                .Build();
            request.HttpMethod = "POST";

            // Act
            var response = await function.CreateComponent(request, context);

            // Assert
            response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GivenValidUpdateComponentCommand_WhenUpdateComponentCalled_Should_ReturnOk()
        {
            // Arrange
            var function = new Function();
            var context = new TestLambdaContext();
            var request = new ApiGatewayProxyRequestBuilder()
                .WithBody(new UpdateComponentCommand { Name = "test.name", Description = "test.description" })
                .WithPathParameter("id", "11")
                .Build();
            request.HttpMethod = "POST";

            // Act
            var response = await function.UpdateComponent(request, context);

            var updatedComponent = JsonSerializer.Deserialize<Component>(response.Body);

            // Assert
            response.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task GivenNotValidUpdateComponentCommand_WhenUpdateComponentCalled_Should_ReturnBadRequest()
        {
            // Arrange
            var function = new Function();
            var context = new TestLambdaContext();
            var request = new ApiGatewayProxyRequestBuilder()
                .WithBody(new UpdateComponentCommand { Name = string.Empty, Description = "test.description" })
                .WithPathParameter("id", "0")
                .Build();
            request.HttpMethod = "POST";

            // Act
            var response = await function.UpdateComponent(request, context);

            // Assert
            response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}
