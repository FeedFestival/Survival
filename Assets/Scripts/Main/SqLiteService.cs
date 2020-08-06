using System.Collections.Generic;
using SQLite4Unity3d;
using UnityEngine;
#if !UNITY_EDITOR
using System.Collections;
using System.IO;
#endif
using System.Linq;
using Assets.Scripts.Main;

public class SqLiteService
{
    private SQLiteConnection _connection;

    public SqLiteService(string DatabaseName)
    {

        #region DataServiceInit


#if UNITY_EDITOR
        var dbPath = string.Format(@"Assets/StreamingAssets/{0}", DatabaseName);
#else
        // check if file exists in Application.persistentDataPath
        var filepath = string.Format("{0}/{1}", Application.persistentDataPath, DatabaseName);

        if (!File.Exists(filepath))
        {
            Debug.Log("Database not in Persistent path");
            // if it doesn't ->
            // open StreamingAssets directory and load the db ->

#if UNITY_ANDROID
            var loadDb = new WWW("jar:file://" + Application.dataPath + "!/assets/" + DatabaseName);  // this is the path to your StreamingAssets in android
            while (!loadDb.isDone) { }  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
            // then save to Application.persistentDataPath
            File.WriteAllBytes(filepath, loadDb.bytes);
#elif UNITY_IOS
                 var loadDb = Application.dataPath + "/Raw/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);
#elif UNITY_WP8
                var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);

#elif UNITY_WINRT
		var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
		// then save to Application.persistentDataPath
		File.Copy(loadDb, filepath);
#else
	var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
	// then save to Application.persistentDataPath
	File.Copy(loadDb, filepath);

#endif

            Debug.Log("Database written");
        }

        var dbPath = filepath;
#endif

        #endregion

        _connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
        Debug.Log("Final PATH: " + dbPath);

    }

    public void CleanUpUsers()
    {
        _connection.DropTable<user>();
        _connection.CreateTable<user>();
    }

    public void CreateDB()
    {
        _connection.DropTable<user>();
        _connection.CreateTable<user>();

        _connection.DropTable<app>();
        _connection.CreateTable<app>();

        _connection.DropTable<setting>();
        _connection.CreateTable<setting>();

        _connection.DropTable<type>();
        _connection.CreateTable<type>();

        _connection.DropTable<game>();
        _connection.CreateTable<game>();

        _connection.DropTable<challenge>();
        _connection.CreateTable<challenge>();

        _connection.DropTable<history>();
        _connection.CreateTable<history>();
    }

    /*
     * User
     * * --------------------------------------------------------------------------------------------------------------------------------------
     */

    public void CreateUser(user user)
    {
        _connection.Insert(user.Setting[0]);
        user.setting_id = user.Setting[0].id;
        int i = 0;
        foreach (setting setting in user.Setting)
        {
            if (i == 0)
            {
                i++;
                continue;
            }
            _connection.Insert(setting.Type);

            setting.type_id = setting.Type.id;
            setting.parent_id = user.setting_id;

            _connection.Insert(setting);
        }

        _connection.Insert(user);
    }

    public void UpdateUser(user user)
    {
        int rowsAffected = _connection.Update(user);
        Debug.Log("(UPDATE User) rowsAffected : " + rowsAffected);
    }

    public user GetUser(int id)
    {
        var user = _connection.Table<user>().FirstOrDefault(x => x.id == id);

        if (user == null)
            return null;

        // get settings
        user.Setting = _connection.Table<setting>().Where(x => x.parent_id == user.setting_id).ToList();

        foreach (setting setting in user.Setting)
        {
            setting.Type = _connection.Table<type>().FirstOrDefault(x => x.id == setting.type_id);
        }

        return user;
    }

    public user GetLastUser()
    {
        return _connection.Table<user>().Last();
    }

    public user GetUserByFacebookId(int facebookId)
    {
        //return _connection.Table<User>().Where(x => x.FacebookApp.FacebookId == facebookId).FirstOrDefault();
        return null;
    }

    /*
    * User - END
    * * --------------------------------------------------------------------------------------------------------------------------------------
    */

    public int AddApp(app app)
    {
        _connection.Insert(app);

        return app.id;
    }

    public int AddType(type type)
    {
        _connection.Insert(type);

        return type.id;
    }

    public void CreateGame(game game)
    {
        game.type_id = AddType(game.Type);

        _connection.Insert(game);
    }

    public List<game> GetGames()
    {
        var list = _connection.Table<game>().ToList();

        foreach (game game in list)
        {
            game.Type = GetType(game.type_id);
        }

        return list;
    }

    private type GetType(int typeId)
    {
        return _connection.Table<type>().Where(x => x.id == typeId).FirstOrDefault();
    }

    /*
     * Map
     * * --------------------------------------------------------------------------------------------------------------------------------------
     */

    // X : 0 - 11
    // Y : 0 - 8
    //public int CreateMap(Map map)
    //{
    //    _connection.Insert(map);
    //    return map.Id;
    //}

    //public int UpdateMap(Map map)
    //{
    //    _connection.Update(map);
    //    return map.Id;
    //}

    //public Map GetMap(int mapId)
    //{
    //    return _connection.Table<Map>().Where(x => x.Id == mapId).FirstOrDefault();
    //}

    //public int GetNextMapId(int number)
    //{
    //    return _connection.Table<Map>().Where(x => x.Number == number).FirstOrDefault().Id;
    //}

    //public void CreateTiles(List<MapTile> mapTiles)
    //{
    //    _connection.InsertAll(mapTiles);
    //}

    //public IEnumerable<Map> GetMaps()
    //{
    //    return _connection.Table<Map>();
    //}

    //public IEnumerable<MapTile> GetTiles(int mapId)
    //{
    //    return _connection.Table<MapTile>().Where(x => x.MapId == mapId);
    //}

    //public void DeleteMapTiles(int mapId)
    //{
    //    var sql = string.Format("delete from MapTile where MapId = {0}", mapId);
    //    _connection.ExecuteSql(sql);
    //}

    /*
     * Map - END
     * * --------------------------------------------------------------------------------------------------------------------------------------
     */
}
