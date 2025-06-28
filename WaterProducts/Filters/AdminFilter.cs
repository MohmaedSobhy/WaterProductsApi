using Microsoft.AspNetCore.Mvc.Filters;

namespace WaterProducts.Filters
{
    public class AdminFilter : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            
        }
    }
}
