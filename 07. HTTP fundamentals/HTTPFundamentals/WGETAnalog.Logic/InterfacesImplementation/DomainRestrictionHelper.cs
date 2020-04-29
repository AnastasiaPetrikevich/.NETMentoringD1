using System;
using WGETAnalog.Logic.Enums;
using WGETAnalog.Logic.Interfaces;

namespace WGETAnalog.Logic.InterfacesImplementation
{
    public class DomainRestrictionHelper : IRestrictionHelper
    {
        private readonly Uri baseUrl;
        private readonly DomainRestriction restrictionType;

        public DomainRestrictionHelper(DomainRestriction restrictionType, Uri baseUrl)
        {
            switch (restrictionType)
            {
                case DomainRestriction.All:
                case DomainRestriction.CurrentDomainOnly:
                case DomainRestriction.ChildDomainOnly:
                    this.restrictionType = restrictionType;
                    this.baseUrl = baseUrl;
                    break;
            }
        }

        public Restriction RestrictionType =>
            Restriction.UrlRestriction | Restriction.FileRestriction;

        public bool IsRestricted(Uri url)
        {
            switch (restrictionType)
            {
                case DomainRestriction.All:
                    return true;

                case DomainRestriction.CurrentDomainOnly:
                    if (baseUrl.DnsSafeHost == url.DnsSafeHost)
                    {
                        return true;
                    }
                    break;

                case DomainRestriction.ChildDomainOnly:
                    if (baseUrl.IsBaseOf(url))
                    {
                        return true;
                    }
                    break;
            }

            return false;
        }
    }
}