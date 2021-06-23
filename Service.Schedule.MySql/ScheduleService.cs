using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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

        public async Task<IEnumerable<ProgramInfo>> GetGroups()
        {
            var queryResult = new List<ProrgamQueryResult>();

            await connection.OpenAsync();

            using (var cmd = new MySqlCommand())
            {
                var query = "SELECT\n" +
                        "	g.educationGroupID AS ProgramId,\n" +
                        "	g.educationGroupName AS ProgramTitle,\n" +
                        "	g.isCourse,\n" +
                        "	edu.educationID AS GroupId,\n" +
                        "	edu.groupName AS GroupTitle \n" +
                            "FROM\n" +
                            "	educationgroup AS g\n" +
                            "	INNER JOIN education AS edu ON edu.educationGroupID = g.educationGroupID \n" +
                            "	AND edu.endDate > NOW() \n" +
                            "WHERE\n" +
                            "	g.isCourse = 1 \n" +
                            "	AND (\n" +
                            "	g.selfID != 117 \n" +
                            "	OR g.selfID IS NULL)";

                cmd.Connection = connection;
                cmd.CommandText = query;
                await cmd.ExecuteNonQueryAsync();

                using (var reader = cmd.ExecuteReader())
                {
                    var table = new DataTable();
                    table.Load(reader);

                    foreach (DataRow row in table.Rows)
                    {
                        var item = new ProrgamQueryResult
                        {
                            ProgramId = String.IsNullOrEmpty(row[0].ToString()) ? 0 : (int)row[0],
                            ProgramTitle = String.IsNullOrEmpty(row[1].ToString()) ? "" : (string)row[1],
                            GroupId = String.IsNullOrEmpty(row[3].ToString()) ? 0 : (int)row[3],
                            GroupTitle = String.IsNullOrEmpty(row[4].ToString()) ? "" : (string)row[4]
                        };

                        queryResult.Add(item);
                    }
                }

                var grouped = queryResult.GroupBy(x => x.ProgramId);

                var result = new List<ProgramInfo>();

                foreach (var elemt in grouped)
                {
                    var item = new ProgramInfo
                    {
                        Id = elemt.FirstOrDefault().ProgramId,
                        Title = elemt.FirstOrDefault().ProgramTitle,
                        Groups = elemt.Select(x => new GroupInfo { Id = x.GroupId, Title = x.GroupTitle })
                    };

                    result.Add(item);
                }

                return result;
            }

        }

        public async Task<IEnumerable<TeacherInfo>> GetTeachers()
        {
            var teachers = new List<TeacherInfo>();

            await connection.OpenAsync();

            using (var cmd = new MySqlCommand())
            {
                var query = @"SELECT teacherID, teacherName FROM teacher";

                cmd.Connection = connection;
                cmd.CommandText = query;
                await cmd.ExecuteNonQueryAsync();

                using (var reader = cmd.ExecuteReader())
                {
                    var table = new DataTable();
                    table.Load(reader);

                    foreach (DataRow row in table.Rows)
                    {
                        var teacher = new TeacherInfo
                        {
                            Id = String.IsNullOrEmpty(row[0].ToString()) ? 0 : (int)row[0],
                            Title = String.IsNullOrEmpty(row[1].ToString()) ? "" : (string)row[1]
                        };

                        teachers.Add(teacher);
                    }
                }
            }

            await connection.CloseAsync();

            return teachers;
        }

        public async Task<IEnumerable<TeacherEventInfo>> TeacherSchedule(int teacherId)
        {
            string sqlExpression = "shedule_of_teacher";

            var events = new List<TeacherEventInfo>();

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
                        var @event = new TeacherEventInfo
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

        public async Task<IEnumerable<GroupEventInfo>> GroupSchedule(int groupId)
        {
            string sqlExpression = "timeline_groups_all_subgroups";

            var events = new List<GroupEventInfo>();

            await connection.OpenAsync();

            try
            {
                using (var cmd = new MySqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = sqlExpression;
                    cmd.Parameters.AddWithValue("@n_group", groupId);
                    await cmd.ExecuteNonQueryAsync();

                    using (var reader = cmd.ExecuteReader())
                    {
                        var table = new DataTable();
                        table.Load(reader);

                        foreach (DataRow row in table.Rows)
                        {
                            var @event = new GroupEventInfo
                            {
                                Title = String.IsNullOrEmpty(row[0].ToString()) ? "" : (string)row[0],
                                Teachers = String.IsNullOrEmpty(row[1].ToString()) ? null : GetTeachers(row[1].ToString()),
                                Place = String.IsNullOrEmpty(row[2].ToString()) ? "" : (string)row[2],
                                BeginDate = String.IsNullOrEmpty(row[4].ToString()) ? default : DateTime.Parse(row[4].ToString()),
                                MonthNumber = String.IsNullOrEmpty(row[5].ToString()) ? "" : (string)row[5],
                                StartTime = String.IsNullOrEmpty(row[6].ToString()) ? "" : (string)row[6],
                                FinishTime = String.IsNullOrEmpty(row[7].ToString()) ? "" : (string)row[7],
                                SubGroups = String.IsNullOrEmpty(row[9].ToString()) ? 0 : int.Parse(row[9].ToString()),
                                IsCanceled = !String.IsNullOrEmpty(row[10].ToString()) && (bool)row[10],
                            };

                            events.Add(@event);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


            await connection.CloseAsync();

            return events;

            IEnumerable<string> GetTeachers(string teachers)
            {
                var array = teachers.Split(',').Select(x=>x.Trim());
                return array;
            }
        }

        internal class ProrgamQueryResult
        {
            public int ProgramId { get; set; }
            public string ProgramTitle { get; set; }
            public int GroupId { get; set; }
            public string GroupTitle { get; set; }
        }
    }
}