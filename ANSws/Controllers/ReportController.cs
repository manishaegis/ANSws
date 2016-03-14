using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ANSws.Models;
using ANSws.Repositories;

namespace ANSws.Controllers
{
    public class ReportController : ApiController
    {
        [HttpPost]
        [Route("report/getledgerdetails")]
        public WSResponse GetLedgerDetails(LedgerDetail oLedgerDetail)
        {
            WSResponse response = new WSResponse();

            try
            {
                DateTime dt = DateTime.ParseExact(oLedgerDetail.Date, "yyyyMMdd", CultureInfo.InvariantCulture);
                response = ReportRepository.GetLedgerDetail(oLedgerDetail);
            }
            catch (Exception x)
            {
                ErrorHandling.LogException(x);
                response.MESSAGE = "Problem while getting Ledger Details";
                response.RESPONSE = false;
            }

            return response;
        }
    }
}
