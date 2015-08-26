#region License

// Distributed under GNU General Public License v2.0
// 
// Copyright (C) 2015  Razim Saidov
// 
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License along
// with this program; if not, write to the Free Software Foundation, Inc.,
// 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.
// 

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using libspotify = Spoti.Library.libspotify;

namespace Spoti.Helper
{
    public static class Functions
    {
        private static readonly List<String> _countryCodes = new List<String>(new string[]
        {
            "–", "AP", "EU", "AD", "AE", "AF", "AG", "AI", "AL", "AM", "AN",
            "AO", "AQ", "AR", "AS", "AT", "AU", "AW", "AZ", "BA", "BB", "BD",
            "BE", "BF", "BG", "BH", "BI", "BJ", "BM", "BN", "BO", "BR", "BS",
            "BT", "BV", "BW", "BY", "BZ", "CA", "CC", "CD", "CF", "CG", "CH",
            "CI", "CK", "CL", "CM", "CN", "CO", "CR", "CU", "CV", "CX", "CY",
            "CZ", "DE", "DJ", "DK", "DM", "DO", "DZ", "EC", "EE", "EG", "EH",
            "ER", "ES", "ET", "FI", "FJ", "FK", "FM", "FO", "FR", "FX", "GA",
            "GB", "GD", "GE", "GF", "GH", "GI", "GL", "GM", "GN", "GP", "GQ",
            "GR", "GS", "GT", "GU", "GW", "GY", "HK", "HM", "HN", "HR", "HT",
            "HU", "ID", "IE", "IL", "IN", "IO", "IQ", "IR", "IS", "IT", "JM",
            "JO", "JP", "KE", "KG", "KH", "KI", "KM", "KN", "KP", "KR", "KW",
            "KY", "KZ", "LA", "LB", "LC", "LI", "LK", "LR", "LS", "LT", "LU",
            "LV", "LY", "MA", "MC", "MD", "MG", "MH", "MK", "ML", "MM", "MN",
            "MO", "MP", "MQ", "MR", "MS", "MT", "MU", "MV", "MW", "MX", "MY",
            "MZ", "NA", "NC", "NE", "NF", "NG", "NI", "NL", "NO", "NP", "NR",
            "NU", "NZ", "OM", "PA", "PE", "PF", "PG", "PH", "PK", "PL", "PM",
            "PN", "PR", "PS", "PT", "PW", "PY", "QA", "RE", "RO", "RU", "RW",
            "SA", "SB", "SC", "SD", "SE", "SG", "SH", "SI", "SJ", "SK", "SL",
            "SM", "SN", "SO", "SR", "ST", "SV", "SY", "SZ", "TC", "TD", "TF",
            "TG", "TH", "TJ", "TK", "TM", "TN", "TO", "TL", "TR", "TT", "TV",
            "TW", "TZ", "UA", "UG", "UM", "US", "UY", "UZ", "VA", "VC", "VE",
            "VG", "VI", "VN", "VU", "WF", "WS", "YE", "YT", "RS", "ZA", "ZM",
            "ME", "ZW", "A1", "A2", "O1", "AX", "GG", "IM", "JE", "BL", "MF"
        });

