using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;

namespace FileRenamer {
    public class FileRenamerClass {

        private List<FilePath> Paths { get; set; }   //maps old to new paths
        private string ParentDirectory { get; set; }  //directory in which user wants to rename files & folders, NOT including the parent folder he wants to rename
        //e.g. directoryPath = C:\\Test\ParentDirectory\FolderToBeRenamed --> contains SubfoldersToBeRenamed

        public FileRenamerClass(string directoryPath) {

            this.ParentDirectory = GetParentDirectory(directoryPath);

            if (this.Paths == null) {    //create only once
                this.Paths = GetAllPathsInDirectory(directoryPath);    //Get all filenames/Paths
            }
        }


        /// <summary>
        /// Aim: Get root directory
        /// e.g. directoryPath = C:\\Test\ParentDirectory\FolderToBeRenamed --> contains SubfoldersToBeRenamed
        /// </summary>
        /// <param name="directoryPath">folder that user wants to rename (& all the subfolders / files it contains)</param>
        /// <returns>ParentDirectory</returns>
        private string GetParentDirectory(string directoryPath) {

            DirectoryInfo parentDirectory = Directory.GetParent(directoryPath);
            
            return parentDirectory.FullName;    //get the full path to the parent folder
        }


        /// <summary>
        /// Aim: Replace original pattern with new pattern for each filename
        /// replace the regexp only in the portion of the dir that is WITHIN the folder the user chose
        /// i.e. NOT in the parentDirectory part
        /// </summary>
        public void CreateNewFilenames(string originalPattern, string replaceString) {

            foreach (FilePath p in this.Paths) {

                //1. Get whatever part of the filename is NOT matching parentDirectory
                string partOfPathAfterParentDir = GetPartOfPathWithinTheParentDirectory(p.OldName);

                //2. Replace the pattern only in the non-matching part (i.e. in the folders & files within the parent directory):
                string replacedPatternInPath = Regex.Replace(partOfPathAfterParentDir, originalPattern, replaceString);  

                //3. Reassemble full path:
                p.NewName = this.ParentDirectory + replacedPatternInPath;    //new path = parentDir + pathToFoldersSubfoldersOrFiles
            }

            //TODO: adjust greediness of replace method

            //TODO: right now the replaces pattern with a string --> Make more sophisticated: replace regex with regex
            //i.e. if originalPattern matches --> create replacement string based on match (using replacement pattern)
        }


        /// <summary>
        /// Aim: Get whatever part of the filename is NOT matching parentDirectory
        /// </summary>
        private string GetPartOfPathWithinTheParentDirectory(string oldName) {

            Regex parentDirPattern = CreateRegexToCaptureFoldersAndFilesToBeRenamed();

            Match match = parentDirPattern.Match(oldName);    //captures part of string NOT matching the parent directory
            string partOfPathAfterParentDir = match.Groups[1].Value;    //Group1: whole match, Group2: 1st bracket = path NOT matching parentDir
            //i.e. c://ParentDir/FolderToBeRenamed/SubfolderToBeRenamed --> gets FolderToBeRenamed/SubfolderToBeRenamed

            return partOfPathAfterParentDir;
        }

        /// <summary>
        /// Aim: Create Regex to capture whatever part of the filename is NOT matching parentDirectory
        /// </summary>
        private Regex CreateRegexToCaptureFoldersAndFilesToBeRenamed() {

            string parentDirPattern = Regex.Escape(this.ParentDirectory);   //adds escape characters to \
            string wholePattern = "^" + parentDirPattern + "(.*)$";  //string starts with parentDirectory path, capture everything between parentDir and end of string
                                                                     //e.g. C:\\GrandParentDir\ParentDir\FolderToBeRenamed\SubfolderToBeRenamed\FileToBeRenamed --> pattern will capture "\FolderToBeRenamed\SubfolderToBeRenamed\FileToBeRenamed"
            Regex r = new Regex(wholePattern);

            return r;
        }

