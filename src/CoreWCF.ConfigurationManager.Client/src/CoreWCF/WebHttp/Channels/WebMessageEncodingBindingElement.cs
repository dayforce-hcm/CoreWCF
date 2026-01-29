// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.ServiceModel.Channels;
using System.Text;
using System.Xml;
using WebContentTypeMapper = CoreWCF.Channels.WebContentTypeMapper;

namespace CoreWCF.ConfigurationManager.Client
{
    public sealed class WebMessageEncodingBindingElement : MessageEncodingBindingElement//, IWsdlExportExtension
    {
        private int _maxReadPoolSize;
        private int _maxWritePoolSize;
        private Encoding _writeEncoding;

        ////TODO: review if this was a correct replacement for :this(TextEncoderDefaults.Encoding)
        public WebMessageEncodingBindingElement() : this(Encoding.Default)
        {
        }

        public WebMessageEncodingBindingElement(Encoding writeEncoding)
        {
            if (writeEncoding == null)
            {
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull(nameof(writeEncoding));
            }

            //TODO: commented out as it is not supported in System.ServiceModel
            //TextEncoderDefaults.ValidateEncoding(writeEncoding);
            _maxReadPoolSize = EncoderDefaults.MaxReadPoolSize;
            _maxWritePoolSize = EncoderDefaults.MaxWritePoolSize;
            ReaderQuotas = new XmlDictionaryReaderQuotas();
            //TODO: commented out as it is not supported in System.ServiceModel
            //EncoderDefaults.ReaderQuotas.CopyTo(ReaderQuotas);
            _writeEncoding = writeEncoding;
        }

        private WebMessageEncodingBindingElement(WebMessageEncodingBindingElement elementToBeCloned)
            : base(elementToBeCloned)
        {
            _maxReadPoolSize = elementToBeCloned._maxReadPoolSize;
            _maxWritePoolSize = elementToBeCloned._maxWritePoolSize;
            ReaderQuotas = new XmlDictionaryReaderQuotas();
            elementToBeCloned.ReaderQuotas.CopyTo(ReaderQuotas);
            _writeEncoding = elementToBeCloned._writeEncoding;
            ContentTypeMapper = elementToBeCloned.ContentTypeMapper;
            CrossDomainScriptAccessEnabled = elementToBeCloned.CrossDomainScriptAccessEnabled;
        }

        public WebContentTypeMapper ContentTypeMapper { get; set; }

        public int MaxReadPoolSize
        {
            get
            {
                return _maxReadPoolSize;
            }
            set
            {
                if (value <= 0)
                {
                    throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException(nameof(value), value,
                        SR.Format(SRCommon.ValueMustBePositive)));
                }

                _maxReadPoolSize = value;
            }
        }

        public int MaxWritePoolSize
        {
            get
            {
                return _maxWritePoolSize;
            }
            set
            {
                if (value <= 0)
                {
                    throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException(nameof(value), value,
                        SR.Format(SRCommon.ValueMustBePositive)));
                }

                _maxWritePoolSize = value;
            }
        }

        public override MessageVersion MessageVersion
        {
            get
            {
                return MessageVersion.None;
            }
            set
            {
                if (value == null)
                {
                    throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull(nameof(value));
                }

                if (value != MessageVersion.None)
                {
                    throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgument(nameof(value), SR.Format(SR.JsonOnlySupportsMessageVersionNone));
                }
            }
        }

        public XmlDictionaryReaderQuotas ReaderQuotas { get; }

        public Encoding WriteEncoding
        {
            get
            {
                return  _writeEncoding;
            }
            set
            {
                if (value == null)
                {
                    throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull(nameof(value));
                }

                ////TODO: commented out as it is not supported in System.ServiceModel
                //TextEncoderDefaults.ValidateEncoding(value);
                _writeEncoding = value;
            }
        }

        public bool CrossDomainScriptAccessEnabled
        {
            get;
            set;
        }

        public override BindingElement Clone()
        {
            return new WebMessageEncodingBindingElement(this);
        }

        
        public override MessageEncoderFactory CreateMessageEncoderFactory()
        {
            //TODO: commented out as it is not supported in System.ServiceModel
            //return new WebMessageEncoderFactory(WriteEncoding, MaxReadPoolSize, MaxWritePoolSize, ReaderQuotas, ContentTypeMapper, CrossDomainScriptAccessEnabled);
            throw new NotImplementedException();
        }

        public override T GetProperty<T>(BindingContext context)
        {
            if (context == null)
            {
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull(nameof(context));
            }
            if (typeof(T) == typeof(XmlDictionaryReaderQuotas))
            {
                return (T)(object)ReaderQuotas;
            }
            else
            {
                return base.GetProperty<T>(context);
            }
        }

        //void IWsdlExportExtension.ExportContract(WsdlExporter exporter, WsdlContractConversionContext context)
        //{
        //}

        //void IWsdlExportExtension.ExportEndpoint(WsdlExporter exporter, WsdlEndpointConversionContext context)
        //{
        //    if (context == null)
        //    {
        //        throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("context");
        //    }

        //    SoapHelper.SetSoapVersion(context, exporter, this.MessageVersion.Envelope);
        //}

        //protected  bool CheckEncodingVersion(EnvelopeVersion version) => MessageVersion.Envelope == version;

        //protected override bool IsMatch(BindingElement b) => false;
    }
}
