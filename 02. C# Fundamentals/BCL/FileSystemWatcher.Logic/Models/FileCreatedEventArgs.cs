using System;

namespace FileSystemWatcher.Models
{
    public class FileCreatedEventArgs<T> : EventArgs
    {
        public T CreatedItem { get; set; }
    }
}
