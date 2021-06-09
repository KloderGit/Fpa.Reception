using System;
using System.Collections.Generic;

namespace Domain.Model
{
    public class StudentSetting
    {
        public Guid StudentKey { get; private set; }

        public List<DisciplineSetting> DisciplineSettings { get; private set; } = new List<DisciplineSetting>();

        public StudentSetting(Guid studentKey)
        {
            this.StudentKey = studentKey;
        }

        public void AddDiscipline(Guid disciplineKey, int? baseSignUpCountSetting, int? baseSignOutCountSetting, DateTime? baseLastDaySetting)
        {
            var disciplineSetting = new DisciplineSetting(disciplineKey, baseSignUpCountSetting, baseSignOutCountSetting, baseLastDaySetting);

            DisciplineSettings.Add(disciplineSetting);
        }

        public void AddSignUpAttempts(Guid disciplineKey, int attempts)
        {
            var disciplineSetting = FindDisciplineSetting(disciplineKey);

            disciplineSetting.AddSignUpCount(attempts);
        }

        public void AddSignOutAttempts(Guid disciplineKey, int attempts)
        {
            var disciplineSetting = FindDisciplineSetting(disciplineKey);

            disciplineSetting.AddSignOutCount(attempts);
        }

        public void ChangeSignUpPeriod(Guid disciplineKey, DateTime date)
        {
            var disciplineSetting = FindDisciplineSetting(disciplineKey);

            disciplineSetting.ChangeAllowedPeriod(date);
        }

        public void SubtractSignUpAttempt(Guid disciplineKey)
        {
            var disciplineSetting = FindDisciplineSetting(disciplineKey);

            disciplineSetting.SubtractSignUpCount();
        }

        public void SubtractSignOutAttempt(Guid disciplineKey)
        {
            var disciplineSetting = FindDisciplineSetting(disciplineKey);

            disciplineSetting.SubtractSignOutCount();
        }

        public int? GetRestSignUpCount(Guid disciplineKey)
        {
            var disciplineSetting = DisciplineSettings.Find(x => x.disciplineKey == disciplineKey);
            if (disciplineSetting == default) return null;

            return disciplineSetting.GetRestSignUpCount();
        }

        public int? GetRestSignOutCount(Guid disciplineKey)
        {
            var disciplineSetting = DisciplineSettings.Find(x => x.disciplineKey == disciplineKey);
            if (disciplineSetting == default) return null;

            return disciplineSetting.GetRestSignOutCount();
        }

        public DateTime? GetSignUpLastDate(Guid disciplineKey)
        {
            var disciplineSetting = DisciplineSettings.Find(x => x.disciplineKey == disciplineKey);
            if (disciplineSetting == default) return null;

            return disciplineSetting.GetSignUpLastDate();
        }

        private DisciplineSetting FindDisciplineSetting(Guid disciplineKey)
        {
            var disciplineSetting = DisciplineSettings.Find(x => x.disciplineKey == disciplineKey);

            if (disciplineSetting == default) throw new ArgumentNullException(nameof(disciplineKey));

            return disciplineSetting;
        }
    }

    public class DisciplineSetting
    {
        private int? signUpCount;
        private int? signOutCount;
        private DateTime? lastDaySetting;
        public Guid disciplineKey { get; set; }

        public DisciplineSetting(Guid disciplineKey, int? baseSignUpCountSetting, int? baseSignOutCountSetting, DateTime? baseLastDaySetting)
        {
            this.disciplineKey = disciplineKey;
            this.signUpCount = baseSignUpCountSetting;
            this.signOutCount = baseSignOutCountSetting;
            this.lastDaySetting = baseLastDaySetting;
        }

        public void AddSignUpCount(int attempts)
        {
            if (signUpCount.HasValue == false) return;
            signUpCount += attempts;
        }

        public void AddSignOutCount(int attempts)
        {
            if (signOutCount.HasValue == false) return;
            signOutCount += attempts;
        }

        public void ChangeAllowedPeriod(DateTime date)
        {
            lastDaySetting = date;
        }

        public void SubtractSignUpCount()
        {
            if (signUpCount.HasValue == false) return;
            signUpCount -= 1;
        }

        public void SubtractSignOutCount()
        {
            if (signOutCount.HasValue == false) return;
            signOutCount -= 1;
        }

        public bool CanSignUp(int currentBaseSignUpCountSetting, DateTime currentBaseLastDaySetting)
        {
            var checkSignUpCount = signUpCount.HasValue == false || currentBaseSignUpCountSetting - signUpCount > 0;

            if (checkSignUpCount && CheckDate(currentBaseLastDaySetting)) return true;

            return false;
        }

        public bool CanSignOut(int currentBaseSignOutCountSetting)
        {
            var checkSignOutCount = signOutCount.HasValue == false || currentBaseSignOutCountSetting - signOutCount > 0;

            if (checkSignOutCount) return true;

            return false;
        }

        private bool CheckDate(DateTime? date)
        {
            if (date.HasValue == false || lastDaySetting > date) return true;
            return false;
        }

        public int? GetRestSignUpCount() => signUpCount;

        public int? GetRestSignOutCount() => signOutCount;

        public DateTime? GetSignUpLastDate() => lastDaySetting;
    }
}
