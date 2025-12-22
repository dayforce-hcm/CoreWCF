// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


namespace CoreWCF.ConfigurationManager.Client.Tests.IntegrationTests
{

    [System.ServiceModel.ServiceContract]
    public interface ISomeService
    {
        [System.ServiceModel.OperationContract]
        string GetValue(int count);
    }

    public class SomeService : ISomeService
    {
        public string GetValue(int count)
        {
            return count.ToString();
        }
    }
}
