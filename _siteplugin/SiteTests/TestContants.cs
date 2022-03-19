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
using System.IO;
using NUnit.Framework;

namespace SiteTests
{
    internal static class TestContants
    {
        // ---------------- Fields ----------------

        public static readonly string RepoRoot;

        public static readonly string PostsDirectory;

        public static readonly string SiteOutput;

        public static readonly ushort Port;

        // ---------------- Constructor ----------------

        static TestContants()
        {
            string assemblyDir = TestContext.CurrentContext.TestDirectory;
            RepoRoot = Path.Combine(
                assemblyDir,
                "..",
                "..",
                "..",
                "..",
                ".."
            );

            PostsDirectory = Path.Combine( RepoRoot, "_posts" );
            SiteOutput = Path.Combine( RepoRoot, "_site" );

            string testPortStr = Environment.GetEnvironmentVariable( "TEST_PORT" ) ?? string.Empty;
            if( ushort.TryParse( testPortStr, out Port ) == false )
            {
                Port = 8080;
            }
        }
    }
}
