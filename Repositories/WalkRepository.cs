using DogGo.Models;
using Microsoft.Data.SqlClient;

namespace DogGo.Repositories
{
    public class WalkRepository : IWalkRepository
    {
        private readonly IConfiguration _config;

        public WalkRepository(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        public List<Walk> GetWalksByWalkerId(int walkerId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    SELECT w.Id as walkId, w.Date, w.Duration, w.DogId, o.Id as ownerId, o.[Name] as ownerName
                    FROM Walks w
                    JOIN Dog d
                    ON w.DogId = d.Id
                    JOIN Owner o
                    ON d.OwnerId = o.Id 
                    WHERE WalkerId = @walkerId";

                    cmd.Parameters.AddWithValue("@walkerId", walkerId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Walk> walks = new List<Walk>();
                    while (reader.Read())
                    {
                        Walk walk = new Walk()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("walkId")),
                            Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                            DogId = reader.GetInt32(reader.GetOrdinal("DogId")),
                            DurationTime = TimeSpan.FromSeconds(reader.GetInt32(reader.GetOrdinal("Duration"))),
                            Owner = new Owner()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("ownerId")),
                                Name = reader.GetString(reader.GetOrdinal("ownerName")),
                            }
                        };
                        
                        walks.Add(walk);
                    }

                    reader.Close();

                    return walks;
                }
            }
        }
    }
}
