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
using Domain.Model;

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

        public IEnumerable<Domain.BaseConstraint> FindCommonSettingsByDiscipline(Guid disciplineKey)
        {
            var dto = database.Constraints.FilterBy(x => x.DisciplineKey == disciplineKey);

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
            var dubles = database.Constraints.FilterBy(x => x.ProgramKey == constraint.ProgramKey && x.DisciplineKey == constraint.DisciplineKey);

            if (dubles.IsNullOrEmpty() == false) throw new ArgumentException("Настройки для Программы и дисциплины уже существуют.");

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
            var settings = (await Task.FromResult(database.Settings.Teacher.AsQueryable())).ToList();

            var result = settings.Adapt<IEnumerable<TeacherSetting>>();

            return result;
        }

        public async Task<Guid> AddTeacherSettings(TeacherSetting model)
        {
            var dubles = database.Settings.Teacher.FilterBy(x => x.ScheduleTeacherId == model.ScheduleTeacherId && x.ServiceTeacherKey == model.ServiceTeacherKey);

            if (dubles.IsNullOrEmpty() == false) throw new ArgumentException("Настройки для преподавателя уже существуют.");

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

            if (setting != default) await database.Settings.Teacher.DeleteOneAsync(x => x.Id == setting.Id);
        }

        public async Task<TeacherSetting> GetTeacherSettings(Guid serviceTeacherKey)
        {
            var setting = await database.Settings.Teacher.FindOneAsync(x => x.ServiceTeacherKey == serviceTeacherKey);

            var model = setting.Adapt<TeacherSetting>();

            return model;
        }



        public async Task<IEnumerable<GroupSettings>> GetAllGroupSettings()
        {
            var settings = await Task.FromResult(database.Settings.Group.AsQueryable());

            var result = settings.Adapt<IEnumerable<GroupSettings>>();

            return result;
        }

        public async Task<GroupSettings> GetGroupSettings(Guid entityKey)
        {
            var setting = await database.Settings.Group.FindOneAsync(x => x.Key == entityKey);

            var model = setting.Adapt<GroupSettings>();

            return model;
        }

        public async Task<Guid> AddGroupSettings(GroupSettings model)
        {
            var dubles = await FindGroupSettings(model.Group.Key);

            if (dubles != default) throw new ArgumentException("Настройки для группы уже существуют.");

            model.Key = Guid.NewGuid();

            var dto = model.Adapt<Service.MongoDB.Model.GroupSettings>();

            database.Settings.Group.InsertOne(dto);

            var result = await Task.FromResult(database.Settings.Group.FindOne(x => x.Key == dto.Key));

            return result.Key;
        }

        public async Task UpdateGroupSettings(GroupSettings model)
        {
            var dto = await Task.FromResult(database.Settings.Group.FindOne(x => x.Key == model.Key));

            var resultDto = model.Adapt(dto);

            database.Settings.Group.ReplaceOne(resultDto);
        }

        public async Task DeleteGroupSettings(Guid key)
        {
            var setting = await Task.FromResult(database.Settings.Group.FindOne(x => x.Key == key));

            if (setting != default) await database.Settings.Group.DeleteOneAsync(x => x.Id == setting.Id);
        }

        public async Task<GroupSettings> FindGroupSettings(Guid serviceGroupKey)
        {
            if(serviceGroupKey == default) return null;

            var setting = await database.Settings.Group.FindOneAsync(x => x.Group.Key == serviceGroupKey);

            var model = setting.Adapt<GroupSettings>();

            return model;
        }

        public async Task<IEnumerable<ScheduleProgramInfo>> GetAllScheduleGroups()
        {
            var dto = await schedule.GetGroups();

            var result = dto.Adapt<IEnumerable<ScheduleProgramInfo>>();

            return result;
        }


        public async Task<IEnumerable<Domain.Education.Program>> GetAllServiceGroups()
        {
            var programManager = lcService.Program;

            var programs = await programManager.GetAll();
            await programManager.IncludeGroups(programs);

            var result = programs.Select(x => GetProgram(x)).Where(x => x != null);

            var model = result.Adapt<IEnumerable<Domain.Education.Program>>();

            return model;

            Service.lC.Model.Program GetProgram(Service.lC.Model.Program program)
            {
                var groups = program.Groups.Where(g => g.Finish > DateTime.Now);

                if (groups != default && groups.Count() > 0)
                {
                    program.Groups = groups;
                    return program;
                }

                return null;
            }
        }



        public async Task<StudentSetting> GetStudentSetting(Guid studentKey)
        {
            var setting = await database.Settings.Student.FindOneAsync(x => x.StudentKey == studentKey);

            if (setting == default) return null;

            var model = new StudentSetting(setting.StudentKey);
            setting.DisciplineSettings.ToList().ForEach(x => model.AddDiscipline(x.DisciplineKey, x.SignUpCount, x.SignOutCount, x.LastDaySetting));

            return model;
        }

        public async Task<Guid> AddStudentSetting(StudentSetting model)
        {
            var setting = await database.Settings.Student.FindOneAsync(x => x.StudentKey == model.StudentKey);

            if (setting != default) throw new ArgumentException("Настройки для студента уже существуют.");

            var dto = new Service.MongoDB.Model.StudentSetting();
            dto.Key = Guid.NewGuid();
            dto.StudentKey = model.StudentKey;
            dto.DisciplineSettings = model.DisciplineSettings.Select(x => new Service.MongoDB.Model.DisciplineSetting
            {
                DisciplineKey = x.disciplineKey,
                SignUpCount = x.GetRestSignUpCount(),
                SignOutCount = x.GetRestSignOutCount(),
                LastDaySetting = x.GetSignUpLastDate()
            });

            database.Settings.Student.InsertOne(dto);

            var result = await Task.FromResult(database.Settings.Student.FindOne(x => x.Key == dto.Key));

            return result.Key;
        }

        public async Task UpdateStudentSetting(StudentSetting model)
        {
            var dto = await database.Settings.Student.FindOneAsync(x => x.StudentKey == model.StudentKey);
            dto.DisciplineSettings = model.DisciplineSettings.Select(x => new Service.MongoDB.Model.DisciplineSetting
            {
                DisciplineKey = x.disciplineKey,
                SignUpCount = x.GetRestSignUpCount(),
                SignOutCount = x.GetRestSignOutCount(),
                LastDaySetting = x.GetSignUpLastDate()
            });

            database.Settings.Student.ReplaceOne(dto);
        }
    }
}
