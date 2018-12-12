using DriveCentric.Core.Interfaces;

namespace DriveCentric.Core.Models
{
    public class PageableSearch : IPageable
    {
        public static PageableSearch Default => new PageableSearch(SearchParameters.OffsetDefault, SearchParameters.LimitDefault, null);
        public static PageableSearch Max => new PageableSearch(SearchParameters.OffsetDefault, SearchParameters.LimitMax, null);
        public int? Offset { get; set; }
        public int? Limit { get; set; }
        public string OrderBy { get; set; }

        public PageableSearch()
        {
            OrderBy = "Id";
        }

        public PageableSearch(int? offset, int? limit, string orderBy)
        {
            Offset = offset;
            Limit = limit;
            OrderBy = string.IsNullOrWhiteSpace(orderBy) ? "Id" : orderBy;
        }
    }
}