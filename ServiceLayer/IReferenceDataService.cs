using DataTransferObjectsAndViewModels.DTO;
using System.Collections.Generic;
using static Utilities.Constants.ProjectConstants;

namespace ServiceLayer
{
    public interface IReferenceDataService
    {
        dynamic FetchItemsList(ReferenceDataTypes refDataType, IList<WhereClauseDTO> selectionCriteria, IList<int> userSelectedValues);
    }
}