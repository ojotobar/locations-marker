namespace LocationMarker.Shared.DTOs
{
    public class PageData
    {
        public int Page { get; set; }
        public int Size { get; set; }
        public int Pages { get; set; }
        public bool HasNext => Page < Pages;
        public bool HasPrevious => Page > 1;
        public int Count { get; set; }
    }
}
