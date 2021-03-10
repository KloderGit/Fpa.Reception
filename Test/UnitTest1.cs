using Application.Component;
using Domain;
using lc.fitnesspro.library;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using Service.lC;
using Service.lC.Manager;
using Service.lC.Provider;
using Service.MongoDB;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;

namespace Test
{

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod3()
        {
            var cal = new CalendarGenerator(2021, 2);
            cal.Generate();

        }

        [TestMethod]
        public void TestMethod2()
        {
            var lcManager = new Manager("kloder", "Kaligula2");

            var http = new HttpClient();
            http.BaseAddress = new Uri("https://api.fitness-pro.ru/");
            var baseHttp = new BaseHttpClient(http);

            var context = new Context(baseHttp, lcManager, null);

            var progs = context.Program.FilterByTeacher(new Guid("f9d94670-5fb3-11eb-8138-0cc47a4b75cc")).Result;
                progs = context.Program.IncludeDisciplines(progs).Result;
                progs = context.Program.IncludeEducationForm(progs).Result;
                progs = context.Program.IncludeTeachers(progs).Result;
                progs = context.Program.IncludeGroups(progs).Result;
        }




        [TestMethod]
        public void TestMethod1()
        {
            var settings = new MongoDbSettings()
            {
                ConnectionString = "http://localhost:7010",
                DatabaseName = "Reception"
            };

            var mongo = new MongoRepository<ReceptionDto>(settings);


            var item = new ReceptionDto
            {
                Date = DateTime.Now,
                IsActive = true,
                Key = Guid.NewGuid(),
                Histories = new List<History> { new History { Object = Guid.NewGuid(), Action = " создал ", Subject = Guid.NewGuid(), DateTime = DateTime.UtcNow } },
                Events = new List<Event> {
                         new Event {
                          Teachers = new List<BaseInfo>{ new BaseInfo { Key = Guid.NewGuid(), Title = "Меркурьев" }, new BaseInfo { Key = Guid.NewGuid(), Title = "Калашников" } },
                           Discipline =  new BaseInfo { Key = Guid.NewGuid(), Title = "Anatomy" },
                             Restrictions = new List<PayloadRestriction>{ new PayloadRestriction { Program = Guid.NewGuid(), Group = Guid.NewGuid(), SubGroup = Guid.NewGuid(),
                                 Option = new PayloadOption{
                                     CheckAttemps = true,
                                      CheckContractExpired = true,
                                       CheckDependings = false
                                 }
                             } },
                              Requirement = new PayloadRequirement{
                               AllowedAttemptCount = 10,
                                SubscribeBefore = DateTime.UtcNow,
                                 UnsubscribeBefore = DateTime.Now,
                                  DependsOnOtherDisciplines = new List<Guid>{
                                   Guid.NewGuid()
                                  }
                              }
                         }
                     },
                PositionManager = new PositionManager()
                {
                    Positions = new List<Position> {
                   new Position{
                     Key = Guid.NewGuid(),
                      IsActive = true,
                       Time = DateTime.Now,
                        Record = new Record{
                             DisciplineKey = Guid.NewGuid(),
                              ProgramKey = Guid.NewGuid(),
                               StudentKey = Guid.NewGuid(),
                                 Result = new Result{
                                     TeacherKey = Guid.NewGuid(),
                                      Comment = "Rate comment",
                                       Score = new Hundred(45)
                                 }
                        }
                   }
                  }
                }
            };


            mongo.InsertOneAsync(item).ConfigureAwait(false).GetAwaiter().GetResult();

        }

        public class ReceptionDto : Reception, IDocument
        {
            ObjectId IDocument.Id { get; set; }
            DateTime IDocument.CreatedAt { get; }
        }



        public class CalendarGenerator
        {
            private readonly DateTime date;
            private LinkedList<DateTime> dates = new LinkedList<DateTime>();

            public CalendarGenerator(int year, int month)
            {
                this.date = new DateTime(year, month, 1);
            }

            public void Generate()
            {
                var monthDayCount = 32 - date.AddDays(31).Day;

                var weekDay = (int)date.DayOfWeek == 0 ? 7 : (int)date.DayOfWeek;

                var startCalendarDay = date;

                startCalendarDay = startCalendarDay.AddDays(-(weekDay - 1));

                for (var i = 0; i < weekDay - 1; i++)
                {
                    dates.AddLast(new LinkedListNode<DateTime>(new DateTime(startCalendarDay.AddDays(i).Ticks)));
                }

                for (var i = 0; i < monthDayCount; i++)
                {
                    dates.AddLast(new LinkedListNode<DateTime>(new DateTime(date.AddDays(i).Ticks)));
                }

                var lastDayWeekDay = (int)date.AddDays(monthDayCount-1).DayOfWeek;

                var lastDate = date.AddDays(monthDayCount - 1);

                for (var i = 1; i <= 7 - lastDayWeekDay; i++)
                {
                    dates.AddLast(new LinkedListNode<DateTime>(new DateTime(lastDate.AddDays(i).Ticks)));
                }
            }


        }



    }
}
