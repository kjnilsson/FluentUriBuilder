using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentUri
{
    public interface IFluentUriBuilder
    {
        Uri Build();

        IFluentUriBuilder WithPath(string path);

        IFluentUriBuilder WithScheme(string scheme);

        IFluentUriBuilder WithPort(int port);
    }

    public class FluentUriBuilder : IFluentUriBuilder
    {
        private string uri;

        private string path;

        private string scheme;

        private string host;

        private int port;

        protected FluentUriBuilder(string uri)
        {
            var u = new Uri(uri);
            this.port = u.Port;
            this.scheme = u.Scheme;
            this.host = u.Host;
            this.path = u.AbsolutePath;
        }

        public static IFluentUriBuilder New(string uri)
        {
            return new FluentUriBuilder(uri);
        }

        public Uri Build()
        {
            var builder = new UriBuilder(scheme, host, port, path)
                {
                };

            return builder.Uri;
        }

        public IFluentUriBuilder WithPath(string path)
        {
            this.path = path;
            return this;
        }

        public IFluentUriBuilder WithScheme(string scheme)
        {
            this.scheme = scheme;
            return this;
        }

        public IFluentUriBuilder WithPort(int port)
        {
            this.port = port;
            return this;
        }
    }
}