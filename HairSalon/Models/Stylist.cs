using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace HairSalon.Models
{
    public class Stylist
    {
       private List<Client> _name;
        private string _stylistName;
        private int _id;

        public Stylist(string name, int id = 0)
        {
            _stylistName = name;
            _id = id;
            _name = new List<Client>{};
        }

        public string GetName()
        {
            return _stylistName;
        }

        public int GetId()
        {
            return _id;
        }

        public override int GetHashCode()
        {
            return this.GetId().GetHashCode();
        }

            public static void ClearAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM stylists;";
            cmd.ExecuteNonQuery();
            conn.Close();
            if(conn != null)
            {
                conn.Dispose();
            }
        }


        public override bool Equals(System.Object otherStylist)
        {
            if(!(otherStylist is Stylist))
            {
                return false;
            }
            else
            {
                Stylist newStylist = (Stylist) otherStylist;
                bool idEquality = this.GetId().Equals(newStylist.GetId());
                bool nameEquality = this.GetName().Equals(newStylist.GetName());
                return (idEquality && nameEquality);
            }
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO stylists (name) VALUES (@name);";
            MySqlParameter name = new MySqlParameter();
            name.ParameterName = "@name";
            name.Value = this._stylistName;
            cmd.Parameters.Add(name);
            cmd.ExecuteNonQuery();
            _id = (int) cmd.LastInsertedId;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static List<Stylist> GetAll()
        {
            List<Stylist> allStylists = new List<Stylist>{};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM stylists;";
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read())
            {
                int stylistId = rdr.GetInt32(0);
                string stylistName = rdr.GetString(1);
                Stylist newStylist = new Stylist(stylistName, stylistId);
                allStylists.Add(newStylist);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allStylists;
        }

        public static Stylist Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM stylists WHERE id = (@searchId);";
            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = id;
            cmd.Parameters.Add(searchId);
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            int stylistId = 0;
            string stylistName = "";
            while(rdr.Read())
            {
                stylistId = rdr.GetInt32(0);
                stylistName = rdr.GetString(1);
            }
            Stylist newStylist = new Stylist(stylistName, stylistId);
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return newStylist;

        }

        public List<Client> GetClients()
        {
            List<Client> allClients = new List<Client>{};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM clients WHERE stylist_id = @stylist_id;";
            MySqlParameter stylistId = new MySqlParameter();
            stylistId.ParameterName = "@stylist_id";
            stylistId.Value = this._id;
            cmd.Parameters.Add(stylistId);
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read())
            {
                int clientId = rdr.GetInt32(0);
                string clientName = rdr.GetString(1);
                int clientStylistId = rdr.GetInt32(2);
                Client newClient = new Client(clientName, clientStylistId, clientId);
                allClients.Add(newClient);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allClients;

        }

        public List<Specialty> GetSpecialties()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT specialties.* FROM stylists JOIN stylists_specialties ON (stylists.id = stylists_specialties.stylist_id) JOIN specialties ON (stylists_specialties.specialty_id = specialties.id) WHERE stylists.id = @StylistId;";
            MySqlParameter specialtyIdParameter = new MySqlParameter();
            specialtyIdParameter.ParameterName = "@StylistId";
            specialtyIdParameter.Value = _id;
            cmd.Parameters.Add(specialtyIdParameter);
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            List<Specialty> specialties = new List<Specialty>{};
            while(rdr.Read())
            {
                int specialtyId = rdr.GetInt32(0);
                string specialtyDescriptioin = rdr.GetString(1);
                Specialty newSpecialty = new Specialty(specialtyDescriptioin, specialtyId );
                specialties.Add(newSpecialty);
            }
            conn.Close();
            if(conn != null)
            {
                conn.Dispose();
            }
            return specialties;
        }

        public void AddClient(Client client)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO stylists_specialties (stylist_id, specialty_id) VALUES (@StylistId, @SpecialtyId); ";
            MySqlParameter stylist_id = new MySqlParameter();
            stylist_id.ParameterName = "@StylistId";
            stylist_id.Value = _id;
            cmd.Parameters.Add(stylist_id);
            MySqlParameter specialty_id = new MySqlParameter();
            specialty_id.ParameterName = "@SpecialtyId";
            specialty_id.Value = _id;
            cmd.Parameters.Add(specialty_id);
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
            MySqlCommand cmd = new MySqlCommand(@"DELETE FROM stylists WHERE id = @StylistId; DELETE FROM stylists_specialties WHERE stylist_id = @StylistId;", conn);
            MySqlParameter stylistIdParameter = new MySqlParameter();
            stylistIdParameter.ParameterName = "@StylistId";
            stylistIdParameter.Value = this.GetId();
            cmd.Parameters.Add(stylistIdParameter);
            cmd.ExecuteNonQuery();
            if (conn != null)
            {
                conn.Close();
            }
        }
    }
}