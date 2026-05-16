using Antlr4.Runtime;

namespace EtAlii.Yagl.Tests;

public class GrammarTests
{
    private void Parse(string fileName)
    {
        var content = File.ReadAllText(fileName);
        var inputStream = new AntlrInputStream(content);
        var lexer = new yaglLexer(inputStream);
        var commonTokenStream = new CommonTokenStream(lexer);
        var parser = new yaglParser(commonTokenStream);

        // Add an error listener to fail the test on syntax errors
        parser.RemoveErrorListeners();
        parser.AddErrorListener(new ThrowingErrorListener());

        var context = parser.query();
    }

    [Fact]
    public void Parse_Simple_Yagl()
    {
        Parse(@"Examples\simple.yagl");
    }

    [Fact]
    public void Parse_Projects_Yagl()
    {
        Parse(@"Examples\projects.yagl");
    }

    [Fact]
    public void Parse_Structure_Yagl()
    {
        Parse(@"Examples\structure.yagl");
    }

    [Fact]
    public void Parse_Wildcards_Yagl()
    {
        Parse(@"Examples\wildcards.yagl");
    }
}
