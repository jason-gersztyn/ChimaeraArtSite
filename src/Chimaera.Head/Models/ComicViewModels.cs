using Chimaera.Beasts.Model;

namespace Chimaera.Head.Models
{
    public class ComicIndexViewModel
    {
        public Comic[] comics { get; set; }
    }

    public class ComicSeriesViewModel
    {
        public Comic comic { get; set; }
    }

    public class ComicChapterViewModel
    {
        public Chapter chapter { get; set; }
    }

    public class ComicPageViewModel
    {
        public Chapter chapter { get; set; }
        public Page previousPage { get; set; }
        public Page currentPage { get; set; }
        public Page nextPage { get; set; }
    }
}