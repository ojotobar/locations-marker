namespace LocationMarker.Shared.DTOs
{
    public class SearchDto
    {
        private int _page = 1;
        private int _size = 500;
        public int Page 
        { 
            get { return _page; }
            set { _page = value < _page ? _page : value; }
        }
        public int PageSize 
        { 
            get => _size; 
            set {  _size = value > _size ? _size : value; } 
        }
        public string Search { get; set; } = string.Empty;
    }
}
