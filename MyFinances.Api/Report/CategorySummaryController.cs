using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyFinances.Api.Report.Contracts;

namespace MyFinances.Api.Report
{
    [Route("users/{userIdentifier}/transactions/[controller]")]
    [ApiController]
    public class CategorySummaryController : ControllerBase
    {
        private readonly Core.Transactions.ITransactionSummaryBuilder transactionSummaryBuilder;
        private readonly ITransactionSummaryContractMapper transactionSummaryContractMapper;

        public CategorySummaryController(Core.Transactions.ITransactionSummaryBuilder transactionSummaryBuilder, 
                                            ITransactionSummaryContractMapper transactionSummaryContractMapper)
        {
            this.transactionSummaryBuilder = transactionSummaryBuilder;
            this.transactionSummaryContractMapper = transactionSummaryContractMapper;
        }

        [HttpGet]
        public async Task<ActionResult<TransactionsSummary>> Generate(string userIdentifier)
        {
            // TODO: Using Utc?
            DateTime from = DateTime.Now.AddDays(-6).Date;
            DateTime to = DateTime.Now;
            var summaryModel = await transactionSummaryBuilder.BuildAsync(userIdentifier,from, to);
            return new JsonResult(transactionSummaryContractMapper.MapToContract(summaryModel, from, to));
        }
    }
}