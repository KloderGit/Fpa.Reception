using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using MySqlConnector;
using Service.Schedule.MySql.Model;

namespace Service.Schedule.MySql
{
    public class ScheduleService : IScheduleService
    {
        private readonly MySqlConnection connection;

        public ScheduleService(MySqlConnection connection)
        {
            this.connection = connection;
        }

        public async Task<IEnumerable<EventInfo>> TeacherSchedule(int teacherId)
        {
            string sqlExpression = "shedule_of_teacher";

            var events = new List<EventInfo>();

            await connection.OpenAsync();

            using (var cmd = new MySqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = sqlExpression;
                cmd.Parameters.AddWithValue("@id_prep", teacherId);
                await cmd.ExecuteNonQueryAsync();

                using (var reader = cmd.ExecuteReader())
                {
                    var table = new DataTable();
                    table.Load(reader);
                    
                    foreach (DataRow row in table.Rows)
                    {
                        var @event = new EventInfo
                        {
                            Title = String.IsNullOrEmpty(row[0].ToString()) ? "" : (string)row[0],
                            IsCanceled = !String.IsNullOrEmpty(row[1].ToString()) && (bool)row[1],
                            BeginDate = String.IsNullOrEmpty(row[10].ToString()) ? default : DateTime.Parse(row[10].ToString()),
                            MonthNumber = String.IsNullOrEmpty(row[3].ToString()) ? "" : (string)row[3],
                            StartTime = String.IsNullOrEmpty(row[4].ToString()) ? "" : (string)row[4],
                            FinishTime = String.IsNullOrEmpty(row[5].ToString()) ? "" : (string)row[5],
                            Groups = String.IsNullOrEmpty(row[6].ToString()) ? "" : (string)row[6],
                            Place = String.IsNullOrEmpty(row[8].ToString()) ? "" : (string)row[8],
                            Education = String.IsNullOrEmpty(row[9].ToString()) ? "" : (string)row[9],
                        };
                        
                        events.Add(@event);
                    }
                }
            }

            await connection.CloseAsync();
            
            return events;
        }
    }
}