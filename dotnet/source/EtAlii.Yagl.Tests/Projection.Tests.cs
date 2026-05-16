using Antlr4.Runtime;

namespace EtAlii.Yagl.Tests;

public class ProjectionTests
{
    private Query Parse(string fileName)
    {
        var content = File.ReadAllText(fileName);
        var inputStream = new AntlrInputStream(content);
        var lexer = new yaglLexer(inputStream);
        var commonTokenStream = new CommonTokenStream(lexer);
        var parser = new yaglParser(commonTokenStream);

        parser.RemoveErrorListeners();
        parser.AddErrorListener(new ThrowingErrorListener());

        var context = parser.query();
        var visitor = new YaglVisitor();
        return (Query)visitor.Visit(context);
    }

    [Fact]
    public void Projection_Simple_Yagl()
    {
        var program = Parse(@"Examples\simple.yagl");
        Assert.NotNull(program);
        Assert.NotEmpty(program.Elements);
        Assert.Equal(2, program.Elements.Count);
        Assert.Equal("User", program.Elements[0].Identifier);
        Assert.Equal("Project", program.Elements[1].Identifier);
    }

    [Fact]
    public void  Projection_Projects_Yagl()
    {
        var program = Parse(@"Examples\projects.yagl");
        Assert.NotNull(program);
        Assert.NotEmpty(program.Elements);
    }
}
