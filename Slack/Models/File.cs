using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Slack.Models
{
    public class File
    {
        public int FileId { get; set; }
        public string FilePath { get; set; }
        public string ContentType { get; set; }
        public string OriginalName { get; set; }
    }
}
