using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem.Core.Models
{
    public class PagedList<T>
    {
        public MetaData MetaData { get; set; }
        public IEnumerable<T> Data { get; set; }

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            if (pageSize <= 0) pageSize = 10;
            MetaData = new MetaData
            {
                TotalCount = count,
                PageSize = pageSize,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize)
            };

            Data = items;
        }
    }
}
