using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyFinances.Core.Tokens;

namespace MyFinances.Api.Integrations
{
    [Route("integration/[controller]")]
    [ApiController]
    public class TrueLayerController : ControllerBase
    {
        private readonly IUserTokenService userTokenService;
        private readonly IStateDecoder stateDecoder;

        public TrueLayerController(IUserTokenService userTokenService, IStateDecoder stateDecoder)
        {
            this.userTokenService = userTokenService;
            this.stateDecoder = stateDecoder;
        }

        [HttpGet("callback")]
        public async Task Callback(string code, string scope, string state)
        {
            // Since we do not have a user registration end point using use email as user identifier
            var requestState = stateDecoder.Decode(state);
            await userTokenService.GenerateTokenAsync(requestState.UserEmail, code);
        }
    }
}