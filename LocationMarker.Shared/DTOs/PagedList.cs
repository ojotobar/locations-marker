using CSharpTypes.Extensions.List;

namespace LocationMarker.Shared.DTOs
{
    public class PagedList<T> : List<T>
    {
        public PageData PageData { get; set; }
        public List<T> Data { get; set; }

        public PagedList(List<T> source, int page, int size, int count)
        {
            PageData = new PageData
            {
                Page = page,
                Size = size,
                Count = count,
                Pages = (int)Math.Ceiling(count / (double)size)
            };

            Data = source.IsNotNullOrEmpty() ? 
                source : [];
        }

        public static PaginatedList<T> Paginate(IEnumerable<T> source, int page, int size)
        {
            var count = source.Count();
            var items = source.
                Skip((page - 1) *  size)
                .Take(size)
                .ToList();

            var pagedList = new PagedList<T>(items, page, size, count);
            return new PaginatedList<T>(pagedList.PageData, pagedList.Data);
        }

        public static PaginatedList<T> Paginate(IQueryable<T> source, int page, int size)
        {
            var count = source.Count();
            var items = source.
                Skip((page - 1) * size)
                .Take(size)
                .ToList();

            var pagedList = new PagedList<T>(items, page, size, count);
            return new PaginatedList<T>(pagedList.PageData, pagedList.Data);
        }
    }
}
