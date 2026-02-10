namespace TestWCFSuite.Framework
{
    internal class ProcessConfig
    {
        internal required bool IsNetCore { get; init; }
        internal required string ProjectPath { get; init; }
        internal required string ApplicationUrls { get; init; }
        internal required string LaunchUrl { get; init; }
    }
}
