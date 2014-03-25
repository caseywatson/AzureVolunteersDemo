using System.Web.Mvc;
using Common;
using Microsoft.ServiceBus.Messaging;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Thanks()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Volunteer model)
        {
            if (ModelState.IsValid == false)
            {
                return View(model);
            }

            // TODO: Setup service bus.
            // For more information on using Azure Service Bus visit http://www.windowsazure.com/en-us/documentation/articles/service-bus-dotnet-how-to-use-topics-subscriptions/.

            TopicClient topicClient = TopicClient.CreateFromConnectionString(
                "Azure Service Bus Namespace Connection String",
                "Azure Service Bus Topic Name");

            topicClient.Send(new BrokeredMessage(model));

            return RedirectToAction("Thanks");
        }
    }
}