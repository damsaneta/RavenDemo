using System.Web.Mvc;
using Demo.Storage.Infrastructure;
using Raven.Client;

namespace Demo.UI.Controllers
{
    public abstract class ControllerBase : Controller
    {
        public IDocumentSessionProvider DocumentSessionProvider { get; set; }

        public IDocumentSession DocumentSession { get { return this.DocumentSessionProvider.Create(); }}
    }
}