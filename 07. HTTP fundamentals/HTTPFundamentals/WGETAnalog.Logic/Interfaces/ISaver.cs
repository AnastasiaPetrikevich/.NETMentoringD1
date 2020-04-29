using System;
using System.IO;

namespace WGETAnalog.Logic.Interfaces
{
    public interface ISaver
    {
        void SaveFile(Uri uri, Stream fileStream);
        void SaveHtml(Uri uri, string name, Stream documentStream);
    }
}