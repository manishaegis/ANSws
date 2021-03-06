﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.ServiceModel.PeerResolvers;
using System.Web;
using System.Web.Mvc;
using ANSws.Models;
using ANSws.Utility;
using Newtonsoft.Json;
using log = ANSws.ErrorHandling;

namespace ANSws.Repositories
{
    public class SqlRepository
    {
        private static anspWMsEntities db = new anspWMsEntities();
        public static WSResponse CheckLogin(string userName, string password)
        {
            UserWeb user = null;
            WSResponse wsResponse = new WSResponse();
            wsResponse.RESULT = string.Empty;
            wsResponse.MESSAGE = string.Empty;
            wsResponse.RESPONSE = false;

            try
            {
                user = db.UserWebs.AsNoTracking().FirstOrDefault(u => u.UserName.Trim() == userName.Trim());
            }
            catch (Exception x)
            {
                log.LogException(x);
            }

            if (user != null )
            {
                // user exist
                if (user.UserPassword.Trim() == password.Trim())
                {
                    //pass is same -- valid user
                    var u = new
                    {
                        UserName = user.UserName.Trim(),
                        UserFullName = user.UserFullName.Trim(),
                        UserGroupType = user.UserGroupType
                    };
                    wsResponse.RESULT = JsonConvert.SerializeObject(u);
                    wsResponse.RESPONSE = true;
                    wsResponse.MESSAGE = "User Login Successful";
                }
                else
                {
                    wsResponse.MESSAGE = "The password you entered is incorrect";
                }
            }
            else
            {
                wsResponse.MESSAGE = "User does not exist";
            }

            return wsResponse;
        }

        public static WSResponse RegisterUser(string userName, string gcmId)
        {
            WSResponse response = new WSResponse();
            response.RESULT = string.Empty;
            response.MESSAGE = string.Empty;
            response.RESPONSE = false;

            try
            {
                tblUserGcm userGcm = db.tblUserGcms.Where(u => u.ClientId.Trim() == userName.Trim()).AsNoTracking().FirstOrDefault();

                if (userGcm != null)
                {
                    // update gcm id here
                    userGcm.GCMId = gcmId;
                    userGcm.LastMessageTimestamp = DateTime.Now;
                    db.Entry(userGcm).State = EntityState.Modified;
                    db.SaveChanges();

                    response.RESULT = JsonConvert.SerializeObject(userGcm);
                    response.RESPONSE = true;
                    response.MESSAGE = "User updated sucessfully";
                }
                else
                {
                    // insert new tblusergcm
                    userGcm = new tblUserGcm();
                    userGcm.ClientId = userName.Trim();
                    userGcm.GCMId = gcmId;
                    userGcm.LastMessageTimestamp = DateTime.Now;

                    db.tblUserGcms.Add(userGcm);
                    db.SaveChanges();

                    response.RESULT = JsonConvert.SerializeObject(userGcm);
                    response.RESPONSE = true;
                    response.MESSAGE = "User registered sucessfully";
                }
            }
            catch (Exception x)
            {
                log.LogException(x);
            }
            return response;
        }

        public static WSResponse DeleteUser(string userName)
        {
            WSResponse response = new WSResponse();
            response.RESULT = string.Empty;
            response.MESSAGE = string.Empty;
            response.RESPONSE = false;

            try
            {
                tblUserGcm userGcm = db.tblUserGcms.Where(u => u.ClientId.Trim() == userName.Trim()).AsNoTracking().FirstOrDefault();

                if (userGcm != null)
                {
                    // Delete here
                    db.tblUserGcms.Remove(userGcm);
                    db.SaveChanges();
                    
                    response.RESULT = userGcm.ClientId;
                    response.MESSAGE = "User Deleted Successfully";
                    response.RESPONSE = true;
                }
            }
            catch (Exception x)
            {
                response.RESULT = userName;
                response.MESSAGE = "Problem while deleting User";
                response.RESPONSE = false;
                log.LogException(x);
            }

            return response;
        }

