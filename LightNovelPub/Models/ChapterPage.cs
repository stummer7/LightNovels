using Core.Entities;

namespace LightNovelPub.Models
{
    public class ChapterPage
    {
        public Novel Novel { get; set; }
        public Chapter Chapter { get; set; }

        public int NovelId { get; set; }
        public int NextChapterId { get; set; }
        public int PrevChapterId { get; set; }
        public bool DisableNextBtn { get; set; } = false;
        public bool DisablePrevBtn { get; set; } = false;
    }
}
