namespace DataTables.Queryable.Support
{
    public class Paging
    {
        public Paging(int pageSize) : this(pageSize, 0)
        {
        }

        public Paging(int pageSize, int skip)
        {
            PageSize = pageSize;
            Skip = skip;
        }

        public int PageSize { get; }
        public int Skip { get; }
    }
}
