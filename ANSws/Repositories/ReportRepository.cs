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
        public static WSResponse GetLedgerDetail(LedgerDetail oLedgerDetail)
        {
            WSResponse response = new WSResponse();

            try
            {
                DateTime date = DateTime.ParseExact(oLedgerDetail.Date, "yyyyMMdd", CultureInfo.InvariantCulture);
                DateTime fyStartDate = Util.FYStartDate(date);
                int year = fyStartDate.Year;

                string dsXML = GetXMLforLedgerDetail(oLedgerDetail);

                if (dsXML != string.Empty)
                {
                    DataTable dt = new DataTable();
                    SqlCommand cmd = new SqlCommand("btspDisplay_Ledger");
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@AccountingYear", year);
                    cmd.Parameters.AddWithValue("@dsXML", dsXML);

                    dt = Util.GetDataTableFromCommandANSPWTrXX(cmd);

                    response.RESPONSE = dt.Rows.Count > 0;
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




        public static WSResponse GetOpenSeriesData(OpenSeriesData oOpenSeriesData)
        {
            WSResponse response = new WSResponse();

            try
            {
                DateTime date = DateTime.ParseExact(oOpenSeriesData.Date, "yyyyMMdd", CultureInfo.InvariantCulture);
                DateTime fyStartDate = Util.FYStartDate(date);
                int year = fyStartDate.Year;

                string dsXML = GetXMLforOpenSeriesData(oOpenSeriesData);

                if (dsXML != string.Empty)
                {
                    DataTable dt = new DataTable();
                    SqlCommand cmd = new SqlCommand("btspDisplay_OpenSeries");
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@AccountingYear", year);
                    cmd.Parameters.AddWithValue("@dsXML", dsXML);

                    dt = Util.GetDataTableFromCommandANSPWTrXX(cmd);

                    response.RESPONSE = dt.Rows.Count > 0;
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

        public static string GetXMLforOpenSeriesData(OpenSeriesData oOpenSeriesData)
        {
            string rXml = string.Empty;

            try
            {
                if (oOpenSeriesData != null)
                {
                    DateTime date = DateTime.ParseExact(oOpenSeriesData.Date, "yyyyMMdd", CultureInfo.InvariantCulture);
                    string rDate = date.ToString("dd/MMM/yyyy", CultureInfo.InvariantCulture);

                    string c_OpenSeriesData =
                        "<t_action>"
                        + "	<row>"
                        + "		<BController>D_OpenSeries</BController>"
                        + "		<BAction>1</BAction>"
                        + "		<UserGroupType>0</UserGroupType>"
                        + "	</row>"
                        + "</t_action>"
                        + "<t_filter>"
                        + "	<row>"
                        + "		<TrxDate1>{0}</TrxDate1>"
                        + "		<TrxDate2></TrxDate2>"
                        + "		<AccountCode>{1}</AccountCode>"
                        + "	</row>"
                        + "</t_filter>";

                    string c_OpenSeriesData2 =
                        "<t_action>"
                        + "	<row>"
                        + "		<BController>D_OpenSeries</BController>"
                        + "		<BAction>2</BAction>"
                        + "		<UserGroupType>0</UserGroupType>"
                        + "	</row>"
                        + "</t_action>"
                        + "<t_filter>"
                        + "	<row>"
                        + "		<TrxDate1>{0}</TrxDate1>"
                        + "		<TrxDate2></TrxDate2>"
                        + "		<AccountCode>{1}</AccountCode>"
                        + "		<BookMarketType>{2}</BookMarketType>"
                        + "		<CompanyCode>{3}</CompanyCode>"
                        + "		<ExchangeCode>{4}</ExchangeCode>"
                        + "	</row>"
                        + "</t_filter>";


                    if (!string.IsNullOrEmpty(oOpenSeriesData.Date)
                        && !string.IsNullOrEmpty(oOpenSeriesData.UserName)
                        && !string.IsNullOrEmpty(oOpenSeriesData.BookMarketType)
                        && !string.IsNullOrEmpty(oOpenSeriesData.CompanyCode)
                        && !string.IsNullOrEmpty(oOpenSeriesData.ExchangeCode))
                    {
                        rXml = string.Format(c_OpenSeriesData2,
                            rDate,
                            oOpenSeriesData.UserName,
                            oOpenSeriesData.BookMarketType,
                            oOpenSeriesData.CompanyCode,
                            oOpenSeriesData.ExchangeCode
                            );

                    }
                    else if (!string.IsNullOrEmpty(oOpenSeriesData.Date)
                        && !string.IsNullOrEmpty(oOpenSeriesData.UserName))
                    {
                        rXml = string.Format(c_OpenSeriesData,
                            rDate,
                            oOpenSeriesData.UserName
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




        public static WSResponse GetCollateralsMISData(CollateralsMISData oCollateralsMisData)
        {
            WSResponse response = new WSResponse();

            try
            {
                DateTime date = DateTime.ParseExact(oCollateralsMisData.Date, "yyyyMMdd", CultureInfo.InvariantCulture);
                DateTime fyStartDate = Util.FYStartDate(date);
                int year = fyStartDate.Year;

                string dsXML = GetXMLforCollateralsMISData(oCollateralsMisData);

                if (dsXML != string.Empty)
                {
                    DataTable dt = new DataTable();
                    SqlCommand cmd = new SqlCommand("btspDisplay_CollateralMIS");
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@AccountingYear", year);
                    cmd.Parameters.AddWithValue("@dsXML", dsXML);

                    dt = Util.GetDataTableFromCommandANSPWTrXX(cmd);

                    response.RESPONSE = dt.Rows.Count > 0;
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

        public static string GetXMLforCollateralsMISData(CollateralsMISData oCollateralsMisData)
        {
            string rXml = string.Empty;

            try
            {
                if (oCollateralsMisData != null)
                {
                    DateTime date = DateTime.ParseExact(oCollateralsMisData.Date, "yyyyMMdd", CultureInfo.InvariantCulture);
                    string rDate = date.ToString("dd/MMM/yyyy", CultureInfo.InvariantCulture);

                    string c_MISData =
                        "<t_action>"
                        + "	<row>"
                        + "		<BController>D_CollateralMIS</BController>"
                        + "		<BAction>1</BAction>"
                        + "		<UserGroupType>0</UserGroupType>"
                        + "	</row>"
                        + "</t_action>"
                        + "<t_filter>"
                        + "	<row>"
                        + "		<TrxDate1>{0}</TrxDate1>"
                        + "		<TrxDate2></TrxDate2>"
                        + "		<AccountCode>{1}</AccountCode>"
                        + "	</row>"
                        + "</t_filter>";

                    string c_MISData2 =
                        "<t_action>"
                        + "	<row>"
                        + "		<BController>D_CollateralMIS</BController>"
                        + "		<BAction>2</BAction>"
                        + "		<UserGroupType>0</UserGroupType>"
                        + "	</row>"
                        + "</t_action>"
                        + "<t_filter>"
                        + "	<row>"
                        + "		<TrxDate1>{0}</TrxDate1>"
                        + "		<TrxDate2></TrxDate2>"
                        + "		<AccountCode>{1}</AccountCode>"
                        + "		<BookMarketType>{2}</BookMarketType>"
                        + "		<CompanyCode>{3}</CompanyCode>"
                        + "		<ExchangeCode>{4}</ExchangeCode>"
                        + "	</row>"
                        + "</t_filter>";


                    if (!string.IsNullOrEmpty(oCollateralsMisData.Date)
                        && !string.IsNullOrEmpty(oCollateralsMisData.UserName)
                        && !string.IsNullOrEmpty(oCollateralsMisData.BookMarketType)
                        && !string.IsNullOrEmpty(oCollateralsMisData.CompanyCode)
                        && !string.IsNullOrEmpty(oCollateralsMisData.ExchangeCode))
                    {
                        rXml = string.Format(c_MISData2,
                            rDate,
                            oCollateralsMisData.UserName,
                            oCollateralsMisData.BookMarketType,
                            oCollateralsMisData.CompanyCode,
                            oCollateralsMisData.ExchangeCode
                            );

                    }
                    else if (!string.IsNullOrEmpty(oCollateralsMisData.Date)
                        && !string.IsNullOrEmpty(oCollateralsMisData.UserName))
                    {
                        rXml = string.Format(c_MISData,
                            rDate,
                            oCollateralsMisData.UserName
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



        public static WSResponse GetDematLedger(DematLedger oDematLedger)
        {
            WSResponse response = new WSResponse();

            try
            {
                DateTime date = DateTime.ParseExact(oDematLedger.Date, "yyyyMMdd", CultureInfo.InvariantCulture);
                DateTime fyStartDate = Util.FYStartDate(date);
                int year = fyStartDate.Year;

                string dsXML = GetXMLforDematLedger(oDematLedger);

                if (dsXML != string.Empty)
                {
                    DataTable dt = new DataTable();
                    SqlCommand cmd = new SqlCommand("btspDisplay_DematLedger");
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@AccountingYear", year);
                    cmd.Parameters.AddWithValue("@dsXML", dsXML);

                    dt = Util.GetDataTableFromCommandANSPWTrXX(cmd);

                    response.RESPONSE = dt.Rows.Count > 0;
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

        public static string GetXMLforDematLedger(DematLedger oDematLedger)
        {
            string rXml = string.Empty;

            try
            {
                if (oDematLedger != null)
                {
                    DateTime date = DateTime.ParseExact(oDematLedger.Date, "yyyyMMdd", CultureInfo.InvariantCulture);
                    string rDate = date.ToString("dd/MMM/yyyy", CultureInfo.InvariantCulture);

                    string cDematLedgerXml =
                        "<t_action>"
                        + "	<row>"
                        + "		<BController>D_DematLedger</BController>"
                        + "		<BAction>1</BAction>"
                        + "		<UserGroupType>0</UserGroupType>"
                        + "	</row>"
                        + "</t_action>"
                        + "<t_filter>"
                        + "	<row>"
                        + "		<TrxDate1>{0}</TrxDate1>"
                        + "		<TrxDate2></TrxDate2>"
                        + "		<AccountCode>{1}</AccountCode>"
                        + "	</row>"
                        + "</t_filter>";
                    
                    
                    if (!string.IsNullOrEmpty(oDematLedger.Date)
                        && !string.IsNullOrEmpty(oDematLedger.UserName))
                    {
                        rXml = string.Format(cDematLedgerXml,
                            rDate,
                            oDematLedger.UserName
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


        public static WSResponse GetClientInfo(ClientInfo oClientInfo)
        {
            WSResponse response = new WSResponse();

            try
            {
                DateTime date = DateTime.ParseExact(oClientInfo.Date, "yyyyMMdd", CultureInfo.InvariantCulture);
                DateTime fyStartDate = Util.FYStartDate(date);
                int year = fyStartDate.Year;

                string dsXML = GetXMLforClientInfo(oClientInfo);

                if (dsXML != string.Empty)
                {
                    DataTable dt = new DataTable();
                    SqlCommand cmd = new SqlCommand("btspDisplay_ClientInfo");
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@AccountingYear", year);
                    cmd.Parameters.AddWithValue("@dsXML", dsXML);

                    dt = Util.GetDataTableFromCommandANSPWTrXX(cmd);

                    response.RESPONSE = dt.Rows.Count > 0;
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

        public static string GetXMLforClientInfo(ClientInfo oClientInfo)
        {
            string rXml = string.Empty;

            try
            {
                if (oClientInfo != null)
                {
                    DateTime date = DateTime.ParseExact(oClientInfo.Date, "yyyyMMdd", CultureInfo.InvariantCulture);
                    string rDate = date.ToString("dd/MMM/yyyy", CultureInfo.InvariantCulture);

                    string cClientInfoXml =
                        "<t_action>"
                        + "	<row>"
                        + "		<BController>D_DematLedger</BController>"
                        + "		<BAction>1</BAction>"
                        + "		<UserGroupType>0</UserGroupType>"
                        + "	</row>"
                        + "</t_action>"
                        + "<t_filter>"
                        + "	<row>"
                        + "		<AccountCode>{0}</AccountCode>"
                        + "	</row>"
                        + "</t_filter>";


                    if (!string.IsNullOrEmpty(oClientInfo.UserName))
                    {
                        rXml = string.Format(cClientInfoXml,oClientInfo.UserName);
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


        public static WSResponse GetDPTrx(DPTrx oDpTrx)
        {
            WSResponse response = new WSResponse();

            try
            {
                DateTime date = DateTime.ParseExact(oDpTrx.Date, "yyyyMMdd", CultureInfo.InvariantCulture);
                DateTime fyStartDate = Util.FYStartDate(date);
                int year = fyStartDate.Year;

                string dsXML = GetXMLforDPTrx(oDpTrx);

                if (dsXML != string.Empty)
                {
                    DataTable dt = new DataTable();
                    SqlCommand cmd = new SqlCommand("btspDisplay_DPTrxMBL");
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@AccountingYear", year);
                    cmd.Parameters.AddWithValue("@dsXML", dsXML);

                    dt = Util.GetDataTableFromCommandANSECDSL(cmd);

                    response.RESPONSE = dt.Rows.Count > 0;
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

        public static string GetXMLforDPTrx(DPTrx oDpTrx)
        {
            string rXml = string.Empty;

            try
            {
                if (oDpTrx != null)
                {
                    DateTime date = DateTime.ParseExact(oDpTrx.Date, "yyyyMMdd", CultureInfo.InvariantCulture);
                    string rDate = date.ToString("dd/MMM/yyyy", CultureInfo.InvariantCulture);
                    string fyStartDate = Util.FYStartDate(date).ToString("dd/MMM/yyyy", CultureInfo.InvariantCulture);

                    string cDpTrxXml1 =
                        "<t_action>"
                        + "	<row>"
                        + "		<BController>D_DPTrx</BController>"
                        + "		<BAction>1</BAction>"
                        + "		<UserGroupType>0</UserGroupType>"
                        + "	</row>"
                        + "</t_action>"
                        + "<t_filter>"
                        + "	<row>"
                        + "     <TrxDate1>{0}</TrxDate1>" 
                        + "     <TrxDate2>{1}</TrxDate2>" 
                        + "     <AccountCode>{2}</AccountCode>"
                        + "	</row>"
                        + "</t_filter>";

                    string cDpTrxXml2 =
                        "<t_action>"
                        + "	<row>"
                        + "		<BController>D_DPTrx</BController>"
                        + "		<BAction>2</BAction>"
                        + "		<UserGroupType>0</UserGroupType>"
                        + "	</row>"
                        + "</t_action>"
                        + "<t_filter>"
                        + "	<row>"
                        + "     <TrxDate1>{0}</TrxDate1>"
                        + "     <TrxDate2>{1}</TrxDate2>"
                        + "     <AccountCode>{2}</AccountCode>"
                        + "     <DPClntId>{3}</DPClntId>"
                        + "	</row>"
                        + "</t_filter>";


                    if (!string.IsNullOrEmpty(oDpTrx.UserName)
                        && !string.IsNullOrEmpty(oDpTrx.Date)
                        && !string.IsNullOrEmpty(oDpTrx.DPClntId))
                    {
                        rXml = string.Format(cDpTrxXml2, fyStartDate, rDate, oDpTrx.UserName, oDpTrx.DPClntId);
                    }
                    else if (!string.IsNullOrEmpty(oDpTrx.UserName)
                        && !string.IsNullOrEmpty(oDpTrx.Date))
                    {
                        rXml = string.Format(cDpTrxXml1, fyStartDate, rDate, oDpTrx.UserName);
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