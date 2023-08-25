namespace BuilderPattern;

public class PersonTests
{
    [Fact]
    public void TestPersonBuilder()
    {
        // Arrange
        var expectedFirstName = "John";
        var expectedLastName = "Doe";
        var expectedAge = 30;
        var expectedEmail = "john@example.com";

        // Act
        var person = new PersonBuilder()
            .WithFirstName(expectedFirstName)
            .WithLastName(expectedLastName)
            .WithAge(expectedAge)
            .WithEmail(expectedEmail)
            .Build();

        // Assert
        Assert.Equal(expectedFirstName, person.FirstName);
        Assert.Equal(expectedLastName, person.LastName);
        Assert.Equal(expectedAge, person.Age);
        Assert.Equal(expectedEmail, person.Email);
    }
}

