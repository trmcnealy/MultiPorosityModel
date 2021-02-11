using System;

using Prism.Events;
using Prism.Mvvm;

namespace MultiPorosity.Presentation.Models
{
    public class RepositoryPathChangedEvent : PubSubEvent<string?>
    {
    }
    
    public class ProjectFileMetaData : BindableBase
    {
        private string _name;

        private string _path;

        private DateTime _creationTime;

        private DateTime _lastWriteTime;

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public string Path
        {
            get { return _path; }
            set { SetProperty(ref _path, value); }
        }

        public DateTime CreationTime
        {
            get { return _creationTime; }
            set { SetProperty(ref _creationTime, value); }
        }

        public DateTime LastWriteTime
        {
            get { return _lastWriteTime; }
            set { SetProperty(ref _lastWriteTime, value); }
        }

        public ProjectFileMetaData(string   name,
                                   string   path,
                                   DateTime creationTime,
                                   DateTime lastWriteTime)
        {
            _name          = name;
            _path          = path;
            _creationTime  = creationTime;
            _lastWriteTime = lastWriteTime;
        }
    }
}