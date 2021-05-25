using Domain;
using Domain.Interface;
using Mapster;
using Service.lC;
using System;
using System.Collections.Generic;
using System.Linq;
using Service.MongoDB;
using Service.Schedule.MySql;
using System.Threading.Tasks;
using Application.Extensions;

namespace Application.Component
{
    public class SettingComponent : ISettingComponent
    {
        private readonly MongoContext database;
        private readonly Context lcService;
        private readonly IScheduleService schedule;

        public SettingComponent(MongoContext database, Context lcService, IScheduleService schedule)
        {
            this.database = database;
            this.lcService = lcService;
            this.schedule = schedule;
        }

        public Domain.BaseConstraint GetByKey(Guid key)
        {
            var result = database.Constraints.FindOne(x => x.Key == key);

            return result.Adapt<Domain.BaseConstraint>();
        }

        public IEnumerable<Domain.BaseConstraint> Find(Guid? programKey, Guid disciplineKey)
        {
            var dto = database.Constraints.FilterBy(
                x => x.ProgramKey == programKey &&
                     x.DisciplineKey == disciplineKey);

            return dto.Adapt<IEnumerable<Domain.BaseConstraint>>();
        }

        public IEnumerable<Domain.BaseConstraint> Get(IEnumerable<Guid> constraintKeys)
        {
            var dto = database.Constraints.FilterByArray("Key", constraintKeys);

            return dto.Adapt<IEnumerable<Domain.BaseConstraint>>();
        }

        public IEnumerable<Domain.BaseConstraint> GetAll()
        {
            var dto = database.Constraints.AsQueryable();

            return dto.Adapt<IEnumerable<Domain.BaseConstraint>>();
        }

        public Guid Store(BaseConstraint constraint)
        {
            var dubles = database.Constraints.FilterBy(x=>x.ProgramKey == constraint.ProgramKey && x.DisciplineKey == constraint.DisciplineKey);

            if(dubles.IsNullOrEmpty() == false) throw new ArgumentException("Настройки для Программы и дисциплины уже существуют.");

            constraint.Key = Guid.NewGuid();
            var dto = constraint.Adapt<Service.MongoDB.Model.BaseConstraintDto>();

            database.Constraints.InsertOne(dto);

            var result = GetByKey(dto.Key);

            return result.Key;
        }

        public void Update(BaseConstraint constraint)
        {
            var dto = database.Constraints.FindOne(x => x.Key == constraint.Key);

            var resultDto = constraint.Adapt(dto);

            database.Constraints.ReplaceOne(resultDto);
        }




        public async Task<IEnumerable<Tuple<int, string>>> GetAllScheduleTeachers()
        {
            var dto = await schedule.GetTeachers();

            var result = dto.Select(x => new Tuple<int, string>(x.Id, x.Title));

            return result;
        }

        public async Task<IEnumerable<BaseInfo>> GetAllServiceTeachers()
        {
            var teachers = await lcService.Education.GetAllTeachers();

            return teachers.Adapt<IEnumerable<BaseInfo>>();
        }

        public async Task<IEnumerable<TeacherSetting>> GetAllTeacherSettings()
        {
            var settings = await Task.FromResult(database.Settings.Teacher.AsQueryable());

            var result = settings.Adapt<IEnumerable<TeacherSetting>>();

            return result;
        }

        public async Task<Guid> AddTeacherSettings(TeacherSetting model)
        {
            model.Key = Guid.NewGuid();

            var dto = model.Adapt<Service.MongoDB.Model.TeacherSetting>();

            database.Settings.Teacher.InsertOne(dto);

            var result = await Task.FromResult(database.Settings.Teacher.FindOne(x => x.Key == dto.Key));

            return result.Key;
        }

        public async Task UpdateTeacherSettings(TeacherSetting model)
        {
            var dto = database.Settings.Teacher.FindOne(x => x.Key == model.Key);

            var resultDto = model.Adapt(dto);

            database.Settings.Teacher.ReplaceOne(resultDto);
        }

        public async Task DeleteTeacherSettings(Guid key)
        {
            var setting = await Task.FromResult(database.Settings.Teacher.FindOne(x => x.Key == key));

            if(setting != default) await database.Settings.Teacher.DeleteOneAsync(x=>x.Id == setting.Id);
        }

        public async Task<TeacherSetting> GetTeacherSettings(Guid serviceTeacherKey)
        {
            var setting = await database.Settings.Teacher.FindOneAsync(x=>x.ServiceTeacherKey == serviceTeacherKey);

            var model = setting.Adapt<TeacherSetting>();

            return model;
        }
    }
}
