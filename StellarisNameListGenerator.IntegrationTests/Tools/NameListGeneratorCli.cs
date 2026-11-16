namespace StellarisNameListGenerator.IntegrationTests.Tools
{
    /// <summary>
    /// Invokes the generator command line entry point in-process, exactly as an external caller would.
    /// </summary>
    internal static class NameListGeneratorCli
    {
        internal static void Run(params string[] arguments)
            => StellarisNameListGenerator.Program.Main(arguments);
    }
}
