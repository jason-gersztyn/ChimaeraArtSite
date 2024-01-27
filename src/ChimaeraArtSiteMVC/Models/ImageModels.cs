using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChimaeraArtSiteMVC.Models
{
    public class Image
    {
        public int ImageID { get; set; }
        public string ImageName { get; set; }
        public string ImageDescription { get; set; }
        public string ImageURL { get; set; }
        public Group GroupItem { get; set; }
        public int? Page { get; set; }
    }

    public class Group
    {
        public int GroupID { get; set; }
        public string GroupName { get; set; }
    }

    public class Chapter
    {
        public Image[] ChapterPages { get; set; }
    }

    public class Comic
    {
        public string ComicID { get; set; }
        public Chapter[] ComicChapters { get; set; }
        public string ComicName { get; set; }
        public string COmicDescription { get; set; }
    }
}