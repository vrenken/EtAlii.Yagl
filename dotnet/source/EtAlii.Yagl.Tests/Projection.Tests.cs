namespace EtAlii.Yagl.Tests;

public class ProjectionTests
{
    [Fact]
    public void Projection_Simple_Yagl()
    {
        var program = YaglParser.ParseFile(@"Examples\simple.yagl");
        Assert.NotNull(program);
        Assert.NotEmpty(program.Elements);
        Assert.Equal(2, program.Elements.Count);
        Assert.Equal("User", program.Elements[0].Identifier);
        Assert.Equal("Project", program.Elements[1].Identifier);
    }

    [Fact]
    public void  Projection_Projects_Yagl()
    {
        var program = YaglParser.ParseFile(@"Examples\projects.yagl");
        Assert.NotNull(program);
        Assert.NotEmpty(program.Elements);
    }
}
