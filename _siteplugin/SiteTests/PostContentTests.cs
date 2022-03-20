//
// SiteTests - Site Tests for meditationenthusiasts.org
// Copyright (C) 2022 Meditation Enthusiasts.
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published
// by the Free Software Foundation, either version 3 of the License, or
// any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
// 
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CommonMark;
using HtmlAgilityPack;
using NUnit.Framework;
using Pretzel.Logic.Extensions;

namespace SiteTests
{
    [TestFixture]
    public sealed class PostContentTests
    {
        // ---------------- Tests -----------------

        [Test]
        public void MoreExistsTest()
        {
            // Setup
            var moreRegex = new Regex(
                @"[\r\n]+\<!--more--\>[\r\n]+",
                RegexOptions.Compiled | RegexOptions.ExplicitCapture
            );

            // Act / Check
            PerformActionOnPost(
                ( postPath, postContents ) =>
                {
                    if( moreRegex.IsMatch( postContents ) == false )
                    {
                        Assert.Fail( $"{postPath} does not contain a more comment!" );
                    }
                }
            );
        }

        [Test]
        public void SiteLinksDont404Test()
        {
            // Setup
            Assert.IsTrue( Directory.Exists( TestContants.SiteOutput ), "Site must be created before running this test." );

            // Act / Check
            PerformActionOnPost(
                ( postPath, postContents ) =>
                {
                    var aNodes = new List<HtmlNode>();
                    var imgNodes = new List<HtmlNode>();

                    string html = CommonMarkConverter.Convert( postContents );

                    var htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml( html );

                    var htmlNodeStack = new Stack<HtmlNode>();
                    htmlNodeStack.Push( htmlDoc.DocumentNode );

                    while( htmlNodeStack.Count != 0 )
                    {
                        HtmlNode currentNode = htmlNodeStack.Pop();
                        foreach( HtmlNode childNode in currentNode.ChildNodes )
                        {
                            htmlNodeStack.Push( childNode );
                        }

                        if( currentNode.Name == "a" )
                        {
                            foreach( HtmlAttribute attribute in currentNode.Attributes )
                            {
                                if( attribute.Name == "href" )
                                {
                                    string attributeValue = attribute.Value;
                                    if( attribute.Value.StartsWith( '/' ) == false )
                                    {
                                        // Its an external URL we have no control over.
                                        continue;
                                    }

                                    attributeValue = attributeValue.TrimStart( '/' );
                                    attributeValue = attributeValue.Replace( '/', Path.DirectorySeparatorChar );

                                    string expectedPath = Path.Combine( TestContants.SiteOutput, attributeValue );
                                    if( expectedPath.EndsWith( ".png" ) || expectedPath.EndsWith( ".jpg" ) || expectedPath.EndsWith( ".gif" ) )
                                    {
                                        // Do nothing.
                                    }
                                    else if( expectedPath.EndsWith( ".html" ) == false )
                                    {
                                        expectedPath = Path.Combine( expectedPath, "index.html" );
                                    }

                                    Assert.IsTrue(
                                        File.Exists( expectedPath ),
                                        $"{postPath} contains a URL that does not map to a file that exists: {expectedPath}"
                                    );
                                }
                            }
                        }
                        else if( currentNode.Name == "img" )
                        {
                            foreach( HtmlAttribute attribute in currentNode.Attributes )
                            {
                                if( attribute.Name == "src" )
                                {
                                    string attributeValue = attribute.Value;
                                    if( attribute.Value.StartsWith( '/' ) == false )
                                    {
                                        // Its an external URL we have no control over.
                                        continue;
                                    }
                                    attributeValue = attributeValue.TrimStart( '/' );
                                    attributeValue = attributeValue.Replace( '/', Path.DirectorySeparatorChar );

                                    string expectedPath = Path.Combine( TestContants.SiteOutput, attributeValue );

                                    Assert.IsTrue(
                                        File.Exists( expectedPath ),
                                        $"{postPath} contains a image that does not map to a file that exists: {expectedPath}"
                                    );
                                }
                            }
                        }
                    }
                }
            );
        }

        // ---------------- Test Helpers -----------------

        private void PerformActionOnPost( Action<string, string> postAction )
        {
            Parallel.ForEach(
                Directory.GetFiles( TestContants.PostsDirectory ),
                ( postPath ) =>
                {
                    string fileContent = File.ReadAllText( postPath );

                    postAction( Path.GetFileName( postPath ), fileContent.ExcludeHeader() );
                }
            );
        }
    }
}
