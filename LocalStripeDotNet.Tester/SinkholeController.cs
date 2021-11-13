using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LocalStripeDotNet.Tester
{
    [ApiController]
    [Route("")]
    public class SinkholeController : ControllerBase
    {
        [HttpPost]
        public Task ConsumeWebhook([FromBody]object context) 
        {
            Console.WriteLine(context);
            return Task.CompletedTask;
        }
    }
}