using API.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace ComponentTests
{
    public class WhileTransformingStateThroughAPI
    {
        [Fact]
        public async Task GivenBookingState_WhenTransforming_ThenShouldTransformIntoPurchaseOrder()
        {
            // given
            var registry = new BookingRegistry();

            // when
            var controller = new QueryController(registry);

            var actionResult = await controller.Get("1aa6ab11-a111-4687-a6e0-cbcf403bc6a8");

            // then
            actionResult.Should().BeOfType<OkObjectResult>();
            ((OkObjectResult)actionResult).Value.Should().BeOfType<PurchaseOrder>();
        }
    }
}