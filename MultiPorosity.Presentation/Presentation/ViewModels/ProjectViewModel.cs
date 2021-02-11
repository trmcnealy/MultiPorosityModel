using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

using Engineering.DataSource;
using Engineering.UI.Collections;

using Microsoft.Win32;

using MultiPorosity.Presentation.Models;
using MultiPorosity.Presentation.Services;
using MultiPorosity.Services;

using Prism.Commands;
using Prism.Mvvm;

namespace MultiPorosity.Presentation
{
    public class ProjectViewModel : BindableBase
    {

        private Dictionary<string, ProjectFileMetaData>? filesMetaData;

        private BindableCollection<ProjectFileMetaData> _repositoryProjectNames = new();

        private ProjectFileMetaData? _selectedRepositoryProjectFile;
        
        public string? RepositoryPath
        {
            get { return _multiPorosityModelService.RepositoryPath; }
            set
            {
                _multiPorosityModelService.RepositoryPath = value;
            }
        }

        public BindableCollection<ProjectFileMetaData> RepositoryProjectFiles
        {
            get { return _repositoryProjectNames; }
            set
            {
                if(SetProperty(ref _repositoryProjectNames, value))
                {
                    void OnCollectionChanged(object?                          sender,
                                             NotifyCollectionChangedEventArgs args)
                    {
                        RaisePropertyChanged(nameof(RepositoryProjectFiles));
                    }

                    _repositoryProjectNames.CollectionChanged -= OnCollectionChanged;
                    _repositoryProjectNames.CollectionChanged += OnCollectionChanged;
                }
            }
        }

        public ProjectFileMetaData? SelectedRepositoryProjectFile
        {
            get { return _selectedRepositoryProjectFile; }
            set
            {
                if(SetProperty(ref _selectedRepositoryProjectFile, value))
                {
                }
            }
        }

        public Project? ActiveProject
        {
            get { return _multiPorosityModelService.ActiveProject; }
            set
            {
                _multiPorosityModelService.ActiveProject = value;

                Console.WriteLine($"Project {value.Name} had been loaded.");
            }
        }

        public DelegateCommand<ProjectFileMetaData> MouseDoubleClickCommand { get; }

        public DelegateCommand NewProjectCommand { get; }

        public DelegateCommand LoadProjectCommand { get; }

        public DelegateCommand SaveProjectCommand { get; }

        public DelegateCommand BrowseToProjectCommand { get; }

        private readonly MultiPorosityModelService _multiPorosityModelService;

        public ProjectViewModel(MultiPorosityModelService? multiPorosityModelService)
        {
            _multiPorosityModelService = Throw.IfNull(multiPorosityModelService);

            _multiPorosityModelService.PropertyChanged += OnMultiPorosityModelServicePropertyChanged;

            UpdateRepositoryProjectFiles();

            //_eventAggregator.GetEvent<RepositoryPathChangedEvent>().Subscribe(OnRepositoryPathChanged);

            NewProjectCommand       = new DelegateCommand(NewProject);
            SaveProjectCommand      = new DelegateCommand(SaveProject);
            LoadProjectCommand      = new DelegateCommand(LoadProject);
            BrowseToProjectCommand  = new DelegateCommand(BrowseToProject);
            MouseDoubleClickCommand = new DelegateCommand<ProjectFileMetaData>(OnMouseDoubleClick);
        }

        private void OnMouseDoubleClick(ProjectFileMetaData projectFileMetaData)
        {
            ActiveProject = new(DataSources.LoadProject(projectFileMetaData.Path));
        }

        private void OnMultiPorosityModelServicePropertyChanged(object?                  sender,
                                                                PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(RepositoryPath))
            {
                UpdateRepositoryProjectFiles();
            }
            else if(e.PropertyName == nameof(ActiveProject))
            {
                
                
                RaisePropertyChanged(nameof(ActiveProject));
            }
        }

        private void UpdateRepositoryProjectFiles()
        {
            Dictionary<string, ProjectFileMetaData>? metaData = ProjectService.ProjectFilesInRepository(RepositoryPath);

            if(metaData is null)
            {
                return;
            }

            filesMetaData = metaData;

            RepositoryProjectFiles = new BindableCollection<ProjectFileMetaData>(metaData.Values);
            RaisePropertyChanged(nameof(RepositoryProjectFiles));
        }

        private void OnRepositoryPathChanged(string? repositoryPath)
        {
            RepositoryPath = repositoryPath;
        }

        private void NewProject()
        {
            NewProjectWindow npw    = new NewProjectWindow();
            
            bool?            result = npw.ShowDialog();

            if(result == true)
            {
                string newProjectName = npw.newProjectName.Text;

                ActiveProject = new(new MultiPorosity.Services.Models.Project(newProjectName));
            }
            
            npw.Close();
        }

        private void SaveProject()
        {
            SaveFileDialog sfg = new()
            {
                InitialDirectory            = RepositoryPath,
                DefaultExt = ".mpm", Filter = "MultiPorosityModel Project (.mpm)|*.mpm"
            };

            bool? result = sfg.ShowDialog();

            if(result == true)
            {
                string filename = sfg.FileName;

                DataSources.SaveProject(filename, ActiveProject);

                UpdateRepositoryProjectFiles();
            }
        }

        private void LoadProject()
        {
            if(SelectedRepositoryProjectFile is not null && filesMetaData is not null)
            {
                if(filesMetaData.TryGetValue(SelectedRepositoryProjectFile.Name, out ProjectFileMetaData fileMetaData))
                {
                    ActiveProject = new(DataSources.LoadProject(fileMetaData.Path));
                }
            }
        }

        private void BrowseToProject()
        {
            OpenFileDialog ofg = new()
            {
                InitialDirectory = RepositoryPath,
                Multiselect      = false,
                DefaultExt       = ".mpm",
                Filter           = "MultiPorosityModel Project (.mpm)|*.mpm"
            };

            bool? result = ofg.ShowDialog();

            if(result == true)
            {
                string filename = ofg.FileName;

                ActiveProject = new(DataSources.LoadProject(filename));
            }
        }
    }
}