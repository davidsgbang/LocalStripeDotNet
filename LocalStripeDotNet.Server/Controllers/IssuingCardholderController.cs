using System;
using System.Threading.Tasks;
using LocalStripeDotNet.Server.Facades;
using LocalStripeDotNet.Server.Generators;
using LocalStripeDotNet.Server.Repositories;
using Microsoft.AspNetCore.Mvc;
using Stripe.Issuing;
using IssuingCardholder = Stripe.Issuing.Cardholder;

namespace LocalStripeDotNet.Server.Controllers
{
    [ApiController]
    [Route("v1/issuing/cardholders")]
    [Consumes("application/x-www-form-urlencoded")]
    public class IssuingCardholderController : ControllerBase
    {
        private readonly IssuingCardholderFacade issuingCardholderFacade;

        public IssuingCardholderController(IssuingCardholderFacade issuingCardholderFacade)
        {
            this.issuingCardholderFacade = issuingCardholderFacade;
        }
        
        [HttpGet]
        [Route("{id}")]
        public ActionResult<IssuingCardholder> GetIssuingCardholder(string id)
        {
            try
            {
                var cardholder = this.issuingCardholderFacade.GetIssuingCardholder(id);
                return this.Ok(cardholder);
            }
            catch (ArgumentException ex)
            {
                return this.NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Cardholder>> CreateIssuingCardholder(
            [FromForm] CardholderCreateOptions cardholderCreateOptions)
        {
            var cardholder = 
                await this.issuingCardholderFacade.CreateIssuingCardholder(cardholderCreateOptions);

            return this.Ok(cardholder);
        }
        
        [HttpPost]
        [Route("{id}")]
        public async Task<ActionResult<IssuingCardholder>> UpdateIssuingCardholder(
            string id,
            [FromForm] CardholderUpdateOptions cardholderUpdateOptions)
        {
            try
            {
                var updatedCardholder =
                    await this.issuingCardholderFacade.UpdateIssuingCardholder(id, cardholderUpdateOptions);

                return this.Ok(updatedCardholder);
            }
            catch (ArgumentException ex)
            {
                return this.BadRequest(ex.Message);
            }

        }
    }
}