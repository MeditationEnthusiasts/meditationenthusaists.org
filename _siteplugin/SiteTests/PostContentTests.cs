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
using System.Text;
using System.Text.RegularExpressions;
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
            var foundIds = new Dictionary<int, string>();

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

        // ---------------- Test Helpers -----------------

        private void PerformActionOnPost( Action<string, string> postAction )
        {
            foreach( string postPath in Directory.GetFiles( TestContants.PostsDirectory ) )
            {
                string fileContent = File.ReadAllText( postPath );

                postAction( Path.GetFileName( postPath ), fileContent.ExcludeHeader() );
            }
        }
    }
}
