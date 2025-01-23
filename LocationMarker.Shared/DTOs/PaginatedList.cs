namespace LocationMarker.Shared.DTOs
{
    public class PaginatedList<T>(PageData meta, List<T> data)
    {
        public PageData MetaData => meta;
        public List<T> Data => data;
    }
}
