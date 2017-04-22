using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using FileRenamer;

namespace FileRenamerTests {
    [TestClass]
    public class FileRenamerTests {
        [TestMethod]
        public void IsAFolder_ShouldReturnFalseTests() {
            //internal bool IsAFolder(string path)

            File.Create("testfile");
            string folder = @"C:\Users\Susanne\Documents\LindseyBieda\Test1000\Test2000";

            FileRenamerClass fr = new FileRenamerClass(folder);

            Assert.IsFalse(fr.IsAFolder("testfile"));
        }


    }
}
