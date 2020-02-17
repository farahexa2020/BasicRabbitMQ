using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicroRabbit.transfer.Application.Intefaces;
using MicroRabbit.Transfer.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MicroRabbit.Transfer.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransferController : ControllerBase
    {
        public readonly ITransferService _transferService;

        public TransferController(ITransferService transferService)
        {
            this._transferService = transferService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<TransferLog>> Get()
        {
            return Ok(this._transferService.GetTransferLogs());
        }
    }
}
