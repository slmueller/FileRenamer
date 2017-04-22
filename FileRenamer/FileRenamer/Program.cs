using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileRenamer {
    public class Program {
        static void Main(string[] args) {

            //Get user input: directory, original pattern, pattern to replace it
            string directoryPath = InputGetter.GetDirectory();

            FileRenamer fr = new FileRenamer(directoryPath);
            do {  //continue until renaming is concluded

                string originalPattern;
                string replaceString;
                InputGetter.GetPatternsForReplacement(out originalPattern, out replaceString); //get further regex to be replaced

                //create shortened filenames
                fr.CreateNewFilenames(originalPattern, replaceString);

                //TODO:
                //check file/foldernames for conflicts (non-unique paths)
                //resolve conflicts

                fr.DisplayNewFilenames();   //Display new filenames for user to check

            } while (InputGetter.ContinueRenaming());   //further rename files?


            fr.RenameFilesAndFolders(); //Rename files
        }
    }
}