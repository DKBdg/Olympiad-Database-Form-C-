using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dbcontextclass {
    static public class DbDataConnect {
        static public List<string> TableNames = new List<string>(new string[] {"Countries","Cities","Olympiads","Sports","SportTypes","Players","CProfiles","Medals"});
        static public List<string> SelectNames = new List<string>(new string[] { "show all", "show all winners", "show all countries with gold", "show the most frequent owner country", "show the team of a single country", "show the winners of an olympiad", "show all countries with gold of an olympiad","show the contenstant with the most of gold in a sport","show all medals of an olympiad","show statistics of a country","show statisticsa of a county in an olympiad" });

        static public List<int> IDExpressions = new List<int>();
        static public List<string> AllSelects = new List<string>(new string[] { 
            @"SELECT * FROM T_Countries INNER JOIN T_Players ON T_Countries.ID=T_Players.CountryID
              INNER JOIN T_CProfiles ON T_CProfiles.PlayerID = T_Players.ID
              INNER JOIN T_Sports ON T_CProfiles.SportID = T_Sports.ID
              INNER JOIN T_SportTypes ON T_Sports.SportTypeID=T_SportTypes.ID
              INNER JOIN T_Medals ON T_Medals.ProfileID = T_CProfiles.ID;",


            @"SELECT T_Players.FirstName, T_PLayers.LastName , T_Medals.Rarity FROM T_Olympiads
            INNER JOIN T_Sports ON T_Olympiads.ID=T_Sports.OlympiadID
            INNER JOIN T_SportTypes ON T_SportTypes.ID = T_Sports.SportTypeID
            INNER JOIN T_CProfiles ON T_CProfiles.SportID = T_Sports.ID
            INNER JOIN T_Players ON T_Players.ID = T_CProfiles.PlayerID
            INNER JOIN T_Medals ON T_Medals.ProfileID = T_CProfiles.ID
            GROUP BY T_Players.FirstName, T_PLayers.LastName , T_Medals.Rarity;",


            @"SELECT  T_Countries.CountryName,COUNT(*) AS 'Medal Count' FROM T_Countries
            INNER JOIN T_Players ON T_Countries.ID = T_Players.CountryID
            INNER JOIN T_CProfiles ON T_CProfiles.PlayerID = T_Players.ID
            INNER JOIN T_Sports ON T_CProfiles.SportID = T_Sports.ID
            INNER JOIN T_Olympiads ON T_Olympiads.ID = T_Sports.OlympiadID
            INNER JOIN T_Medals ON T_Medals.ProfileID=T_CProfiles.ID
            WHERE T_Medals.Rarity='Gold'
            GROUP BY  T_Countries.CountryName",




            @"SELECT TOP(1) T_Countries.CountryName ,COUNT(*) AS 'Owner Count' FROM T_Countries
            INNER JOIN T_Cities ON T_Countries.ID = T_Cities.CountryID
            INNER JOIN T_Olympiads ON T_Cities.ID= T_Olympiads.CityID
            GROUP BY T_Countries.CountryName
            ORDER BY [Owner Count] DESC",

            @"SELECT *  FROM T_Countries
            INNER JOIN T_Players ON T_Players.CountryID = T_Countries.ID
            WHERE T_Countries.ID=0",



            @"SELECT T_Players.ID,T_Players.FirstName, T_PLayers.LastName , T_Medals.Rarity FROM T_Olympiads
            INNER JOIN T_Sports ON T_Olympiads.ID=T_Sports.OlympiadID
            INNER JOIN T_SportTypes ON T_SportTypes.ID = T_Sports.SportTypeID
            INNER JOIN T_CProfiles ON T_CProfiles.SportID = T_Sports.ID
            INNER JOIN T_Players ON T_Players.ID = T_CProfiles.PlayerID
            INNER JOIN T_Medals ON T_Medals.ProfileID = T_CProfiles.ID
            WHERE T_Olympiads.ID=0
            GROUP BY T_Players.ID,T_Players.FirstName, T_PLayers.LastName , T_Medals.Rarity;",

            @"SELECT  T_Countries.CountryName,COUNT(*) AS 'Medal Count' FROM T_Countries
            INNER JOIN T_Players ON T_Countries.ID = T_Players.CountryID
            INNER JOIN T_CProfiles ON T_CProfiles.PlayerID = T_Players.ID
            INNER JOIN T_Sports ON T_CProfiles.SportID = T_Sports.ID
            INNER JOIN T_Olympiads ON T_Olympiads.ID = T_Sports.OlympiadID
            INNER JOIN T_Medals ON T_Medals.ProfileID=T_CProfiles.ID
            WHERE T_Olympiads.ID=0 AND T_Medals.Rarity='Gold'
            GROUP BY  T_Countries.CountryName",


            @"SELECT T_Players.ID,T_Players.FirstName,T_Players.LastName,COUNT(*) AS'Gold medal count' FROM T_Players
            INNER JOIN T_CProfiles ON T_Players.ID=T_CProfiles.PlayerID
            INNER JOIN T_Medals ON T_CProfiles.ID=T_Medals.ProfileID
            INNER JOIN T_Sports ON T_CProfiles.SportID = T_Sports.ID
            INNER JOIN T_SportTypes ON T_SportTypes.ID = T_Sports.SportTypeID
            WHERE T_SportTypes.ID=0 AND T_Medals.Rarity='Gold'
			GROUP BY T_Players.ID,T_Players.FirstName,T_Players.LastName",

            @"SELECT * FROM T_Countries
            INNER JOIN T_Players ON T_Countries.ID=T_Players.CountryID
            INNER JOIN T_CProfiles ON T_CProfiles.PlayerID = T_Players.ID
            INNER JOIN T_Sports ON T_CProfiles.SportID = T_Sports.ID
            INNER JOIN T_Olympiads ON T_Olympiads.ID = T_Sports.OlympiadID
            INNER JOIN T_SportTypes ON T_Sports.SportTypeID=T_SportTypes.ID
            INNER JOIN T_Medals ON T_Medals.ProfileID = T_CProfiles.ID
            WHERE T_Olympiads.ID = 0;",

            @"SELECT * FROM T_Countries
            INNER JOIN T_Players ON T_Countries.ID=T_Players.CountryID
            INNER JOIN T_CProfiles ON T_CProfiles.PlayerID = T_Players.ID
            INNER JOIN T_Sports ON T_CProfiles.SportID = T_Sports.ID
            INNER JOIN T_Olympiads ON T_Olympiads.ID = T_Sports.OlympiadID
            INNER JOIN T_SportTypes ON T_Sports.SportTypeID=T_SportTypes.ID
            LEFT OUTER JOIN T_Medals ON T_Medals.ProfileID = T_CProfiles.ID
            WHERE T_Countries.ID =0 ;",

            $@"SELECT * FROM T_Countries
            INNER JOIN T_Players ON T_Countries.ID=T_Players.CountryID
            INNER JOIN T_CProfiles ON T_CProfiles.PlayerID = T_Players.ID
            INNER JOIN T_Sports ON T_CProfiles.SportID = T_Sports.ID
            INNER JOIN T_Olympiads ON T_Olympiads.ID = T_Sports.OlympiadID
            INNER JOIN T_SportTypes ON T_Sports.SportTypeID=T_SportTypes.ID
            LEFT OUTER JOIN T_Medals ON T_Medals.ProfileID = T_CProfiles.ID
            WHERE T_Countries.ID =0 AND T_Olympiads.ID=1;"

        });
      private static DbConnection conn = null;
      private static DbProviderFactory fact = null;
      private static string connectionString = "";

        public  async static Task<DataTable> GetTableById(int id,string constr) {
            return await GetTableBySelect(AllSelects[id],constr);
            
        
        
        
        }
       static private void InitFactoryConn(string constr) { 

            fact = DbProviderFactories.GetFactory("System.Data.SqlClient");
            conn = fact.CreateConnection();
            connectionString = constr;
            conn.ConnectionString = connectionString;
        
        
        }

        public static string GetConnectionStringByProvider(string providerName) {
            string returnValue = null;

            ConnectionStringSettingsCollection settings = ConfigurationManager.ConnectionStrings;

            if (settings != null) {
                foreach (ConnectionStringSettings cs in settings) {
                    if (cs.ProviderName == providerName) {
                        returnValue = cs.ConnectionString;
                        break;
                    }
                }
            }

            return returnValue;

        }
        public async static Task<DataTable> GetTableBySelect(string selectCommand, string constr) {
            InitFactoryConn(constr);
            await conn.OpenAsync();


            DbDataAdapter adapter = fact.CreateDataAdapter();
            adapter.SelectCommand = conn.CreateCommand();
            adapter.SelectCommand.CommandText = selectCommand;
            await adapter.SelectCommand.ExecuteNonQueryAsync();

            DataTable table = new DataTable();


            adapter.Fill(table);


            conn.Close();
            
            return table;



        }
    }
}
