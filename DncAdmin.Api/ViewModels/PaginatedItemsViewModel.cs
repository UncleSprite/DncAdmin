using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DncAdmin.Api.ViewModels
{
    public class PaginatedItemsViewModel<T> where T : class
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public long Count { get; set; }

        public IEnumerable<T> Data { get; set; }

        public PaginatedItemsViewModel(int pageIndex, int pageSize, long count, IEnumerable<T> data)
        {
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
            this.Count = count;
            this.Data = data;
        }
    }
}
