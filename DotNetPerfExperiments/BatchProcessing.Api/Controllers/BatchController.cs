using System;
using System.Threading;
using BatchProcessing.Api.Contracts.Requests;
using BatchProcessing.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BatchProcessing.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BatchController : ControllerBase
    {
        private readonly IBatchProcessorService batchProcessorService;

        public BatchController(IBatchProcessorService batchProcessorService)
        {
            this.batchProcessorService = batchProcessorService ?? throw new ArgumentNullException(nameof(batchProcessorService));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(void))]
        public IActionResult Post(UploadBatchRequest request, CancellationToken cancellationToken)
        {
            batchProcessorService.Process(request.ContentBase64, cancellationToken);

            return NoContent();
        }
    }
}