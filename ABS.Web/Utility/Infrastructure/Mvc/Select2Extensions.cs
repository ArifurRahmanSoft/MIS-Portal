using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace CTGroup.Web.Utility.Infrastructure.Mvc
{
    public class Select2Result
    {
        public string id { get; set; }

        public string text { get; set; }

        public bool disabled { get; set; }
    }

    public class Select2Options
    {
        // TODO: Not Implemented yet.
        public List<SelectListItem> Data { get; set; }

        public bool Multiple { get; set; }

        public bool Disabled { get; set; }

        public string Placeholder { get; set; }

        public string Url { get; set; }

        public string ParameterName { get; set; }

        public int Delay { get; set; }

        public bool AllowClear { get; set; }

        public int MinimumInputLength { get; set; }

        public int MinimumResultsForSearch { get; set; }

        public int Tags { get; set; }

        public char[] TokenSeparators { get; set; }

        public string Language { get; set; }

        public bool IsRTL { get; set; }


        /// <summary>
        /// Change is fired whenever an option is selected or removed.
        /// </summary>
        public string OnChange { get; set; }

        /// <summary>
        /// Open is fired whenever the dropdown is opened
        /// </summary>
        public string OnOpen { get; set; }

        /// <summary>
        /// Opening is fired before this and can be prevented
        /// </summary>
        public string OnOpening { get; set; }

        /// <summary>
        /// Close is fired whenever the dropdown is closed.
        /// </summary>
        public string OnClose { get; set; }

        /// <summary>
        /// Closing is fired before this and can be prevented.
        /// </summary>
        public string OnClosing { get; set; }

        /// <summary>
        /// Select is fired whenever a result is selected. 
        /// </summary>
        public string OnSelect { get; set; }

        /// <summary>
        /// Selecting is fired before this and can be prevented.
        /// </summary>
        public string OnSelecting { get; set; }

        /// <summary>
        /// Unselect is fired whenever a result is unselected. 
        /// </summary>
        public string OnUnselect { get; set; }

        /// <summary>
        /// Unselecting is fired before this and can be prevented.
        /// </summary>
        public string OnUnselecting { get; set; }
    }


    public static class Select2Extensions
    {
        #region Select2

        public static MvcHtmlString Select2<TModel>(this HtmlHelper<TModel> htmlHelper, string name, Select2Options options, object htmlAttributes)
        {
            return getSelect2(htmlHelper, name, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes), options);
        }

        public static MvcHtmlString Select2<TModel>(this HtmlHelper<TModel> htmlHelper, string name, Select2Options options, IDictionary<string, object> htmlAttributes)
        {
            return getSelect2(htmlHelper, name, htmlAttributes, options);
        }

        public static MvcHtmlString Select2<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, Select2Options options, object htmlAttributes)
        {
            return getSelect2(htmlHelper, expression, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes), options);
        }

        public static MvcHtmlString Select2<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, Select2Options options, IDictionary<string, object> htmlAttributes)
        {
            return getSelect2(htmlHelper, expression, htmlAttributes, options);
        }

        private static List<SelectListItem> getSelectListItem(Select2Options options)
        {
            if (options.Data == null)
                return new List<SelectListItem>();

            return options.Data;
        }

        private static MvcHtmlString getSelect2<TModel>(this HtmlHelper<TModel> htmlHelper, string name, IDictionary<string, object> htmlAttribute, Select2Options options)
        {
            IDictionary<string, object> attributes = BuildSelect2Attributes(htmlAttribute, options);
            return htmlHelper.DropDownList(name, selectList: getSelectListItem(options), htmlAttributes: attributes);
        }

        private static MvcHtmlString getSelect2<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttribute, Select2Options options)
        {
            IDictionary<string, object> attributes = BuildSelect2Attributes(htmlAttribute, options);
            return htmlHelper.DropDownListFor(expression, getSelectListItem(options), htmlAttributes: attributes);
        }

        private static IDictionary<string, object> BuildSelect2Attributes(IDictionary<string, object> attributes, Select2Options options)
        {
            if (attributes == null)
                attributes = new Dictionary<string, object>();

            if (attributes.ContainsKey("class"))
                attributes["class"] += " " + "swazer-select2"; // the space is very important before the class name, do not remove it
            else
                attributes.Add("class", "swazer-select2");

            attributes.Add("data-placeholder", options.Placeholder);
            attributes.Add("data-url", options.Url);
            attributes.Add("data-param-name", options.ParameterName);
            attributes.Add("data-delay", options.Delay);
            attributes.Add("data-allow-clear", options.AllowClear);
            attributes.Add("data-minimum-input-length", options.MinimumInputLength);
            attributes.Add("data-miminum-results-for-search", options.MinimumResultsForSearch);
            attributes.Add("data-theme", "bootstrap");
            attributes.Add("data-tags", options.Tags);
            attributes.Add("data-token-separators", options.TokenSeparators);
            attributes.Add("data-language", options.Language);

            attributes.Add("data-on-change", options.OnChange);
            attributes.Add("data-on-open", options.OnOpen);
            attributes.Add("data-on-opening", options.OnOpening);
            attributes.Add("data-on-close", options.OnClose);
            attributes.Add("data-on-closing", options.OnClosing);
            attributes.Add("data-on-select", options.OnSelect);
            attributes.Add("data-on-selecting", options.OnSelecting);
            attributes.Add("data-on-unselect", options.OnUnselect);
            attributes.Add("data-on-unselecting", options.OnUnselecting);

            if (options.IsRTL)
                attributes.Add("data-dir", "rtl");
            else
                attributes.Add("data-dir", "ltr");

            if (options.Multiple)
                attributes.Add("multiple", "multiple");

            if (options.Disabled)
                attributes.Add("disabled", "disabled");

            return attributes;
        }

        #endregion

    }
}


