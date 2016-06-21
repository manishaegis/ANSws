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
        [Route("report/getLedgerDetails")]
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

        [HttpPost]
        [Route("report/getOpenSeriesData")]
        public WSResponse GetOpenSeriesData(OpenSeriesData oOpenSeriesData)
        {
            WSResponse response = new WSResponse();

            try
            {
                DateTime dt = DateTime.ParseExact(oOpenSeriesData.Date, "yyyyMMdd", CultureInfo.InvariantCulture);
                response = ReportRepository.GetOpenSeriesData(oOpenSeriesData);
            }
            catch (Exception x)
            {
                ErrorHandling.LogException(x);
                response.MESSAGE = "Problem while getting Open Series Data";
                response.RESPONSE = false;
            }

            return response;
        }

        [HttpPost]
        [Route("report/getCollateralsMISData")]
        public WSResponse GetCollateralsMISData(CollateralsMISData oCollateralsMisData)
        {
            WSResponse response = new WSResponse();

            try
            {
                DateTime dt = DateTime.ParseExact(oCollateralsMisData.Date, "yyyyMMdd", CultureInfo.InvariantCulture);
                response = ReportRepository.GetCollateralsMISData(oCollateralsMisData);
            }
            catch (Exception x)
            {
                ErrorHandling.LogException(x);
                response.MESSAGE = "Problem while getting Collaterals MIS Data";
                response.RESPONSE = false;
            }

            return response;
        }

        [HttpPost]
        [Route("report/getDematLedger")]
        public WSResponse GetDematLedger(DematLedger oDematLedger)
        {
            WSResponse response = new WSResponse();

            try
            {
                DateTime dt = DateTime.ParseExact(oDematLedger.Date, "yyyyMMdd", CultureInfo.InvariantCulture);
                response = ReportRepository.GetDematLedger(oDematLedger);
            }
            catch (Exception x)
            {
                ErrorHandling.LogException(x);
                response.MESSAGE = "Problem while getting Demat Ledger";
                response.RESPONSE = false;
            }

            return response;
        }

        [HttpPost]
        [Route("report/getClientInfo")]
        public WSResponse GetClientInfo(ClientInfo oClientInfo)
        {
            WSResponse response = new WSResponse();

            try
            {
                DateTime dt = DateTime.ParseExact(oClientInfo.Date, "yyyyMMdd", CultureInfo.InvariantCulture);
                response = ReportRepository.GetClientInfo(oClientInfo);
            }
            catch (Exception x)
            {
                ErrorHandling.LogException(x);
                response.MESSAGE = "Problem while getting Client Info";
                response.RESPONSE = false;
            }

            return response;
        }

        [HttpPost]
        [Route("report/getDPTrx")]
        public WSResponse GetDPTrx(DPTrx oDpTrx)
        {
            WSResponse response = new WSResponse();

            try
            {
                DateTime dt = DateTime.ParseExact(oDpTrx.Date, "yyyyMMdd", CultureInfo.InvariantCulture);
                response = ReportRepository.GetDPTrx(oDpTrx);
            }
            catch (Exception x)
            {
                ErrorHandling.LogException(x);
                response.MESSAGE = "Problem while getting Client Info";
                response.RESPONSE = false;
            }

            return response;
        }


        [HttpPost]
        [Route("report/getDPHolding")]
        public WSResponse GetDPHolding(DPHolding oDpHolding)
        {
            WSResponse response = new WSResponse();

            try
            {
                DateTime dt = DateTime.ParseExact(oDpHolding.Date, "yyyyMMdd", CultureInfo.InvariantCulture);
                response = ReportRepository.GetDPHolding(oDpHolding);
            }
            catch (Exception x)
            {
                ErrorHandling.LogException(x);
                response.MESSAGE = "Problem while getting Client Info";
                response.RESPONSE = false;
            }

            return response;
        }

        
        [HttpPost]
        [Route("report/getDPLedger")]
        public WSResponse GetDPLedger(DPLedger oDpLedger)
        {
            WSResponse response = new WSResponse();

            try
            {
                DateTime dt = DateTime.ParseExact(oDpLedger.Date, "yyyyMMdd", CultureInfo.InvariantCulture);
                response = ReportRepository.GetDPLedger(oDpLedger);
            }
            catch (Exception x)
            {
                ErrorHandling.LogException(x);
                response.MESSAGE = "Problem while getting Client Info";
                response.RESPONSE = false;
            }

            return response;
        }



    }
}
