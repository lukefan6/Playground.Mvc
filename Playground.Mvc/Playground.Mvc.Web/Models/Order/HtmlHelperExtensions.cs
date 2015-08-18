using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Playground.Mvc.Web.Models.Order
{
    public static class HtmlHelperExtensions
    {
        public static void RenderDictionaryItem<TModel, TValue>(
            this HtmlHelper<TModel> htmlHelper,
            string partialViewName,
            Expression<Func<TModel, IDictionary<string, TValue>>> expression,
            string dictionaryKey)
            where TValue : OrderViewModelBase
        {
            string memberName = expression.ToMemberExpression().Member.Name;
            var parent = htmlHelper.ViewData.Model;
            var model = expression.Compile().Invoke(parent)[dictionaryKey];
            model.DictionaryRepresentationPrefix = memberName;

            if (parent is OrderViewModelBase)
            {
                model.DictionaryRepresentationPrefix = string.Format("{0}[{1}].{2}",
                    (parent as OrderViewModelBase).DictionaryRepresentationPrefix,
                    (parent as OrderViewModelBase).ViewModelId,
                    model.DictionaryRepresentationPrefix);
            }

            htmlHelper.RenderPartial(partialViewName, model);
        }

        public static MvcHtmlString TextBoxForOrderViewModel<TModel, TValue>(
            this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression,
            object htmlAttributes) where TModel : OrderViewModelBase
        {
            return html.TextBox(
                html.GetDictionaryRepresentation(expression),
                html.GetMemberValue(expression),
                htmlAttributes);
        }

        public static MvcHtmlString LabelForOrderViewModel<TModel, TValue>(
            this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression,
            object htmlAttributes) where TModel : OrderViewModelBase
        {
            var displayAttribute = expression.ToMemberExpression().Member
                .GetCustomAttributes(typeof(DisplayAttribute), false)
                .Cast<DisplayAttribute>()
                .FirstOrDefault();

            string labelText = (displayAttribute == null)
                ? expression.ToMemberExpression().Member.Name
                : displayAttribute.Name;

            return html.Label(html.GetDictionaryRepresentation(expression), labelText, htmlAttributes);
        }

        public static MvcHtmlString HiddenForOrderViewModel<TModel, TValue>(
            this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression) where TModel : OrderViewModelBase
        {
            return html.Hidden(html.GetDictionaryRepresentation(expression), html.GetMemberValue(expression));
        }

        public static MvcHtmlString EnumDropDownListForOrderViewModel<TModel, TEnum>(
            this HtmlHelper<TModel> html,
            Expression<Func<TModel, TEnum>> expression,
            string optionLabel,
            object htmlAttributes,
            bool excludeZeroValue = true) where TModel : OrderViewModelBase
        {
            var enumType = typeof(TEnum).ToUnderlyingTypeIfNullable();
            if (!enumType.IsEnum)
            {
                throw new NotSupportedException(string.Format(
                    "EnumDropDownListFor does not support this member type: <{0}>",
                    typeof(TEnum).FullName));
            }

            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);

            var items = Enum.GetValues(enumType)
                .Cast<TEnum>()
                .Where(value => excludeZeroValue ? Convert.ToInt32(value) != 0 : true)
                .Select(value => new SelectListItem
                {
                    Text = value.ToString(),
                    Value = value.ToString(),
                    Selected = value.Equals(metadata.Model)
                });

            return html.DropDownList(
                name: html.GetDictionaryRepresentation(expression),
                selectList: items,
                optionLabel: optionLabel,
                htmlAttributes: htmlAttributes);
        }

        public static MvcHtmlString ValidationMessageForOrderViewModel<TModel, TValue>(
            this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression,
            string validationMessage,
            object htmlAttributes) where TModel : OrderViewModelBase
        {
            return html.ValidationMessage(html.GetDictionaryRepresentation(expression), validationMessage, htmlAttributes);
        }

        private static string GetDictionaryRepresentation<TModel, TValue>(
            this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression) where TModel : OrderViewModelBase
        {
            var model = html.ViewData.Model;

            string prefix = model.DictionaryRepresentationPrefix;
            string id = model.ViewModelId.ToString();
            string memberName = expression.ToMemberExpression().Member.Name;

            return string.Format("{0}[{1}].{2}", prefix, id, memberName);
        }

        public static string GetMemberValue<TModel, TValue>(
            this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression) where TModel : OrderViewModelBase
        {
            object result = expression.Compile().Invoke(html.ViewData.Model);

            var displayFormatAttribute = expression.ToMemberExpression().Member
                .GetCustomAttributes(typeof(DisplayFormatAttribute), false)
                .Cast<DisplayFormatAttribute>()
                .FirstOrDefault();

            if (result != null && displayFormatAttribute != null)
            {
                result = string.Format(displayFormatAttribute.DataFormatString, result);
            }

            return result as string;
        }

        public static MemberExpression ToMemberExpression<T>(this Expression<T> expression)
        {
            if (expression.Body.NodeType != ExpressionType.MemberAccess)
            {
                throw new NotSupportedException(string.Format(
                    "Lambda expression body node type <{0}> not supported",
                    expression.Body.NodeType));
            }

            return expression.Body as MemberExpression;
        }

        public static Type ToUnderlyingTypeIfNullable(this Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return type.GetGenericArguments()[0];
            }

            return type;
        }
    }
}
