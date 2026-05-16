namespace EtAlii.Yagl.Tests;

public class GrammarTests
{
    [Fact]
    public void Parse_Yagl_Simple()
    {
        // Arrange.

        // Act.
        var query = YaglParser.ParseFile(@"Examples\simple.yagl");

        // Assert.
        Assert.NotNull(query);
    }

    [Fact]
    public void Parse_Yagl_Projects()
    {
        // Arrange.

        // Act.
        var query = YaglParser.ParseFile(@"Examples\projects.yagl");

        // Assert.
        Assert.NotNull(query);
    }

    [Fact]
    public void Parse_Yagl_Structure()
    {
        // Arrange.

        // Act.
        var query = YaglParser.ParseFile(@"Examples\structure.yagl");

        // Assert.
        Assert.NotNull(query);
    }

    [Fact]
    public void Parse_Wildcards_Yagl()
    {
        // Arrange.

        // Act.
        var query = YaglParser.ParseFile(@"Examples\wildcards.yagl");

        // Assert.
        Assert.NotNull(query);
    }

    [Theory, MemberData(nameof(Parse_Yagl_All_Data))]
    public void Parse_Yagl_All(string fileName)
    {
        // Arrange.

        // Act.
        var query = YaglParser.ParseFile(fileName);

        // Assert.
        Assert.NotNull(query);
    }

    public static IEnumerable<object[]> Parse_Yagl_All_Data()
    {
        return Directory
            .EnumerateFiles(@"Examples", "*.yagl")
            .Select(f => new object[] { f });
    }
}
