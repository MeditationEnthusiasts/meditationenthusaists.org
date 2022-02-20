//
// SitePlugin - Pretzel Plugin for meditationenthusiasts.org
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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;

namespace SitePlugin
{
    public static class BreadCrumbGenerator
    {
        /// <summary>
        /// Generates Breadcrumbs, with index 0 being
        /// the one closest to the "root".
        /// </summary>
        public static IList<BreadCrumb> GenerateBreadCrumbs( string pageUrl )
        {
            var crumbs = new List<BreadCrumb>();

            List<string> splitString = pageUrl.Split( '/', StringSplitOptions.RemoveEmptyEntries ).ToList();
            if( splitString.Count <= 0 )
            {
                return crumbs;
            }

            // Don't want "index.html" appearing in the breadcrumb list.
            if( splitString.Last().EndsWith( ".html" ) )
            {
                splitString.RemoveAt( splitString.Count - 1 );
            }

            if( splitString.Count <= 0 )
            {
                return crumbs;
            }

            // If we are a post, each bread-crumb is a category.
            bool isPost = splitString[0].Equals( "posts" );

            TextInfo ti = CultureInfo.CurrentCulture.TextInfo;

            var currentUrl = new StringBuilder();
            currentUrl.Append( "/" );

            for( int i = 0; i < splitString.Count; ++i )
            {
                string str = splitString[i];

                // Replace any dashes in the URL with a space.
                string text = str.Replace( '-', ' ' );
                text = HttpUtility.UrlDecode( text );
                text = ti.ToTitleCase( text );

                // If we are a post, then the first breadcrumb should go to /posts.
                // Each breadcrumb after it should be category/ and each URL after it.
                // The last breadcrumb is the post itself, so
                // it should point to the current URL.
                // If we are not a post, then the breadcrumbs are find as-is.
                string url;
                if( ( i == 0 ) && isPost )
                {
                    url = "/posts/";
                    currentUrl.Append( "category/" );
                }
                else if( ( i == splitString.Count - 1 ) && isPost )
                {
                    url = pageUrl;
                }
                else
                {
                    currentUrl.Append( $"{str}/" );
                    url = currentUrl.ToString();
                }

                var crumb = new BreadCrumb
                {
                    Text = text,
                    Url = url
                };
                crumbs.Add( crumb );
            }

            return crumbs;
        }
    }
}
