namespace OrderGenerator.Domain.ValueObjects;

public class Symbol
{
    private static readonly string[] ValidSymbols = { "PETR4", "VALE3", "VIIA4" };

    public string Value { get; private init; }

    public static Symbol Create(string value)
    {
        ValidateSymbol(value);

        return new Symbol(value.ToUpper());
    }

    public static Symbol Reconstruct(string value)
    {
        return new Symbol(value);
    }

    private Symbol(string value)
    {
        Value = value;
    }

    private static void ValidateSymbol(string value)
    {
        if (!ValidSymbols.Contains(value.ToUpper()))
        {
            throw new ArgumentException($"Invalid symbol. Valid symbols are: {string.Join(", ", ValidSymbols)}", nameof(value));
        }
    }
}
