using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace VimbHistory.items
{
    class SQLConnect
    {
        private static string SQLq = @"with hst as (
                                     select dldscDiscountValue*100-100 AS 'Скидка'
                                     , DP.dsctName as 'Тип'
                                     , IP.incpShortName as 'Канал'
                                     , getdate () as 'Дата изменения'
                                     , null as 'Действие'
                                     , usrName as 'Автор'
                                     from DealDiscounts DD (nolock) 
                                     inner join DiscountTypes DP with (nolock) on DD.dldscDiscountTypeID=DP.dsctID 
                                     inner join Users U with (nolock) on U.usrID=DD.dldscCreatorID
                                     left join DealDiscountDetails DDD with (nolock) on DDD.dlddDealDiscountID=DD.dldscID
                                     left join IncutPoints IP with (nolock) on IP.incpID=DDD.dlddTargetID
                                     where DD.dldscDealID = {0} 
                                     union all
                                     select dldschDiscountValue*100-100 AS 'Скидка'
                                     , DP.dsctName as 'Тип'
                                     , InP.incpShortName as 'Канал'
                                     , dldschDeleteMoment_History  as 'Дата изменения'
                                     , dldschAction_History as 'Действие' 
                                     , usrName  as 'Автор'
                                     from hst_dbo.DealDiscounts_History DDH (nolock) 
                                     inner join DiscountTypes DP with (nolock) on DDH.dldschDiscountTypeID=DP.dsctID
                                     inner join Users U with (nolock) on U.usrID=DDH.dldschDeleteUserID_History
                                     left join hst_dbo.DealDiscountDetails_History DDDH with (nolock) on DDDH.dlddhDealDiscountID=DDH.dldschID
                                     left join IncutPoints InP with (nolock) on InP.incpID=DDDH.dlddhTargetID
                                     where dldschDealID = {0} 
                                     )
                                     select [Скидка]
                                     , lag ([Скидка]) over(partition by [Тип] order by [Дата изменения] desc) as [Новая скидка]
                                     , [Тип]
                                     , [Канал]
                                     , [Дата изменения]
                                     , [Действие]
                                     , [Автор]
                                     from hst";
        public static DataView SQLout(string deal)
        {
            if (deal == "") { MessageBox.Show("Введите ID сделки", "Error"); return null; }
            else
            {
                SqlConnection SQLconnect = new SqlConnection(
                    @"Integrated Security=SSPI;Persist Security Info=True;Data Source=*****;Initial Catalog=*****;");
                SQLconnect.Open();
                SqlCommand thisCommand = SQLconnect.CreateCommand();
                thisCommand.CommandText = string.Format(SQLq, deal);
                DataTable DT = new DataTable();
                SqlDataAdapter DA = new SqlDataAdapter(thisCommand);
                DA.Fill(DT);
                SQLconnect.Close();
                return DT.DefaultView;
            }            
        }
    }
}
