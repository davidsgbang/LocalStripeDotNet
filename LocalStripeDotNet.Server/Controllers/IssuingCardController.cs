using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using LocalStripeDotNet.Server.Facades;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Stripe;
using CardCreateOptions = Stripe.Issuing.CardCreateOptions;
using CardUpdateOptions = Stripe.Issuing.CardUpdateOptions;
using IssuingCard = Stripe.Issuing.Card;
using IssuingCardholder = Stripe.Issuing.Cardholder;

namespace LocalStripeDotNet.Server.Controllers
{
    [ApiController]
    [Route("v1/issuing/cards")]
    [Consumes("application/x-www-form-urlencoded")]
    public class IssuingCardController : ControllerBase
    {
        private readonly IssuingCardFacade issuingCardFacade;
        
        public IssuingCardController(IssuingCardFacade issuingCardFacade)
        {
            this.issuingCardFacade = issuingCardFacade;
        }
        
        [HttpGet]
        [Route("{id}")]
        public ActionResult<IssuingCard> GetIssuingCard(string id)
        {
            try
            {
                var card = this.issuingCardFacade.GetIssuingCard(id);
                return this.Ok(card);
            }
            catch (ArgumentException)
            {
                return this.NotFound();
            }
        }

        [HttpPost]
        public async Task<StripeResponse> CreateIssuingCard(
            [FromForm] CardCreateOptions cardCreateOptions)
        {
            try
            {
                var card = await this.issuingCardFacade.CreateIssuingCard(cardCreateOptions);
                return new StripeResponse(
                    HttpStatusCode.OK, 
                    new HttpResponseMessage().Headers, 
                    JsonConvert.SerializeObject(card));
            }
            catch (Exception ex)
            {
                return new StripeResponse(
                    HttpStatusCode.BadRequest,
                    new HttpResponseMessage().Headers, 
                    null);
            }
        }
        
        [HttpPost]
        [Route("{id}")]
        public async Task<ActionResult<IssuingCard>> UpdateIssuingCard(
            string id,
            [FromForm] CardUpdateOptions cardUpdateOptions)
        {
            try
            {
                var updatedCard = await this.issuingCardFacade.UpdateIssuingCard(id, cardUpdateOptions);
                return this.Ok(updatedCard);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
    }
}