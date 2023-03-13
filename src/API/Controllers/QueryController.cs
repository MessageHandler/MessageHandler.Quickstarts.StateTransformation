using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("purchaseorders")]
    public class QueryController : ControllerBase
    {
        private BookingRegistry registry;

        public QueryController(BookingRegistry registry)
        {
            this.registry = registry;
        }

        [HttpGet("{bookingId}")]
        //[Authorize]
        public async Task<IActionResult> Get([FromRoute] string bookingId)
        {
            var booking = registry.Get(bookingId);

            if (booking == null) return NotFound();

            // state transformation, turns the state of a booking process into a purchase order model

            var purchaseOrder = new PurchaseOrder
            {
                PurchaseOrderReference = booking.PurchaseOrderReference,
                BuyerReference = booking.BuyerReference,
                Status = booking.Status ?? "Pending",
                OrderLines = booking.OrderLines.Select(ol => new OrderLine
                {
                    OrderLineId = ol.OrderLineId,
                    Quantity = ol.Quantity,
                    OrderedItem = new Item
                    {
                        ItemId = ol.OrderedItem.ItemId,
                        CatalogId = ol.OrderedItem.CatalogId,
                        CollectionId = ol.OrderedItem.CollectionId,
                        Name = ol.OrderedItem.Name,
                        Price = new Price()
                        {
                            Currency = ol.OrderedItem.Price.Currency,
                            Value = ol.OrderedItem.Price.Value
                        }
                    }
                }).ToList()
            };

            return Ok(purchaseOrder);
        }
    }
}