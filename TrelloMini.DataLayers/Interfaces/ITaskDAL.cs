using System.Collections.Generic;

namespace TrelloMini.DataLayers
{
    public interface ITaskDAL
    {
        List<TrelloMini.Models.Task> List(int userId);

        bool Add(TrelloMini.Models.Task data);

        bool Update(TrelloMini.Models.Task data);

        bool Delete(int id);
    }
}

