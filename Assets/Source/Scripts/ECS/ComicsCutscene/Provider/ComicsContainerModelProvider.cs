using System.Collections.Generic;
using System.Linq;
using Ingame.Comics;
using Ingame.Movement;
using Voody.UniLeo;
using Zenject;

namespace Ingame.ComicsCutscene
{
    public class ComicsContainerModelProvider : MonoProvider<ComicsContainerModel>
    {
        [Inject]
        private void Construct()
        {
            var dic = value.comicsHolderContainer.Pages.ToDictionary(i => i.Name);
            
            value = new ComicsContainerModel
            {
                comicsHolderContainer = value.comicsHolderContainer,
                comics = dic
            };
        }
    }
}