using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using System.Web.Http.ValueProviders;
using System.Web.Http.ValueProviders.Providers;

namespace Demo.Common.DataTables
{
    public class DtModelBinder : IModelBinder
    {
        private static T GetValue<T>(IValueProvider valueProvider, string key)
        {
            ValueProviderResult valueResult = valueProvider.GetValue(key);
            return (valueResult == null)
                ? default(T)
                : (T)valueResult.ConvertTo(typeof(T));
        }

        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType != typeof(DtRequest))
            {
                return false;
            }

            var compositeValueProvider = (bindingContext.ValueProvider as CompositeValueProvider);
            var valueProvider = compositeValueProvider[0];

            var result = new DtRequest();
            result.Draw = GetValue<int>(valueProvider, "draw");
            result.Length = GetValue<int>(valueProvider, "length");
            result.Start = GetValue<int>(valueProvider, "start");
            result.Search = GetValue<string>(valueProvider, "search[value]");
            if (!string.IsNullOrEmpty(result.Search))
            {
                result.Search += "%";
            }

            int colIdx = 0;
            var columnNames = new List<string>();
            while (true)
            {
                string colNameKey = string.Format("columns[{0}].name", colIdx);
                string colName = GetValue<string>(valueProvider, colNameKey);
                if (string.IsNullOrWhiteSpace(colName))
                {
                    break;
                }

                columnNames.Add(colName);
                colIdx++;
            }

            int? colOrderId = GetValue<int?>(valueProvider, "order[0].column");
            if (colOrderId.HasValue)
            {
                result.OrderColumn = columnNames[colOrderId.Value];
                string order = GetValue<string>(valueProvider, "order[0].dir");
                result.OrderDirection = (DtOrderDirection)Enum.Parse(typeof(DtOrderDirection), order, true);
            }
            else
            {
                result.OrderColumn = columnNames.FirstOrDefault();
                result.OrderDirection = DtOrderDirection.ASC;
            }

            bindingContext.Model = result;
            return true;
        }
    }
}