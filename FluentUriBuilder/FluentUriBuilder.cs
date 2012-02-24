using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace FluentUriUtility
{
    public sealed class FluentUriBuilder// : IFluentUriBuilder
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
            this.uri = uri;
        
            this.port = uri.Port;
            this.scheme = uri.Scheme;
            this.host = uri.DnsSafeHost;
        
            this.path = new StringBuilder(uri.LocalPath); // LocalPath doesnt http encode it - nice
            this.fragment = uri.Fragment.Replace("#", string.Empty);
            this.query = new StringBuilder(string.IsNullOrEmpty(uri.Query) ? "?" : uri.Query);
        }

        /// <summary>
        /// Factory method to create new Fluent Uri Builder
        /// Use for {segment} for uri templates that can be replaced sing WithSegment
        /// </summary>
        /// <param name="uri"></param>
        /// <returns>a Fluent Uri Builder</returns>
        public static FluentUriBuilder New(string uri)
        {
            return new FluentUriBuilder(new Uri(uri));
        }

        public static FluentUriBuilder New(Uri uri)
        {
            return new FluentUriBuilder(uri);
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

        public FluentUriBuilder WithPath(string path)
        {
            EnsureValidString(path, "path");

            this.path.Clear();
            this.path.Append(path);

            return this;
        }

        public FluentUriBuilder WithScheme(string scheme)
        {
            EnsureValidString(scheme, "scheme");

            this.scheme = scheme;

            return this;
        }

        public FluentUriBuilder WithPort(int port)
        {
            this.port = port;

            return this;
        }

        public FluentUriBuilder WithSegment(string segmentIdentifier, string segmentValue)
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

        public FluentUriBuilder AddPathTemplate(string pathTemplate)
        {
            EnsureValidString(pathTemplate, "pathTemplate");
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
        public FluentUriBuilder WithFragment(string fragment)
        {
            EnsureValidString(fragment, "fragment");

            this.fragment = fragment;
            return this;
        }

        public FluentUriBuilder AddQuery(string parameter, string value)
        {
            EnsureValidString(parameter, "parameter");
            EnsureValidString(value, "value");

            AddQuery(string.Format("{0}={1}", parameter, value));

            return this;
        }

        public FluentUriBuilder AddQueries(IEnumerable<KeyValuePair<string, string>> values)
        {
            foreach (var value in values)
            {
                AddQuery(value.Key, value.Value);
            }

            return this;
        }

        public FluentUriBuilder AddQueries(IEnumerable<string> queryParameters)
        {
            foreach (var query in queryParameters)
            {
                AddQuery(query);
            }

            return this;
        }

        private void AddQuery(string q)
        {
            if (query[query.Length - 1] != '?')
            {
                query.Append("&");
            }

            this.query.Append(q);
        }

        public FluentUriBuilder WithHost(string host)
        {
            EnsureValidString(host, "host");
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

        private static void EnsureValidString(string fragment, string field)
        {
            if (string.IsNullOrEmpty(fragment))
            {
                throw new ArgumentException("{0} is null or empty!", field);
            }
        }
    }
}