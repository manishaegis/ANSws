using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using ANSws.Models;
using ANSws.Utility;
using log = ANSws.ErrorHandling;

namespace ANSws.Repositories
{
    public class ReportRepository
    {
        /// <summary>
        /// Get Ledger Details from username and date
        /// </summary>
        /// <param name="userName">financial start date's year</param>
        /// <param name="date">DateTime obj</param>
        /// <returns>object of WSResponse</returns>
        public static WSResponse GetLedgerDetail(LedgerDetail oLedgerDetail)
        {
            WSResponse response = new WSResponse();

            try
            {
                DateTime date = DateTime.ParseExact(oLedgerDetail.Date, "yyyyMMdd", CultureInfo.InvariantCulture);
                DateTime fyStartDate = Util.FYStartDate(date);
                int year = fyStartDate.Year;

                //const string c_dsXML = "<t_action><row><BController>D_Ledger</BController><BAction>1</BAction><UserGroupType>0</UserGroupType></row></t_action><t_filter><row><TrxDate1>01/Apr/2015</TrxDate1><TrxDate2>09/Dec/2015</TrxDate2><AccountCode>A999</AccountCode></row></t_filter>";
                //const string c_dsXML = "<t_action><row><BController>D_Ledger</BController><BAction>1</BAction><UserGroupType>0</UserGroupType></row></t_action><t_filter><row><TrxDate1>{0}</TrxDate1><TrxDate2>{1}</TrxDate2><AccountCode>{2}</AccountCode></row></t_filter>";
                
                //string dsXML = string.Format(c_dsXML, fyStartDate.ToString("dd/MMM/yyyy", CultureInfo.InvariantCulture), date.ToString("dd/MMM/yyyy", CultureInfo.InvariantCulture), userName);
                
                string dsXML = GetXMLforLedgerDetail(oLedgerDetail);

                if (dsXML != string.Empty)
                {
                    DataTable dt = new DataTable();
                    SqlCommand cmd = new SqlCommand("btspDisplay_Ledger");
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@AccountingYear", year);
                    cmd.Parameters.AddWithValue("@dsXML", dsXML);

                    dt = Util.GetDataTableFromCommand(cmd);

                    response.RESPONSE = true;
                    response.RESULT = Util.GetJsonFromDataTable(dt);
                    response.MESSAGE = dt.Rows.Count <= 0 ? "No Record(s) Found" : string.Empty;
                }
                else
                {
                    response.RESPONSE = false;
                    response.MESSAGE = "Invalid parameter values";
                }

            }
            catch (Exception x)
            {
                log.LogException(x);
            }

            return response;
        }

        public static string GetXMLforLedgerDetail(LedgerDetail oLedgerDetail)
        {
            string rXml = string.Empty;

            try
            {
                if (oLedgerDetail != null)
                {
                    DateTime date = DateTime.ParseExact(oLedgerDetail.Date, "yyyyMMdd", CultureInfo.InvariantCulture);
                    string fyStartDate = Util.FYStartDate(date).ToString("dd/MMM/yyyy", CultureInfo.InvariantCulture);
                    string rDate = date.ToString("dd/MMM/yyyy", CultureInfo.InvariantCulture);

                    string c_LedgerDetail1 =
                        "<t_action>"
                        + "	<row>"
                        + "		<BController>D_Ledger</BController>"
                        + "		<BAction>1</BAction>"
                        + "		<UserGroupType>0</UserGroupType>"
                        + "	</row>"
                        + "</t_action>"
                        + "<t_filter>"
                        + "	<row>"
                        + "		<TrxDate1>{0}</TrxDate1>"
                        + "		<TrxDate2>{1}</TrxDate2>"
                        + "		<AccountCode>{2}</AccountCode>"
                        + "	</row>"
                        + "</t_filter>";

                    string c_LedgerDetail2 =
                        "<t_action>"
                        + "	<row>"
                        + "		<BController>D_Ledger</BController>"
                        + "		<BAction>2</BAction>"
                        + "		<UserGroupType>0</UserGroupType>"
                        + "	</row>"
                        + "</t_action>"
                        + "<t_filter>"
                        + "	<row>"
                        + "		<TrxDate1>{0}</TrxDate1>"
                        + "		<TrxDate2>{1}</TrxDate2>"
                        + "		<AccountCode>{2}</AccountCode>"
                        + "		<BookMarketType>{3}</BookMarketType>"
                        + "		<CompanyCode>{4}</CompanyCode>"
                        + "		<ExchangeCode>{5}</ExchangeCode>"
                        + "	</row>"
                        + "</t_filter>";

                    string c_LedgerDetail3 =
                        "<t_action>"
                        + "	<row>"
                        + "		<BController>D_Ledger</BController>"
                        + "		<BAction>3</BAction>"
                        + "		<UserGroupType>0</UserGroupType>"
                        + "	</row>"
                        + "</t_action>"
                        + "<t_filter>"
                        + "	<row>"
                        + "		<TrxDate1>{0}</TrxDate1>"
                        + "		<TrxDate2>{1}</TrxDate2>"
                        + "		<AccountCode>{2}</AccountCode>"
                        + "		<BookMarketType>{3}</BookMarketType>"
                        + "		<CompanyCode>{4}</CompanyCode>"
                        + "		<ExchangeCode>{5}</ExchangeCode>"
                        + "		<LedgerKey>{6}</LedgerKey>"
                        + "	</row>"
                        + "</t_filter>";



                    if (!string.IsNullOrEmpty(oLedgerDetail.Date)
                        && !string.IsNullOrEmpty(oLedgerDetail.UserName)
                        && !string.IsNullOrEmpty(oLedgerDetail.BookMarketType)
                        && !string.IsNullOrEmpty(oLedgerDetail.CompanyCode)
                        && !string.IsNullOrEmpty(oLedgerDetail.ExchangeCode)
                        && !string.IsNullOrEmpty(oLedgerDetail.LedgerKey))
                    {
                        rXml = string.Format(c_LedgerDetail3,
                            rDate,
                            rDate,
                            oLedgerDetail.UserName,
                            oLedgerDetail.BookMarketType,
                            oLedgerDetail.CompanyCode,
                            oLedgerDetail.ExchangeCode,
                            oLedgerDetail.LedgerKey
                            );
                    }
                    else if (!string.IsNullOrEmpty(oLedgerDetail.Date)
                        && !string.IsNullOrEmpty(oLedgerDetail.UserName)
                        && !string.IsNullOrEmpty(oLedgerDetail.BookMarketType)
                        && !string.IsNullOrEmpty(oLedgerDetail.CompanyCode)
                        && !string.IsNullOrEmpty(oLedgerDetail.ExchangeCode))
                    {
                        rXml = string.Format(c_LedgerDetail2,
                            fyStartDate,
                            rDate,
                            oLedgerDetail.UserName,
                            oLedgerDetail.BookMarketType,
                            oLedgerDetail.CompanyCode,
                            oLedgerDetail.ExchangeCode
                            );

                    }
                    else if (!string.IsNullOrEmpty(oLedgerDetail.Date)
                        && !string.IsNullOrEmpty(oLedgerDetail.UserName))
                    {
                        rXml = string.Format(c_LedgerDetail1,
                            fyStartDate,
                            rDate,
                            oLedgerDetail.UserName
                            );
                    }

                }
                else
                {
                    rXml = string.Empty;
                }

            }
            catch (Exception x)
            {
                log.LogException(x);
            }

            return rXml;
        }
    }
}