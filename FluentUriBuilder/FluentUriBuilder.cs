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

        private int port;

        private string fragment;

        private IDictionary<string, string> segments = new Dictionary<string, string>();

        private StringBuilder query;
        
        protected FluentUriBuilder(Uri uri)
        {
            this.uri = uri;
        
            this.port = uri.Port;
            this.scheme = uri.Scheme;
            this.host = uri.DnsSafeHost;
        
            this.path = new StringBuilder(uri.LocalPath); // LocalPath doesnt http encode it - nice
            this.fragment = uri.Fragment.Replace("#", string.Empty);
            this.query = new StringBuilder(string.IsNullOrEmpty(uri.Query) ? "?" : uri.Query);
        }

        /// <summary>
        /// Factory method to create new Builder
        /// Use for {segment} for uri templates that can be replaced sing WithSegment
        /// </summary>
        /// <param name="uri"></param>
        /// <returns>a Fluent Uri Builder</returns>
        public static IFluentUriBuilder New(string uri)
        {
            return new FluentUriBuilder(new Uri(uri));
        }

        //public static IFluentUriBuilder New(string uri)
        //{
        //    return new FluentUriBuilder(new Uri(uri));
        //}

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
            Contract.Requires<ArgumentException>(string.IsNullOrEmpty(pathTemplate), "pathTemplate parameter cannot be null or empty");

            AppendSlashToPathIfRequired(pathTemplate);

            path.Append(pathTemplate);

            return this;
        }

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

        private void AppendSlashToPathIfRequired(string pathTemplate)
        {
            
            if (path[path.Length - 1] != '/' && !pathTemplate[0].Equals('/'))
            {
                path.Append('/');
            }
        }



        public IFluentUriBuilder WithHost(string host)
        {
            return this;
        }
    }
}