        public static WSResponse GetTradeData(string userName,DateTime lastTimeStamp)
        {
            WSResponse response = new WSResponse();
            response.RESULT = string.Empty;
            response.MESSAGE = string.Empty;
            response.RESPONSE = false;

            List<tblTrade> lstTrades = new List<tblTrade>();

            try
            {
                lstTrades = db.tblTrades.Where(t => t.Client.Trim() == userName.Trim() && t.TradeTimestamp > lastTimeStamp).AsNoTracking().ToList();
                lstTrades.AddRange(db.tblTrades.Where(t => t.Buy_Sell_Message.Trim().ToLower() == "m" && t.TradeTimestamp > lastTimeStamp).AsNoTracking());

                response.MESSAGE = lstTrades.Count > 0 ? "Records Found" : "No Records Found";
                response.RESPONSE = lstTrades.Count > 0;
            }
            catch (Exception x)
            {
                log.LogException(x);
                
                response.MESSAGE = "Problem while getting trade data";
                response.RESPONSE = false;
            }

            response.RESULT = JsonConvert.SerializeObject(lstTrades);

            return response;
        }

        public static WSResponse GetTradeData2(string userName, DateTime lastTimeStamp)
        {
            WSResponse response = new WSResponse();
            response.RESULT = string.Empty;
            response.MESSAGE = string.Empty;
            response.RESPONSE = false;
            
            DataTable dt = new DataTable();

            try
            {                
                const string q = @"SELECT * FROM tblTrade WHERE Client = @pClient AND TradeTimestamp > @pTradeTimestamp
                                    UNION ALL
                                   SELECT * FROM tblTrade WHERE LOWER(LTRIM(RTRIM(Buy_Sell_Message))) = 'm' AND TradeTimestamp > @pTradeTimestamp";
                
                SqlCommand cmd = new SqlCommand(q);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@pClient", userName);
                cmd.Parameters.AddWithValue("@pTradeTimestamp", lastTimeStamp);

                dt = Util.GetDataTableFromCommandanspWMs(cmd);

                response.RESPONSE = dt.Rows.Count > 0;
                response.RESULT = Util.GetJsonFromDataTable(dt);
                response.MESSAGE = dt.Rows.Count <= 0 ? "No Record(s) Found" : "Records Found";
            }
            catch (Exception x)
            {
                log.LogException(x);

                response.MESSAGE = "Problem while getting trade data";
                response.RESPONSE = false;
                response.RESULT = string.Empty;
            }            

            return response;
        }

        public static DataTable GetFamilyTradeData(List<string> lstUserName)
        {
            DataTable dt = new DataTable();

            try
            {
                const string q = @"SELECT distinct Client FROM tblTrade WHERE Client in ( {0} )";
                string csvUsers = string.Empty;
                int i = 1;

                SqlCommand cmd = new SqlCommand();

                foreach (string userName in lstUserName)
                {
                    csvUsers += "@ClientId" + i + ",";
                    cmd.Parameters.AddWithValue("@ClientId" + i, userName);
                    i++;
                }

                if (!string.IsNullOrEmpty(csvUsers))
                {
                    csvUsers = csvUsers.TrimEnd(',');
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = string.Format(q,csvUsers);
                    dt = Util.GetDataTableFromCommandanspWMs(cmd);
                }
            }
            catch(Exception x)
            {
                log.LogException(x);
            }

            return dt;
        }

        public static WSResponse UpdateLastMessageTimestamp(string userName)
        {
            WSResponse response = new WSResponse();

            try
            {
                tblUserGcm userGcm = db.tblUserGcms.AsNoTracking().FirstOrDefault(u => u.ClientId.Trim() == userName.Trim());

                if (userGcm != null)
                {
                    userGcm.LastMessageTimestamp = DateTime.Now;
                    db.Entry(userGcm).State = EntityState.Modified;
                    db.SaveChanges();
                    
                    response.RESPONSE = true;
                    response.MESSAGE = "LastMessageTimestamp updated sucessfully in tblUserGcm";
                }
                else
                {
                    response.MESSAGE = userName + " not found in in tblUserGcm to update LastMessageTimestamp";
                }
            }
            catch (Exception x)
            {
                response.MESSAGE = "Problem while updating LastMessageTimestamp in tblUserGcm";
                response.RESPONSE = false;
                log.LogException(x);
            }

            response.RESULT = userName;

            return response;
        }
        
    }

    
}