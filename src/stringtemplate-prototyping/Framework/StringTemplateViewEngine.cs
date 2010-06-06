using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Antlr3.ST;

namespace stringtemplate_prototyping.Framework
{
    public class StringTemplateGroupProvider
    {
        private readonly string _viewPath;

        public StringTemplateGroupProvider(string viewPath)
        {
            _viewPath = viewPath;
            LoadGroup();
            var watcher = new FileSystemWatcher(viewPath);
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
           | NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.CreationTime;
            watcher.IncludeSubdirectories = true;

            watcher.Changed += (sender, e) => LoadGroup();
            watcher.Created += (sender, e) => LoadGroup();
            watcher.Deleted += (sender, e) => LoadGroup();
            watcher.Renamed += (sender, e) => LoadGroup();
            watcher.EnableRaisingEvents = true;
        }

        private void LoadGroup()
        {
            _group = new StringTemplateGroup("views", _viewPath);
            _group.ErrorListener = new StringTemplateErrorListener();
        }

        private StringTemplateGroup _group;

        public StringTemplateGroup Group
        {
            get
            {
                return _group;
            }
        }
    }

    internal class StringTemplateErrorListener : IStringTemplateErrorListener
    {
        public void Error(string msg, Exception e)
        {
            throw new Exception("String template went wrong: " + msg, e);
        }

        public void Warning(string msg)
        {
            Debug.WriteLine("StringTemplate warning " + msg);
        }
    }

    /// <summary>
    /// The ViewEngine for StringTemplate Views
    /// </summary>
    public class StringTemplateViewEngine : IViewEngine
    {

        private readonly StringTemplateGroupProvider groupProvider;

        public StringTemplateViewEngine(StringTemplateGroupProvider groupProvider)
        {
            this.groupProvider = groupProvider;
        }

        #region IViewEngine Members
        /// <summary>
        /// Locates a view.
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="partialViewName"></param>
        /// <param name="useCache"></param>
        /// <returns></returns>
        public ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            return LoadView(controllerContext, partialViewName);
        }

        /// <summary>
        /// Locates a view
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="viewName"></param>
        /// <param name="masterName"></param>
        /// <param name="useCache"></param>
        /// <returns></returns>
        public ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            return LoadView(controllerContext, viewName);
        }

        /// <summary>
        /// Loads a view instance from the Group object
        /// </summary>
        /// <param name="controllerContext">The calling controller</param>
        /// <param name="viewName">The name of the view</param>
        /// <returns>a ViewEngineResult</returns>
        private ViewEngineResult LoadView(ControllerContext controllerContext, string viewName)
        {
            //load template from loader
            StringTemplate template;
            var match = Regex.Match(viewName, "^~/?(.*)");
            if (match.Success)
            {
                viewName = match.Groups[1].Value;
            }
            else
            {
                var controllerName = controllerContext.Controller.GetType().Name.Replace("Controller", "");
                viewName = string.Format("{0}/{1}", controllerName, viewName);
            }
            template = groupProvider.Group.GetInstanceOf(viewName);
            if(template==null)
            {
                throw new ArgumentException("Cannot find template: " + viewName + " please make sure it's in the views folder");
            }

            //return view result
            return new ViewEngineResult(new StringTemplateView(template), this);
        }

        /// <summary>
        /// Not used. String templates are cached by the static group object.
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="view"></param>
        public void ReleaseView(ControllerContext controllerContext, IView view) { }
        #endregion
    }
}
