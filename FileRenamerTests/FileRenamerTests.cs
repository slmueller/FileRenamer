using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using FileRenamer;
using System.IO.Abstractions;
using Moq;

namespace FileRenamerTests {

    //1 class per tested method, source: http://haacked.com/archive/2012/01/02/structuring-unit-tests.aspx/

    
    [TestClass]
    public class IsAFolderMethod {
        string filepath;
        DirectoryInfo dir;

        [TestInitialize]
        public void TestInit() {
            filepath = File.Create("testfile").Name;

            dir = Directory.CreateDirectory("directory1");
        }


        [TestMethod]
        public void ShouldReturnFalse() {
            //internal bool IsAFolder(string path)

            FileRenamerClass fr = new FileRenamerClass(dir.FullName);

            Assert.IsFalse(fr.IsAFolder(filepath));
        }


        [TestCleanup]
        public void TestCleanup() {

            Directory.Delete(dir.FullName);
        }
    }


    [TestClass]
    public class UpdateOldFileAndDirectoryNames {
        //internal void UpdateOldFileAndDirectoryNames(string oldDirectory, string newDirectory) {

        [TestMethod]
        public void FilenameHasChanged() {

            //Arrange:
            string oldDirectory = @"C:\Users\Susanne\Documents\LindseyBieda\Test1000";
            string newDirectory = @"C:\Users\Susanne\Documents\LindseyBieda\Test1";

            Mock<System.IO.Abstractions.FileWrapper> file = new Mock<System.IO.Abstractions.FileWrapper>(); //creates a mock file
            //inherits from System.IO.Abstractions.FileBase

            //Act:
            FileRenamerClass f = new FileRenamerClass(oldDirectory);
            f.UpdateOldFileAndDirectoryNames(oldDirectory, newDirectory);

            //Assert:
            foreach (FilePath fp in f.Paths) {
                //Assert.AreEqual(newDirectory, fp.OldName);  //Check that old name was updated
                Assert.IsTrue(fp.OldName.Contains(newDirectory));  //Check that old name was updated
            }
            
        }
        
    }


    [TestClass]
    public class RenameFilesAndFolders{

        //public void RenameFilesAndFolders()

        string filepath;
        DirectoryInfo oldDir;
        string newDir;

        [TestInitialize]
        public void TestInit() {

            oldDir = Directory.CreateDirectory("directory1000");

            newDir = "directory1";
        }


        [TestMethod]
        public void FilesHaveBeenMoved() {

            //Arrange:
            //string oldDirectory = @"C:\Users\Susanne\Documents\LindseyBieda\Test1000";
            //string newDirectory = @"C:\Users\Susanne\Documents\LindseyBieda\Test1";
            

            FileRenamerClass f = new FileRenamerClass(oldDir.FullName);
            foreach (FilePath fp in f.Paths) {
                fp.NewName = newDir;  
            }

            //Act:            
            f.RenameFilesAndFolders();

            //Assert:
            Assert.IsTrue(Directory.Exists(newDir));
            Assert.IsFalse(Directory.Exists(oldDir.FullName));

        }

        [TestCleanup]
        public void TestCleanup() {

            Directory.Delete(newDir);
        }
    }

    //Unit tests: 
    //Integration tests: same code/framework, not common in C# space?
    //Functional tests: GUI testing


}
