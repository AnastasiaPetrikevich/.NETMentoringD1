using System;
using WGETAnalog.Logic.Enums;

namespace WGETAnalog.Logic.Interfaces
{
    public interface IRestrictionHelper
    {
        Restriction RestrictionType { get; }
        bool IsRestricted(Uri uri);
    }
}