        /// <summary>
        /// Aim: Get List of all folders/files in directory
        /// </summary>
        private List<FilePath> GetAllPathsInDirectory(string directoryPath) {

            //Get all folders and subfolders in directory:
            List<string> foldersInDirectory = new List<string> { directoryPath };
            GetAllFoldersAndSubfoldersInDirectory(directoryPath, foldersInDirectory);

            //get all files:
            //List<string> filesInDirectory = GetAllFilesInFolders(foldersInDirectory); //get all files in directory & subfolders 
            List<string> filesInDirectory = new List<string>(Directory.GetFiles(directoryPath, "*.*", SearchOption.AllDirectories));

            //Get all items in directory:
            List<string> allItemsInDirectory = new List<string>(foldersInDirectory);
            allItemsInDirectory.AddRange(filesInDirectory);

            //create path for each file/folder:
            List<FilePath> paths = CreatePathForEachFileOrFolder(allItemsInDirectory);

            return paths;
        }


        /// <summary>
        /// Aim: Get all folders and subfolders in the directory
        /// </summary>
        private void GetAllFoldersAndSubfoldersInDirectory(string directoryPath, List<string> foldersInDirectory) {

            List<string> folders = Directory.GetDirectories(directoryPath).ToList();    //get all folders in the directory

            foreach (string folder in folders) {
                GetAllFoldersAndSubfoldersInDirectory(folder, foldersInDirectory);  //recursive call to get subdirs
            }

            foldersInDirectory.AddRange(folders); //add folders in current dir   
        }



        /// <summary>
        /// Aim: Get all file paths in a list of folders
        /// </summary>
        private List<string> GetAllFilesInFolders(List<string> folders) {

            List<string> filesInFolders = new List<string>();   //collected files from all folders

            foreach (string folder in folders) {   //go through all folders

                List<string> filesInFolder = Directory.EnumerateFiles(folder).ToList(); //get the files in 1 folder
                filesInFolders.AddRange(filesInFolder);
            }

            return filesInFolders;
        }


        /// <summary>
        /// Aim: Create path for each file and folder
        /// </summary>
        private List<FilePath> CreatePathForEachFileOrFolder(List<string> allFilesOrFolders) {

            List<FilePath> paths = new List<FilePath>();
            foreach (string path in allFilesOrFolders) {
                FilePath f = new FilePath(path);
                paths.Add(f);
            }

            return paths;
        }


        public void DisplayNewFilenames() {

            Console.WriteLine("New paths: ");

            foreach (FilePath path in this.Paths) {
                Console.WriteLine(path.NewName);
            }

            Console.WriteLine("*********************");
        }


        /// <summary>
        /// Aim: Rename files and folders (move them to new location & delete old location)
        /// </summary>
        public void RenameFilesAndFolders() {

            foreach (FilePath fp in this.Paths) {

                if (!fp.OldName.Equals(fp.NewName)) {   //only move items that got a new name

                    if (IsAFolder(fp.OldName)) {

                        Directory.Move(fp.OldName, fp.NewName); 
                        //if no access to path: probably folder properties are set to read-only --> change them in file explorer

                        //Update all filenames: replace the name of the moved directory in the oldNames of all files/directories 
                        //ensures that they will be found in their "old" location
                        UpdateOldFileAndDirectoryNames(fp.OldName, fp.NewName);

                    } else {
                        File.Move(fp.OldName, fp.NewName);  //is a file
                    }
                }
            }
        }

        /// <summary>
        /// Aim: 
        /// Update all filenames: replace the name of the moved directory in the oldNames of all files/directories 
        /// ensures that they will be found in their changed "old" location
        /// </summary>
        internal void UpdateOldFileAndDirectoryNames(string oldDirectory, string newDirectory) {

            string oldDirPattern = Regex.Escape(oldDirectory);  //converts all \ to \\ (puts proper escape chars)

            foreach (FilePath path in this.Paths) { //go through all file/folder paths

                //replace old directory string with new directory
                path.OldName = Regex.Replace(path.OldName, oldDirPattern, newDirectory);
            }
        }

        /// <summary>
        /// Aim: Check if a path leads to a folder (or to a file)
        /// </summary>
        internal bool IsAFolder(string path) {

            FileAttributes attr = File.GetAttributes(path);
            return (attr.HasFlag(FileAttributes.Directory));    //true if folder, false if file
        }
    }
}