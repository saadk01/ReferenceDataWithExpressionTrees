using DataTransferObjectsAndViewModels.DTO;
using DataTransferObjectsAndViewModels.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceLayer;
using System.Collections.Generic;
using System.Linq;
using static Utilities.Constants.ProjectConstants;

namespace MvcClient.ViewComponents
{
    public class ReferenceDataRendererViewComponent : ViewComponent
    {
        protected IReferenceDataService _refDataService;

        public ReferenceDataRendererViewComponent(IReferenceDataService rdrs)
        {
            _refDataService = rdrs;
        }

        public IViewComponentResult Invoke(ReferenceDataTypes refDataType, List<WhereClauseDTO> selectionCriteria, string elementName, string htmlClasses, int? userSelectedValue, bool required = false, bool disabled = false)
        {
            var userSelectedVals = userSelectedValue == null ? new List<int>() : new List<int>() { (int)userSelectedValue };

            return View("Default", new SelectBoxDisplayViewModel()
            {
                ElementName = elementName,
                SelectListItems = PopulateSelectListItems(refDataType, userSelectedValue, _refDataService.FetchItemsList(refDataType, selectionCriteria, userSelectedVals)),
                HtmlAttributes = SetHtmlAttributes(htmlClasses, required, disabled),
                UserSelectedValue = userSelectedValue
            });
        }

        protected List<SelectListItem> PopulateSelectListItems(ReferenceDataTypes refDataType, int? userSelectedValue, dynamic items)
        {
            var selectListItems = new List<SelectListItem>();
            foreach (var i in items)
                selectListItems.Add(new SelectListItem(i.Name, i.ID.ToString(), userSelectedValue == i.ID ? true : false));

            selectListItems = selectListItems.OrderBy(x => x.Text).ToList();
            selectListItems.Insert(0, new SelectListItem("Please choose one", "", false, false));

            return selectListItems;
        }

        protected Dictionary<string, object> SetHtmlAttributes(string htmlClasses, bool required, bool disabled)
        {
            var attributes = new Dictionary<string, object>
            {
                { "class", htmlClasses }
            };
            if (required)
            {
                attributes.Add("data-val", "true");
                attributes.Add("data-val-required", "Please select an option");
            }
            if (disabled)
                attributes.Add("disabled", "disabled");

            return attributes;
        }
    }
}
