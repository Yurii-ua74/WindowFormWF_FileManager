using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WF_FileManager
{
    internal class Anime
    {
        public string Animation(int k)
        {
            string path = "";
            path = @"..\..\..\images\" + (k++).ToString() + ".jpg";
            return path;
        }
    }
}
