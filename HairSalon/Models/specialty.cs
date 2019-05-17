using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace HairSalon.Models
{
    public class Specialty
    {
        private int _id;
        private string _description;
        
        public Specialty( string description, int id = 0)
        {
            _id = id;
            _description = description;

        }

        public string GetDescription()
        {
            return _description;
        }

        public int GetId()
        {
            return _id;
        }

        public void setDescription(string newDescription)
        {
            _description = newDescription;
        }

        public static List<Specialty> GetAll()
        {
            List<Specialty> allSpecialties = new List<Specialty>{};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText=@"SELECT * FROM specialties;";
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read())
            {
                int specialty_id = rdr.GetInt32(0);
                string specialtyDescription = rdr.GetString(1);
                Specialty newSpecialty = new Specialty(specialtyDescription, specialty_id );
                allSpecialties.Add(newSpecialty);
            }
            conn.Close();
            if(conn != null)
            {
                conn.Dispose();
            }
            return allSpecialties;
        }

        public static void ClearAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM specialties;";
            cmd.ExecuteNonQuery();
            conn.Close();
            if(conn != null)
            {
                conn.Dispose();
            }

        }

        public static Specialty Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;  
            cmd.CommandText = @"SELECT * FROM specialties WHERE id = @thisId;";
            MySqlParameter thisId = new MySqlParameter();
            thisId.ParameterName = "@thisId";
            thisId.Value = id;
            cmd.Parameters.Add(thisId);
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            int specialtyId = 0;
            string specialtyDescription = "";
            while(rdr.Read())
            {
                specialtyId = rdr.GetInt32(0);
                specialtyDescription = rdr.GetString(1);
            }
            Specialty newSpecialty = new Specialty(specialtyDescription, specialtyId);
            conn.Close();
            if(conn != null)
            {
                conn.Dispose();
            }
            return newSpecialty;
        }

        public override bool Equals(System.Object otherSpecialty)
        {
            if(!(otherSpecialty is Specialty))
            {
                return false;
            }
            else
            {
                Specialty newSpecialty = (Specialty) otherSpecialty;
                bool idEquality = (this.GetId()) == newSpecialty.GetId();
                bool descriptionEquality = (this.GetDescription()) == newSpecialty.GetDescription();
                return (idEquality && descriptionEquality);        
            }
        }

        public void Edit(string newDescription)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"UPDATE specialties SET description = @newDescription WHERE id = @searchId;";
            MySqlParameter searchId = new MySqlParameter(); 
            searchId.ParameterName = "@searchId";
            searchId.Value = _id;
            cmd.Parameters.Add(searchId);
            MySqlParameter description = new MySqlParameter();
            description.ParameterName = "@newDescription";
            description.Value = newDescription;
            cmd.Parameters.Add(description);
            cmd.ExecuteNonQuery();
            _description = newDescription;
            conn.Close();
            if(conn != null)
            {
                conn.Dispose();
            }
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO specialties (description) VALUES (@description);";
            MySqlParameter description = new MySqlParameter();
            description.ParameterName = "@description";
            description.Value = this._description;
            cmd.Parameters.Add(description);
            cmd.ExecuteNonQuery();
            conn.Close();
            if(conn != null)
            {
                conn.Dispose();
            }
        }

        public void Delete()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM specialties WHERE id = @searchId;";
            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = _id;
            cmd.Parameters.Add(searchId);
            cmd.ExecuteNonQuery();
            conn.Close();
            if(conn != null)
            {
                conn.Dispose();
            }
        }

        public List<Stylist> GetStylists()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT stylist_id FROM stylists_specialties WHERE specialty_id = @specialtyId;";
            MySqlParameter specailtyId = new MySqlParameter();
            specailtyId.ParameterName = "@specialtyId";
            specailtyId.Value = _id;
            cmd.Parameters.Add(specailtyId);
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            List<int> stylistIds = new List<int>{};
            while(rdr.Read())
            {
                int stylistId = rdr.GetInt32(0);
            }
            rdr.Dispose();
            List<Stylist> stylists = new List<Stylist>{};
            foreach(int stylistId in stylistIds)
            {
                var stylistQuery = conn.CreateCommand() as MySqlCommand;
                stylistQuery.CommandText = @"SELECT * FROM stylists WHERE id = @stylistId;";
                MySqlParameter stylistIdParameter = new MySqlParameter();
                stylistIdParameter.ParameterName = "@stylistId";
                stylistIdParameter.Value = stylistId;
                stylistQuery.Parameters.Add(stylistIdParameter);
                 var stylistQueryRdr = stylistQuery.ExecuteReader() as MySqlDataReader;
                 while(stylistQueryRdr.Read())
                 {
                     int thisStylistId = stylistQueryRdr.GetInt32(0);
                     string thisStylistName = stylistQueryRdr.GetString(1);
                     Stylist foundStylist = new Stylist(thisStylistName, thisStylistId);
                     stylists.Add(foundStylist);
                 }
                 stylistQueryRdr.Dispose();
            }
            conn.Close();
            if(conn != null)
            {
                conn.Dispose();
            }
            return stylists;
        }

        public void AddStylist(Stylist newStylist)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO stylist_id WHERE (stylist_id, specialty_id) VALUES (@stylistId, @SpecialtyId;";
            MySqlParameter stylist_id = new MySqlParameter();
            stylist_id.ParameterName = "@stylist_id";
            stylist_id.Value = newStylist.GetId();
            cmd.Parameters.Add(stylist_id);
            MySqlParameter specialty_id = new MySqlParameter();
            specialty_id.ParameterName = "@specialtyId";
            specialty_id.Value = _id;
            cmd.Parameters.Add(specialty_id);
            cmd.ExecuteNonQuery();
            conn.Close();
            if(conn != null)
            {
                conn.Close();
            }
        }

    }
}