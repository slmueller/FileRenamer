using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileRenamer {
    public class InputGetter {

        //GET INPUT FROM USER

        //METHODS:

        /// <summary>
        /// Aim: Ask user for directory to be renamed
        /// </summary>
        /// <returns>directory path</returns>
        public static string GetDirectory() {
            //ask for input directory
            Console.WriteLine(@"Enter directory path to be renamed: e.g. C:\Users\Susanne\Documents\LindseyBieda\Test1000\Test2000");
            string directoryPath = Console.ReadLine().Trim();
            return directoryPath;
        }


        /// <summary>
        /// Aim: Get original pattern & pattern it is to be replaced with
        /// patterns are regex
        /// </summary>
        public static void GetPatternsForReplacement(out string originalPattern, out string replaceString) {

            //ask for regex1: what pattern should be replaced
            Console.WriteLine("Which pattern do you want to replace? Enter a regular rexpression: e.g. 000 or Test1000");
            originalPattern = Console.ReadLine().Trim();

            //ask for regex2 with what should pattern be replaced
            Console.WriteLine("With what do you want to replace it? Enter a string: e.g. '' or Test1");
            replaceString = Console.ReadLine().Trim();
        }


        /// <summary>
        /// Aim: Ask user if he wants to continue renaming filenames
        /// </summary>
        /// <returns>true if user wants to further rename files</returns>
        public static bool ContinueRenaming() {

            Console.WriteLine("Do you want to further rename the files?: y / n");
            string answer = Console.ReadLine().Trim().ToLower();

            //repeat until clear answer:
            while (!answer.Equals("y") && !answer.Equals("n")) {

                Console.WriteLine("Please enter y or n: ");
                answer = Console.ReadLine().Trim().ToLower();
            }

            return answer.Equals("y");
        }



    }
}