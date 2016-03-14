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
    public class UserController : ApiController
    {
         
        [HttpPost]
        [Route("user/checklogin")]
        public HttpResponseMessage CheckLogin(UserWeb userWeb)
        {
            HttpResponseMessage response = null;

            WSResponse user = new WSResponse();

            user = SqlRepository.CheckLogin(userWeb.UserName, userWeb.UserPassword);

            if (user != null)
            {
                response = Request.CreateResponse(HttpStatusCode.OK, user, "application/json");
                return response;
            }
            else
            {
                response = Request.CreateResponse(HttpStatusCode.OK, user, "application/json");
                return response;
            }
        }

        [HttpPost]
        [Route("user/register")]
        public HttpResponseMessage RegisterUser(tblUserGcm userGcm)
        {

            HttpResponseMessage response = null;

            WSResponse user = new WSResponse();

            user = SqlRepository.RegisterUser(userGcm.ClientId,userGcm.GCMId);
            
            response = Request.CreateResponse(HttpStatusCode.OK, user, "application/json");

            return response;
        }

        [HttpPost]
        [Route("user/delete")]
        public HttpResponseMessage DeleteUser(tblUserGcm userGcm)
        {

            HttpResponseMessage response = null;

            WSResponse user = new WSResponse();

            user = SqlRepository.DeleteUser(userGcm.ClientId);

            response = Request.CreateResponse(HttpStatusCode.OK, user, "application/json");

            return response;
        }

        [HttpPost]
        [Route("user/updatelastmessagetimestamp/{username}")]
        public WSResponse UpdateLastMessageTimestamp(string username)
        {
            WSResponse response = new WSResponse();

            try
            {
                response = SqlRepository.UpdateLastMessageTimestamp(username);
            }
            catch (Exception x)
            {
                ErrorHandling.LogException(x);
                response.MESSAGE = "Problem while Updateing lastmessagetimestamp";
                response.RESPONSE = false;
            }

            return response;
        }
    }
}
