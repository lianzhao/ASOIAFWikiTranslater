namespace Wiki.Api.Map
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IMapService
    {
        Task<IEnumerable<Point>> GetPointsAsync(int mapId);
    }
}