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
    public class TradeController : ApiController
    {
        [HttpPost]
        [Route("trade/getdata/{username}/{lasttimestamp}")]
        public WSResponse GetTradeData(string username, string lasttimestamp)
        {
            WSResponse response=new WSResponse();

            try
            {
                DateTime dt = DateTime.ParseExact(lasttimestamp, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                response = SqlRepository.GetTradeData(username, dt);
            }
            catch (Exception x)
            {
                ErrorHandling.LogException(x);
                response.MESSAGE = "Problem while getting trade data";
                response.RESPONSE = false;
            }

            return response;
        }

        [HttpPost]
        [Route("trade/getdata2/{username}/{lasttimestamp}")]
        public WSResponse getTradeData(string username, string lasttimestamp)
        {
            WSResponse response = new WSResponse();

            try
            {
                DateTime dt = DateTime.ParseExact(lasttimestamp, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                response = SqlRepository.GetTradeData2(username, dt);
            }
            catch (Exception x)
            {
                ErrorHandling.LogException(x);
                response.MESSAGE = "Problem while getting trade data";
                response.RESPONSE = false;
            }

            return response;
        }
    }
}
