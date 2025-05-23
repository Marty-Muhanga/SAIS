using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace SAIS.Services
{
    public interface IDropdownService
    {
        Task PopulateDropdowns(object viewModel);
        //Task PopulateViewDataDropdowns(ViewDataDictionary viewData);
    }
}
