using Mapster;
using System;

namespace Application.Mappings
{
    public class ScoreMaps
    {
        public ScoreMaps()
        {
            TypeAdapterConfig<Service.MongoDB.Model.Score, Domain.Score>
            .NewConfig()
                .MapWith(src => new Domain.Score((Domain.AttestationScoreType)(int)src.Type, GetParametr(src)));

            TypeAdapterConfig<Domain.Score, Service.MongoDB.Model.Score>
            .NewConfig()
                .Map(dest => dest.Type, src => (Service.MongoDB.Model.ScoreType)(int)src.Type)
                .Map(dest => dest.Value, src => new Tuple<string, object>(src.GetScoreType().FullName, src.GetScoreValue()));
        }

        private Tuple<Type, Object> GetParametr(Service.MongoDB.Model.Score score)
        {
            var type = Type.GetType(score.Value.Item1);
            var value = score.Value.Item2;

            var tuple = new Tuple<Type, Object>(type, value);

            return tuple;
        }
    }
}