        private static readonly string[] s_countryNames =
        {
            "N/A", "Asia/Pacific Region", "Europe", "Andorra",
            "United Arab Emirates", "Afghanistan", "Antigua and Barbuda",
            "Anguilla", "Albania", "Armenia", "Netherlands Antilles", "Angola",
            "Antarctica", "Argentina", "American Samoa", "Austria", "Australia",
            "Aruba", "Azerbaijan", "Bosnia and Herzegovina", "Barbados",
            "Bangladesh", "Belgium", "Burkina Faso", "Bulgaria", "Bahrain",
            "Burundi", "Benin", "Bermuda", "Brunei Darussalam", "Bolivia",
            "Brazil", "Bahamas", "Bhutan", "Bouvet Island", "Botswana",
            "Belarus", "Belize", "Canada", "Cocos (Keeling) Islands",
            "Congo, The Democratic Republic of the", "Central African Republic",
            "Congo", "Switzerland", "Cote D’Ivoire", "Cook Islands", "Chile",
            "Cameroon", "China", "Colombia", "Costa Rica", "Cuba", "Cape Verde",
            "Christmas Island", "Cyprus", "Czech Republic", "Germany",
            "Djibouti", "Denmark", "Dominica", "Dominican Republic", "Algeria",
            "Ecuador", "Estonia", "Egypt", "Western Sahara", "Eritrea", "Spain",
            "Ethiopia", "Finland", "Fiji", "Falkland Islands (Malvinas)",
            "Micronesia, Federated States of", "Faroe Islands", "France",
            "France, Metropolitan", "Gabon", "United Kingdom", "Grenada",
            "Georgia", "French Guiana", "Ghana", "Gibraltar", "Greenland",
            "Gambia", "Guinea", "Guadeloupe", "Equatorial Guinea", "Greece",
            "South Georgia and the South Sandwich Islands", "Guatemala", "Guam",
            "Guinea-Bissau", "Guyana", "Hong Kong",
            "Heard Island and McDonald Islands", "Honduras", "Croatia", "Haiti",
            "Hungary", "Indonesia", "Ireland", "Israel", "India",
            "British Indian Ocean Territory", "Iraq",
            "Iran, Islamic Republic of", "Iceland", "Italy", "Jamaica", "Jordan"
            , "Japan", "Kenya", "Kyrgyzstan", "Cambodia", "Kiribati", "Comoros",
            "Saint Kitts and Nevis", "Korea, Democratic People’s Republic of",
            "Korea, Republic of", "Kuwait", "Cayman Islands", "Kazakstan",
            "Lao People’s Democratic Republic", "Lebanon", "Saint Lucia",
            "Liechtenstein", "Sri Lanka", "Liberia", "Lesotho", "Lithuania",
            "Luxembourg", "Latvia", "Libyan Arab Jamahiriya", "Morocco",
            "Monaco", "Moldova, Republic of", "Madagascar", "Marshall Islands",
            "Macedonia, the Former Yugoslav Republic of", "Mali", "Myanmar",
            "Mongolia", "Macau", "Northern Mariana Islands", "Martinique",
            "Mauritania", "Montserrat", "Malta", "Mauritius", "Maldives",
            "Malawi", "Mexico", "Malaysia", "Mozambique", "Namibia",
            "New Caledonia", "Niger", "Norfolk Island", "Nigeria", "Nicaragua",
            "Netherlands", "Norway", "Nepal", "Nauru", "Niue", "New Zealand",
            "Oman", "Panama", "Peru", "French Polynesia", "Papua New Guinea",
            "Philippines", "Pakistan", "Poland", "Saint Pierre and Miquelon",
            "Pitcairn", "Puerto Rico", "" + "Palestinian Territory, Occupied",
            "Portugal", "Palau", "Paraguay", "Qatar", "Reunion", "Romania",
            "Russian Federation", "Rwanda", "Saudi Arabia", "Solomon Islands",
            "Seychelles", "Sudan", "Sweden", "Singapore", "Saint Helena",
            "Slovenia", "Svalbard and Jan Mayen", "Slovakia", "Sierra Leone",
            "San Marino", "Senegal", "Somalia", "Suriname",
            "Sao Tome and Principe", "El Salvador", "Syrian Arab Republic",
            "Swaziland", "Turks and Caicos Islands", "Chad",
            "French Southern Territories", "Togo", "Thailand", "Tajikistan",
            "Tokelau", "Turkmenistan", "Tunisia", "Tonga", "Timor-Leste",
            "Turkey", "Trinidad and Tobago", "Tuvalu", "Taiwan",
            "Tanzania, United Republic of", "Ukraine", "Uganda",
            "United States Minor Outlying Islands", "the United States", "Uruguay",
            "Uzbekistan", "Holy See (Vatican City State)",
            "Saint Vincent and the Grenadines", "Venezuela",
            "Virgin Islands, British", "Virgin Islands, U.S.", "Vietnam",
            "Vanuatu", "Wallis and Futuna", "Samoa", "Yemen", "Mayotte",
            "Serbia", "South Africa", "Zambia", "Montenegro", "Zimbabwe",
            "Anonymous Proxy", "Satellite Provider", "Other", "Aland Islands",
            "Guernsey", "Isle of Man", "Jersey", "Saint Barthelemy",
            "Saint Martin"
        };

