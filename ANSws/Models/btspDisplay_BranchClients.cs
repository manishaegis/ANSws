using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ANSws.Models
{
    public class btspDisplay_BranchClients
    {
        public string Date { get; set; }
        public string mCode { get; set; }        
    }
}

//1. Result column : SrNo, AccountCode, AccountName, FAAmount 
//---------------------------------------------------------------------------------------------------- 
//use anspwtr16 
//declare @dsXML as XML = ' 
//<t_action><row><BController>D_Ledger</BController><BAction>1</BAction><UserGroupType>3</UserGroupType></row></t_action> 
//<t_filter> 
//<row> 
//        <TrxDate1>02-Mar-2017</TrxDate1><mCode>821385</mCode> 
//</row> 
//</t_filter>' 
//exec btspDisplay_BranchClients 2016, @dsXML