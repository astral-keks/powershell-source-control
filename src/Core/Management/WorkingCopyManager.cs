using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AstralKeks.SourceControl.Core.Data;
using AstralKeks.SourceControl.Core.Client;
using AstralKeks.SourceControl.Core.Resources;
using AstralKeks.Workbench.Common.FileSystem;

namespace AstralKeks.SourceControl.Core.Management
{
    public class WorkingCopyManager
    {
        private readonly RepositoryManager _repositoryManager;

        public WorkingCopyManager(RepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager ?? throw new ArgumentNullException(nameof(repositoryManager));
        }

        public WorkingCopy CreateWorkingCopy(string workingCopyName, string repositoryName)
        {
            if (string.IsNullOrWhiteSpace(workingCopyName))
                throw new ArgumentNullException(nameof(workingCopyName));
            if (string.IsNullOrWhiteSpace(repositoryName))
                throw new ArgumentNullException(nameof(repositoryName));

            var repository = _repositoryManager.GetRepositories(repositoryName).First();

            var workingCopy = GetWorkingCopy(workingCopyName, null);
            var client = _repositoryManager.GetRepositoryClient(repository.Vcs);
            client.CreateWorkingCopy(workingCopy.OriginPath, repository.Uri);
            return workingCopy;
        }

        public List<WorkingCopy> GetWorkingCopies(Func<WorkingCopy, bool> predicate = null)
        {
            var workingCopies = GetWorkingCopies();
            if (predicate != null)
                workingCopies = workingCopies.Where(predicate);
            return workingCopies.ToList();
        }

        public WorkingCopy GetWorkingCopy(string workingCopyName)
        {
            if (string.IsNullOrWhiteSpace(workingCopyName))
                throw new ArgumentException("Working copy name is invalid");

            var workingCopy = GetWorkingCopy(workingCopyName, null);
            if (workingCopy == null)
                throw new DirectoryNotFoundException($"Working copy {workingCopyName} was not found");

            return workingCopy;
        }

        public void PopWorkingCopy(string query)
        {
            InvokeClient(query, (client, path) => client.PopWorkingCopy(path));
        }

        public void PushWorkingCopy(string query)
        {
            InvokeClient(query, (client, path) => client.PushWorkingCopy(path));
        }

        public void ResetWorkingCopy(string query)
        {
            InvokeClient(query, (client, path) => client.ResetWorkingCopy(path));
        }

        public void ShowWorkingCopyLog(string query)
        {
            InvokeClient(query, (client, path) => client.ShowWorkingCopyLog(path));
        }

        public void ShowWorkingCopyDiff(string query)
        {
            InvokeClient(query, (client, path) => client.ShowWorkingCopyDiff(path));
        }

        public IEnumerable<string> ExportWorkingCopyDiff(string query)
        {
            return InvokeClient(query, (client, path) =>
            {
                var fileName = $"{DateTime.UtcNow.ToString("yy-MM-dd-HH-mm-ss-ffff")}.patch";
                var workingCopyDiffPath = FsPath.Absolute(Paths.DiffDirectory, fileName);
                client.ExportWorkingCopyDiff(path, workingCopyDiffPath);
                return workingCopyDiffPath;
            });
        }

        public void ImportWorkingCopyDiff(string query, string workingCopyDiffPath)
        {
            InvokeClient(query, (client, path) => client.ImportWorkingCopyDiff(path, workingCopyDiffPath));
        }

        public void AddWorkingCopyEntry(string query)
        {
            InvokeClient(query, (client, path) => client.AddWorkingCopyItem(path));
        }
        

        private void InvokeClient(string query, Action<RepositoryClient, string> action)
        {
            query = WorkingPathBuilder.Validate(query);
            var binding = new WorkingPathBinding(GetWorkingCopies());

            var queryPath = new WorkingPathBuilder(query).WorkingPath;
            foreach (var boundPath in binding.Bind(queryPath))
            {
                var workingCopy = GetWorkingCopy(boundPath.Root);
                var client = _repositoryManager.GetRepositoryClient(workingCopy.OriginPath);
                if (client == null)
                    throw new ArgumentException($"Repository client was not found for query '{query}'");

                foreach (var unboundPath in binding.Unbind(boundPath))
                    action(client, WorkingPathBuilder.Validate(unboundPath.ToString()));
            }
        }

        private IEnumerable<TResult> InvokeClient<TResult>(string query, Func<RepositoryClient, string, TResult> func)
        {
            query = WorkingPathBuilder.Validate(query);
            var binding = new WorkingPathBinding(GetWorkingCopies());

            var queryPath = new WorkingPathBuilder(query).WorkingPath;
            foreach (var boundPath in binding.Bind(queryPath))
            {
                var workingCopy = GetWorkingCopy(boundPath.Root);
                var client = _repositoryManager.GetRepositoryClient(workingCopy.OriginPath);
                if (client == null)
                    throw new ArgumentException($"Repository client was not found for query '{query}'");

                foreach (var unboundPath in binding.Unbind(boundPath))
                    yield return func(client, WorkingPathBuilder.Validate(unboundPath.ToString()));
            }
        }

        private IEnumerable<WorkingPath> BindQuery(string query)
        {
            var binding = new WorkingPathBinding(GetWorkingCopies());

            var queryPath = new WorkingPathBuilder(query).WorkingPath;
            return binding.Bind(queryPath);
        }

        private IEnumerable<WorkingPath> UnbindQuery(string query)
        {
            var binding = new WorkingPathBinding(GetWorkingCopies());

            var queryPath = new WorkingPathBuilder(query).WorkingPath;
            return binding.Unbind(queryPath);
        }

        private IEnumerable<WorkingCopy> GetWorkingCopies()
        {
            var directories = Directory.GetDirectories(Paths.SourceDirectory);
            return directories
                .Where(d => _repositoryManager.GetRepositoryClient(d) != null)
                .Select(d => GetWorkingCopy(null, d));
        }

        private WorkingCopy GetWorkingCopy(string workingCopyName, string workingCopyPath = null)
        {
            if (string.IsNullOrWhiteSpace(workingCopyName))
                workingCopyName = Path.GetFileName(workingCopyPath);
            if (string.IsNullOrWhiteSpace(workingCopyPath))
                workingCopyPath = FsPath.Absolute(Paths.SourceDirectory, workingCopyName);

            return new WorkingCopy(workingCopyName, workingCopyPath);
        }
    }
}
