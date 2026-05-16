using Antlr4.Runtime;

namespace EtAlii.Yagl.Tests;

public class ThrowingErrorListener : BaseErrorListener
{
    public override void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
    {
        throw new Exception($"Syntax error at line {line}:{charPositionInLine} - {msg}");
    }
}