using System;
using System.Web.Mvc;
using Antlr3.ST;

namespace stringtemplate_prototyping.Framework
{
    /// <summary>
    /// A view that renders a StringTemplate
    /// </summary>
    public class StringTemplateView : IView
    {
        #region Properties
        /// <summary>
        /// The template associated with this view
        /// </summary>
        private StringTemplate _template { get; set; }
        #endregion

        #region .ctors
        public StringTemplateView(StringTemplate template)
        {
            //null check
            if (template == null) throw new ArgumentNullException("template");

            //set template
            _template = template;
        }
        #endregion

        #region IView Members
        /// <summary>
        /// Renders the string template
        /// </summary>
        /// <param name="viewContext"></param>
        /// <param name="writer"></param>
        public void Render(ViewContext viewContext, System.IO.TextWriter writer)
        {
            //persist the controller's viewdata in the template's attribute store
            foreach (var item in viewContext.Controller.ViewData)
                _template.SetAttribute(item.Key, item.Value);

            //persist the context (so the template has access to it)
            _template.SetAttribute("context", viewContext.HttpContext);

            //render the template to the text writer
            var noIndentWriter = new NoIndentWriter(writer);
            _template.Write(noIndentWriter);
        }

        #endregion
    }
}