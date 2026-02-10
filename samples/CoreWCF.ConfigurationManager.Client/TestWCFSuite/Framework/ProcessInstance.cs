using System;
using System.Linq;
using Xunit;

namespace TestWCFSuite.Framework
{
    internal class ProcessInstance : IDisposable
    {
        private readonly ProcessConfig m_config;
        private readonly ITestOutputHelper m_output;
        private int processId;

        internal ProcessInstance(ProcessConfig config, ITestOutputHelper output)
        {
            m_config = config;
            m_output = output;
            StartProcess();
        }

        public void Dispose()
        {
            StopProcess();
        }

        public bool IsRunning()
        {
            var serviceUrl = m_config.ApplicationUrls.Split(';').Last() + m_config.LaunchUrl;
            var result = Utility.WaitForServerReadyAsync(serviceUrl, m_output).GetAwaiter().GetResult();
            return result;
        }

        private void StartProcess()
        {
            if (m_config.IsNetCore)
            {
                processId = Utility.StartKestrel(m_config.ProjectPath, m_config.ApplicationUrls, m_output);
            }
        }

        private void StopProcess()
        {
            Utility.StopKestrel(processId, m_config.ProjectPath, m_config.ApplicationUrls, m_output);
        }
    }
}