        internal static string PtrToString(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
                return String.Empty;

            System.Collections.Generic.List<byte> l = new System.Collections.Generic.List<byte>();
            byte read = 0;
            do
            {
                read = Marshal.ReadByte(ptr, l.Count);
                l.Add(read);
            } while (read != 0);

            if (l.Count > 0)
                return System.Text.Encoding.UTF8.GetString(l.ToArray(), 0, l.Count - 1);
            else
                return string.Empty;
        }

        public static string MakeValidFileName(string name)
        {
            string invalidChars =
                System.Text.RegularExpressions.Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars()));
            string invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);

            return System.Text.RegularExpressions.Regex.Replace(name, invalidRegStr, "_");
        }

        internal static IntPtr StringToPtr(string s)
        {
            if (s == null)
                return IntPtr.Zero;
            var buffer = Encoding.UTF8.GetBytes(s);
            IntPtr hStr = Marshal.AllocHGlobal(buffer.Length + 1);
            Marshal.Copy(buffer, 0, hStr, buffer.Length);
            Marshal.WriteByte(hStr, buffer.Length, 0);
            return hStr;
        }

        internal static IntPtr StringToLinkPtr(string link)
        {
            IntPtr hStr = IntPtr.Zero;
            try
            {
                hStr = StringToPtr(link);
                IntPtr linkPtr = libspotify.sp_link_create_from_string(hStr);
                return linkPtr;
            }
            finally
            {
                if (hStr != IntPtr.Zero)
                    Marshal.FreeHGlobal(hStr);
            }
        }

        internal static string LinkPtrToString(IntPtr linkPtr)
        {
            byte[] buffer = new byte[128];
            IntPtr bufferPtr = IntPtr.Zero;

            try
            {
                bufferPtr = Marshal.AllocHGlobal(buffer.Length);

                int i = libspotify.sp_link_as_string(linkPtr, bufferPtr, buffer.Length);

                if (i == 0)
                    return null;

                Marshal.Copy(bufferPtr, buffer, 0, buffer.Length);

                return System.Text.Encoding.UTF8.GetString(buffer, 0, i);
            }
            finally
            {
                try
                {
                    if (bufferPtr != IntPtr.Zero)
                        Marshal.FreeHGlobal(bufferPtr);
                }
                catch
                {
                }
            }
        }

        internal static string GetCountryName(int country)
        {
            string countryCode = Encoding.ASCII.GetString(new byte[] {(byte) (country >> 8), (byte) (country & 0xff)});

            if (_countryCodes.Count != s_countryNames.Length)
                throw new InvalidOperationException("Bogus country data");

            int i = _countryCodes.IndexOf(countryCode);
            if (i < 0)
                return "My Country";
            else
                return s_countryNames[i];
        }

        public static string ConvertUrlToUri(string url)
        {
            var searchTerm = "//open.spotify.com/";
            if (!url.Contains(searchTerm))
                return url;

            url = url.Substring(url.IndexOf(searchTerm) + searchTerm.Length);

            var components = new List<string> {"spotify"};

            var parts = url.Split('/');
            foreach (var part in parts)
            {
                components.Add(part);
            }


            return String.Join(":", components.ToArray());
        }
    }
}