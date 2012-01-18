using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FluentUri;

using Xunit;
using Xunit.Extensions;

namespace FluentUriBuilderTest
{
    public class FluentUriBuilderTest
    {
        [Theory]
        [InlineData("http://thehost")]
        [InlineData("http://thehost:9898")]
        [InlineData("http://thehost#frag")]
        [InlineData("http://thehost?que=val&que=val2")]
        [InlineData("http://thehost?que=val&que=val2#frag")]
        [InlineData("http://thehost:9898/thepath/path2")]
        [InlineData("http://thehost:9898/thepath/path2?que=val&que=val2")]
        [InlineData("http://thehost:9898/thepath/path2?que=val&que=val2#frag")]
        public void New_ShouldProduceSameUriThatWasGiven(string uri)
        {
            var result = FluentUriBuilder.New(uri).Build();
            Assert.Equal(new Uri(uri), result);
        }

        [Fact]
        public void New_ShouldBuildBasicUriFromStringInput()
        {
            var result = FluentUriBuilder.New("http://thehost").Build();

            Assert.Equal("thehost", result.Host);
            Assert.Equal("http", result.Scheme);
        }

        [Fact]
        public void New_ShouldBuildUriFromStringInputWithPath()
        {
            var result = FluentUriBuilder.New("http://thehost/thepath").Build();

            Assert.Equal("thehost", result.Host);
            Assert.Equal("http", result.Scheme);
            Assert.Equal("http://thehost/thepath", result.ToString());
        }

        [Fact]
        public void New_ShouldBuildUriFromStringInputWithPathAndQueryString()
        {
            var result = FluentUriBuilder.New("http://thehost/thepath?id=54").Build();

            Assert.Equal("thehost", result.Host);
            Assert.Equal("http", result.Scheme);
            Assert.Equal("http://thehost/thepath?id=54", result.ToString());
        }

        [Fact]
        public void New_ShouldBuildUriFromStringInputWithPathAndPortAndQueryString()
        {
            var result = FluentUriBuilder.New("http://thehost:8945/thepath?id=54").Build();

            Assert.Equal("thehost", result.Host);
            Assert.Equal("http", result.Scheme);
            Assert.Equal("http://thehost:8945/thepath?id=54", result.ToString());
        }

        [Fact]
        public void WithPath_ShouldAddPathToBasicUri()
        {
            var result = FluentUriBuilder.New("http://host")
                .WithPath("mypath/hello")
                .Build();

            Assert.Equal("/mypath/hello", result.AbsolutePath);
        }

        [Fact]
        public void WithPath_ShouldAddPathToUriWithPath()
        {
            var result = FluentUriBuilder.New("http://host")
                .WithPath("mypath/hello")
                .Build();

            Assert.Equal("/mypath/hello", result.AbsolutePath);
        }

        [Fact]
        public void WithScheme_ShouldChangeScheme()
        {
            var result = FluentUriBuilder.New("http://host")
                .WithScheme("test")
                .Build();

            Assert.Equal("test", result.Scheme);
        }

        [Fact]
        public void WithPort_ShouldSetPort()
        {
            var result = FluentUriBuilder.New("http://host/path")
                .WithPort(1234)
                .Build();

            Assert.Equal(1234, result.Port);
            Assert.Equal("http://host:1234/path", result.ToString());
        }

        [Fact]
        public void WithPathSegment_ShouldSetSegmentForTemplate()
        {
            var result = FluentUriBuilder.New("http://host:1234/{seg1}")
                .WithSegment("seg1", "segmentone")
                .Build();

            Assert.Equal("http://host:1234/segmentone", result.ToString());
        }

        [Fact]
        public void WithPathSegment_ShouldReplaceSegmentForTemplate()
        {
            var builder = FluentUriBuilder.New("http://host:1234/{seg1}")
                .WithSegment("seg1", "segmentone");
            var result = builder.Build();

            Assert.Equal("http://host:1234/segmentone", result.ToString());

            result = builder.WithSegment("seg1", "segmentnew").Build();

            Assert.Equal("http://host:1234/segmentnew", result.ToString());
        }

        [Fact]
        public void WithPathTemplate_ShouldAddTemplateToPath()
        {
            var result = FluentUriBuilder.New("http://host:1234/{seg1}")
                .WithSegment("seg1", "segone")
                .AddPathTemplate("{newseg}/someseg")
                .WithSegment("newseg", "test")
                .Build();

            Assert.Equal("http://host:1234/segone/test/someseg", result.ToString()); 
        }

        [Fact]
        public void WithFragment_ShouldSetFragment()
        {
            var result = FluentUriBuilder.New("http://host:1234/test")
                .WithFragment("frag")
                .Build().ToString();

            Assert.Equal("http://host:1234/test#frag", result);
        }

        [Fact]
        public void WithFragment_ShouldSetPreExistingFragment()
        {
            var result = FluentUriBuilder.New("http://host:1234/test#frag")
                .Build().ToString();

            Assert.Equal("http://host:1234/test#frag", result);
        }

        [Fact]
        public void AddQuery_ShouldAddQueryParameter()
        {
            var result = FluentUriBuilder.New("http://host:1234/test")
                .AddQuery("myparam", "myvalue")
                .Build().ToString();

            Assert.Equal("http://host:1234/test?myparam=myvalue", result);
        }

        [Fact]
        public void AddQuery_ShouldAddMultipleQueryParameter()
        {
            var result = FluentUriBuilder.New("http://host:1234/test")
                .AddQuery("myparam", "myvalue")
                .AddQuery("myparam2", "myvalue2")
                .Build().ToString();

            Assert.Equal("http://host:1234/test?myparam=myvalue&myparam2=myvalue2", result);
        }

        [Fact]
        public void AddPathTemplate_ShouldHandleSlashesCorrectlyWhenTemplateBeginsWithSlash()
        {
            var result = FluentUriBuilder.New("http://host:1234/test")
                .AddPathTemplate("/{test}/whatever/{test2}")
                .WithSegment("test", "karl")
                .WithSegment("test2", "karl2")
                .Build();

            Assert.Equal(new Uri("http://host:1234/test/karl/whatever/karl2"), result);
        }

        [Fact]
        public void Explore()
        {
            var result = FluentUriBuilder.New("http://host:1234/test")
                .AddPathTemplate("/{test}/whatever/{test2}")
                .WithSegment("test", "karl")
                .WithSegment("test2", "karl2")
                .Build();

            Assert.Equal(new Uri("http://host:1234/test/karl/whatever/karl2"), result);
        }

        [Fact]
        public void WithHost_ShouldUpdateHost()
        {
            var result = FluentUriBuilder.New("http://host:1234/test")
                .WithHost("newhost").Build();

            Assert.Equal(new Uri("http://newhost:1234/test"), result); 
        }

        [Fact]
        public void AddPathTemplate_ShouldThrowIfGivenEmptyString()
        {
            Assert.Throws<ArgumentException>(() => FluentUriBuilder.New("http://host:1234/test")
                .AddPathTemplate(string.Empty));
        }

        [Fact]
        public void WithFragment_ShouldThrowIfGivenEmptyString()
        {
            Assert.Throws<ArgumentException>(() => FluentUriBuilder.New("http://host:1234/test")
                .WithFragment(string.Empty));
        }

        [Fact]
        public void Host_CannotPassNullAsUriString()
        {
            try
            {
                FluentUriBuilder.New("http://test").WithHost(null);
            }
            catch (Exception e)
            {
                Assert.Equal("ContractException", e.GetType().Name);
            }
        }
    }
}
