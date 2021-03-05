using reception.fitnesspro.ru.ViewModel;
using Service.lC;
using Service.lC.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace reception.fitnesspro.ru.Controllers.Education
{
    public class EducationStructureViewModel
    {
        private readonly IEnumerable<ProgramInfoDto> programInfos;
        private readonly IEnumerable<GroupDto> groupDtos;
        private readonly IEnumerable<SubGroupDto> subgroupDtos;

        public EducationStructureViewModel(IEnumerable<ProgramInfoDto> programInfos, IEnumerable<GroupDto> groupDtos, IEnumerable<SubGroupDto> subgroupDtos)
        {
            this.programInfos = programInfos;
            this.groupDtos = groupDtos;
            this.subgroupDtos = subgroupDtos;

            CreatePrograms();
        }

        public IEnumerable<ProgramStructureViewModel> Programs { get; set; }


        private void CreatePrograms()
        {
            Programs = programInfos?.Select(x =>
                new ProgramStructureViewModel
                {
                    Key = x.Key,
                    Title = x.Title,
                    Groups = this.FindGroup(x.Key)
                });
        }

        private IEnumerable<GroupStructureViewModel> FindGroup(Guid key)
        {
            var array = groupDtos.Where(x => x.ProgramKey == key)
                  .Select(x =>
                    new GroupStructureViewModel
                    {
                        Key = x.Key,
                        Title = x.Title,
                        SubGroups = this.FindSubgroup(x.Key)
                    }
                  );

            return array ?? Enumerable.Empty<GroupStructureViewModel>();
        }

        private IEnumerable<SubgroupViewModel> FindSubgroup(Guid key)
        {
            var array = subgroupDtos.Where(x => x.GroupKey == key)
                              .Select(x =>
                                new SubgroupViewModel
                                {
                                    Key = x.Key,
                                    Title = x.Title
                                }
                              );

            return array ?? Enumerable.Empty<SubgroupViewModel>();
        }
    }

    public class ProgramStructureViewModel : BaseInfoViewModel
    {
        public IEnumerable<GroupStructureViewModel> Groups { get; set; }
    }

    public class GroupStructureViewModel : BaseInfoViewModel
    {
        public IEnumerable<SubgroupViewModel> SubGroups { get; set; }
    }

    public class SubgroupViewModel : BaseInfoViewModel
    { }
}
