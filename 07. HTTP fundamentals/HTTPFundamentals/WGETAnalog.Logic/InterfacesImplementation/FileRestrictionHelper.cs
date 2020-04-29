using System;
using System.Collections.Generic;
using System.Linq;
using WGETAnalog.Logic.Enums;
using WGETAnalog.Logic.Interfaces;

namespace WGETAnalog.ConsoleApp.Restrictions
{
    public class FileRestrictionHelper : IRestrictionHelper
    {
        private readonly IEnumerable<string> extensions;

        public FileRestrictionHelper(IEnumerable<string> extensions)
        {
            this.extensions = extensions;
        }

        public Restriction RestrictionType => Restriction.FileRestriction;

        public bool IsRestricted(Uri url)
        {
            bool result = false;
            string lastSegment = url.Segments.Last();

            result = !extensions.Any(e => lastSegment.EndsWith(e, StringComparison.Ordinal));

            return result;
        }
    }
}