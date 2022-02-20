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

namespace SitePlugin
{
    /// <summary>
    /// A specific BreadCrumb.
    /// </summary>
    public class BreadCrumb
    {
        // ---------------- Constructor ----------------

        public BreadCrumb()
        {
            this.Text = string.Empty;
            this.Url = string.Empty;
        }

        // ---------------- Properties ----------------

        /// <summary>
        /// The text to display in the webpage.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The URL to use on the breadcrumb.
        /// </summary>
        public string Url { get; set; }
    }
}
