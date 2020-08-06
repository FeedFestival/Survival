using System.Collections.Generic;
using JetBrains.Annotations;
using SQLite4Unity3d;

namespace Assets.Scripts.Main
{
    public class user
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }

        public string name { get; set; }

        public string display_name { get; set; }

        public string password { get; set; }

        public int setting_id { get; set; }

        //

        [Ignore]
        public List<setting> Setting { get; set; }

        [Ignore]
        public app CurrentApp { get; set; }

        [Ignore]
        public List<user> Friends { get; set; }

        [Ignore]
        public List<challenge> Challenges { get; set; }

        public static user FillUser(string properties)
        {
            return new user
            {

            };
        }
    }

    public class app
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }

        public string name { get; set; }

        public string setting_id { get; set; }

        //

        [Ignore]
        public setting Setting { get; set; }

        [Ignore]
        public setting AppSetting { get; set; }

        public static app FillApp()
        {
            return null;
        }
    }

    public class user_con_app
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }

        public int app_id { get; set; }

        public int user_id { get; set; }

        public string setting_id { get; set; }

        public string settings_json { get; set; }
    }

    public class setting
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        
        public int type_id { get; set; }
        
        public string value { get; set; }

        public int parent_id { get; set; }

        // 

        [Ignore]
        public List<setting> settings { get; set; }

        [Ignore]
        public type Type { get; set; }

        public static setting Fillsetting()
        {
            return null;
        }
    }

    public class type
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }

        public string prog_id { get; set; }

        public string name { get; set; }

        public static type Filltype()
        {
            return null;
        }
    }

    public class game
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }

        public int app_id { get; set; }
        
        public int type_id { get; set; }

        public int session_id { get; set; }

        public string guid { get; set; }

        public string start_date { get; set; }

        public string active_date { get; set; }

        public bool web { get; set; }

        public bool unity { get; set; }

        public bool active { get; set; }

        //

        [Ignore]
        public type Type { get; set; }

    }

    public class challenge
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }

        public int game_id { get; set; }

        public int user_id { get; set; }

        public string start_date { get; set; }

        public string type_id { get; set; }

        //

        [Ignore]
        public type Type { get; set; }

        [Ignore]
        public List<user> Users { get; set; }

    }

    public class challenge_con_user
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }

        public int challenge_id { get; set; }

        public int user_id { get; set; }

        public int response_type_id { get; set; }

        //

        [Ignore]
        public type ResponseType { get; set; }
    }

    public class history
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }

        public int challenge_con_user_id { get; set; }

        public string json_data { get; set; }

        public int index_key { get; set; }

        public string value { get; set; }

        //

        [Ignore]
        public int ChallengeId { get; set; }

        [Ignore]
        public int Round { get; set; }

        [Ignore]
        public int Score { get; set; }

    }
}