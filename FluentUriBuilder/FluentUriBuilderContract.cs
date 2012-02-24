using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Diagnostics.Contracts;


namespace FluentUriUtility
{
    internal abstract class FluentUriBuilderContract : IFluentUriBuilder
    {
        public Uri Build()
        {
            Contract.Ensures(Contract.Result<Uri>() != null);
            return null;
        }

        public IFluentUriBuilder WithPath(string path)
        {
            return null;
        }

        public IFluentUriBuilder WithScheme(string scheme)
        {
            return null;
        }

        public IFluentUriBuilder WithPort(int port)
        {
            Contract.Requires(port > -2);
            return null;
        }

        public IFluentUriBuilder WithSegment(string segmentIdentifier, string segmentValue)
        {
            return null;
        }

        public IFluentUriBuilder AddPathTemplate(string pathTemplate)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(pathTemplate), "pathTemplate parameter cannot be null or empty");
            Contract.Requires(pathTemplate.Length > 1);
            return null;
        }

        public IFluentUriBuilder WithFragment(string fragment)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(fragment));
            return null;
        }

        public IFluentUriBuilder AddQuery(string parameter, string value)
        {
            return null;
        }

        public IFluentUriBuilder WithHost(string host)
        {
            Contract.Requires(!string.IsNullOrEmpty(host));
            return null;
        }


        public IFluentUriBuilder AddQueries(string[] p)
        {
            throw new NotImplementedException();
        }
    }
}
