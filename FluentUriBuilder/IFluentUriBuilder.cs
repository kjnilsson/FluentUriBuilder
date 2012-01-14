using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace FluentUri
{
    public interface IFluentUriBuilder
    {
        Uri Build();

        IFluentUriBuilder WithPath(string path);

        IFluentUriBuilder WithScheme(string scheme);

        IFluentUriBuilder WithPort(int port);

        IFluentUriBuilder WithSegment(string segmentIdentifier, string segmentValue);

        IFluentUriBuilder AddPathTemplate(string pathTemplate);

        IFluentUriBuilder WithFragment(string fragment);

        IFluentUriBuilder AddQuery(string parameter, string value);

        IFluentUriBuilder WithHost(string host);
    }
}
