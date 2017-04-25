using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.ModelBinding;

namespace Demo.SqlApi.Model.DataTables
{
    [ModelBinder(typeof(DtModelBinder))]
    public class DtRequest
    {
        private string orderColumn;
        public int Draw { get; set; }

        public int Length { get; set; }

        public int Start { get; set; }

        public string Search { get; set; }

        public string OrderColumn
        {
            get { return orderColumn; }
            set
            {
                if (value != null && !this.Fields.Contains(value))
                {
                    throw new InvalidOperationException("Nieznane pole");
                }

                orderColumn = value;
            }
        }

        public DtOrderDirection OrderDirection { get; set; }

        public ISet<string> Fields { get; set; } 
    }

    [ModelBinder(typeof(DtModelBinder))]
    public class DtRequest<TDto> : DtRequest
        where TDto : class
    {
        private static readonly ISet<string> Names =
            new HashSet<string>(typeof (TDto).GetProperties().Select(x => x.Name).ToList());

        public DtRequest()
        {
            this.Fields = Names;
        }
    }
}