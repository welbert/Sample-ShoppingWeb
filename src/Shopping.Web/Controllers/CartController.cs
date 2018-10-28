using System;
using System.Web.Mvc;

namespace Shopping.Web.Controllers
{
   using Contracts;
   using System.Threading.Tasks;
   using ViewModels;


   public class CartController : Controller
   {
      // GET: Cart
      public ActionResult Index()
      {
         return View();
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<ActionResult> Submit(AddItemViewModel model, string option)
      {
         switch (option)
         {
            case "Add":
               await MvcApplication.Bus.Publish<CartItemAdded>(new
               {
                  UserName = model.UserName ?? "Unknown",
                  Timestamp = DateTime.UtcNow,

               });
               break;

            case "Finish":
               await MvcApplication.Bus.Publish<OrderSubmitted>(new
               {
                  OrderId = Guid.NewGuid(),
                  Timestamp = DateTime.UtcNow,
                  UserName = model.UserName ?? "Unknown"
               });
               break;

            case "Remove":
               await MvcApplication.Bus.Publish<CartRemoved>(new
               {
                  Timestamp = DateTime.UtcNow,
                  UserName = model.UserName ?? "Unknown"
               });
               break;

            default:
               break;
         }



         return View("Index");
      }
   }
}