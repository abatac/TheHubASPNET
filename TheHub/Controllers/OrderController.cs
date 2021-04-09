using Swashbuckle.Swagger.Annotations;
using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Script.Serialization;
using TheHub.Errors;
using TheHub.Models;
using TheHub.Services;

namespace TheHub.Controllers
{
    public class OrderController : ApiController
    {
        private readonly IOrderService orderService = new OrderService();

        /// <summary>
        ///     Creates an order
        /// </summary>
        /// <param name="order"></param>
        /// <response code="200">Order is successfully created.</response>
        /// <response code="400">If the order has invalid data or no data on required fields.</response>
        /// <response code="401">User is not authorized.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPost]
        [Authorize]
        [Route("api/order/add")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(OrderResponse))]
        [SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(OrderResponse))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(OrderResponse))]
        public IHttpActionResult Add([FromBody] Order order)
        {

            // TODO: Remove this logging 
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(ConfigurationManager.AppSettings["LogFileHttp"], true))
            {
                var json = new JavaScriptSerializer().Serialize(order);
                file.WriteLine(DateTime.Now.ToString() + " " + Request.Method.ToString() + " /api/order/add " + " Request Content: " + json);
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => (e.Exception != null ? e.Exception.Message : e.ErrorMessage)).ToList();
                throw new HttpResponseException(ResponseUtility.CreateHttpResponseMessage(errors, System.Net.HttpStatusCode.BadRequest));
            }

            return Ok(orderService.Add(order));
        }

        /// <summary>
        ///     Gets shipment details
        /// </summary>
        /// <param name="salesforceOrderNumber"></param>
        /// <response code="200"></response>
        /// <response code="401">User is not authorized</response>
        /// <response code="500">Internal server error.</response>
        /// <response code="404">Resource is not found.</response>
        [HttpGet]
        [Authorize]
        [Route("api/order/shipment")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(Shipment))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(ErrorResponse))]
        [SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(ErrorResponse))]
        public IHttpActionResult GetShipment([FromUri(Name = "salesforce_order_number")]
            string salesforceOrderNumber)
        {

			// TODO: Remove this logging 
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(ConfigurationManager.AppSettings["LogFileHttp"], true))
            {
                file.WriteLine(DateTime.Now.ToString() + " " + Request.Method.ToString() + " /api/order/shipment " + " Request Content: " + salesforceOrderNumber);
            }

            return Ok(orderService.GetShipmentDetails(salesforceOrderNumber));
        }

        /// <summary>
        /// </summary>
        /// <param name="orderStatus"></param>
        /// <response code="401">User is not authorized</response>
        /// <response code="500">Internal server error.</response>
        [HttpPost]
        [Authorize]
        [Route("api/order/status")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(ErrorResponse))]
        public async Task<IHttpActionResult> OrderStatusAsync([FromBody] OrderStatusRequest orderStatus)
        {
			// TODO: Remove this logging 
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(ConfigurationManager.AppSettings["LogFileHttp"], true))
            {
                var json = new JavaScriptSerializer().Serialize(orderStatus);
                file.WriteLine(DateTime.Now.ToString() + " " + Request.Method.ToString() + " /api/order/status " + " Request Content: " + json);
            }

            var orderStatusResponse = await orderService.OrderStatus(orderStatus);
            return Ok(orderStatusResponse);
        }
    }
}