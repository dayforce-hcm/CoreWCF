using System;
using System.Configuration;

namespace CoreWCF.ConfigurationManager.Client
{
    public sealed class ServiceModelSectionGroup : ConfigurationSectionGroup
    {
        public ServiceModelSectionGroup()
        {
        }

        public BindingsSection Bindings
        {
            get { return (BindingsSection)Sections[ConfigurationStrings.BindingsSectionGroupName]; }
        }

        public ClientSection Client
        {
            get { return (ClientSection)Sections[ConfigurationStrings.ClientSectionName]; }
        }


        public static ServiceModelSectionGroup GetSectionGroup(System.Configuration.Configuration config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            return (ServiceModelSectionGroup)config.SectionGroups[ConfigurationStrings.SectionGroupName];
        }
    }
}
