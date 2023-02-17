using System.Collections.Generic;
using WCStatsTracker.Models;
using System.Configuration;
using System.Data;
using Microsoft.Data.Sqlite;
using System.IO;
using Dapper;
using System.Linq;
using System;

namespace WCStatsTracker.Database;

public class SqliteDataAccess
{
    private static bool DbExists => File.Exists(DbFile);
    public static string DbFile => ConfigurationManager.AppSettings["DBName"];
    public static List<WCRun> LoadWCRuns()
    {
        if (!DbExists) CreateDatabase();
        using (IDbConnection con = new SqliteConnection(LoadConnectionString()))
        {
            var sql = @"Select * from Run r
                        INNER JOIN FlagSet f ON r.RunFlagSet = f.rowid";
            var runs = con.Query<WCRun, FlagSet, WCRun>(sql, (run, flagSet) =>
            {
                run.RunFlagSet = flagSet;
                return run;
            },
            splitOn: "RunFlagSet");
            return runs.ToList();
        }
    }

    public static void SaveRun(WCRun run)
    {
        if (!DbExists) CreateDatabase();
        using (IDbConnection con = new SqliteConnection(LoadConnectionString()))
        {
            
        }
    }

    public static List<FlagSet> LoadFlagSets()
    {
        if (!DbExists) CreateDatabase();
        using (IDbConnection con = new SqliteConnection(LoadConnectionString()))
        {
            var output = con.Query<FlagSet>("Select * from FlagSet", new DynamicParameters());
            return output.ToList();
        }
    }

    public static void SaveFlagSet(FlagSet flagSet)
    {
        if (!DbExists) CreateDatabase();
        using (IDbConnection con = new SqliteConnection(LoadConnectionString()))
        {
            /// Todo : Save here
        }
    }

    private static string LoadConnectionString(string id = "Default")
    {
        return ConfigurationManager.ConnectionStrings[id].ConnectionString;
    }

    private static void CreateDatabase()
    {
        using (IDbConnection con = new SqliteConnection(LoadConnectionString()))
        {
            con.Open();
            con.Execute(
                @"create table Run
                (
                    RunLength           TEXT,
                    CharactersFound     INTEGER,
                    EspersFound         INTEGER,
                    DragonsKilled       INTEGER,
                    BossesKilled        INTEGER,
                    ChecksDone          INTEGER,
                    ChestsOpened        INTEGER,
                    DidKTSkip           INTEGER,
                    RunFlagSet          INTEGER,
                    FOREIGN KEY(RunFlagSet) REFERENCES FlagSet(Id)
                )                  
                ");
            con.Execute(
                @"CREATE TABLE FlagSet
                (
                    Id            INTEGER NOT NULL PRIMARY KEY,
                    Name          TEXT,
                    FlagString    TEXT
                )
                ");
        }
    }
}
