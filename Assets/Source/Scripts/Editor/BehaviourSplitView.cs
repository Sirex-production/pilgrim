using UnityEngine.UIElements;

namespace Ingame.Editor
{
    public class BehaviourSplitView : TwoPaneSplitView
    {
        public new class UxmlFactory : UxmlFactory<BehaviourSplitView,TwoPaneSplitView.UxmlTraits>{}
    }
}