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
using NUnit.Framework;
using Pretzel.Logic.Extensions;

namespace SiteTests
{
    [TestFixture]
    public sealed class PostMetadataTests
    {
        // ---------------- Tests -----------------

        [Test]
        public void WpidIsUniqueTest()
        {
            // Setup
            var foundIds = new Dictionary<int, string>();

            // Act / Check
            PerformActionOnPost(
                ( postPath, metadata ) =>
                {
                    if( metadata.ContainsKey( "wpid" ) == false )
                    {
                        return;
                    }

                    bool parsed = int.TryParse( metadata["wpid"].ToString(), out int wpid );
                    Assert.IsTrue( parsed, $"{postPath} contains a non-int wpid" );

                    if( foundIds.ContainsKey( wpid ) )
                    {
                        Assert.Fail( $"{postPath} ID is a duplicate with {foundIds[wpid]}!" );
                    }

                    foundIds.Add( wpid, postPath );
                }
            );
        }

        // ---------------- Test Helpers -----------------

        private void PerformActionOnPost( Action<string, IDictionary<string, object>> postAction )
        {
            foreach( string postPath in Directory.GetFiles( TestContants.PostsDirectory ) )
            {
                string fileContent = File.ReadAllText( postPath );

                IDictionary<string, object> metadata = fileContent.YamlHeader();
                postAction( postPath, metadata );
            }
        }
    }
}
