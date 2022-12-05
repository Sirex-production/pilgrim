using Ingame.Enemy;
using Ingame.Movement;
using Leopotam.Ecs;

namespace Ingame.Behaviour
{
    public sealed class BehaviourSystem : IEcsRunSystem
    {
        //todo refactor - change a name convention
        /*private readonly EcsFilter<BehaviourAgentModel,BehaviourCheckerTag> _behaviorAgentBeforeInitFilter;
        private readonly EcsFilter<BehaviourAgentModel,BehaviourAgentActiveTag>.Exclude<BehaviourCheckerTag> _behaviorAgentFilter;*/
        private readonly EcsFilter<BehaviourAgentModel,BehaviourCheckerTag,TransformModel> _behaviorAgentBeforeInitFilter;
        private readonly EcsFilter<BehaviourAgentModel,BehaviourAgentActiveTag,TransformModel>.Exclude<BehaviourCheckerTag> _behaviorAgentFilter;
        private readonly EcsFilter<BehaviourAgentModel>.Exclude<BehaviourCheckerTag,BehaviourAgentActiveTag> _behaviorAgentRunFilter;
        private EcsWorld _world;
        public void Run()
        {
            //create copy of tree
            foreach (var i in _behaviorAgentBeforeInitFilter)
            { 
                ref var agent =  ref _behaviorAgentBeforeInitFilter.Get1(i);
                agent.Tree = agent.OriginalTree.Clone();
                ref var entity =  ref _behaviorAgentBeforeInitFilter.GetEntity(i);
                
#if UNITY_EDITOR
                UnityEngine.Debug.Log($"{agent.Tree.name} in {entity.ToString()} has been Initialized correctly");         
#endif
               
                //Inject
                agent.Tree.InjectWorld(_world);
                
                //Modify Components
                entity.Get<BehaviourAgentActiveTag>();
                entity.Del<BehaviourCheckerTag>();
            }

            //inject entity
            foreach (var i in _behaviorAgentFilter)
            {
                ref var agent =  ref _behaviorAgentFilter.Get1(i);
                ref var transform =  ref _behaviorAgentFilter.Get3(i);
                ref var entity = ref _behaviorAgentFilter.GetEntity(i);
              
                if (!transform.transform.gameObject.TryGetComponent(out EntityReference reference)) continue;
                if (reference.Entity == null)
                {
                    continue;
                }
                agent.Tree.Entity = reference.Entity;
                agent.Tree.InjectEntity(reference.Entity);
                entity.Del<BehaviourAgentActiveTag>();
            }
            //Run
            foreach (var i in _behaviorAgentRunFilter)
            {
                ref var agent =  ref _behaviorAgentRunFilter.Get1(i);
                ref var entity = ref _behaviorAgentRunFilter.GetEntity(i);
                if (agent.Tree.Tick() != Node.State.Running)
                {
                    entity.Del<BehaviourAgentActiveTag>();
                } 
            }
            //Init 
            /*foreach (var i in _behaviorAgentBeforeInitFilter)
            { 
                ref var agent =  ref _behaviorAgentBeforeInitFilter.Get1(i);
                agent.Tree = agent.Tree.Clone();
                ref var entity =  ref _behaviorAgentBeforeInitFilter.GetEntity(i);
                
                #if UNITY_EDITOR
                    UnityEngine.Debug.Log($"{agent.Tree.name} in {entity.ToString()} has been Initialized correctly");         
                #endif
               
                //Inject
                agent.Tree.Entity = entity;
                agent.Tree.InjectEntity(entity);
                agent.Tree.InjectWorld(_world);
              
                //if (!agent.Tree.Init()) continue;
                //Modify Components
                entity.Get<BehaviourAgentActiveTag>();
                entity.Del<BehaviourCheckerTag>();*/
            }
            
            //Tick
            /*foreach (var i in _behaviorAgentFilter)
            {
                ref var agent =  ref _behaviorAgentBeforeInitFilter.Get1(i);
                ref var entity = ref _behaviorAgentBeforeInitFilter.GetEntity(i);
                UnityEngine.Debug.Log(agent.Tree.Entity.ToString());
                if (agent.Tree.Tick() != Node.State.Running)
                {
                   
                    entity.Del<BehaviourAgentActiveTag>();
                } 
            }*/
        //}
    }
}