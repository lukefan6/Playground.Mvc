using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace Playground.Mvc.Web.Models.Order
{
    public static class ValidationHelper
    {
        public static bool ValidateDictionaryItems<TModel, TValue>(
            ModelStateDictionary modelState,
            TModel model,
            Expression<Func<TModel, IDictionary<string, TValue>>> itemSelection) where TValue : OrderViewModelBase
        {
            modelState.Clear();

            foreach (var item in itemSelection.Compile().Invoke(model).Values)
            {
                var validationResults = Validate(item);

                foreach (var validationResult in validationResults)
                {
                    string prefix = itemSelection.ToMemberExpression().Member.Name;
                    string id = item.ViewModelId.ToString();
                    string memberName = validationResult.MemberNames.First();

                    modelState.AddModelError(
                        string.Format("{0}[{1}].{2}", prefix, id, memberName),
                        validationResult.ErrorMessage);
                }
            }

            return modelState.Values.SelectMany(v => v.Errors).Count() == 0;
        }

        public static ICollection<ValidationResult> Validate(object model)
        {
            ICollection<ValidationResult> result = new List<ValidationResult>();
            bool validateAllProperties = true;
            Validator.TryValidateObject(model, new ValidationContext(model), result, validateAllProperties);
            return result;
        }
    }
}
