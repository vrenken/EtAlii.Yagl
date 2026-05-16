using Antlr4.Runtime;

namespace EtAlii.Yagl;

public static class YaglParser
{
    public static Query ParseFile(string fileName)
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
        if(context == null)
        {
            throw new InvalidOperationException($"Failed to parse {fileName}");
        }
        var visitor = new YaglVisitor();
        return (Query)visitor.Visit(context);
    }
}
