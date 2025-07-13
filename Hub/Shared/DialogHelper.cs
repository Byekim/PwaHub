using Microsoft.AspNetCore.Components;
using Radzen;

namespace Hub.Shared
{
    public static class DialogHelper
    {
        public static Task OpenDialog<T>(DialogService dialogService, string title, object parameters = null) where T : ComponentBase
        {
            var parameterDictionary = new Dictionary<string, object>();
            if (parameters != null)
            {
                parameterDictionary["Parameters"] = parameters;
            }

            return dialogService.OpenAsync<T>(title, parameterDictionary);
        }
    }
}
