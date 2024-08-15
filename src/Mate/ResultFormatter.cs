using System.Globalization;


namespace Mate;


public static class ResultFormatter
{
    public static string SingleResultToString(double result)
        => result.ToString(CultureInfo.InvariantCulture);
}