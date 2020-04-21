using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyFinances.Core;

namespace MyFinances.Api.Import
{
    [Route("users/{userIdentifier}/[controller]")]
    [ApiController]
    public class ImportController : ControllerBase
    {
        private readonly ITransactionsImporter transactionsImporter;

        public ImportController(ITransactionsImporter transactionsImporter)
        {
            this.transactionsImporter = transactionsImporter;
        }

        [HttpPost]
        public async Task<ActionResult> ImportBankAccounts(string userIdentifier)
        {
            var result = await transactionsImporter.ImportTransactionsAsync(userIdentifier);
            return Ok();
        }
    }
}