using System.Web.Http.ModelBinding;

namespace Demo.SqlApi.Model.DataTables
{
    [ModelBinder(typeof(DtModelBinder))]
    public class DtRequest
    { 
        public int Draw { get; set; }

        public int Length { get; set; }

        public int Start { get; set; }

        public string Search { get; set; }

        public string OrderColumn { get; set; }

        public DtOrderDirection OrderDirection { get; set; }
    }
}