using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlinkCheats
{
    class OnlinkDatabase
    {
        public SQLiteConnection Connection;
        public Dictionary<string, int> agentRanking = new Dictionary<string, int>
        {
            {"TERMINAL", 16},
            {"TechnoMage", 15},
            {"Veteran", 14},
            {"Expert", 13},
            {"Mage", 12},
            {"Elite", 11},
            {"Professional", 10},
            {"UberSkilled", 9 },
            {"Knowledgeable", 8 },
            {"Experienced", 7 },
            {"Skilled",6 },
            {"Intermediate",5 },
            {"Confident", 4},
            {"Novice", 3 },
            {"Beginner",2 },
            {"Registered",1 }
        };

        public OnlinkDatabase(string path)
        {
            Connection=new SQLiteConnection(@"Data Source = "+@path);
            
        }
        public void Open() { Connection.Open();}

        

        public void setMoney(int Money, string Agent)
        {
            string query = "UPDATE bankaccount "+
        "SET balance = @money where name = @name; ";
            SQLiteCommand command = new SQLiteCommand(query, Connection);
            SQLiteParameter moneyPar = new SQLiteParameter();
            moneyPar.ParameterName = "money";
            moneyPar.Value = Money;
            command.Parameters.Add(moneyPar);
            SQLiteParameter namePar = new SQLiteParameter();
            namePar.ParameterName = "name";
            namePar.Value = Agent;
            command.Parameters.Add(namePar);
            
            command.ExecuteNonQuery();
        }
        public int GetUplinkRatingInt(string rating)
        {
            return agentRanking[rating];
        }
        
        
        public void setAllRating(int uplinkRating = 16, int creditRating = 8, int uplinkScore = 2500)
        {
            string query = "UPDATE rating " +
                           "SET creditrating = @cr, uplinkrating = @rating, uplinkscore = @score where id = 9;";
            SQLiteCommand cmd = new SQLiteCommand(query, Connection);
            SQLiteParameter crPar = new SQLiteParameter();
            SQLiteParameter ratingPar = new SQLiteParameter();
            SQLiteParameter scorePar = new SQLiteParameter();
            crPar.ParameterName = "cr";
            ratingPar.ParameterName = "rating";
            scorePar.ParameterName = "score";
            crPar.Value = creditRating;
            ratingPar.Value = uplinkRating;
            scorePar.Value = uplinkScore;
            cmd.Parameters.Add(crPar);
            cmd.Parameters.Add(ratingPar);
            cmd.Parameters.Add(scorePar);
            cmd.ExecuteNonQuery();
        }
        public void setUplinkRating(int uplinkRating = 16)
        {
            string query = "UPDATE rating " +
                           "SET uplinkrating = @rating where id = 9;";
            SQLiteCommand cmd = new SQLiteCommand(query, Connection);
            
            SQLiteParameter ratingPar = new SQLiteParameter();
            
            
            ratingPar.ParameterName = "rating";
            
            
            ratingPar.Value = uplinkRating;
            
            
            cmd.Parameters.Add(ratingPar);
            
            cmd.ExecuteNonQuery();
        }
        public void setCreditRating(int creditRating = 8)
        {
            string query = "UPDATE rating " +
                           "SET creditrating = @cr where id = 9;";
            SQLiteCommand cmd = new SQLiteCommand(query, Connection);
            SQLiteParameter crPar = new SQLiteParameter();

            crPar.ParameterName = "cr";
            crPar.Value = creditRating;
            cmd.Parameters.Add(crPar);
            cmd.ExecuteNonQuery();
        }
        public void setUplinkScore(int uplinkScore = 2500)
        {
            string query = "UPDATE rating " +
                           "SET uplinkscore = @score where id = 9;";
            SQLiteCommand cmd = new SQLiteCommand(query, Connection);
            SQLiteParameter scorePar = new SQLiteParameter();
            scorePar.ParameterName = "score";
            scorePar.Value = uplinkScore;
            cmd.Parameters.Add(scorePar);
            cmd.ExecuteNonQuery();
        }

        public void setAllSecuritySystemsStatus(bool Enabled)
        {
            int status;
            if (Enabled)
            {
                status = 1;
            }
            else
            {
                status = 0;
            }

            string query = "UPDATE secsystem " +
               "SET enabled = @status;";
            SQLiteCommand cmd = new SQLiteCommand(query, Connection);
            SQLiteParameter statusPar = new SQLiteParameter();

            statusPar.ParameterName = "status";
            statusPar.Value = status;
            cmd.Parameters.Add(statusPar);
            cmd.ExecuteNonQuery();
        }



        public void Close()
        {
            Connection.Close();
        }
    }
}
