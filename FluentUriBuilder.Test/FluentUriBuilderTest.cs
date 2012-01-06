using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FluentUri;

using Xunit;

namespace FluentUriBuilderTest
{
    public class FluentUriBuilderTest
    {
        [Fact]
        public void TestNew_ShouldBuildBasicUriFromStringInput()
        {
            var result = FluentUriBuilder.New("http://thehost").Build();

            Assert.Equal("thehost", result.Host);
            Assert.Equal("http", result.Scheme);
        }

        [Fact]
        public void TestNew_ShouldBuildUriFromStringInputWithPath()
        {
            var result = FluentUriBuilder.New("http://thehost/thepath").Build();

            Assert.Equal("thehost", result.Host);
            Assert.Equal("http", result.Scheme);
            Assert.Equal("http://thehost/thepath", result.ToString());
        }

        [Fact]
        public void TestNew_ShouldBuildUriFromStringInputWithPathAndQueryString()
        {
            var result = FluentUriBuilder.New("http://thehost/thepath?id=54").Build();

            Assert.Equal("thehost", result.Host);
            Assert.Equal("http", result.Scheme);
            Assert.Equal("http://thehost/thepath", result.ToString());
        }

        [Fact]
        public void TestNew_ShouldBuildUriFromStringInputWithPathAndPortAndQueryString()
        {
            var result = FluentUriBuilder.New("http://thehost:8945/thepath?id=54").Build();

            Assert.Equal("thehost", result.Host);
            Assert.Equal("http", result.Scheme);
            Assert.Equal("http://thehost:8945/thepath", result.ToString());
        }

        [Fact]
        public void TestWithPath_ShouldAddPathToBasicUri()
        {
            var result = FluentUriBuilder.New("http://host")
                .WithPath("mypath/hello")
                .Build();

            Assert.Equal("/mypath/hello", result.AbsolutePath);
        }

        [Fact]
        public void TestWithPath_ShouldAddPathToUriWithPath()
        {
            var result = FluentUriBuilder.New("http://host")
                .WithPath("mypath/hello")
                .Build();

            Assert.Equal("/mypath/hello", result.AbsolutePath);
        }

        [Fact]
        public void TestWithScheme_ShouldChangeScheme()
        {
            var result = FluentUriBuilder.New("http://host")
                .WithScheme("test")
                .Build();

            Assert.Equal("test", result.Scheme);
        }

        [Fact]
        public void TestWithPort_ShouldSetPort()
        {
            var result = FluentUriBuilder.New("http://host/path")
                .WithPort(1234)
                .Build();

            Assert.Equal(1234, result.Port);
            Assert.Equal("http://host:1234/path", result.ToString());
        }
    }
}
