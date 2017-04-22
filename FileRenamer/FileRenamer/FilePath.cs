using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileRenamer {
    public class FilePath {
        public string OldName { get; set; }
        public string NewName { get; set; }

        public FilePath(string oldName) {
            this.OldName = oldName;
            this.NewName = oldName; //at start oldName = newName
        }
    }
}