using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace FluentUri
{
    public class FluentUriBuilder : IFluentUriBuilder
    {
        private Uri uri;

        private StringBuilder path;

        private string scheme;

        private string host;

        private int port = 80;

        private string fragment;

        private IDictionary<string, string> segments = new Dictionary<string, string>();

        private StringBuilder query;
        
        protected FluentUriBuilder(Uri uri)
        {
            Contract.Requires(uri.Port > 0);

            this.uri = uri;
        
            this.port = uri.Port;
            this.scheme = uri.Scheme;
            this.host = uri.DnsSafeHost;
        
            this.path = new StringBuilder(uri.LocalPath); // LocalPath doesnt http encode it - nice
            this.fragment = uri.Fragment.Replace("#", string.Empty);
            this.query = new StringBuilder(string.IsNullOrEmpty(uri.Query) ? "?" : uri.Query);
        }

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(host != null);
            Contract.Invariant(port > 0);
        }

        /// <summary>
        /// Factory method to create new Fluent Uri Builder
        /// Use for {segment} for uri templates that can be replaced sing WithSegment
        /// </summary>
        /// <param name="uri"></param>
        /// <returns>a Fluent Uri Builder</returns>
        public static IFluentUriBuilder New(string uri)
        {
            Contract.Requires(!string.IsNullOrEmpty(uri));
            return new FluentUriBuilder(new Uri(uri));
        }

        public Uri Build()
        {
            var tmpPath = new StringBuilder(path.ToString());
            
            foreach (var segment in segments)
            {
                tmpPath = tmpPath.Replace("{" + segment.Key + "}", segment.Value); 
            }

            var builder = new UriBuilder(scheme, host, port, tmpPath.ToString(), query.ToString())
            {
              Fragment = fragment  
            };

            return builder.Uri;
        }

        public IFluentUriBuilder WithPath(string path)
        {
            this.path.Clear();
            this.path.Append(path);

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

        public IFluentUriBuilder WithSegment(string segmentIdentifier, string segmentValue)
        {
            if(segments.ContainsKey(segmentIdentifier))
            {
                segments[segmentIdentifier] = segmentValue;
            }
            else
            {
                segments.Add(segmentIdentifier, segmentValue);
            }

            return this;
        }

        public IFluentUriBuilder AddPathTemplate(string pathTemplate)
        {
            // do more validations on the format of the path template - regex
            AppendSlashToPathIfRequired(pathTemplate);

            path.Append(pathTemplate);

            return this;
        }

        /// <summary>
        /// Appends or replaces the uri fragment
        /// </summary>
        /// <param name="fragment">the frament to append (do not include #)</param>
        /// <returns>The Fluent UriBuilder instance</returns>
        public IFluentUriBuilder WithFragment(string fragment)
        {
            this.fragment = fragment;
            return this;
        }

        public IFluentUriBuilder AddQuery(string parameter, string value)
        {
            if (query[query.Length -1] != '?')
            {
                query.Append("&");
            }
            
            this.query.Append(string.Format("{0}={1}", parameter, value));
            
            return this;
        }

        public IFluentUriBuilder WithHost(string host)
        {
            this.host = host;
            return this;
        }

        private void AppendSlashToPathIfRequired(string pathTemplate)
        {
            if (path[path.Length - 1] != '/' && !pathTemplate[0].Equals('/'))
            {
                path.Append('/');
            }
        }
    }
}