using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using Xunit;

namespace TestWCFSuite.Framework
{
    public static class Utility
    {
#if DEBUG
        private const string CONFIGURATION = "Debug";
#else
    private const string CONFIGURATION = "Release";
#endif

        

        public static string FindSolutionDirectory(ITestOutputHelper? output = null)
        {
            var currentDir = AppDomain.CurrentDomain.BaseDirectory;
            output?.WriteLine($"Current directory: {currentDir}");

            while (currentDir != null)
            {
                var slnFiles = Directory.GetFiles(currentDir, "*.sln");
                if (slnFiles.Length > 0)
                {
                    output?.WriteLine($"Found solution directory: {currentDir}");
                    return currentDir;
                }

                currentDir = Directory.GetParent(currentDir)?.FullName;
            }

            throw new InvalidOperationException("Could not find solution directory");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectPath"></param>
        /// <param name="port"></param>
        /// <param name="output"></param>
        /// <returns>Returns process Id</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static int StartKestrel(string projectPath, string applicationUrls, ITestOutputHelper output)
        {
            if (!Directory.Exists(projectPath))
            {
                throw new InvalidOperationException($"Test project not found at: {projectPath}");
            }

            output.WriteLine($"Starting Kestrel on urls {applicationUrls} for {projectPath}");

            var p = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "dotnet",
                    Arguments = $"run --no-build --urls \"{applicationUrls}\" --project \"{projectPath}\" -c:{CONFIGURATION}",
                    WorkingDirectory = projectPath,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                }
            };

            p.OutputDataReceived += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(args.Data))
                {
                    output.WriteLine($"[Kestrel] [{applicationUrls}] {args.Data}");
                }
            };
            p.ErrorDataReceived += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(args.Data))
                {
                    output.WriteLine($"[Kestrel ERROR] [{applicationUrls}] {args.Data}");
                }
            };

            p.Start();
            p.BeginOutputReadLine();
            p.BeginErrorReadLine();
            return p.Id;
        }

        public static void StopKestrel(int processId, string projectPath, string applicationUrls, ITestOutputHelper output)
        {
            try
            {
                var process = Process.GetProcessById(processId);
                if (process != null)
                {
                    try
                    {
                        output.WriteLine($"Stopping Kestrel for: {projectPath} on urls {applicationUrls}");
                        process.Kill();
                        process.WaitForExit(5000);
                        output.WriteLine("Kestrel stopped");
                    }
                    catch (Exception ex)
                    {
                        output.WriteLine($"Error stopping Kestrel process: {ex.Message}");
                    }
                }
            }
            catch (ArgumentException)
            {
                output.WriteLine($"Kestrel process with id {processId} not found, may have already exited.");
            }
        }

        public static async Task<bool> WaitForServerReadyAsync(string healthCheckUrl, ITestOutputHelper output)
        {
            output.WriteLine($"Waiting for {healthCheckUrl} to be available");

            var maxAttempts = 15;
            var attempt = 0;

            using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(2) };

            ExceptionDispatchInfo? edi = null;
            string? errorContent = null;
            while (attempt < maxAttempts)
            {
                attempt++;
                try
                {
                    var response = await client.GetAsync(healthCheckUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        output.WriteLine($"Server ready after {attempt} attempts");
                        return true;
                    }
                    output.WriteLine($"Attempt {attempt} out of {maxAttempts} - server not ready - {response.StatusCode}");
                    var stream = await response.Content.ReadAsStreamAsync();
                    errorContent = await new StreamReader(stream).ReadToEndAsync();
                    edi = null;
                }
                catch (Exception exc)
                {
                    output.WriteLine($"Attempt {attempt} out of {maxAttempts} - server not ready - {exc.Message}");
                    edi = ExceptionDispatchInfo.Capture(exc);
                    errorContent = null;
                }

                await Task.Delay(1000);
            }

            if (errorContent != null)
            {
                output.WriteLine("Last error content from server:");
                output.WriteLine(errorContent);
            }
            else
            {
                edi?.Throw();
            }
            return false;
        }
    }
}
